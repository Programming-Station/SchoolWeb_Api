using Microsoft.AspNetCore.Mvc;
using School_API.Common.Interface;
using School.Services.Hr;

namespace School_API.Controllers.Hr
{
    [Route("api/[controller]")]
    public class DesignationController : HrController<global::School.Domain.Hr.Designation>
    {
        public DesignationController(IHrMasterService<global::School.Domain.Hr.Designation> masterService, ICurrentUserService currentUserService) 
            : base(masterService, currentUserService)
        {
        }
    }
}
