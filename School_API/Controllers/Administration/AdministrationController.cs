using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School_DTOs;
using School_DTOs.Administration;

namespace School_API.Controllers.Administration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdministrationController : BaseController
    {
        private readonly IAdministrationService _adminService;
        private readonly ITenantService _tenantService;

        public AdministrationController(
            IAdministrationService adminService,
            ITenantService tenantService,
            ICurrentUserService currentUserService) : base(currentUserService)
        {
            _adminService = adminService;
            _tenantService = tenantService;
        }

        private int SchoolId => _tenantService.GetTenantId() ?? 1;

        // ── School Branch Management ──────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetBranches()
        {
            var r = await _adminService.GetBranchesAsync(SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBranchById(int id)
        {
            var r = await _adminService.GetBranchByIdAsync(id, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBranch([FromBody] SchoolBranchDto dto)
        {
            var r = await _adminService.SaveBranchAsync(dto, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var r = await _adminService.DeleteBranchAsync(id, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ── Workflow Engine ───────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetWorkflows()
        {
            var r = await _adminService.GetWorkflowsAsync(SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkflowById(int id)
        {
            var r = await _adminService.GetWorkflowByIdAsync(id, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SaveWorkflow([FromBody] WorkflowDefinitionDto dto)
        {
            var r = await _adminService.SaveWorkflowAsync(dto, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkflow(int id)
        {
            var r = await _adminService.DeleteWorkflowAsync(id, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ── Workflow Instances & Approval ─────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetWorkflowInstances([FromQuery] string? status, [FromQuery] string? targetEntityName)
        {
            var filter = new WorkflowInstanceFilterDto { Status = status, TargetEntityName = targetEntityName };
            var r = await _adminService.GetWorkflowInstancesAsync(filter, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> StartWorkflowInstance([FromQuery] int definitionId, [FromQuery] string entityName, [FromQuery] int entityId)
        {
            var r = await _adminService.StartWorkflowInstanceAsync(definitionId, entityName, entityId, UserName, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitApproval([FromQuery] int instanceId, [FromQuery] string action, [FromQuery] string comments)
        {
            var r = await _adminService.SubmitApprovalAsync(instanceId, UserName, action, comments, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ── Audit Logs ────────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetAuditLogs([FromQuery] string? tableName, [FromQuery] string? action, [FromQuery] System.DateTime? fromDate, [FromQuery] System.DateTime? toDate)
        {
            var filter = new AuditLogFilterDto { TableName = tableName, Action = action, FromDate = fromDate, ToDate = toDate };
            var r = await _adminService.GetAuditLogsAsync(filter, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ── System Settings & Operations ──────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetSystemSettings()
        {
            var r = await _adminService.GetSystemSettingsAsync(SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSystemSettings([FromBody] Dictionary<string, string> settings)
        {
            var r = await _adminService.SaveSystemSettingsAsync(settings, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> TriggerBackup()
        {
            var r = await _adminService.TriggerBackupAsync(SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> TriggerRestore([FromQuery] string backupFile)
        {
            var r = await _adminService.TriggerRestoreAsync(backupFile, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> ImportData([FromQuery] string entityName, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            using var ms = new System.IO.MemoryStream();
            await file.CopyToAsync(ms);
            var fileBytes = ms.ToArray();

            var r = await _adminService.ImportDataAsync(entityName, fileBytes, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportData([FromQuery] string entityName)
        {
            var r = await _adminService.ExportDataAsync(entityName, SchoolId);
            if (!r.Success || r.Data == null)
            {
                return StatusCode((int)r.StatusCode, r);
            }
            return File(r.Data, "text/csv", $"{entityName}_export.csv");
        }
    }
}
