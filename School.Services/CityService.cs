using System.Net;
using AutoMapper;
using School.Domain.Location;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.City;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.City;

namespace School.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;

        public CityService(ICityRepository cityRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<CityDto>> AddCityAsync(CityModel model)
        {
            var entity = _mapper.Map<City>(model);
            entity = await _cityRepository.AddCityAsync(entity);
            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<CityDto>
                {
                    Success = false,
                    Data = _mapper.Map<CityDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, typeof(City).Name, model.Name),
                    StatusCode = HttpStatusCode.BadRequest,
                };

            }
            else if (entity != null && entity.Id >= 0)
            {
                return new APIResponse<CityDto>
                {
                    Success = true,
                    Data = _mapper.Map<CityDto>(entity),
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<CityDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<CityDto>> GetCityByIdAsync(int id)
        {
            var result = await _cityRepository.GetCityByIdAsync(id);

            if (result != null)
            {
                return new APIResponse<CityDto>
                {
                    Data = _mapper.Map<CityDto>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<CityDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }


        public async Task<APIResponse<IEnumerable<CityDto>>> GetAllCitiesAsync(int? stateId = null)
        {
            var result = await _cityRepository.GetAllAsync(stateId);
            if (result != null)
            {
                return new APIResponse<IEnumerable<CityDto>>
                {
                    Data = _mapper.Map<IEnumerable<CityDto>>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<CityDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> UpdateCityAsync(CityModel model)
        {
            var entity = _mapper.Map<City>(model);
            var result = await _cityRepository.UpdateCityAsync(entity);
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
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> DeleteCityAsync(int id)
        {
            int changes = await _cityRepository.DeleteCityAsync(id);
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

        public async Task<APIResponse> ToggleCityStatusAsync(int id)
        {
            int changes = await _cityRepository.ToggleCityStatusAsync(id);
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
    }
}
