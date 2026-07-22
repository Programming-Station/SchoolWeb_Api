using School_DTOs;
using School_DTOs.Hr.Attendance;

namespace School.Services.Interfaces.Hr.Attendance
{
    public interface IAttendanceService
    {
        Task<APIResponse<List<AttendanceDto>>> GetAllByEmployeeIdAsync(int foreignKeyId);
        Task<APIResponse<AttendanceDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateAttendanceDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateAttendanceDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
        Task<APIResponse<object>> PunchInAsync(int employeeId, string username);
        Task<APIResponse<object>> PunchOutAsync(int employeeId, string username);
    }
}