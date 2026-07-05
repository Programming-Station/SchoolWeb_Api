using Microsoft.AspNetCore.Mvc;
using School_API.Common.Interface;
using School.Services.Hr;

namespace School_API.Controllers.Hr.LeaveManagement
{
    [Route("api/[controller]")]
    public class LeaveTypeController : HrController<global::School.Domain.Hr.LeaveManagement.LeaveType>
    {
        public LeaveTypeController(IHrMasterService<global::School.Domain.Hr.LeaveManagement.LeaveType> masterService, ICurrentUserService currentUserService) 
            : base(masterService, currentUserService)
        {
        }
    }
}
