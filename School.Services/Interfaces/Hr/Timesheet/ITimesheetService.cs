using School_DTOs.Common;
using School_DTOs.Hr.Timesheet;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace School.Services.Interfaces.Hr.Timesheet
{
    public interface ITimesheetService
    {
        Task<APIResponse<List<TimesheetDto>>> GetAllByEmployeeIdAsync(int foreignKeyId);
        Task<APIResponse<TimesheetDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateTimesheetDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateTimesheetDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
}