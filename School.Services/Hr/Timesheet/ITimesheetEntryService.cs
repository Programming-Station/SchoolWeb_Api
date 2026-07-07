using School_DTOs.Common;
using School_DTOs;
using School_DTOs.Hr.Timesheet;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace School.Services.Interfaces.Hr.Timesheet
{
    public interface ITimesheetEntryService
    {
        Task<APIResponse<List<TimesheetEntryDto>>> GetAllByTimesheetIdAsync(int foreignKeyId);
        Task<APIResponse<TimesheetEntryDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateTimesheetEntryDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateTimesheetEntryDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
}