using School_DTOs;
using School_DTOs.Common;
using School_DTOs.Location;

namespace School.Services.Location
{
    public interface ICityLocationService
    {
        Task<APIResponse<PagedResponse<CityLocationDto>>> GetAllAsync(PaginationFilterDto filter);
        Task<APIResponse<CityLocationDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateCityLocationDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateCityLocationDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
        Task<APIResponse<object>> ToggleStatusAsync(int id, string username);
        Task<APIResponse<IEnumerable<DropdownDto>>> GetDropdownAsync(int? stateId = null);
    }
}
