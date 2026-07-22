using School_DTOs;
using School_DTOs.Hr;

namespace School.Services.Hr
{
    public interface IEmployeeDetailService
    {
        Task<APIResponse<List<EmployeeDetailDto>>> GetAllByEmployeeIdAsync(int employeeId);
        Task<APIResponse<EmployeeDetailDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateEmployeeDetailDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeDetailDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
}