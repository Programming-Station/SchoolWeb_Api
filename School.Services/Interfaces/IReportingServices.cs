using School.Domain.Reporting;
using School_DTOs.Reporting;

namespace School.Services.Interfaces
{
    /// <summary>
    /// Core reporting engine — the single entry point for all report generation.
    /// Handles RDLC rendering, export format selection, branding injection, and execution logging.
    /// </summary>
    public interface IReportingEngineService
    {
        /// <summary>Generate a report and return the file bytes.</summary>
        Task<byte[]> GenerateAsync(ReportExecutionRequest request, string generatedBy, int? tenantId = null);

        /// <summary>Generate a report preview as base64 PDF for embedding in an iframe.</summary>
        Task<string> GeneratePreviewBase64Async(ReportExecutionRequest request, int? tenantId = null);

        /// <summary>Get the list of expected parameters for a given report code.</summary>
        Task<List<ReportParameterDto>> GetParametersAsync(string reportCode, int? tenantId = null);

        /// <summary>Toggle a report as favorite for the current tenant.</summary>
        Task SetFavoriteAsync(int templateId, bool isFavorite, int? tenantId = null);

        /// <summary>Get report templates grouped by category for the dashboard.</summary>
        Task<List<ReportCategoryDto>> GetDashboardCategoriesAsync(int? tenantId = null);

        /// <summary>Search reports across all categories.</summary>
        Task<List<ReportTemplateDto>> SearchReportsAsync(string searchTerm, int? tenantId = null);
    }

    /// <summary>Service for managing report categories.</summary>
    public interface IReportCategoryService
    {
        Task<List<ReportCategoryDto>> GetAllAsync(int? tenantId = null);
        Task<ReportCategoryDto?> GetByIdAsync(int id);
        Task<ReportCategoryDto> CreateAsync(CreateReportCategoryRequest request, int? tenantId = null);
        Task<ReportCategoryDto> UpdateAsync(int id, CreateReportCategoryRequest request);
        Task DeleteAsync(int id);
    }

    /// <summary>Service for managing report templates (CRUD + RDLC upload).</summary>
    public interface IReportTemplateService
    {
        Task<List<ReportTemplateDto>> GetAllAsync(int? tenantId = null, int? categoryId = null, string? search = null);
        Task<ReportTemplateDto?> GetByIdAsync(int id);
        Task<ReportTemplateDto?> GetByCodeAsync(string code, int? tenantId = null);
        Task<ReportTemplateDto> CreateAsync(CreateReportTemplateRequest request, int? tenantId = null);
        Task<ReportTemplateDto> UpdateAsync(int id, CreateReportTemplateRequest request);
        Task DeleteAsync(int id);
        Task<bool> UploadRdlcAsync(int templateId, byte[] rdlcContent, string fileName);
        Task<byte[]?> DownloadRdlcAsync(int templateId);
        Task<bool> ValidateRdlcAsync(byte[] rdlcContent);
    }

    /// <summary>Service for managing report parameters.</summary>
    public interface IReportParameterService
    {
        Task<List<ReportParameterDto>> GetForTemplateAsync(int templateId);
        Task SaveForTemplateAsync(int templateId, List<ReportParameterDto> parameters);
    }

    /// <summary>Service for reading and writing report execution history.</summary>
    public interface IReportHistoryService
    {
        Task<ReportHistoryPagedResult> GetHistoryAsync(
            int? tenantId, int? templateId, string? generatedBy,
            DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 20);

        Task<ReportHistory?> GetByIdAsync(int id);
        Task LogDownloadAsync(int historyId, string downloadedBy);
        Task LogPrintAsync(int historyId, string printedBy, string? printerName = null);
        Task DeleteAsync(int id);
        Task CleanupOldAsync(int daysToKeep = 90);
    }

    /// <summary>Service for permission matrix management.</summary>
    public interface IReportPermissionService
    {
        Task<List<ReportPermissionDto>> GetAllAsync(int? tenantId = null, int? templateId = null);
        Task<ReportPermissionDto?> GetForRoleAsync(int templateId, string roleName, int? tenantId = null);
        Task SaveAsync(ReportPermissionDto dto, int? tenantId = null);
        Task DeleteAsync(int id);
        Task<bool> HasPermissionAsync(int templateId, string roleName, string action, int? tenantId = null);
    }

    /// <summary>Service for scheduling reports with cron expressions.</summary>
    public interface IReportScheduleService
    {
        Task<List<ReportScheduleDto>> GetAllAsync(int? tenantId = null);
        Task<ReportScheduleDto?> GetByIdAsync(int id);
        Task<ReportScheduleDto> CreateAsync(CreateReportScheduleRequest request, int? tenantId = null);
        Task<ReportScheduleDto> UpdateAsync(int id, CreateReportScheduleRequest request);
        Task DeleteAsync(int id);
        Task ToggleActiveAsync(int id, bool isActive);
        Task<List<ReportScheduleDto>> GetDueSchedulesAsync();
        Task ExecuteScheduledAsync(int scheduleId);
    }

    /// <summary>Service for QR code and barcode generation.</summary>
    public interface IQrBarcodeService
    {
        Task<byte[]> GenerateQrAsync(string data, int size = 250);
        Task<string> GenerateQrBase64Async(string data, int size = 250);
        Task<byte[]> GenerateBarcodeAsync(string data, string format = "Code128", int width = 300, int height = 100);
        Task<string> GenerateBarcodeBase64Async(string data, string format = "Code128", int width = 300, int height = 100);

        // Entity-specific QR generators
        Task<byte[]> GenerateStudentQrAsync(int studentId, int? tenantId = null);
        Task<byte[]> GenerateEmployeeQrAsync(int employeeId, int? tenantId = null);
        Task<byte[]> GenerateFeeReceiptQrAsync(int paymentId, int? tenantId = null);
        Task<byte[]> GenerateCertificateQrAsync(string certificateNumber, int? tenantId = null);
        Task<byte[]> GenerateBookBarcodeAsync(string isbn, int? tenantId = null);
        Task<byte[]> GenerateInventoryBarcodeAsync(string itemCode, int? tenantId = null);
        Task<byte[]> GenerateStudentIdBarcodeAsync(string studentId, int? tenantId = null);
    }

    /// <summary>Service for emailing generated reports.</summary>
    public interface IReportEmailService
    {
        Task<bool> EmailReportAsync(ReportEmailRequest request, string triggeredBy, int? tenantId = null);
    }

    /// <summary>
    /// Service for non-RDLC exports: CSV streaming, Excel with styling, XML, JSON.
    /// </summary>
    public interface IReportExportService
    {
        Task<byte[]> ExportToCsvAsync(IEnumerable<object> data, string[] columns);
        Task<byte[]> ExportToExcelAsync(
            IEnumerable<object> data, string sheetName = "Report",
            string[]? columns = null, bool freezeHeader = true, bool autoWidth = true);
        Task<byte[]> ExportToXmlAsync(IEnumerable<object> data, string rootElement = "Report");
        Task<byte[]> ExportToJsonAsync(IEnumerable<object> data);
        Task<byte[]> ExportMultiSheetExcelAsync(Dictionary<string, IEnumerable<object>> sheets);
    }

    /// <summary>Service for managing and retrieving tenant report branding.</summary>
    public interface IReportingBrandingService
    {
        Task<ReportBrandingDto> GetBrandingAsync(int? tenantId = null);
        Task<ReportBrandingDto> SaveBrandingAsync(ReportBrandingDto dto);
        Task<string?> UploadLogoAsync(int tenantId, byte[] imageBytes, string fileName, string logoType);
        void InvalidateBrandingCache(int tenantId);
    }
}
