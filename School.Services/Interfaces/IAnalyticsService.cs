using School_DTOs;
using School_DTOs.Analytics;

namespace School.Services.Interfaces
{
    public interface IAnalyticsService
    {
        // Dashboard Config
        Task<APIResponse<DashboardConfigDto>> GetDashboardConfigAsync(string role, int schoolId);
        Task<APIResponse<DashboardConfigDto>> SaveDashboardConfigAsync(DashboardConfigDto dto, int schoolId);

        // Dashboard Widgets
        Task<APIResponse<List<DashboardWidgetDto>>> GetDashboardWidgetsAsync(int configId, int schoolId);
        Task<APIResponse<DashboardWidgetDto>> SaveDashboardWidgetAsync(DashboardWidgetDto dto, int schoolId);
        Task<APIResponse<bool>> DeleteDashboardWidgetAsync(int id, int schoolId);

        // Report Templates
        Task<APIResponse<List<ReportTemplateDto>>> GetReportTemplatesAsync(ReportTemplateFilterDto filter, int schoolId);
        Task<APIResponse<ReportTemplateDto>> GetReportTemplateByIdAsync(int id, int schoolId);
        Task<APIResponse<ReportTemplateDto>> SaveReportTemplateAsync(ReportTemplateDto dto, int schoolId);
        Task<APIResponse<bool>> DeleteReportTemplateAsync(int id, int schoolId);
        Task<APIResponse<object>> RunReportTemplateAsync(int id, Dictionary<string, string> parameters, int schoolId);

        // Analytics KPIs
        Task<APIResponse<List<AnalyticsKpiDto>>> GetAnalyticsKpisAsync(AnalyticsKpiFilterDto filter, int schoolId);
        Task<APIResponse<AnalyticsKpiDto>> GetAnalyticsKpiByIdAsync(int id, int schoolId);
        Task<APIResponse<AnalyticsKpiDto>> SaveAnalyticsKpiAsync(AnalyticsKpiDto dto, int schoolId);
        Task<APIResponse<bool>> DeleteAnalyticsKpiAsync(int id, int schoolId);
    }
}
