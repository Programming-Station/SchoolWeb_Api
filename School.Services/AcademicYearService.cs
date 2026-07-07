using AutoMapper;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.AcademicYear;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.AcademicYear;
using System.Net;

namespace School.Services
{
    public class AcademicYearService : IAcademicYearService
    {
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IMapper _mapper;

        public AcademicYearService(IAcademicYearRepository academicYearRepository, IMapper mapper)
        {
            _academicYearRepository = academicYearRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<AcademicYearDto>> AddAcademicYearAsync(AcademicYearModel model)
        {
            if (model.StartDate >= model.EndDate)
            {
                return new APIResponse<AcademicYearDto>
                {
                    Success = false,
                    Message = "Start date must be before end date",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var entity = _mapper.Map<AcademicYear>(model);
            entity = await _academicYearRepository.AddAcademicYearAsync(entity);

            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<AcademicYearDto>
                {
                    Success = false,
                    Data = _mapper.Map<AcademicYearDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, typeof(AcademicYear).Name, model.YearName),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            else if (entity != null && entity.Id == -1)
            {
                return new APIResponse<AcademicYearDto>
                {
                    Success = false,
                    Message = "Academic year dates overlap with an existing academic year",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            else if (entity != null && entity.Id > 0)
            {
                return new APIResponse<AcademicYearDto>
                {
                    Success = true,
                    Data = _mapper.Map<AcademicYearDto>(entity),
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<AcademicYearDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<AcademicYearDto>> GetAcademicYearByIdAsync(int id)
        {
            var result = await _academicYearRepository.GetAcademicYearByIdAsync(id);

            if (result != null && result.Id > 0)
            {
                var dto = _mapper.Map<AcademicYearDto>(result);
                return new APIResponse<AcademicYearDto>
                {
                    Data = dto,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<AcademicYearDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<APIResponse<IEnumerable<AcademicYearDto>>> GetAllAcademicYearsAsync(bool? isActive = null)
        {
            var result = await _academicYearRepository.GetAllAcademicYearsAsync(isActive);
            if (result != null && result.Any())
            {
                return new APIResponse<IEnumerable<AcademicYearDto>>
                {
                    Data = _mapper.Map<IEnumerable<AcademicYearDto>>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<AcademicYearDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse<AcademicYearDto>> GetCurrentAcademicYearAsync()
        {
            var result = await _academicYearRepository.GetCurrentAcademicYearAsync();

            if (result != null && result.Id > 0)
            {
                return new APIResponse<AcademicYearDto>
                {
                    Data = _mapper.Map<AcademicYearDto>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<AcademicYearDto>
                {
                    Message = "No current academic year found",
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<APIResponse> UpdateAcademicYearAsync(AcademicYearModel model)
        {
            if (model.StartDate >= model.EndDate)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Start date must be before end date",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            if (!model.Id.HasValue)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Academic year ID is required",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var entity = _mapper.Map<AcademicYear>(model);
            var result = await _academicYearRepository.UpdateAcademicYearAsync(entity);

            if (result > 0)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Message = CommonResource.UpdateSuccess
                };
            }
            else if (result == -1)
            {
                return new APIResponse
                {
                    Message = string.Format(CommonResource.AlreadyExists, typeof(AcademicYear).Name, model.YearName),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            else if (result == -2)
            {
                return new APIResponse
                {
                    Message = "Academic year dates overlap with an existing academic year",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            else
            {
                return new APIResponse
                {
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> DeleteAcademicYearAsync(int id)
        {
            int changes = await _academicYearRepository.DeleteAcademicYearAsync(id);
            if (changes > 0)
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.DeleteSuccess,
                    StatusCode = HttpStatusCode.OK,
                };
            else if (changes == -1)
                return new APIResponse
                {
                    Message = "Cannot delete current academic year. Please set another academic year as current first.",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            else
                return new APIResponse
                {
                    Message = CommonResource.DeleteFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
        }

        public async Task<APIResponse> ToggleAcademicYearStatusAsync(int id)
        {
            int changes = await _academicYearRepository.ToggleAcademicYearStatusAsync(id);
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

        public async Task<APIResponse> SetCurrentAcademicYearAsync(int id)
        {
            int changes = await _academicYearRepository.SetCurrentAcademicYearAsync(id);
            if (changes > 0)
                return new APIResponse
                {
                    Success = true,
                    Message = "Current academic year set successfully",
                    StatusCode = HttpStatusCode.OK,
                };
            else if (changes == -1)
                return new APIResponse
                {
                    Message = "Cannot set inactive academic year as current",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            else
                return new APIResponse
                {
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
        }
    }
}
