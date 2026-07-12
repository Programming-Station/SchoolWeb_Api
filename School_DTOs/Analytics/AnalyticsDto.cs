using System;
using System.Collections.Generic;

namespace School_DTOs.Analytics
{
    public class DashboardConfigDto : BaseDto
    {
        public string Role { get; set; } = null!;
        public string LayoutJson { get; set; } = null!;
        public int SchoolRegistrationId { get; set; }
        public List<DashboardWidgetDto> Widgets { get; set; } = new();
    }

    public class DashboardWidgetDto : BaseDto
    {
        public int DashboardConfigId { get; set; }
        public string Title { get; set; } = null!;
        public string WidgetType { get; set; } = null!; // e.g. Chart, Table, Metric
        public string DataSourceUrl { get; set; } = null!;
        public int ColSpan { get; set; } = 1;
        public int RowSpan { get; set; } = 1;
        public int SchoolRegistrationId { get; set; }
    }

    public class ReportTemplateDto : BaseDto
    {
        public string Name { get; set; } = null!;
        public string ReportType { get; set; } = null!; // e.g. Student, Fee, Attendance
        public string QueryDefinitionJson { get; set; } = null!;
        public string LayoutTemplate { get; set; } = null!;
        public int SchoolRegistrationId { get; set; }
    }

    public class AnalyticsKpiDto : BaseDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? CalculationFormula { get; set; }
        public decimal TargetValue { get; set; }
        public decimal CurrentValue { get; set; }
        public int SchoolRegistrationId { get; set; }
    }

    public class AnalyticsKpiFilterDto
    {
        public string? SearchTerm { get; set; }
    }

    public class ReportTemplateFilterDto
    {
        public string? ReportType { get; set; }
        public string? SearchTerm { get; set; }
    }
}
