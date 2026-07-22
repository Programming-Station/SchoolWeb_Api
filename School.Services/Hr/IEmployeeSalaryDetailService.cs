using School_DTOs;
using School_DTOs.Hr;

namespace School.Services.Hr
{
    public interface IEmployeeSalaryDetailService
    {
        Task<APIResponse<List<EmployeeSalaryDetailDto>>> GetAllByEmployeeIdAsync(int employeeId);
        Task<APIResponse<EmployeeSalaryDetailDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateEmployeeSalaryDetailDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeSalaryDetailDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
}