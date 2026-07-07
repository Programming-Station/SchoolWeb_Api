using Microsoft.AspNetCore.Mvc;
using School.Domain.Hr;
using School.Services.Hr;
using School_API.Common.Interface;

namespace School_API.Controllers.Hr
{
    [Route("api/[controller]")]
    public class EmploymentStatusController : HrController<EmploymentStatus>
    {
        public EmploymentStatusController(IHrMasterService<EmploymentStatus> masterService, ICurrentUserService currentUserService) : base(masterService, currentUserService)
        {
        }
    }
}

