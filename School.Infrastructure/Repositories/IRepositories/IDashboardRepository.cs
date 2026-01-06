using School_DTOs;
using School_DTOs.Dashboard;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IDashboardRepository
    {
        Task<APIResponse<DashboardDto>> GetDashboardDataAsync();
        Task<APIResponse<DashboardStatsDto>> GetDashboardStatsAsync();
        Task<APIResponse<List<ActivityDto>>> GetRecentActivitiesAsync(int count = 10);
        Task<APIResponse<List<RecentRegistrationDto>>> GetRecentRegistrationsAsync(int count = 10);
        Task<APIResponse<List<UpcomingEventDto>>> GetUpcomingEventsAsync(int count = 10);
        Task<APIResponse<FeeCollectionDto>> GetFeeCollectionStatsAsync();
        Task<APIResponse<AttendanceStatsDto>> GetAttendanceStatsAsync();
    }
}

