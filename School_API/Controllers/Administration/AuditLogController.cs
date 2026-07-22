using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School_DTOs.Administration;

namespace School_API.Controllers.Administration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuditLogController : BaseController
    {
        private readonly IAdministrationService _adminService;
        private readonly ITenantService _tenantService;

        public AuditLogController(
            IAdministrationService adminService,
            ITenantService tenantService,
            ICurrentUserService currentUserService) : base(currentUserService)
        {
            _adminService = adminService;
            _tenantService = tenantService;
        }

        private int SchoolId => _tenantService.GetTenantId() ?? 1;

        [HttpGet]
        public async Task<IActionResult> GetAuditLogs([FromQuery] string? tableName, [FromQuery] string? action, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var filter = new AuditLogFilterDto { TableName = tableName, Action = action, FromDate = fromDate, ToDate = toDate };
            var r = await _adminService.GetAuditLogsAsync(filter, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }
    }
}
