using Microsoft.AspNetCore.Mvc;
using School_API.Common.Interface;

namespace School_API.Controllers.Hr
{
    public class AttendanceController : BaseController
    {
        public AttendanceController(ICurrentUserService currentUserService) : base(currentUserService)
        {
        }

        // Stub for Attendance operations
        [HttpGet("today")]
        public IActionResult GetTodaysAttendance() => Ok();
    }
}
