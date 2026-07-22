using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using School.Domain.Auth;
using School.Domain.Hr;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Common;
using School_DTOs.Hr;

namespace School.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<EmployeeService> logger,
            UserManager<ApplicationUser> userManager,
            IEmailService emailService)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<APIResponse<EmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto model, string username)
        {
            try
            {
                if (await _employeeRepository.IsDuplicateEmployeeAsync(model.Email, model.MobileNumber, model.AadhaarNumber, model.PANNumber))
                {
                    return new APIResponse<EmployeeDto> { Success = false, StatusCode = HttpStatusCode.Conflict, Message = "Employee with same Email, Mobile, Aadhaar or PAN already exists." };
                }

                var entity = _mapper.Map<Employee>(model);
                entity.CreatedBy = username;

                // Auto generate Employee Code logic here
                string generatedCode = "EMP" + DateTime.Now.ToString("yyyyMMddHHmmss");
                entity.EmployeeCode = generatedCode;

                entity.EmployeeDetail = new EmployeeDetail
                {
                    FatherName = model.FatherName,
                    MotherName = model.MotherName,
                    PinCode = model.PinCode,
                    AadhaarNumber = model.AadhaarNumber,
                    PANNumber = model.PANNumber,
                    Address = model.Address,
                    City = model.City,
                    State = model.State,
                    Country = model.Country,
                    CreatedBy = username,
                    CreatedDate = DateTime.Now
                };

                await _employeeRepository.AddAsync(entity);
                await _unitOfWork.CommitAsync();

                // Identity Auto-Creation
                var user = new ApplicationUser
                {
                    UserName = generatedCode,
                    Email = model.Email,
                    PhoneNumber = model.MobileNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SchoolRegistrationId = entity.SchoolRegistrationId
                };

                // BUG-005 FIX: Generate a secure random password meeting Identity policy (12+ chars, upper, lower, digit, special)
                var tempPassword = GenerateSecurePassword();
                var createResult = await _userManager.CreateAsync(user, tempPassword);
                if (createResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                    entity.ApplicationUserId = user.Id;
                    _employeeRepository.Update(entity);
                    await _unitOfWork.CommitAsync();

                    // Send Welcome Email with generated credentials
                    await _emailService.SendWelcomeEmailAsync(model.Email, generatedCode, tempPassword, "Approved");
                }
                else
                {
                    _logger.LogWarning("Failed to create Identity User for Employee {EmployeeCode}. Errors: {Errors}", generatedCode, string.Join(", ", createResult.Errors.Select(e => e.Description)));
                    // We don't rollback the employee creation, they just won't have a login. Admin can create it later.
                }

                return new APIResponse<EmployeeDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<EmployeeDto>(entity) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                return new APIResponse<EmployeeDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<EmployeeDto>> UpdateEmployeeAsync(UpdateEmployeeDto model, string username)
        {
            try
            {
                var existingEntity = await _employeeRepository.GetEmployeeWithDetailsAsync(model.Id);
                if (existingEntity == null)
                {
                    return new APIResponse<EmployeeDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Employee not found." };
                }

                if (await _employeeRepository.IsDuplicateEmployeeAsync(model.Email, model.MobileNumber, model.AadhaarNumber, model.PANNumber, model.Id))
                {
                    return new APIResponse<EmployeeDto> { Success = false, StatusCode = HttpStatusCode.Conflict, Message = "Employee with same Email, Mobile, Aadhaar or PAN already exists." };
                }

                _mapper.Map(model, existingEntity);
                existingEntity.UpdatedBy = username;
                existingEntity.UpdatedDate = DateTime.Now;

                if (existingEntity.EmployeeDetail == null)
                {
                    existingEntity.EmployeeDetail = new EmployeeDetail
                    {
                        FatherName = model.FatherName,
                        MotherName = model.MotherName,
                        PinCode = model.PinCode,
                        AadhaarNumber = model.AadhaarNumber,
                        PANNumber = model.PANNumber,
                        Address = model.Address,
                        City = model.City,
                        State = model.State,
                        Country = model.Country,
                        CreatedBy = username,
                        CreatedDate = DateTime.Now
                    };
                }
                else
                {
                    existingEntity.EmployeeDetail.FatherName = model.FatherName;
                    existingEntity.EmployeeDetail.MotherName = model.MotherName;
                    existingEntity.EmployeeDetail.PinCode = model.PinCode;
                    existingEntity.EmployeeDetail.AadhaarNumber = model.AadhaarNumber;
                    existingEntity.EmployeeDetail.PANNumber = model.PANNumber;
                    existingEntity.EmployeeDetail.Address = model.Address;
                    existingEntity.EmployeeDetail.City = model.City;
                    existingEntity.EmployeeDetail.State = model.State;
                    existingEntity.EmployeeDetail.Country = model.Country;
                    existingEntity.EmployeeDetail.UpdatedBy = username;
                    existingEntity.EmployeeDetail.UpdatedDate = DateTime.Now;
                }

                _employeeRepository.Update(existingEntity);
                await _unitOfWork.CommitAsync();

                return new APIResponse<EmployeeDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<EmployeeDto>(existingEntity) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee");
                return new APIResponse<EmployeeDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<EmployeeDto>> GetEmployeeByIdAsync(int id)
        {
            var entity = await _employeeRepository.GetEmployeeWithDetailsAsync(id);
            if (entity == null)
                return new APIResponse<EmployeeDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Employee not found." };

            return new APIResponse<EmployeeDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<EmployeeDto>(entity) };
        }

        public async Task<APIResponse<PagedResponse<EmployeeDto>>> GetAllEmployeesAsync(PaginationFilterDto filter)
        {
            var query = _employeeRepository.List();

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                var lowerSearch = filter.SearchText.ToLower();
                query = query.Where(e => e.FirstName.ToLower().Contains(lowerSearch) ||
                                         e.LastName.ToLower().Contains(lowerSearch) ||
                                         e.EmployeeCode.ToLower().Contains(lowerSearch) ||
                                         e.Email.ToLower().Contains(lowerSearch));
            }

            // BUG-001 FIX: safe column map — EF.Property<object> crashes on navigation properties
            bool isDesc = filter.SortDirection?.ToLower() == "desc";
            query = (filter.SortBy?.ToLower()) switch
            {
                "firstname" => isDesc ? query.OrderByDescending(e => e.FirstName) : query.OrderBy(e => e.FirstName),
                "lastname" => isDesc ? query.OrderByDescending(e => e.LastName) : query.OrderBy(e => e.LastName),
                "employeecode" => isDesc ? query.OrderByDescending(e => e.EmployeeCode) : query.OrderBy(e => e.EmployeeCode),
                "email" => isDesc ? query.OrderByDescending(e => e.Email) : query.OrderBy(e => e.Email),
                "mobilenumber" => isDesc ? query.OrderByDescending(e => e.MobileNumber) : query.OrderBy(e => e.MobileNumber),
                "status" => isDesc ? query.OrderByDescending(e => e.Status) : query.OrderBy(e => e.Status),
                "joiningdate" => isDesc ? query.OrderByDescending(e => e.JoiningDate) : query.OrderBy(e => e.JoiningDate),
                "createddate" => isDesc ? query.OrderByDescending(e => e.CreatedDate) : query.OrderBy(e => e.CreatedDate),
                "designationname" or "designation" =>
                    isDesc ? query.OrderByDescending(e => e.Designation != null ? e.Designation.Name : "")
                           : query.OrderBy(e => e.Designation != null ? e.Designation.Name : ""),
                "departmentname" or "department" =>
                    isDesc ? query.OrderByDescending(e => e.Department != null ? e.Department.Name : "")
                           : query.OrderBy(e => e.Department != null ? e.Department.Name : ""),
                _ => query.OrderByDescending(e => e.Id)
            };

            // BUG-007 FIX: count before Include to avoid unnecessary join in COUNT query
            var totalRecords = await query.CountAsync();

            // BUG-007 FIX: Include only after pagination to prevent N+1
            var entities = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .ToListAsync();

            var pagedResponse = new PagedResponse<EmployeeDto>
            {
                Data = _mapper.Map<IEnumerable<EmployeeDto>>(entities),
                CurrentPage = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalRecords = totalRecords,

            };

            return new APIResponse<PagedResponse<EmployeeDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = pagedResponse };
        }

        public async Task<APIResponse<bool>> DeleteEmployeeAsync(int id, string username)
        {
            var entity = await _employeeRepository.GetByIdAsync(id);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Employee not found." };

            entity.IsDeleted = true;
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.Now;

            _employeeRepository.Update(entity);
            await _unitOfWork.CommitAsync();

            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true, Message = "Employee deleted successfully." };
        }

        public async Task<APIResponse<bool>> ToggleEmployeeStatusAsync(int id, string username)
        {
            var entity = await _employeeRepository.GetByIdAsync(id);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Employee not found." };

            entity.Status = entity.Status == "active" ? "inactive" : "active";
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.Now;

            _employeeRepository.Update(entity);
            await _unitOfWork.CommitAsync();

            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true };
        }

        public async Task<APIResponse<int>> BulkDeleteAsync(IEnumerable<int> ids, string username)
        {
            int updatedCount = await _employeeRepository.BulkDeleteAsync(ids, username);
            return new APIResponse<int> { Success = true, StatusCode = HttpStatusCode.OK, Data = updatedCount, Message = $"{updatedCount} records deleted." };
        }

        public async Task<APIResponse<int>> BulkRestoreAsync(IEnumerable<int> ids, string username)
        {
            int updatedCount = await _employeeRepository.BulkRestoreAsync(ids, username);
            return new APIResponse<int> { Success = true, StatusCode = HttpStatusCode.OK, Data = updatedCount, Message = $"{updatedCount} records restored." };
        }

        public async Task<APIResponse<int>> BulkStatusUpdateAsync(IEnumerable<int> ids, string status, string username)
        {
            int updatedCount = await _employeeRepository.BulkStatusUpdateAsync(ids, status, username);
            return new APIResponse<int> { Success = true, StatusCode = HttpStatusCode.OK, Data = updatedCount, Message = $"{updatedCount} records updated." };
        }

        // ─── Private Helpers ───────────────────────────────────────────────────────

        /// <summary>
        /// Generates a cryptographically secure temporary password satisfying ASP.NET Identity policy:
        /// minimum 12 characters, at least one uppercase, lowercase, digit, and special character.
        /// </summary>
        private static string GenerateSecurePassword()
        {
            const string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            const string lower = "abcdefghjkmnpqrstuvwxyz";
            const string digits = "23456789";
            const string special = "!@#$%^&*";
            const string all = upper + lower + digits + special;

            var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var bytes = new byte[18];
            rng.GetBytes(bytes);

            // Guarantee at least one character from each required set
            var pwd = new System.Text.StringBuilder();
            pwd.Append(upper[bytes[0] % upper.Length]);
            pwd.Append(lower[bytes[1] % lower.Length]);
            pwd.Append(digits[bytes[2] % digits.Length]);
            pwd.Append(special[bytes[3] % special.Length]);

            // Fill remaining 8 characters from the full set
            for (int i = 4; i < 14; i++)
                pwd.Append(all[bytes[i] % all.Length]);

            // Fisher-Yates shuffle using fresh random bytes
            rng.GetBytes(bytes);
            var arr = pwd.ToString().ToCharArray();
            for (int i = arr.Length - 1; i > 0; i--)
            {
                int j = bytes[i % bytes.Length] % (i + 1);
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }

            return new string(arr);
        }
    }
}

