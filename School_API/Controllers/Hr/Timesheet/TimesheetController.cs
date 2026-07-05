using Microsoft.AspNetCore.Mvc;
using School.Services.Hr.Timesheet;
using School_API.Common.Interface;

namespace School_API.Controllers.Hr.Timesheet
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : BaseController
    {
        private readonly ITimesheetService _service;

        public TimesheetController(ITimesheetService service, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _service = service;
        }
    }
}
