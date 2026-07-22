using School_DTOs;
using School_DTOs.Hr;

namespace School.Services.Hr
{
    public interface IEmployeeBankDetailService
    {
        Task<APIResponse<List<EmployeeBankDetailDto>>> GetAllByEmployeeIdAsync(int employeeId);
        Task<APIResponse<EmployeeBankDetailDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateEmployeeBankDetailDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeBankDetailDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
}