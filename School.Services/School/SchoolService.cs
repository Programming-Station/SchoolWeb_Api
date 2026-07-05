using AutoMapper;
using School.Domain.School;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.School;
using School.Services.School.ISchoolServices;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.School;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace School.Services.School
{
    public class SchoolService:ISchoolService
    {
        private readonly ISchoolRepository _schoolRepo;
        private readonly IMapper _mapper;
        public SchoolService(ISchoolRepository schoolRepo, IMapper mapper)
        {
            _schoolRepo = schoolRepo;
            _mapper = mapper;  
        }

        public async Task<APIResponse<SchoolRegistrationDto>> AddAsync(SchoolRegistrationModel model)
        {
            try
            {
                var entity = _mapper.Map<schoolRegistion>(model);
                entity = await _schoolRepo.AddSchoolAsync(entity);
                if (entity != null && entity.Id == 0)
                {
                    return new APIResponse<SchoolRegistrationDto>
                    {
                        Data = _mapper.Map<SchoolRegistrationDto>(entity),
                        Message = string.Format(CommonResource.AlreadyExistsRecord, nameof(schoolRegistion), nameof(model.SchoolName), model.SchoolName),
                        StatusCode = HttpStatusCode.Forbidden,
                    };
                }
                else if (entity != null && entity.Id > 0)
                {
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
                var entity = _mapper.Map<schoolRegistion>(model);
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

        public Task<APIResponse<IEnumerable<SchoolRegistrationDto>>> GetAllsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<APIResponse<SchoolRegistrationDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
