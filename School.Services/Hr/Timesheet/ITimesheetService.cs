using School_DTOs;
using School_DTOs.Hr.Timesheet;

namespace School.Services.Interfaces.Hr.Timesheet
{
    public interface ITimesheetService
    {
        Task<APIResponse<List<TimesheetDto>>> GetAllByEmployeeIdAsync(int foreignKeyId);
        Task<APIResponse<TimesheetDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateTimesheetDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateTimesheetDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
        Task<APIResponse<object>> SubmitTimesheetAsync(int id, string username);
        Task<APIResponse<object>> ApproveTimesheetAsync(int id, int approverEmployeeId, string username);
        Task<APIResponse<object>> RejectTimesheetAsync(int id, int approverEmployeeId, string reason, string username);
    }
}