using School.Services.Interfaces;
using School_API.Common.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace School_API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer,Basic")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService, ICurrentUserService currentUserService) 
            : base(currentUserService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Get complete dashboard data (stats, activities, registrations, events, etc.)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            var result = await _dashboardService.GetDashboardDataAsync();

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get dashboard statistics only
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDashboardStats()
        {
            var result = await _dashboardService.GetDashboardStatsAsync();

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get recent activities
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetRecentActivities([FromQuery] int count = 10)
        {
            var result = await _dashboardService.GetRecentActivitiesAsync(count);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get recent student registrations
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetRecentRegistrations([FromQuery] int count = 10)
        {
            var result = await _dashboardService.GetRecentRegistrationsAsync(count);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get upcoming events
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUpcomingEvents([FromQuery] int count = 10)
        {
            var result = await _dashboardService.GetUpcomingEventsAsync(count);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get fee collection statistics
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetFeeCollectionStats()
        {
            var result = await _dashboardService.GetFeeCollectionStatsAsync();

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get attendance statistics
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAttendanceStats()
        {
            var result = await _dashboardService.GetAttendanceStatsAsync();

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }
    }
}

