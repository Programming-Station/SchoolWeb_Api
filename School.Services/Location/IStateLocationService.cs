using School_DTOs;
using School_DTOs.Common;
using School_DTOs.Location;

namespace School.Services.Location
{
    public interface IStateLocationService
    {
        Task<APIResponse<PagedResponse<StateLocationDto>>> GetAllAsync(PaginationFilterDto filter);
        Task<APIResponse<StateLocationDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateStateLocationDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateStateLocationDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
        Task<APIResponse<object>> ToggleStatusAsync(int id, string username);
        Task<APIResponse<IEnumerable<DropdownDto>>> GetDropdownAsync(int? countryId = null);
    }
}
