using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School_API.Common.Interface;

namespace School_API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer,Basic")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;

        public BaseController(ICurrentUserService currentUserService)
        {
            _currentUser = currentUserService;
        }

        protected string UserId => _currentUser.UserId;
        protected string UserName => _currentUser.UserName;
        protected string RoleName => _currentUser.RoleName;
        protected string RoleId => _currentUser.RoleId;
        protected bool IsAuthenticated => _currentUser.IsAuthenticated;
        protected int? TenantId => _currentUser.TenantId;
    }
}

