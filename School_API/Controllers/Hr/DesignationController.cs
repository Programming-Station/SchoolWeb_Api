using Microsoft.AspNetCore.Mvc;
using School_API.Common.Interface;
using School.Services.Hr;

namespace School_API.Controllers.Hr
{
    [Route("api/[controller]")]
    public class DesignationController : HrController<global::School.Domain.Designation>
    {
        public DesignationController(IHrMasterService<global::School.Domain.Designation> masterService, ICurrentUserService currentUserService) 
            : base(masterService, currentUserService)
        {
        }
    }
}
