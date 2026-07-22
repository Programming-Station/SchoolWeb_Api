using School_DTOs;
using School_DTOs.Hr;

namespace School.Services.Hr
{
    public interface IEmployeeExperienceService
    {
        Task<APIResponse<List<EmployeeExperienceDto>>> GetAllByEmployeeIdAsync(int employeeId);
        Task<APIResponse<EmployeeExperienceDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateEmployeeExperienceDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeExperienceDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
}