using Microsoft.AspNetCore.Mvc;
using School.Domain;
using School.Services.Hr;
using School_API.Common.Interface;

namespace School_API.Controllers.Hr
{
    [Route("api/[controller]")]
    public class FacultyController : HrController<Faculty>
    {
        public FacultyController(IHrMasterService<Faculty> masterService, ICurrentUserService currentUserService) : base(masterService, currentUserService)
        {
        }
    }
}


