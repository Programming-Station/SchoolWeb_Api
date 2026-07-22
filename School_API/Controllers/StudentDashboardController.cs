using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer,Basic", Roles = "STUDENT,Student,Admin,Owner")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentDashboardController : BaseController
    {
        private readonly IStudentDashboardService _dashboardService;

        public StudentDashboardController(IStudentDashboardService dashboardService, ICurrentUserService currentUserService)
            : base(currentUserService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Get unified Student dashboard stats, profile, timetable, subjects, library, transport, notice, and calendar.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            // UserId is retrieved from claims via BaseController
            var result = await _dashboardService.GetStudentDashboardDataAsync(UserId);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
