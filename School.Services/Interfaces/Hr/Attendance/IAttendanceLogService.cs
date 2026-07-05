using School_DTOs.Common;
using School_DTOs.Hr.Attendance;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace School.Services.Interfaces.Hr.Attendance
{
    public interface IAttendanceLogService
    {
        Task<APIResponse<List<AttendanceLogDto>>> GetAllByEmployeeIdAsync(int foreignKeyId);
        Task<APIResponse<AttendanceLogDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateAttendanceLogDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateAttendanceLogDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
}