using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer,Basic", Roles = "SUPERADMIN,SuperAdmin")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SuperAdminDashboardController : BaseController
    {
        private readonly ISuperAdminDashboardService _dashboardService;

        public SuperAdminDashboardController(ISuperAdminDashboardService dashboardService, ICurrentUserService currentUserService)
            : base(currentUserService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Get unified Super Admin dashboard stats, charts, tables, health, calendars, and activity feeds.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            var result = await _dashboardService.GetSuperAdminDashboardDataAsync();

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get system health status metrics (CPU, RAM, Disk, DB connection) in real-time.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSystemHealth()
        {
            var result = await _dashboardService.GetSystemHealthAsync();

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
