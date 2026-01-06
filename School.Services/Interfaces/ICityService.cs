using School.Models.City;
using School_DTOs;
using School_DTOs.City;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Services.Interfaces
{
    public interface ICityService
    {
        Task<APIResponse<CityDto>> AddCityAsync(CityModel model);

        Task<APIResponse<CityDto>> GetCityByIdAsync(int id);

        Task<APIResponse<IEnumerable<CityDto>>> GetAllCitiesAsync(int? stateId = null);

        Task<APIResponse> UpdateCityAsync(CityModel model);

        Task<APIResponse> DeleteCityAsync(int id);

        Task<APIResponse> ToggleCityStatusAsync(int id);
    }

}
