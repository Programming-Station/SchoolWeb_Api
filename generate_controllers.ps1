$basePath = "e:\GIT\SchoolSAAS\SchoolWeb_Api\School_API\Controllers\Hr"

# HrMasterController.cs
$content = @"
using Microsoft.AspNetCore.Mvc;
using School_API.Common.Interface;
using System.Threading.Tasks;

namespace School_API.Controllers.Hr
{
    public class HrMasterController : BaseController
    {
        public HrMasterController(ICurrentUserService currentUserService) : base(currentUserService)
        {
        }

        // Stub for Master data operations (Designations, Shifts, etc.)
        [HttpGet(`"designations`")]
        public IActionResult GetDesignations() => Ok();
    }
}
"@
Set-Content -Path "$basePath\HrMasterController.cs" -Value $content

# LeaveManagementController.cs
$content = @"
using Microsoft.AspNetCore.Mvc;
using School_API.Common.Interface;
using System.Threading.Tasks;

namespace School_API.Controllers.Hr
{
    public class LeaveManagementController : BaseController
    {
        public LeaveManagementController(ICurrentUserService currentUserService) : base(currentUserService)
        {
        }

        // Stub for Leave operations
        [HttpGet(`"requests`")]
        public IActionResult GetLeaveRequests() => Ok();
    }
}
"@
Set-Content -Path "$basePath\LeaveManagementController.cs" -Value $content

# AttendanceController.cs
$content = @"
using Microsoft.AspNetCore.Mvc;
using School_API.Common.Interface;
using System.Threading.Tasks;

namespace School_API.Controllers.Hr
{
    public class AttendanceController : BaseController
    {
        public AttendanceController(ICurrentUserService currentUserService) : base(currentUserService)
        {
        }

        // Stub for Attendance operations
        [HttpGet(`"today`")]
        public IActionResult GetTodaysAttendance() => Ok();
    }
}
"@
Set-Content -Path "$basePath\AttendanceController.cs" -Value $content

# TimesheetController.cs
$content = @"
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
        [HttpGet(`"employee/{employeeId}`")]
        public IActionResult GetTimesheets(int employeeId) => Ok();
    }
}
"@
Set-Content -Path "$basePath\TimesheetController.cs" -Value $content
