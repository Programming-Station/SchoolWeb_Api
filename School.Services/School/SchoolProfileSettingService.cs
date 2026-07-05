using AutoMapper;
using School.Domain.School;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using School.Services.School.ISchoolServices;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.School;
using System;
using System.Net;
using System.Threading.Tasks;

namespace School.Services.School
{
    public class SchoolProfileSettingService : ISchoolProfileSettingService
    {
        private readonly ISchoolProfileSettingRepository _profileSettingRepo;
        private readonly ITenantService _tenantService;
        private readonly IMapper _mapper;

        public SchoolProfileSettingService(
            ISchoolProfileSettingRepository profileSettingRepo, 
            ITenantService tenantService, 
            IMapper mapper)
        {
            _profileSettingRepo = profileSettingRepo;
            _tenantService = tenantService;
            _mapper = mapper;
        }

        public async Task<APIResponse<SchoolProfileSettingDto>> GetMyProfileSettingsAsync()
        {
            try
            {
                int? tenantId = _tenantService.GetTenantId();
                if (tenantId == null || tenantId <= 0)
                {
                    return new APIResponse<SchoolProfileSettingDto>
                    {
                        Success = false,
                        Message = "Tenant Information Missing",
                        StatusCode = HttpStatusCode.Unauthorized
                    };
                }

                var entity = await _profileSettingRepo.GetBySchoolIdAsync(tenantId.Value);
                
                if (entity == null)
                {
                    // Return empty DTO if not found
                    return new APIResponse<SchoolProfileSettingDto>
                    {
                        Success = true,
                        Data = new SchoolProfileSettingDto { SchoolRegistrationId = tenantId.Value },
                        Message = "Success",
                        StatusCode = HttpStatusCode.OK
                    };
                }

                return new APIResponse<SchoolProfileSettingDto>
                {
                    Success = true,
                    Data = _mapper.Map<SchoolProfileSettingDto>(entity),
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<SchoolProfileSettingDto>
                {
                    Success = false,
                    Message = CommonResource.PleaseTryAgain,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Error = new APIException(ex.Message, ex.InnerException)
                };
            }
        }

        public async Task<APIResponse<SchoolProfileSettingDto>> UpdateMyProfileSettingsAsync(SchoolProfileSettingModel model)
        {
            try
            {
                int? tenantId = _tenantService.GetTenantId();
                if (tenantId == null || tenantId <= 0)
                {
                    return new APIResponse<SchoolProfileSettingDto>
                    {
                        Success = false,
                        Message = "Tenant Information Missing",
                        StatusCode = HttpStatusCode.Unauthorized
                    };
                }

                var entity = _mapper.Map<SchoolProfileSetting>(model);
                entity.SchoolRegistrationId = tenantId.Value;

                int changes = await _profileSettingRepo.UpdateProfileSettingAsync(entity);

                if (changes > 0)
                {
                    var updatedEntity = await _profileSettingRepo.GetBySchoolIdAsync(tenantId.Value);
                    return new APIResponse<SchoolProfileSettingDto>
                    {
                        Success = true,
                        Message = CommonResource.UpdateSuccess,
                        StatusCode = HttpStatusCode.OK,
                        Data = _mapper.Map<SchoolProfileSettingDto>(updatedEntity)
                    };
                }

                return new APIResponse<SchoolProfileSettingDto>
                {
                    Success = false,
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<SchoolProfileSettingDto>
                {
                    Success = false,
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Error = new APIException(ex.Message, ex.InnerException)
                };
            }
        }
    }
}





