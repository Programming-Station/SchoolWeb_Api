using Microsoft.AspNetCore.Mvc;
using School.Services.Hr.Attendance;
using School_API.Common.Interface;

namespace School_API.Controllers.Hr.Attendance
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : BaseController
    {
        private readonly IAttendanceService _service;

        public AttendanceController(IAttendanceService service, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _service = service;
        }
    }
}
