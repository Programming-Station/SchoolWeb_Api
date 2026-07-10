using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Repositories.IRepositories;
using School_API.Common.Interface;
using School_DTOs.Parent;

namespace School_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ParentController : ControllerBase
    {
        private readonly IParentRepository _parentRepo;
        private readonly ICurrentUserService _currentUser;

        public ParentController(IParentRepository parentRepo, ICurrentUserService currentUser)
        {
            _parentRepo = parentRepo;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Parent login using mobile number + password.
        /// Returns JWT token and all linked children across all school tenants.
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] ParentLoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _parentRepo.ParentLoginAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all children linked to the authenticated parent (cross-tenant).
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer,Basic", Roles = "Parent")]
        [HttpGet]
        public async Task<IActionResult> GetMyChildren()
        {
            var parentUserId = _currentUser.UserId;
            if (string.IsNullOrEmpty(parentUserId))
                return Unauthorized();

            var result = await _parentRepo.GetChildrenAsync(parentUserId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get dashboard data for a specific child.
        /// Validates that the child belongs to this parent before returning data.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer,Basic", Roles = "Parent")]
        [HttpGet]
        public async Task<IActionResult> GetChildDashboard([FromQuery] int studentId, [FromQuery] int schoolRegistrationId)
        {
            var parentUserId = _currentUser.UserId;
            if (string.IsNullOrEmpty(parentUserId))
                return Unauthorized();

            var result = await _parentRepo.GetChildDashboardAsync(parentUserId, studentId, schoolRegistrationId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
