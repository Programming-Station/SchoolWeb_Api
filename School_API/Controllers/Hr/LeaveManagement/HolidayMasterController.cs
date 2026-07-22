using Microsoft.AspNetCore.Mvc;
using School.Services.Hr;
using School_API.Common.Interface;

namespace School_API.Controllers.Hr.LeaveManagement
{
    [Route("api/[controller]")]
    public class HolidayMasterController : HrController<global::School.Domain.Hr.LeaveManagement.HolidayMaster>
    {
        public HolidayMasterController(IHrMasterService<global::School.Domain.Hr.LeaveManagement.HolidayMaster> masterService, ICurrentUserService currentUserService)
            : base(masterService, currentUserService)
        {
        }
    }
}
