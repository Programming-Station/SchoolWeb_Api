using School_DTOs.Common;
using School_DTOs.Hr.LeaveManagement;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace School.Services.Interfaces.Hr.LeaveManagement
{
    public interface ILeaveBalanceService
    {
        Task<APIResponse<List<LeaveBalanceDto>>> GetAllByEmployeeIdAsync(int foreignKeyId);
        Task<APIResponse<LeaveBalanceDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateLeaveBalanceDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateLeaveBalanceDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
}