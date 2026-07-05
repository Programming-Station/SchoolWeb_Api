using Microsoft.AspNetCore.Mvc;
using School.Services.Hr.LeaveManagement;
using School_API.Common.Interface;

namespace School_API.Controllers.Hr.LeaveManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : BaseController
    {
        private readonly ILeaveRequestService _service;

        public LeaveRequestController(ILeaveRequestService service, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _service = service;
        }
    }
}
