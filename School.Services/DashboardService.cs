using School.Infrastructure.Repositories.IRepositories;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Dashboard;

namespace School.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public Task<APIResponse<DashboardDto>> GetDashboardDataAsync()
        {
            return _dashboardRepository.GetDashboardDataAsync();
        }

        public Task<APIResponse<DashboardStatsDto>> GetDashboardStatsAsync()
        {
            return _dashboardRepository.GetDashboardStatsAsync();
        }

        public Task<APIResponse<List<ActivityDto>>> GetRecentActivitiesAsync(int count = 10)
        {
            return _dashboardRepository.GetRecentActivitiesAsync(count);
        }

        public Task<APIResponse<List<RecentRegistrationDto>>> GetRecentRegistrationsAsync(int count = 10)
        {
            return _dashboardRepository.GetRecentRegistrationsAsync(count);
        }

        public Task<APIResponse<List<UpcomingEventDto>>> GetUpcomingEventsAsync(int count = 10)
        {
            return _dashboardRepository.GetUpcomingEventsAsync(count);
        }

        public Task<APIResponse<FeeCollectionDto>> GetFeeCollectionStatsAsync()
        {
            return _dashboardRepository.GetFeeCollectionStatsAsync();
        }

        public Task<APIResponse<AttendanceStatsDto>> GetAttendanceStatsAsync()
        {
            return _dashboardRepository.GetAttendanceStatsAsync();
        }
    }
}

