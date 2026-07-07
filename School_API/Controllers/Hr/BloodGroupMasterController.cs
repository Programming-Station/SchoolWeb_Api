using Microsoft.AspNetCore.Mvc;
using School.Domain.Hr;
using School.Services.Hr;
using School_API.Common.Interface;

namespace School_API.Controllers.Hr
{
    [Route("api/[controller]")]
    public class BloodGroupMasterController : HrController<BloodGroupMaster>
    {
        public BloodGroupMasterController(IHrMasterService<BloodGroupMaster> masterService, ICurrentUserService currentUserService) : base(masterService, currentUserService)
        {
        }
    }
}

