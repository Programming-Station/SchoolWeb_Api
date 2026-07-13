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

using School_DTOs.Hr;
using School_DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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

                var createResult = await _userManager.CreateAsync(user, "School@123");
                if (createResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                    entity.ApplicationUserId = user.Id;
                    _employeeRepository.Update(entity);
                    await _unitOfWork.CommitAsync();

                    // Send Welcome Email using template
                    await _emailService.SendWelcomeEmailAsync(model.Email, generatedCode, "School@123", "Approved");
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

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                bool isDesc = filter.SortDirection?.ToLower() == "desc";
                query = isDesc 
                    ? query.OrderByDescending(e => EF.Property<object>(e, filter.SortBy))
                    : query.OrderBy(e => EF.Property<object>(e, filter.SortBy));
            }
            else
            {
                query = query.OrderByDescending(e => e.Id);
            }

            var totalRecords = await query.CountAsync();
            var entities = await query
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
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
    }
}
