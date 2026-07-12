using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School_DTOs;
using School_DTOs.Analytics;

namespace School_API.Controllers.Analytics
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnalyticsController : BaseController
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly ITenantService _tenantService;

        public AnalyticsController(
            IAnalyticsService analyticsService,
            ITenantService tenantService,
            ICurrentUserService currentUserService) : base(currentUserService)
        {
            _analyticsService = analyticsService;
            _tenantService = tenantService;
        }

        private int SchoolId => _tenantService.GetTenantId() ?? 1;

        // ── Dashboard Config ───────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetDashboardConfig([FromQuery] string role)
        {
            var r = await _analyticsService.GetDashboardConfigAsync(role, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDashboardConfig([FromBody] DashboardConfigDto dto)
        {
            var r = await _analyticsService.SaveDashboardConfigAsync(dto, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ── Dashboard Widgets ──────────────────────────────────────────────────

        [HttpGet("{configId}")]
        public async Task<IActionResult> GetDashboardWidgets(int configId)
        {
            var r = await _analyticsService.GetDashboardWidgetsAsync(configId, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDashboardWidget([FromBody] DashboardWidgetDto dto)
        {
            var r = await _analyticsService.SaveDashboardWidgetAsync(dto, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDashboardWidget(int id)
        {
            var r = await _analyticsService.DeleteDashboardWidgetAsync(id, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ── Report Templates ───────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetReportTemplates([FromQuery] string? reportType, [FromQuery] string? searchTerm)
        {
            var filter = new ReportTemplateFilterDto { ReportType = reportType, SearchTerm = searchTerm };
            var r = await _analyticsService.GetReportTemplatesAsync(filter, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportTemplateById(int id)
        {
            var r = await _analyticsService.GetReportTemplateByIdAsync(id, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SaveReportTemplate([FromBody] ReportTemplateDto dto)
        {
            var r = await _analyticsService.SaveReportTemplateAsync(dto, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReportTemplate(int id)
        {
            var r = await _analyticsService.DeleteReportTemplateAsync(id, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> RunReportTemplate(int id, [FromBody] Dictionary<string, string> parameters)
        {
            var r = await _analyticsService.RunReportTemplateAsync(id, parameters, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ── Analytics KPIs ─────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetAnalyticsKpis([FromQuery] string? searchTerm)
        {
            var filter = new AnalyticsKpiFilterDto { SearchTerm = searchTerm };
            var r = await _analyticsService.GetAnalyticsKpisAsync(filter, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnalyticsKpiById(int id)
        {
            var r = await _analyticsService.GetAnalyticsKpiByIdAsync(id, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAnalyticsKpi([FromBody] AnalyticsKpiDto dto)
        {
            var r = await _analyticsService.SaveAnalyticsKpiAsync(dto, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnalyticsKpi(int id)
        {
            var r = await _analyticsService.DeleteAnalyticsKpiAsync(id, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }
    }
}
