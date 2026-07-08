using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using School.Domain;
using School.Domain.Auth;
using School.Domain.Student;
using School.Infrastructure;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces;
using School.Utilities.Enums;
using School_DTOs;

namespace School.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IAdmissionApplicationRepository _applicationRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SchoolDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EnrollmentService(
            IAdmissionApplicationRepository applicationRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SchoolDbContext context,
            IEmailService emailService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponse> EnrollStudentAsync(int applicationId, string username)
        {
            try
            {
                var application = await _applicationRepository.GetByIdAsync(applicationId);
                if (application == null || application.Id == 0)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Admission Application not found.",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                if (application.Status == "Enrolled")
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Student is already enrolled.",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                // 1. Resolve generated codes (AdmissionNo, EnrollmentNo, StudentCode) based on dynamic configurator templates
                string yearStr = DateTime.Now.Year.ToString();
                string admPrefix = $"ADM-{yearStr}-";
                string enrPrefix = $"ENR-{yearStr}-";
                string stdPrefix = $"STU-{yearStr}-";

                var config = await _context.AdmissionFormConfigs
                    .FirstOrDefaultAsync(c => c.CampusId == application.CampusId && 
                                              c.EducationLevelId == application.EducationLevelId && 
                                              (c.ProgramId == application.ProgramId || c.ProgramId == null));

                if (config != null && !string.IsNullOrEmpty(config.AutoGenRulesJson))
                {
                    try
                    {
                        var ruleObj = JsonSerializer.Deserialize<Dictionary<string, string>>(config.AutoGenRulesJson);
                        if (ruleObj != null)
                        {
                            if (ruleObj.TryGetValue("admissionNoPrefix", out string ap) && !string.IsNullOrEmpty(ap))
                                admPrefix = ap;
                            if (ruleObj.TryGetValue("enrollmentNoPrefix", out string ep) && !string.IsNullOrEmpty(ep))
                                enrPrefix = ep;
                            if (ruleObj.TryGetValue("studentCodePrefix", out string sp) && !string.IsNullOrEmpty(sp))
                                stdPrefix = sp;
                        }
                    }
                    catch { /* Fallback to defaults */ }
                }

                if (string.IsNullOrEmpty(application.AdmissionNo))
                {
                    string lastAdm = await _applicationRepository.GetLastAdmissionNoAsync(admPrefix);
                    int admSeq = 1;
                    if (!string.IsNullOrEmpty(lastAdm) && lastAdm.Length > admPrefix.Length)
                    {
                        if (int.TryParse(lastAdm.Substring(admPrefix.Length), out int seq))
                            admSeq = seq + 1;
                    }
                    application.AdmissionNo = $"{admPrefix}{admSeq:D4}";
                }

                if (string.IsNullOrEmpty(application.EnrollmentNo))
                {
                    string lastEnr = await _applicationRepository.GetLastEnrollmentNoAsync(enrPrefix);
                    int enrSeq = 1;
                    if (!string.IsNullOrEmpty(lastEnr) && lastEnr.Length > enrPrefix.Length)
                    {
                        if (int.TryParse(lastEnr.Substring(enrPrefix.Length), out int seq))
                            enrSeq = seq + 1;
                    }
                    application.EnrollmentNo = $"{enrPrefix}{enrSeq:D4}";
                }

                if (string.IsNullOrEmpty(application.StudentCode))
                {
                    string lastStd = await _applicationRepository.GetLastStudentCodeAsync(stdPrefix);
                    int stdSeq = 1;
                    if (!string.IsNullOrEmpty(lastStd) && lastStd.Length > stdPrefix.Length)
                    {
                        if (int.TryParse(lastStd.Substring(stdPrefix.Length), out int seq))
                            stdSeq = seq + 1;
                    }
                    application.StudentCode = $"{stdPrefix}{stdSeq:D4}";
                }

                // Generate secure temporary passwords
                string studentPassword = GenerateSecureRandomPassword();
                string parentPassword = GenerateSecureRandomPassword();

                // 2. Identity User creation for Student
                string studentUsername = application.StudentCode;
                var studentUser = new ApplicationUser
                {
                    UserName = studentUsername,
                    Email = application.Email ?? $"{studentUsername.ToLower()}@school.com",
                    PhoneNumber = application.Mobile,
                    FirstName = application.FullName.Split(' ').FirstOrDefault() ?? application.FullName,
                    LastName = application.FullName.Contains(' ') ? application.FullName.Split(' ').Last() : string.Empty,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SchoolRegistrationId = application.SchoolRegistrationId,
                    IsActive = true,
                    IsDefaultPassword = true
                };

                var createStudentResult = await _userManager.CreateAsync(studentUser, studentPassword);
                if (!createStudentResult.Succeeded)
                {
                    string errorMsg = string.Join(", ", createStudentResult.Errors.Select(e => e.Description));
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"Failed to create student user login: {errorMsg}",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                // Ensure "Student" role exists and assign
                if (!await _roleManager.RoleExistsAsync("Student"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Student"));
                }
                await _userManager.AddToRoleAsync(studentUser, "Student");

                // 3. Identity User creation for Parent
                string parentUsername = $"p.{application.StudentCode}";

                // Use normalized columns directly — no JSON parsing needed
                string parentEmail = application.Email ?? $"parent.{studentUsername.ToLower()}@school.com";
                string parentPhone = application.GuardianMobile ?? application.Mobile;
                string fatherName = application.FathersName ?? application.GuardianName ?? "Parent/Guardian";

                var parentUser = new ApplicationUser
                {
                    UserName = parentUsername,
                    Email = parentEmail,
                    PhoneNumber = parentPhone,
                    FirstName = fatherName.Split(' ').FirstOrDefault() ?? fatherName,
                    LastName = fatherName.Contains(' ') ? fatherName.Split(' ').Last() : string.Empty,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SchoolRegistrationId = application.SchoolRegistrationId,
                    IsActive = true,
                    IsDefaultPassword = true
                };

                var createParentResult = await _userManager.CreateAsync(parentUser, parentPassword);
                if (!createParentResult.Succeeded)
                {
                    // Rollback student user
                    await _userManager.DeleteAsync(studentUser);
                    string errorMsg = string.Join(", ", createParentResult.Errors.Select(e => e.Description));
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"Failed to create parent user login: {errorMsg}",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                // Ensure "Parent" role exists and assign
                if (!await _roleManager.RoleExistsAsync("Parent"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Parent"));
                }
                await _userManager.AddToRoleAsync(parentUser, "Parent");

                // 4. Map application into active Student record
                var newStudent = new Student
                {
                    StudentId = application.StudentCode,
                    EnrollmentNumber = application.EnrollmentNo,
                    ApplicationUserId = studentUser.Id,
                    CourseType = application.Program?.Code ?? "School",
                    CourseId = application.CourseId ?? 0,
                    CourseOpted = application.Program?.Name,
                    Name = application.FullName,
                    FathersName = fatherName,
                    Gender = application.Gender,
                    DateOfBirth = application.DateOfBirth.ToString("yyyy-MM-dd"),
                    MobileNo1 = application.Mobile,
                    MobileNo2 = parentPhone,
                    ClassId = application.ClassId,
                    StatusId = (int)DefaultStatus.Active,
                    SchoolRegistrationId = application.SchoolRegistrationId,
                    CreatedBy = username,
                    CreatedDate = DateTime.Now
                };

                // Copy address details directly from normalized columns
                newStudent.PostalAddress = application.PermanentAddress;
                newStudent.PinCode = application.PermanentPinCode;

                await _context.Students.AddAsync(newStudent);

                // 5. Update application with foreign user keys and statuses
                application.StudentUserId = studentUser.Id;
                application.ParentUserId = parentUser.Id;
                application.Status = "Enrolled";

                await _applicationRepository.UpdateAsync(application);

                await _applicationRepository.AddAuditLogAsync(new AdmissionAuditLog
                {
                    AdmissionApplicationId = application.Id,
                    Action = "Enrolled",
                    StatusFrom = "Approved",
                    StatusTo = "Enrolled",
                    PerformedBy = username,
                    PerformedDate = DateTime.Now,
                    DetailsJson = JsonSerializer.Serialize(new
                    {
                        StudentUser = studentUsername,
                        ParentUser = parentUsername
                    })
                });

                // 6. Send transactional notification emails using the template resolver
                var studentPlaceholders = new Dictionary<string, string>
                {
                    { "UserName", application.FullName },
                    { "EmployeeCode", studentUsername }, // Re-using standard variables from default templates
                    { "Password", studentPassword },
                    { "Status", "Student Account Seeding" },
                    { "CurrentDate", DateTime.Now.ToString("dd MMM yyyy") }
                };
                await _emailService.SendTemplateAsync(studentUser.Email, "Employee Account Created", studentPlaceholders);

                var parentPlaceholders = new Dictionary<string, string>
                {
                    { "UserName", fatherName },
                    { "EmployeeCode", parentUsername },
                    { "Password", parentPassword },
                    { "Status", "Parent Account Seeding" },
                    { "CurrentDate", DateTime.Now.ToString("dd MMM yyyy") }
                };
                await _emailService.SendTemplateAsync(parentUser.Email, "Employee Account Created", parentPlaceholders);

                return new APIResponse
                {
                    Success = true,
                    Message = "Student enrolled successfully and credential notification emails dispatched.",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Enrollment failed: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        private string GenerateSecureRandomPassword()
        {
            var uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var lowercase = "abcdefghijklmnopqrstuvwxyz";
            var digits = "0123456789";
            var special = "!@#$%^&*()_+-=[]{}|;:,.<>?";
            var random = new Random();

            var chars = new char[12];
            chars[0] = uppercase[random.Next(uppercase.Length)];
            chars[1] = lowercase[random.Next(lowercase.Length)];
            chars[2] = digits[random.Next(digits.Length)];
            chars[3] = special[random.Next(special.Length)];

            var allChars = uppercase + lowercase + digits + special;
            for (int i = 4; i < 12; i++)
            {
                chars[i] = allChars[random.Next(allChars.Length)];
            }

            return new string(chars.OrderBy(x => random.Next()).ToArray());
        }
    }
}
