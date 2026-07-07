using Microsoft.AspNetCore.Mvc;
using School_API.Common.Interface;
using System.Threading.Tasks;

namespace School_API.Controllers.Hr
{
    public class TimesheetController : BaseController
    {
        public TimesheetController(ICurrentUserService currentUserService) : base(currentUserService)
        {
        }

        // Stub for Timesheet operations
        [HttpGet("employee/{employeeId}")]
        public IActionResult GetTimesheets(int employeeId) => Ok();
    }
}
