using AutoMapper;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.AffiliationCollege;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.AffiliationCollege;
using Microsoft.EntityFrameworkCore;
using System.Net;
using School.Infrastructure;

namespace School.Services
{
    public class AffiliatedService : IAffiliatedService
    {
        private readonly IAffiliatedRepository _affiliatedRepository;
        private readonly IMapper _mapper;
        private readonly SchoolDbContext _context;

        public AffiliatedService(IAffiliatedRepository affiliatedRepository, IMapper mapper, SchoolDbContext context)
        {
            _affiliatedRepository = affiliatedRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<APIResponse<AffiliatedDto>> AddAffiliationCollegeAsync(AffiliatedModel model)
        {
            // Validate StateId is provided and valid
            if (model.StateId <= 0)
            {
                return new APIResponse<AffiliatedDto>
                {
                    Success = false,
                    Message = "StateId is required and must be greater than 0",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            // Validate State exists and is active
            var state = await _context.States
                .FirstOrDefaultAsync(s => s.Id == model.StateId && !s.IsDeleted);

            if (state == null)
            {
                return new APIResponse<AffiliatedDto>
                {
                    Success = false,
                    Message = $"State with ID {model.StateId} not found or has been deleted",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            if (!state.IsActive)
            {
                return new APIResponse<AffiliatedDto>
                {
                    Success = false,
                    Message = $"State '{state.Name}' is inactive. Please select an active state.",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            // Validate CityId is provided and valid
            if (model.CityId <= 0)
            {
                return new APIResponse<AffiliatedDto>
                {
                    Success = false,
                    Message = "CityId is required and must be greater than 0",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            // Validate City exists and belongs to State
            var city = await _context.Cities
                .FirstOrDefaultAsync(c => c.Id == model.CityId && !c.IsDeleted);

            if (city == null)
            {
                return new APIResponse<AffiliatedDto>
                {
                    Success = false,
                    Message = $"City with ID {model.CityId} not found or has been deleted",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            if (city.StateId != model.StateId)
            {
                return new APIResponse<AffiliatedDto>
                {
                    Success = false,
                    Message = $"City '{city.Name}' does not belong to selected State '{state.Name}'",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            if (!city.IsActive)
            {
                return new APIResponse<AffiliatedDto>
                {
                    Success = false,
                    Message = $"City '{city.Name}' is inactive. Please select an active city.",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            // Check for duplicate CollegeCode (if provided)
            if (!string.IsNullOrEmpty(model.CollegeCode))
            {
                var existingCode = await _context.Affiliateds
                    .AnyAsync(x => x.CollegeCode == model.CollegeCode && !x.IsDeleted);

                if (existingCode)
                {
                    return new APIResponse<AffiliatedDto>
                    {
                        Success = false,
                        Message = string.Format(CommonResource.AlreadyExists, "Affiliation College", $"Code: {model.CollegeCode}"),
                        StatusCode = HttpStatusCode.BadRequest,
                    };
                }
            }

            // Map Model to Entity
            var entity = _mapper.Map<Affiliated>(model);
            entity.CreatedBy = model.CreatedBy;
            entity.CreatedDate = DateTime.UtcNow;

            // Trim string fields
            entity.CollegeName = entity.CollegeName?.Trim() ?? "";
            entity.CollegeCode = entity.CollegeCode?.Trim();
            entity.UniversityName = entity.UniversityName?.Trim();
            entity.UniversityCode = entity.UniversityCode?.Trim();
            entity.Address = entity.Address?.Trim();
            entity.Pincode = entity.Pincode?.Trim();
            entity.ContactPerson = entity.ContactPerson?.Trim();
            entity.MobileNo = entity.MobileNo?.Trim();
            entity.Email = entity.Email?.Trim().ToLower();

            entity = await _affiliatedRepository.AddAffiliationCollegeAsync(entity);

            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<AffiliatedDto>
                {
                    Success = false,
                    Data = MapToDto(entity),
                    Message = string.Format(CommonResource.AlreadyExists, typeof(Affiliated).Name, model.CollegeName),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            else if (entity != null && entity.Id > 0)
            {
                // Reload with State navigation property
                var savedEntity = await _affiliatedRepository.GetAffiliationCollegeByIdAsync(entity.Id);
                return new APIResponse<AffiliatedDto>
                {
                    Success = true,
                    Data = MapToDto(savedEntity),
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<AffiliatedDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<AffiliatedDto>> GetAffiliationCollegeByIdAsync(int id)
        {
            var result = await _affiliatedRepository.GetAffiliationCollegeByIdAsync(id);

            if (result != null && result.Id > 0)
            {
                return new APIResponse<AffiliatedDto>
                {
                    Data = MapToDto(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<AffiliatedDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
        }

        public async Task<APIResponse<IEnumerable<AffiliatedDto>>> GetAllAffiliationCollegesAsync(int? stateId = null, int? cityId = null, bool? isActive = null)
        {
            var result = await _affiliatedRepository.GetAllAsync(stateId, cityId, isActive);

            if (result != null && result.Any())
            {
                var dtos = result.Select(MapToDto);
                return new APIResponse<IEnumerable<AffiliatedDto>>
                {
                    Data = dtos,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<AffiliatedDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> UpdateAffiliationCollegeAsync(AffiliatedModel model)
        {
            if (model.Id <= 0)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Invalid affiliation college ID",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            // Check for duplicate CollegeCode (if provided and changed)
            var existingEntity = await _affiliatedRepository.GetAffiliationCollegeByIdAsync(model.Id);
            if (existingEntity == null || existingEntity.Id == 0)
            {
                return new APIResponse
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            if (!string.IsNullOrEmpty(model.CollegeCode) && existingEntity.CollegeCode != model.CollegeCode)
            {
                var exists = await _context.Affiliateds
                    .AnyAsync(x => x.CollegeCode == model.CollegeCode && x.Id != model.Id && !x.IsDeleted);

                if (exists)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = string.Format(CommonResource.AlreadyExists, "Affiliation College", $"Code: {model.CollegeCode}"),
                        StatusCode = HttpStatusCode.BadRequest,
                    };
                }
            }

            // Validate StateId and CityId if changed
            if (model.StateId != existingEntity.StateId || model.CityId != existingEntity.CityId)
            {
                // Validate State exists and is active
                var state = await _context.States
                    .FirstOrDefaultAsync(s => s.Id == model.StateId && !s.IsDeleted);

                if (state == null || !state.IsActive)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Invalid or inactive State",
                        StatusCode = HttpStatusCode.BadRequest,
                    };
                }

                // Validate City exists and belongs to State
                var city = await _context.Cities
                    .FirstOrDefaultAsync(c => c.Id == model.CityId && !c.IsDeleted);

                if (city == null || city.StateId != model.StateId || !city.IsActive)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Invalid or inactive City for the selected State",
                        StatusCode = HttpStatusCode.BadRequest,
                    };
                }
            }

            // Update properties from model
            existingEntity.CollegeName = model.CollegeName.Trim();
            existingEntity.CollegeCode = model.CollegeCode?.Trim();
            existingEntity.UniversityName = model.UniversityName?.Trim();
            existingEntity.UniversityCode = model.UniversityCode?.Trim();
            existingEntity.StateId = model.StateId;
            existingEntity.CityId = model.CityId;
            existingEntity.Address = model.Address?.Trim();
            existingEntity.Pincode = model.Pincode?.Trim();
            existingEntity.ContactPerson = model.ContactPerson?.Trim();
            existingEntity.MobileNo = model.MobileNo?.Trim();
            existingEntity.Email = model.Email?.Trim().ToLower();
            existingEntity.ImagePath = model.ImagePath?.Trim();
            existingEntity.IsActive = model.IsActive;
            existingEntity.UpdatedBy = model.UpdatedBy;
            existingEntity.UpdatedDate = DateTime.UtcNow;

            var result = await _affiliatedRepository.UpdateAffiliationCollegeAsync(existingEntity);
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

        public async Task<APIResponse> DeleteAffiliationCollegeAsync(int id)
        {
            int changes = await _affiliatedRepository.DeleteAffiliationCollegeAsync(id);
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

        public async Task<APIResponse> ToggleAffiliationCollegeStatusAsync(int id)
        {
            int changes = await _affiliatedRepository.ToggleAffiliationCollegeStatusAsync(id);
            if (changes > 0)
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.UpdateSuccess,
                    StatusCode = HttpStatusCode.OK,
                };
            else
                return new APIResponse
                {
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
        }

        private AffiliatedDto MapToDto(Affiliated entity)
        {
            var dto = _mapper.Map<AffiliatedDto>(entity);

            // Map StateName from navigation property
            dto.StateName = entity.State?.Name ?? "";

            // Map CityName - get from Cities through State (loaded via ThenInclude)
            if (entity.State != null && entity.State.Cities != null && entity.State.Cities.Any())
            {
                var city = entity.State.Cities.FirstOrDefault(c => c.Id == entity.CityId);
                dto.CityName = city?.Name ?? "";
            }

            return dto;
        }
    }
}
