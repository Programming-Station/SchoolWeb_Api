using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School_API.Common.Interface;

namespace School_API.Controllers.Hr
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class HrController: BaseController
    {
        
        public HrController(ICurrentUserService current):base(current)
        {
            
        }
    }
}
