using School.Domain;
using Microsoft.AspNetCore.Mvc;
using School.Domain.Hr;
using School_API.Common.Interface;
using School.Services.Hr;

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


