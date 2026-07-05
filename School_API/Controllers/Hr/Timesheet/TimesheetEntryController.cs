using Microsoft.AspNetCore.Mvc;
using School.Services.Hr.Timesheet;
using School_API.Common.Interface;

namespace School_API.Controllers.Hr.Timesheet
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetEntryController : BaseController
    {
        private readonly ITimesheetEntryService _service;

        public TimesheetEntryController(ITimesheetEntryService service, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _service = service;
        }
    }
}
