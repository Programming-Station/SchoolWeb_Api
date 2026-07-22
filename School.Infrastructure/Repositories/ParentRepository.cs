using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.Domain.Auth;
using School.Infrastructure.JWTAuthenticationManager.Interfaces;
using School.Infrastructure.Repositories.IRepositories;
using School_DTOs;
using School_DTOs.Parent;

namespace School.Infrastructure.Repositories
{
    public class ParentRepository : IParentRepository
    {
        private readonly SchoolDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJWTAuthenticationManager _jwtAuthManager;

        public ParentRepository(
            SchoolDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJWTAuthenticationManager jwtAuthManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtAuthManager = jwtAuthManager;
        }

        // ── Login ─────────────────────────────────────────────────────────────────

        public async Task<APIResponse<ParentLoginResponseDto>> ParentLoginAsync(ParentLoginModel model)
        {
            try
            {
                // Find by phone number (mobile)
                var user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == model.Mobile);

                if (user == null)
                {
                    return Unauthorized<ParentLoginResponseDto>("Mobile number not registered.");
                }

                // Lockout check
                if (user.LockoutEndDate.HasValue && user.LockoutEndDate.Value > DateTime.Now)
                {
                    return new APIResponse<ParentLoginResponseDto>
                    {
                        Success = false,
                        StatusCode = HttpStatusCode.Forbidden,
                        Message = $"Account is locked until {user.LockoutEndDate.Value:yyyy-MM-dd HH:mm}."
                    };
                }

                // Verify password
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!result.Succeeded)
                {
                    user.FailedLoginAttempts++;
                    if (user.FailedLoginAttempts >= 5)
                    {
                        user.LockoutEndDate = DateTime.Now.AddMinutes(30);
                        user.FailedLoginAttempts = 0;
                        await _userManager.UpdateAsync(user);
                        return new APIResponse<ParentLoginResponseDto>
                        {
                            Success = false,
                            StatusCode = HttpStatusCode.Forbidden,
                            Message = "Too many failed attempts. Account locked for 30 minutes."
                        };
                    }
                    await _userManager.UpdateAsync(user);
                    return Unauthorized<ParentLoginResponseDto>("Invalid mobile or password.");
                }

                // Verify this user has at least one ParentStudentMapping
                var hasMapping = await _context.ParentStudentMappings
                    .IgnoreQueryFilters()
                    .AnyAsync(m => m.ParentUserId == user.Id && !m.IsDeleted);

                if (!hasMapping)
                {
                    return Unauthorized<ParentLoginResponseDto>(
                        "No student records linked to this account. Please contact the school.");
                }

                // Reset failed attempts
                user.FailedLoginAttempts = 0;
                user.LastLoginDate = DateTime.Now;
                user.LastLoginIpAddress = model.IpAddress;
                user.LockoutEndDate = null;
                await _userManager.UpdateAsync(user);

                // Build JWT — no TenantId claim; children are loaded dynamically
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName ?? user.Email ?? user.PhoneNumber ?? ""),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.Role, "Parent"),
                    new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? "")
                };

                var authResponse = await _jwtAuthManager.Authenticate(user.Id, claims);

                // Fetch children across all tenants
                var children = await GetChildrenInternalAsync(user.Id);

                return new APIResponse<ParentLoginResponseDto>
                {
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Login successful",
                    Data = new ParentLoginResponseDto
                    {
                        ParentUserId = user.Id,
                        FullName = $"{user.FirstName} {user.LastName}".Trim(),
                        Mobile = user.PhoneNumber ?? "",
                        AccessToken = authResponse.AccessToken,
                        RefreshToken = authResponse.RefreshToken,
                        ExpireTime = authResponse.ExpireTime,
                        Children = children
                    }
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<ParentLoginResponseDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"Login failed: {ex.Message}"
                };
            }
        }

        // ── Children list ─────────────────────────────────────────────────────────

        public async Task<APIResponse<List<ChildSummaryDto>>> GetChildrenAsync(string parentUserId)
        {
            try
            {
                var children = await GetChildrenInternalAsync(parentUserId);
                return new APIResponse<List<ChildSummaryDto>>
                {
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Data = children
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<List<ChildSummaryDto>>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }
        }

        // ── Child dashboard ───────────────────────────────────────────────────────

        public async Task<APIResponse<ChildDashboardDto>> GetChildDashboardAsync(
            string parentUserId, int studentId, int schoolRegistrationId)
        {
            try
            {
                // Security: verify mapping exists for this parent
                var mapping = await _context.ParentStudentMappings
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(m =>
                        m.ParentUserId == parentUserId &&
                        m.StudentId == studentId &&
                        m.SchoolRegistrationId == schoolRegistrationId &&
                        !m.IsDeleted);

                if (mapping == null)
                {
                    return new APIResponse<ChildDashboardDto>
                    {
                        Success = false,
                        StatusCode = HttpStatusCode.Forbidden,
                        Message = "Access denied. Student not linked to your account."
                    };
                }

                // Load student details (ignore tenant filter — we already validated access)
                var student = await _context.Students
                    .IgnoreQueryFilters()
                    .Include(s => s.Class)
                    .FirstOrDefaultAsync(s => s.Id == studentId && s.SchoolRegistrationId == schoolRegistrationId);

                if (student == null)
                {
                    return new APIResponse<ChildDashboardDto>
                    {
                        Success = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Student not found."
                    };
                }

                // Load school name
                var school = await _context.SchoolRegistrations
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(s => s.Id == schoolRegistrationId);

                // Attendance snapshot (current academic year)
                var attendanceRecords = await _context.StudentAttendances
                    .IgnoreQueryFilters()
                    .Where(a => a.StudentId == studentId)
                    .ToListAsync();

                int totalDays = attendanceRecords.Count;
                int presentDays = attendanceRecords.Count(a => a.Status == "Present");
                double attendancePct = totalDays > 0
                    ? Math.Round((double)presentDays / totalDays * 100, 1)
                    : 0;

                // Fee snapshot
                var feeInstallments = await _context.FeeInstallments
                    .IgnoreQueryFilters()
                    .Where(f => f.StudentId == studentId)
                    .ToListAsync();

                decimal totalFee = feeInstallments.Sum(f => f.Amount);
                decimal paidFee = feeInstallments.Where(f => f.Status == "Paid").Sum(f => f.Amount);
                decimal pendingFee = totalFee - paidFee;

                return new APIResponse<ChildDashboardDto>
                {
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Data = new ChildDashboardDto
                    {
                        StudentId = student.Id,
                        StudentName = student.Name,
                        SchoolName = school?.SchoolName ?? "",
                        ClassName = student.Class?.Name,
                        Photo = student.Image,
                        EnrollmentNumber = student.EnrollmentNumber,
                        FathersName = student.FathersName,
                        DateOfBirth = student.DateOfBirth,
                        MobileNo = student.MobileNo1,
                        TotalAttendanceDays = totalDays,
                        PresentDays = presentDays,
                        AttendancePercentage = attendancePct,
                        TotalFee = totalFee,
                        PaidFee = paidFee,
                        PendingFee = pendingFee
                    }
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<ChildDashboardDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }
        }

        // ── Private helpers ───────────────────────────────────────────────────────

        private async Task<List<ChildSummaryDto>> GetChildrenInternalAsync(string parentUserId)
        {
            var mappings = await _context.ParentStudentMappings
                .IgnoreQueryFilters()
                .Where(m => m.ParentUserId == parentUserId && !m.IsDeleted)
                .ToListAsync();

            if (!mappings.Any()) return new List<ChildSummaryDto>();

            var studentIds = mappings.Select(m => m.StudentId).ToList();
            var schoolIds = mappings.Select(m => m.SchoolRegistrationId).Distinct().ToList();

            var students = await _context.Students
                .IgnoreQueryFilters()
                .Include(s => s.Class)
                .Where(s => studentIds.Contains(s.Id))
                .ToListAsync();

            var schools = await _context.SchoolRegistrations
                .IgnoreQueryFilters()
                .Where(s => schoolIds.Contains(s.Id))
                .ToListAsync();

            var result = new List<ChildSummaryDto>();
            foreach (var mapping in mappings)
            {
                var student = students.FirstOrDefault(s => s.Id == mapping.StudentId);
                var school = schools.FirstOrDefault(s => s.Id == mapping.SchoolRegistrationId);
                if (student == null) continue;

                result.Add(new ChildSummaryDto
                {
                    StudentId = student.Id,
                    StudentName = student.Name,
                    SchoolRegistrationId = mapping.SchoolRegistrationId,
                    SchoolName = school?.SchoolName ?? "",
                    ClassName = student.Class?.Name,
                    Photo = student.Image,
                    EnrollmentNumber = student.EnrollmentNumber,
                    Relationship = mapping.Relationship
                });
            }

            return result;
        }

        private static APIResponse<T> Unauthorized<T>(string message) =>
            new APIResponse<T>
            {
                Success = false,
                StatusCode = HttpStatusCode.Unauthorized,
                Message = message
            };
    }
}
