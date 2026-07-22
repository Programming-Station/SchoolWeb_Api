using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Services.Administration;
using School_API.Common.Interface;

namespace School_API.Controllers.Administration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,SUPERADMIN")]
    public class TenantAdminController : BaseController
    {
        private readonly ITenantAdminService _tenantAdminService;

        public TenantAdminController(ITenantAdminService tenantAdminService, ICurrentUserService currentUserService)
            : base(currentUserService)
        {
            _tenantAdminService = tenantAdminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTenants([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, [FromQuery] string? status = null)
        {
            var result = await _tenantAdminService.GetAllTenantsAsync(pageNumber, pageSize, search, status);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTenantById(int id)
        {
            var result = await _tenantAdminService.GetTenantByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> ApproveTenant(int id)
        {
            var result = await _tenantAdminService.ApproveTenantAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> RejectTenant(int id, [FromQuery] string? reason = null)
        {
            var result = await _tenantAdminService.RejectTenantAsync(id, reason);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> SuspendTenant(int id, [FromQuery] string? reason = null)
        {
            var result = await _tenantAdminService.SuspendTenantAsync(id, reason);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> ActivateTenant(int id)
        {
            var result = await _tenantAdminService.ActivateTenantAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTenantQuota(int id, [FromQuery] int? maxStudents)
        {
            var result = await _tenantAdminService.UpdateTenantQuotaAsync(id, maxStudents);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTenantStats()
        {
            var result = await _tenantAdminService.GetTenantStatsAsync();
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
