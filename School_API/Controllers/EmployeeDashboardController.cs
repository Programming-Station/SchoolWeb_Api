using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer,Basic", Roles = "EMPLOYEE,Teacher,Staff,Admin,Owner")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeDashboardController : BaseController
    {
        private readonly IEmployeeDashboardService _dashboardService;

        public EmployeeDashboardController(IEmployeeDashboardService dashboardService, ICurrentUserService currentUserService)
            : base(currentUserService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Get unified Employee dashboard stats, schedule, timetables, evaluations, and payroll.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            // UserId is retrieved from claims via BaseController
            var result = await _dashboardService.GetEmployeeDashboardDataAsync(UserId);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Action to clock in or clock out for work.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ClockInOut([FromQuery] bool isClockIn)
        {
            var result = await _dashboardService.ClockInOutAsync(UserId, isClockIn);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
