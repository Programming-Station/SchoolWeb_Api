using Microsoft.AspNetCore.Mvc;
using School.Domain.Hr;
using School.Services.Hr;
using School_API.Common.Interface;

namespace School_API.Controllers.Hr
{
    [Route("api/[controller]")]
    public class EmployeeCategoryController : HrController<EmployeeCategory>
    {
        public EmployeeCategoryController(IHrMasterService<EmployeeCategory> masterService, ICurrentUserService currentUserService) : base(masterService, currentUserService)
        {
        }
    }
}

