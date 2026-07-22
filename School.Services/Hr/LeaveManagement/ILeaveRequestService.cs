using School_DTOs;
using School_DTOs.Hr.LeaveManagement;

namespace School.Services.Interfaces.Hr.LeaveManagement
{
    public interface ILeaveRequestService
    {
        Task<APIResponse<List<LeaveRequestDto>>> GetAllByEmployeeIdAsync(int foreignKeyId);
        Task<APIResponse<LeaveRequestDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateLeaveRequestDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateLeaveRequestDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
        Task<APIResponse<object>> ApproveLeaveAsync(int id, int approverEmployeeId, string username);
        Task<APIResponse<object>> RejectLeaveAsync(int id, int approverEmployeeId, string reason, string username);
    }
}