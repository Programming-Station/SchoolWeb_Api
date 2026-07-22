using School_DTOs;
using School_DTOs.Common;
using School_DTOs.Hr;

namespace School.Services.Hr
{
    public interface IDepartmentService
    {
        Task<APIResponse<PagedResponse<DepartmentDto>>> GetAllAsync(PaginationFilterDto filter);
        Task<APIResponse<DepartmentDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateDepartmentDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateDepartmentDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
        Task<APIResponse<object>> ToggleStatusAsync(int id, string username);
    }
}
