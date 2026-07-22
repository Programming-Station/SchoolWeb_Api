using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Domain.Analytics;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Analytics;

namespace School.Services.Analytics
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly SchoolDbContext _context;
        private readonly IMapper _mapper;

        public AnalyticsService(SchoolDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ── Dashboard Config ───────────────────────────────────────────────────

        public async Task<APIResponse<DashboardConfigDto>> GetDashboardConfigAsync(string role, int schoolId)
        {
            var config = await _context.Set<DashboardConfig>()
                .Include(c => c.Widgets)
                .FirstOrDefaultAsync(c => c.RoleScope == role && c.SchoolRegistrationId == schoolId);

            if (config == null)
            {
                // Create a default config for this role if none exists
                config = new DashboardConfig
                {
                    RoleScope = role,
                    Name = role + " Dashboard",
                    SchoolRegistrationId = schoolId,
                    Widgets = new List<DashboardWidget>()
                };
                _context.Set<DashboardConfig>().Add(config);
                await _context.SaveChangesAsync();
            }

            var dto = _mapper.Map<DashboardConfigDto>(config);
            return new APIResponse<DashboardConfigDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dto
            };
        }

        public async Task<APIResponse<DashboardConfigDto>> SaveDashboardConfigAsync(DashboardConfigDto dto, int schoolId)
        {
            var config = await _context.Set<DashboardConfig>()
                .Include(c => c.Widgets)
                .FirstOrDefaultAsync(c => c.Id == dto.Id && c.SchoolRegistrationId == schoolId);

            if (config == null)
            {
                config = new DashboardConfig { SchoolRegistrationId = schoolId };
                _context.Set<DashboardConfig>().Add(config);
            }

            _mapper.Map(dto, config);
            config.SchoolRegistrationId = schoolId; // Ensure correct tenant

            await _context.SaveChangesAsync();

            var savedDto = _mapper.Map<DashboardConfigDto>(config);
            return new APIResponse<DashboardConfigDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = savedDto
            };
        }

        // ── Dashboard Widgets ──────────────────────────────────────────────────

        public async Task<APIResponse<List<DashboardWidgetDto>>> GetDashboardWidgetsAsync(int configId, int schoolId)
        {
            var widgets = await _context.Set<DashboardWidget>()
                .Where(w => w.DashboardConfigId == configId && w.SchoolRegistrationId == schoolId)
                .ToListAsync();

            var dtos = _mapper.Map<List<DashboardWidgetDto>>(widgets);
            return new APIResponse<List<DashboardWidgetDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dtos
            };
        }

        public async Task<APIResponse<DashboardWidgetDto>> SaveDashboardWidgetAsync(DashboardWidgetDto dto, int schoolId)
        {
            DashboardWidget? widget = null;
            if (dto.Id.HasValue && dto.Id.Value > 0)
            {
                widget = await _context.Set<DashboardWidget>()
                    .FirstOrDefaultAsync(w => w.Id == dto.Id.Value && w.SchoolRegistrationId == schoolId);
            }

            if (widget == null)
            {
                widget = new DashboardWidget { SchoolRegistrationId = schoolId };
                _context.Set<DashboardWidget>().Add(widget);
            }

            _mapper.Map(dto, widget);
            widget.SchoolRegistrationId = schoolId;

            await _context.SaveChangesAsync();

            var savedDto = _mapper.Map<DashboardWidgetDto>(widget);
            return new APIResponse<DashboardWidgetDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = savedDto
            };
        }

        public async Task<APIResponse<bool>> DeleteDashboardWidgetAsync(int id, int schoolId)
        {
            var widget = await _context.Set<DashboardWidget>()
                .FirstOrDefaultAsync(w => w.Id == id && w.SchoolRegistrationId == schoolId);

            if (widget == null)
            {
                return new APIResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Widget not found",
                    Data = false
                };
            }

            _context.Set<DashboardWidget>().Remove(widget);
            await _context.SaveChangesAsync();

            return new APIResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = true
            };
        }

        // ── Report Templates ───────────────────────────────────────────────────

        public async Task<APIResponse<List<ReportTemplateDto>>> GetReportTemplatesAsync(ReportTemplateFilterDto filter, int schoolId)
        {
            var q = _context.Set<ReportTemplate>()
                .Where(r => r.SchoolRegistrationId == schoolId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.ReportType))
            {
                q = q.Where(r => r.Category == filter.ReportType);
            }
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                q = q.Where(r => r.Name.Contains(filter.SearchTerm));
            }

            var list = await q.ToListAsync();
            var dtos = _mapper.Map<List<ReportTemplateDto>>(list);

            return new APIResponse<List<ReportTemplateDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dtos
            };
        }

        public async Task<APIResponse<ReportTemplateDto>> GetReportTemplateByIdAsync(int id, int schoolId)
        {
            var template = await _context.Set<ReportTemplate>()
                .FirstOrDefaultAsync(r => r.Id == id && r.SchoolRegistrationId == schoolId);

            if (template == null)
            {
                return new APIResponse<ReportTemplateDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Report template not found"
                };
            }

            var dto = _mapper.Map<ReportTemplateDto>(template);
            return new APIResponse<ReportTemplateDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dto
            };
        }

        public async Task<APIResponse<ReportTemplateDto>> SaveReportTemplateAsync(ReportTemplateDto dto, int schoolId)
        {
            ReportTemplate? template = null;
            if (dto.Id.HasValue && dto.Id.Value > 0)
            {
                template = await _context.Set<ReportTemplate>()
                    .FirstOrDefaultAsync(r => r.Id == dto.Id.Value && r.SchoolRegistrationId == schoolId);
            }

            if (template == null)
            {
                template = new ReportTemplate { SchoolRegistrationId = schoolId };
                _context.Set<ReportTemplate>().Add(template);
            }

            _mapper.Map(dto, template);
            template.SchoolRegistrationId = schoolId;

            await _context.SaveChangesAsync();

            var savedDto = _mapper.Map<ReportTemplateDto>(template);
            return new APIResponse<ReportTemplateDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = savedDto
            };
        }

        public async Task<APIResponse<bool>> DeleteReportTemplateAsync(int id, int schoolId)
        {
            var template = await _context.Set<ReportTemplate>()
                .FirstOrDefaultAsync(r => r.Id == id && r.SchoolRegistrationId == schoolId);

            if (template == null)
            {
                return new APIResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Report template not found",
                    Data = false
                };
            }

            _context.Set<ReportTemplate>().Remove(template);
            await _context.SaveChangesAsync();

            return new APIResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = true
            };
        }

        public async Task<APIResponse<object>> RunReportTemplateAsync(int id, Dictionary<string, string> parameters, int schoolId)
        {
            var template = await _context.Set<ReportTemplate>()
                .FirstOrDefaultAsync(r => r.Id == id && r.SchoolRegistrationId == schoolId);

            if (template == null)
            {
                return new APIResponse<object>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Report template not found"
                };
            }

            // Execute dynamic query or simulate data based on the type
            object reportData;
            if (template.Category.Equals("Student", StringComparison.OrdinalIgnoreCase))
            {
                var students = await _context.Students
                    .Where(s => s.SchoolRegistrationId == schoolId)
                    .Select(s => new { s.Id, s.Name, s.StudentId, Status = s.Status != null ? s.Status.Name : "Active" })
                    .Take(100)
                    .ToListAsync();
                reportData = students;
            }
            else if (template.Category.Equals("Fee", StringComparison.OrdinalIgnoreCase))
            {
                var feeCollections = await _context.Set<global::School.Domain.FeeManagnment.FeePayment>()
                    .Where(f => f.SchoolRegistrationId == schoolId)
                    .Select(f => new { f.Id, f.StudentId, f.AmountPaid, f.PaymentDate, f.PaymentMode, f.ReceiptNo })
                    .Take(100)
                    .ToListAsync();
                reportData = feeCollections;
            }
            else
            {
                // Return generic/simulated structure for other types
                reportData = new[]
                {
                    new { Key = "Metric1", Value = 120 },
                    new { Key = "Metric2", Value = 450 },
                    new { Key = "Metric3", Value = 95 }
                };
            }

            return new APIResponse<object>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = reportData
            };
        }

        // ── Analytics KPIs ─────────────────────────────────────────────────────

        public async Task<APIResponse<List<AnalyticsKpiDto>>> GetAnalyticsKpisAsync(AnalyticsKpiFilterDto filter, int schoolId)
        {
            var q = _context.Set<AnalyticsKpi>()
                .Where(k => k.SchoolRegistrationId == schoolId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                q = q.Where(k => k.Name.Contains(filter.SearchTerm) || k.Code.Contains(filter.SearchTerm));
            }

            var list = await q.ToListAsync();
            var dtos = _mapper.Map<List<AnalyticsKpiDto>>(list);

            return new APIResponse<List<AnalyticsKpiDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dtos
            };
        }

        public async Task<APIResponse<AnalyticsKpiDto>> GetAnalyticsKpiByIdAsync(int id, int schoolId)
        {
            var kpi = await _context.Set<AnalyticsKpi>()
                .FirstOrDefaultAsync(k => k.Id == id && k.SchoolRegistrationId == schoolId);

            if (kpi == null)
            {
                return new APIResponse<AnalyticsKpiDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "KPI not found"
                };
            }

            var dto = _mapper.Map<AnalyticsKpiDto>(kpi);
            return new APIResponse<AnalyticsKpiDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dto
            };
        }

        public async Task<APIResponse<AnalyticsKpiDto>> SaveAnalyticsKpiAsync(AnalyticsKpiDto dto, int schoolId)
        {
            AnalyticsKpi? kpi = null;
            if (dto.Id.HasValue && dto.Id.Value > 0)
            {
                kpi = await _context.Set<AnalyticsKpi>()
                    .FirstOrDefaultAsync(k => k.Id == dto.Id.Value && k.SchoolRegistrationId == schoolId);
            }

            if (kpi == null)
            {
                kpi = new AnalyticsKpi { SchoolRegistrationId = schoolId };
                _context.Set<AnalyticsKpi>().Add(kpi);
            }

            _mapper.Map(dto, kpi);
            kpi.SchoolRegistrationId = schoolId;

            await _context.SaveChangesAsync();

            var savedDto = _mapper.Map<AnalyticsKpiDto>(kpi);
            return new APIResponse<AnalyticsKpiDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = savedDto
            };
        }

        public async Task<APIResponse<bool>> DeleteAnalyticsKpiAsync(int id, int schoolId)
        {
            var kpi = await _context.Set<AnalyticsKpi>()
                .FirstOrDefaultAsync(k => k.Id == id && k.SchoolRegistrationId == schoolId);

            if (kpi == null)
            {
                return new APIResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "KPI not found",
                    Data = false
                };
            }

            _context.Set<AnalyticsKpi>().Remove(kpi);
            await _context.SaveChangesAsync();

            return new APIResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = true
            };
        }
    }
}
