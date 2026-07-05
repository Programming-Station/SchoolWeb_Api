using School_DTOs.Payroll;
using School_DTOs.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace School.Services.Interfaces.Payroll
{
    public interface ISalaryComponentService
    {
        Task<APIResponse<List<SalaryComponentDto>>> GetAllAsync();
        Task<APIResponse<SalaryComponentDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateSalaryComponentDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateSalaryComponentDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
    public interface IPayrollRunService
    {
        Task<APIResponse<List<PayrollRunDto>>> GetAllByEmployeeIdAsync(int employeeId);
        Task<APIResponse<PayrollRunDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreatePayrollRunDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdatePayrollRunDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
        Task<APIResponse<object>> ProcessPayrollAsync(int id, string username);
        Task<APIResponse<object>> MarkAsPaidAsync(int id, string username);
    }
}
