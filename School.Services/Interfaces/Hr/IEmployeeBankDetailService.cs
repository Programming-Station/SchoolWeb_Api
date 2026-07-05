using School_DTOs.Common;
using School_DTOs;
using School_DTOs.Hr;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace School.Services.Interfaces.Hr
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