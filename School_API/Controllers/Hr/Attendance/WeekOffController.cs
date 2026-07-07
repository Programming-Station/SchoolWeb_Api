using Microsoft.AspNetCore.Mvc;
using School_API.Common.Interface;
using School.Services.Hr;

namespace School_API.Controllers.Hr.Attendance
{
    [Route("api/[controller]")]
    public class WeekOffController : HrController<global::School.Domain.Hr.Attendance.WeekOff>
    {
        public WeekOffController(IHrMasterService<global::School.Domain.Hr.Attendance.WeekOff> masterService, ICurrentUserService currentUserService) 
            : base(masterService, currentUserService)
        {
        }
    }
}
