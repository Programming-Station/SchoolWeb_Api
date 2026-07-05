using School_DTOs;
using School_DTOs.Hr;
using School_DTOs.Common;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<APIResponse<EmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto model, string username);
        Task<APIResponse<EmployeeDto>> UpdateEmployeeAsync(UpdateEmployeeDto model, string username);
        Task<APIResponse<EmployeeDto>> GetEmployeeByIdAsync(int id);
        Task<APIResponse<PagedResponse<EmployeeDto>>> GetAllEmployeesAsync(PaginationFilterDto filter);
        Task<APIResponse<bool>> DeleteEmployeeAsync(int id, string username);
        Task<APIResponse<bool>> ToggleEmployeeStatusAsync(int id, string username);
        Task<APIResponse<int>> BulkDeleteAsync(IEnumerable<int> ids, string username);
        Task<APIResponse<int>> BulkRestoreAsync(IEnumerable<int> ids, string username);
        Task<APIResponse<int>> BulkStatusUpdateAsync(IEnumerable<int> ids, string status, string username);
    }
}
