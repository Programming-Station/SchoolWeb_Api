using Microsoft.AspNetCore.Mvc;
using School_API.Common.Interface;
using School.Services.Hr;

namespace School_API.Controllers.Hr.Attendance
{
    [Route("api/[controller]")]
    public class ShiftMasterController : HrController<global::School.Domain.Hr.Attendance.ShiftMaster>
    {
        public ShiftMasterController(IHrMasterService<global::School.Domain.Hr.Attendance.ShiftMaster> masterService, ICurrentUserService currentUserService) 
            : base(masterService, currentUserService)
        {
        }
    }
}
