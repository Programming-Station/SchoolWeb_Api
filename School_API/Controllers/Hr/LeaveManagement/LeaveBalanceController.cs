using Microsoft.AspNetCore.Mvc;
using School.Services.Hr.LeaveManagement;
using School_API.Common.Interface;

namespace School_API.Controllers.Hr.LeaveManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveBalanceController : BaseController
    {
        private readonly ILeaveBalanceService _service;

        public LeaveBalanceController(ILeaveBalanceService service, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _service = service;
        }
    }
}
