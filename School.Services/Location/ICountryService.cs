using School_DTOs;
using School_DTOs.Common;
using School_DTOs.Location;

namespace School.Services.Location
{
    public interface ICountryService
    {
        Task<APIResponse<PagedResponse<CountryDto>>> GetAllAsync(PaginationFilterDto filter);
        Task<APIResponse<CountryDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateCountryDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateCountryDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
        Task<APIResponse<object>> ToggleStatusAsync(int id, string username);
        Task<APIResponse<IEnumerable<DropdownDto>>> GetDropdownAsync();
        Task<APIResponse<bool>> BulkDeleteAsync(IEnumerable<int> ids, string username);
        Task<APIResponse<bool>> BulkStatusChangeAsync(IEnumerable<int> ids, bool isActive, string username);
    }
}
