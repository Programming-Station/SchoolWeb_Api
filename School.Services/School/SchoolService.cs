using AutoMapper;
using Microsoft.AspNetCore.Identity;
using School.Domain.Auth;
using School.Domain.School;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.School;
using School.Services.School.ISchoolServices;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace School.Services.School
{
    public class SchoolService:ISchoolService
    {
        private readonly ISchoolRepository _schoolRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISchoolOwnerRepository _schoolOwnerRepo;
        private readonly ISchoolSubscriptionRepository _schoolSubscriptionRepo;

        public SchoolService(
            ISchoolRepository schoolRepo, 
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ISchoolOwnerRepository schoolOwnerRepo,
            ISchoolSubscriptionRepository schoolSubscriptionRepo)
        {
            _schoolRepo = schoolRepo;
            _mapper = mapper;  
            _userManager = userManager;
            _roleManager = roleManager;
            _schoolOwnerRepo = schoolOwnerRepo;
            _schoolSubscriptionRepo = schoolSubscriptionRepo;
        }

        public async Task<APIResponse<SchoolRegistrationDto>> AddAsync(SchoolRegistrationModel model)
        {
            try
            {
                var entity = _mapper.Map<SchoolRegistration>(model);
                entity = await _schoolRepo.AddSchoolAsync(entity);
                if (entity != null && entity.Id == 0)
                {
                    return new APIResponse<SchoolRegistrationDto>
                    {
                        Data = _mapper.Map<SchoolRegistrationDto>(entity),
                        Message = string.Format(CommonResource.AlreadyExistsRecord, nameof(SchoolRegistration), nameof(model.SchoolName), model.SchoolName),
                        StatusCode = HttpStatusCode.Forbidden,
                    };
                }
                else if (entity != null && entity.Id > 0)
                {
                    // Create ApplicationUser
                    var user = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FirstName = string.IsNullOrWhiteSpace(model.ContactPersonName) ? "School Admin" : model.ContactPersonName,
                        LastName = string.Empty,
                        IsDefaultPassword = true,
                        PasswordUpdatedOn = null
                    };

                    var result = await _userManager.CreateAsync(user, "SchoolAdmin@123");
                    if (result.Succeeded)
                    {
                        // Ensure role exists
                        if (!await _roleManager.RoleExistsAsync("SchoolAdmin"))
                        {
                            await _roleManager.CreateAsync(new IdentityRole("SchoolAdmin"));
                        }
                        await _userManager.AddToRoleAsync(user, "SchoolAdmin");

                        // Create SchoolOwner
                        var owner = new SchoolOwner
                        {
                            SchoolRegistrationId = entity.Id,
                            ApplicationUserId = user.Id,
                            StatusId = 1, // Assuming 1 is Active status ID
                            EmailVerified = false,
                            MobileVerified = false,
                            IsLocked = false
                        };
                        await _schoolOwnerRepo.AddAsync(owner);

                        // Create SchoolSubscription (Default 1-month trial)
                        var subscription = new SchoolSubscription
                        {
                            SchoolRegistrationId = entity.Id,
                            SubscriptionPlanId = 1, // Default Plan
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddMonths(1),
                            AmountPaid = 0,
                            PaymentStatus = "Free",
                            IsActive = true
                        };
                        await _schoolSubscriptionRepo.AddAsync(subscription);
                    }

                    return new APIResponse<SchoolRegistrationDto>
                    {
                        Data = _mapper.Map<SchoolRegistrationDto>(entity),
                        Message = CommonResource.AddSuccess,
                        StatusCode = HttpStatusCode.Created,
                    };
                }
                else
                {
                    return new APIResponse<SchoolRegistrationDto>
                    {
                        Message = CommonResource.AddFailed,
                        Success = false,
                        StatusCode = HttpStatusCode.Forbidden
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse<SchoolRegistrationDto>
                {
                    Message = CommonResource.PleaseTryAgain,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Error = new APIException(ex.Message, ex.InnerException)
                };
            }
        }

        public async Task<APIResponse> DeleteAsync(int Id)
        {
            APIResponse apiResponse = new APIResponse();
            try
            {
                int chanes = await _schoolRepo.DeleteSchoolAsync(Id);
                if (chanes > 0)
                {
                    apiResponse.Success = true;
                    apiResponse.Message = CommonResource.DeleteSuccess;
                    apiResponse.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    apiResponse.Success = false;
                    apiResponse.Message = CommonResource.DeleteFailed;
                    apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                }
                return apiResponse;
            }
            catch(Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Error = new APIException(ex.Message, ex.InnerException);
                apiResponse.Message = CommonResource.DeleteFailed;
                apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                return apiResponse;
            }
        }

        public async Task<APIResponse<SchoolRegistrationDto>> EditAsync(SchoolRegistrationModel model)
        {
            try
            {
                var entity = _mapper.Map<SchoolRegistration>(model);
                int changes = await _schoolRepo.UpdateSchoolAsync(entity);
                if (changes > 0)
                {
                    return new APIResponse<SchoolRegistrationDto>
                    {
                        Data = _mapper.Map<SchoolRegistrationDto>(model),
                        Success = true,
                        Message= CommonResource.UpdateSuccess,
                        StatusCode = HttpStatusCode.OK,
                    };
                }
                else
                {
                    return new APIResponse<SchoolRegistrationDto>
                    {
                        Message = CommonResource.UpdateFailed,
                        StatusCode = HttpStatusCode.OK,
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse<SchoolRegistrationDto>
                {
                    Error = new APIException(ex.Message, ex.InnerException),
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<PagedResponse<SchoolRegistrationDto>> GetAllsAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, bool? isActive = null)
        {
            try
            {
                var query = _schoolRepo.GetAllSchoolsQueryable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(s => 
                        s.SchoolName.ToLower().Contains(searchTerm) || 
                        s.SchoolCode.ToLower().Contains(searchTerm) ||
                        s.Address.ToLower().Contains(searchTerm)
                    );
                }

                if (isActive.HasValue)
                {
                    query = query.Where(s => s.IsActive == isActive.Value);
                }

                var totalCount = await query.CountAsync();

                var items = await query
                    .OrderByDescending(s => s.CreatedDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<IEnumerable<SchoolRegistrationDto>>(items);

                return new PagedResponse<SchoolRegistrationDto>
                {
                    Success = true,
                    Message = "Schools fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dtos,
                    TotalRecords = totalCount,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                return new PagedResponse<SchoolRegistrationDto>
                {
                    Success = false,
                    Message = $"Failed to get schools: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = new List<SchoolRegistrationDto>()
                };
            }
        }

        public async Task<APIResponse<SchoolRegistrationDto>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _schoolRepo.GetSchoolByIdAsync(id);
                if (entity == null || entity.Id == 0)
                {
                    return new APIResponse<SchoolRegistrationDto>
                    {
                        Success = false,
                        Message = "School not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                return new APIResponse<SchoolRegistrationDto>
                {
                    Success = true,
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Data = _mapper.Map<SchoolRegistrationDto>(entity)
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<SchoolRegistrationDto>
                {
                    Success = false,
                    Message = $"Failed to get school: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
