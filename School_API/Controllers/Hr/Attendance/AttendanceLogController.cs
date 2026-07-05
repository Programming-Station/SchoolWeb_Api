using Microsoft.AspNetCore.Mvc;
using School.Services.Hr.Attendance;
using School_API.Common.Interface;

namespace School_API.Controllers.Hr.Attendance
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceLogController : BaseController
    {
        private readonly IAttendanceLogService _service;

        public AttendanceLogController(IAttendanceLogService service, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _service = service;
        }
    }
}
