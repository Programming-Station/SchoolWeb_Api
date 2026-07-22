using Microsoft.AspNetCore.Mvc;
using School_API.Common.Interface;

namespace School_API.Controllers.Hr
{
    public class LeaveManagementController : BaseController
    {
        public LeaveManagementController(ICurrentUserService currentUserService) : base(currentUserService)
        {
        }

        // Stub for Leave operations
        [HttpGet("requests")]
        public IActionResult GetLeaveRequests() => Ok();
    }
}
