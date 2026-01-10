using AutoMapper;
using School.Domain.Website;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Website;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Website;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using School.Domain.Auth;
using School.Infrastructure;

namespace School.Services
{
    public class SchoolRegistrationService : ISchoolRegistrationService
    {
        private readonly ISchoolRegistrationRepository _schoolRegistrationRepository;
        private readonly IMapper _mapper;
        private readonly SchoolDbContext _context;
        private readonly PasswordHasher<ApplicationUser> _passwordHasher;

        public SchoolRegistrationService(
            ISchoolRegistrationRepository schoolRegistrationRepository,
            IMapper mapper,
            SchoolDbContext context)
        {
            _schoolRegistrationRepository = schoolRegistrationRepository;
            _mapper = mapper;
            _context = context;
            _passwordHasher = new PasswordHasher<ApplicationUser>();
        }

        public async Task<APIResponse<SchoolRegistrationDto>> AddSchoolRegistrationAsync(SchoolRegistrationModel model, string? ipAddress = null, string? userAgent = null)
        {
            // Check if email already exists
            var existsByEmail = await _schoolRegistrationRepository.ExistsByEmailAsync(model.PrincipalEmail);
            if (existsByEmail)
            {
                return new APIResponse<SchoolRegistrationDto>
                {
                    Success = false,
                    Message = string.Format(CommonResource.AlreadyExists, "School Registration", $"Email: {model.PrincipalEmail}"),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            // Check if mobile already exists
            var existsByMobile = await _schoolRegistrationRepository.ExistsByMobileAsync(model.PhoneNumber);
            if (existsByMobile)
            {
                return new APIResponse<SchoolRegistrationDto>
                {
                    Success = false,
                    Message = string.Format(CommonResource.AlreadyExists, "School Registration", $"Mobile: {model.PhoneNumber}"),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            // Map Model to Entity
            var entity = _mapper.Map<SchoolRegistration>(model);

            // Get or set StatusId - if not provided or invalid, find "Pending" status by name
            int statusId = model.StatusId;
            if (statusId <= 0)
            {
                var pendingStatus = await _context.Statuses
                    .FirstOrDefaultAsync(s => s.Name.ToLower() == "pending");
                if (pendingStatus != null)
                {
                    statusId = pendingStatus.Id;
                }
                else
                {
                    // Fallback: use first status if "Pending" doesn't exist
                    var firstStatus = await _context.Statuses.FirstOrDefaultAsync();
                    statusId = firstStatus?.Id ?? 2; // Default to StatusId = 2 (Pending)
                }
            }
            else
            {
                // Validate that the provided StatusId exists
                var statusExists = await _context.Statuses
                    .AnyAsync(s => s.Id == statusId);
                if (!statusExists)
                {
                    // If provided StatusId doesn't exist, find "Pending" status
                    var pendingStatus = await _context.Statuses
                        .FirstOrDefaultAsync(s => s.Name.ToLower() == "pending");
                    statusId = pendingStatus?.Id ?? 2;
                }
            }

            entity.StatusId = statusId;
            entity.SubmittedAt = DateTime.UtcNow;
            entity.CreatedDate = DateTime.UtcNow;

            // Hash password
            var dummyUser = new ApplicationUser(); // Just for password hashing
            entity.PasswordHash = _passwordHasher.HashPassword(dummyUser, model.Password);

            // Serialize facilities to JSON
            entity.FacilitiesJson = JsonSerializer.Serialize(model.Facilities);

            // Set tracking info
            entity.IpAddress = ipAddress;
            entity.UserAgent = userAgent;
            entity.CreatedBy = model.CreatedBy ?? "Public";
            entity.CreatedDate = DateTime.UtcNow;

            // Trim string fields
            entity.SchoolName = entity.SchoolName?.Trim() ?? "";
            entity.PrincipalName = entity.PrincipalName?.Trim() ?? "";
            entity.PrincipalEmail = entity.PrincipalEmail?.Trim().ToLower() ?? "";
            entity.Address = entity.Address?.Trim() ?? "";
            entity.City = entity.City?.Trim() ?? "";
            entity.State = entity.State?.Trim() ?? "";
            entity.Pincode = entity.Pincode?.Trim() ?? "";
            entity.PhoneNumber = entity.PhoneNumber?.Trim() ?? "";
            entity.AlternatePhone = entity.AlternatePhone?.Trim();
            entity.GovernmentRegistrationNumber = entity.GovernmentRegistrationNumber?.Trim();
            entity.SchoolWebsite = entity.SchoolWebsite?.Trim();
            entity.Description = entity.Description?.Trim();

            entity = await _schoolRegistrationRepository.AddSchoolRegistrationAsync(entity);

            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<SchoolRegistrationDto>
                {
                    Success = false,
                    Data = _mapper.Map<SchoolRegistrationDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, typeof(SchoolRegistration).Name, model.SchoolName),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            else if (entity != null && entity.Id > 0)
            {
                // Reload with Status navigation property
                var savedEntity = await _schoolRegistrationRepository.GetSchoolRegistrationByIdAsync(entity.Id);
                return new APIResponse<SchoolRegistrationDto>
                {
                    Success = true,
                    Data = MapToDto(savedEntity),
                    Message = "School registration submitted successfully. Your request is pending approval. We will contact you soon.",
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<SchoolRegistrationDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<SchoolRegistrationDto>> GetSchoolRegistrationByIdAsync(int id)
        {
            var result = await _schoolRegistrationRepository.GetSchoolRegistrationByIdAsync(id);

            if (result != null && result.Id > 0)
            {
                return new APIResponse<SchoolRegistrationDto>
                {
                    Data = MapToDto(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<SchoolRegistrationDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
        }

        public async Task<APIResponse<SchoolRegistrationDto>> GetSchoolRegistrationByIdAsync(string id)
        {
            if (int.TryParse(id, out int intId))
            {
                return await GetSchoolRegistrationByIdAsync(intId);
            }

            return new APIResponse<SchoolRegistrationDto>
            {
                Success = false,
                Message = "Invalid registration ID format",
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        public async Task<APIResponse<IEnumerable<SchoolRegistrationDto>>> GetAllSchoolRegistrationsAsync(string? status = null, int? pageNumber = null, int? pageSize = null)
        {
            var result = await _schoolRegistrationRepository.GetAllAsync(status, pageNumber, pageSize);

            if (result != null && result.Any())
            {
                var dtos = result.Select(MapToDto);
                return new APIResponse<IEnumerable<SchoolRegistrationDto>>
                {
                    Data = dtos,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<SchoolRegistrationDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<PagedResponse<SchoolRegistrationDto>> GetAllPagedAsync(int pageNumber = 1, int pageSize = 10, string? status = null)
        {
            var result = await _schoolRegistrationRepository.GetAllAsync(status, pageNumber, pageSize);
            var totalCount = await _schoolRegistrationRepository.GetTotalCountAsync(status);

            if (result != null && result.Any())
            {
                return new PagedResponse<SchoolRegistrationDto>
                {
                    Data = result.Select(MapToDto),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    TotalRecords = totalCount,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                };
            }
            else
            {
                return new PagedResponse<SchoolRegistrationDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> UpdateSchoolRegistrationAsync(SchoolRegistrationModel model)
        {
            if (!model.Id.HasValue || model.Id.Value <= 0)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Invalid registration ID",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            // Check if email already exists (excluding current record)
            var existsByEmail = await _schoolRegistrationRepository.ExistsByEmailAsync(model.PrincipalEmail, model.Id);
            if (existsByEmail)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = string.Format(CommonResource.AlreadyExists, "School Registration", $"Email: {model.PrincipalEmail}"),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            // Check if mobile already exists (excluding current record)
            var existsByMobile = await _schoolRegistrationRepository.ExistsByMobileAsync(model.PhoneNumber, model.Id);
            if (existsByMobile)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = string.Format(CommonResource.AlreadyExists, "School Registration", $"Mobile: {model.PhoneNumber}"),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            var existingEntity = await _schoolRegistrationRepository.GetSchoolRegistrationByIdAsync(model.Id.Value);
            if (existingEntity == null || existingEntity.Id == 0)
            {
                return new APIResponse
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            // Update properties from model
            existingEntity.SchoolName = model.SchoolName.Trim();
            existingEntity.SchoolType = model.SchoolType;
            existingEntity.EstablishmentYear = model.EstablishmentYear;
            existingEntity.PrincipalName = model.PrincipalName.Trim();
            existingEntity.PrincipalEmail = model.PrincipalEmail.Trim().ToLower();
            existingEntity.Address = model.Address.Trim();
            existingEntity.City = model.City.Trim();
            existingEntity.State = model.State.Trim();
            existingEntity.Pincode = model.Pincode.Trim();
            existingEntity.PhoneNumber = model.PhoneNumber.Trim();
            existingEntity.AlternatePhone = model.AlternatePhone?.Trim();
            existingEntity.BoardAffiliation = model.BoardAffiliation;
            existingEntity.GovernmentRegistrationNumber = model.GovernmentRegistrationNumber?.Trim();
            existingEntity.SchoolWebsite = model.SchoolWebsite?.Trim();
            existingEntity.FacilitiesJson = JsonSerializer.Serialize(model.Facilities);
            existingEntity.Description = model.Description?.Trim();
            existingEntity.TermsAccepted = model.TermsAccepted;
            existingEntity.UpdatedBy = model.UpdatedBy;
            existingEntity.UpdatedDate = DateTime.UtcNow;

            // Update password if provided
            if (!string.IsNullOrEmpty(model.Password))
            {
                var dummyUser = new ApplicationUser();
                existingEntity.PasswordHash = _passwordHasher.HashPassword(dummyUser, model.Password);
            }

            // Update StatusId if provided and valid
            if (model.StatusId > 0)
            {
                var statusExists = await _context.Statuses.AnyAsync(s => s.Id == model.StatusId);
                if (statusExists)
                {
                    existingEntity.StatusId = model.StatusId;
                }
            }

            var result = await _schoolRegistrationRepository.UpdateSchoolRegistrationAsync(existingEntity);
            if (result > 0)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Message = CommonResource.UpdateSuccess
                };
            }
            else
            {
                return new APIResponse
                {
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
        }

        public async Task<APIResponse> UpdateSchoolRegistrationStatusAsync(int id, int statusId, string? approvedBy = null, string? rejectionReason = null)
        {
            var result = await _schoolRegistrationRepository.UpdateSchoolRegistrationStatusAsync(id, statusId, approvedBy, rejectionReason);
            if (result > 0)
            {
                var newStatus = await _context.Statuses.FirstOrDefaultAsync(s => s.Id == statusId);
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Message = $"School registration status updated to {newStatus?.Name ?? "new status"} successfully"
                };
            }
            else
            {
                return new APIResponse
                {
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
        }

        public async Task<APIResponse> DeleteSchoolRegistrationAsync(int id)
        {
            int changes = await _schoolRegistrationRepository.DeleteSchoolRegistrationAsync(id);
            if (changes > 0)
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.DeleteSuccess,
                    StatusCode = HttpStatusCode.OK,
                };
            else
                return new APIResponse
                {
                    Message = CommonResource.DeleteFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
        }

        public async Task<APIResponse<bool>> CheckEmailExistsAsync(string email)
        {
            try
            {
                var exists = await _schoolRegistrationRepository.ExistsByEmailAsync(email);
                return new APIResponse<bool>
                {
                    Success = true,
                    Message = exists ? "Email already exists" : "Email is available",
                    StatusCode = HttpStatusCode.OK,
                    Data = exists
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<bool>
                {
                    Success = false,
                    Message = $"Failed to check email: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = false
                };
            }
        }

        public async Task<APIResponse<bool>> CheckMobileExistsAsync(string mobile)
        {
            try
            {
                var exists = await _schoolRegistrationRepository.ExistsByMobileAsync(mobile);
                return new APIResponse<bool>
                {
                    Success = true,
                    Message = exists ? "Mobile number already exists" : "Mobile number is available",
                    StatusCode = HttpStatusCode.OK,
                    Data = exists
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<bool>
                {
                    Success = false,
                    Message = $"Failed to check mobile: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = false
                };
            }
        }

        public async Task<APIResponse<int>> GetSchoolRegistrationCountAsync(string? status = null)
        {
            try
            {
                var count = await _schoolRegistrationRepository.GetTotalCountAsync(status);
                return new APIResponse<int>
                {
                    Success = true,
                    Message = CommonResource.FetchSuccess,
                    StatusCode = HttpStatusCode.OK,
                    Data = count
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<int>
                {
                    Success = false,
                    Message = $"Failed to get count: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        private SchoolRegistrationDto MapToDto(SchoolRegistration entity)
        {
            var dto = _mapper.Map<SchoolRegistrationDto>(entity);
            
            // Map StatusName from navigation property
            dto.StatusName = entity.Status?.Name;

            // Deserialize FacilitiesJson to Facilities object
            if (!string.IsNullOrEmpty(entity.FacilitiesJson))
            {
                try
                {
                    dto.Facilities = JsonSerializer.Deserialize<FacilitiesDto>(entity.FacilitiesJson) ?? new FacilitiesDto();
                }
                catch
                {
                    dto.Facilities = new FacilitiesDto();
                }
            }

            return dto;
        }
    }
}
