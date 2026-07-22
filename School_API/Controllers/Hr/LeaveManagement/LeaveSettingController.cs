using Microsoft.AspNetCore.Mvc;
using School.Services.Hr;
using School_API.Common.Interface;

namespace School_API.Controllers.Hr.LeaveManagement
{
    [Route("api/[controller]")]
    public class LeaveSettingController : HrController<global::School.Domain.Hr.LeaveManagement.LeaveSetting>
    {
        public LeaveSettingController(IHrMasterService<global::School.Domain.Hr.LeaveManagement.LeaveSetting> masterService, ICurrentUserService currentUserService)
            : base(masterService, currentUserService)
        {
        }
    }
}
