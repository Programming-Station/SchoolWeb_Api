using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs.Reporting;
using System.Security.Claims;

namespace School_API.Controllers
{
    /// <summary>
    /// Enterprise Reporting Engine — REST API Controller.
    /// Exposes 30+ endpoints covering the full reporting lifecycle:
    /// categories, templates, parameters, execution, export, email, print,
    /// history, scheduling, branding, permissions, and QR/barcode generation.
    /// </summary>
    [ApiController]
    [Route("api/reporting")]
    [Authorize]
    public class ReportingController : ControllerBase
    {
        private readonly IReportingEngineService _engine;
        private readonly IReportCategoryService _categoryService;
        private readonly IReportTemplateService _templateService;
        private readonly IReportHistoryService _historyService;
        private readonly IReportScheduleService _scheduleService;
        private readonly IReportPermissionService _permissionService;
        private readonly IReportingBrandingService _brandingService;
        private readonly IReportExportService _exportService;
        private readonly IReportEmailService _emailService;
        private readonly IQrBarcodeService _qrBarcodeService;
        private readonly SchoolDbContext _db;
        private readonly ILogger<ReportingController> _logger;

        public ReportingController(
            IReportingEngineService engine,
            IReportCategoryService categoryService,
            IReportTemplateService templateService,
            IReportHistoryService historyService,
            IReportScheduleService scheduleService,
            IReportPermissionService permissionService,
            IReportingBrandingService brandingService,
            IReportExportService exportService,
            IReportEmailService emailService,
            IQrBarcodeService qrBarcodeService,
            SchoolDbContext db,
            ILogger<ReportingController> logger)
        {
            _engine = engine;
            _categoryService = categoryService;
            _templateService = templateService;
            _historyService = historyService;
            _scheduleService = scheduleService;
            _permissionService = permissionService;
            _brandingService = brandingService;
            _exportService = exportService;
            _emailService = emailService;
            _qrBarcodeService = qrBarcodeService;
            _db = db;
            _logger = logger;
        }

        // ─── Report Dashboard ─────────────────────────────────────────────────

        /// <summary>Get the reporting dashboard — categories with report counts.</summary>
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var tenantId = GetTenantId();
            var categories = await _engine.GetDashboardCategoriesAsync(tenantId);
            return Ok(new { success = true, data = categories });
        }

        /// <summary>Full-text search across all reports.</summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchReports([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest(new { success = false, message = "Search term required." });

            var tenantId = GetTenantId();
            var results = await _engine.SearchReportsAsync(q, tenantId);
            return Ok(new { success = true, data = results, count = results.Count });
        }

        // ─── Categories ───────────────────────────────────────────────────────

        /// <summary>Get all report categories.</summary>
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var cats = await _categoryService.GetAllAsync(GetTenantId());
            return Ok(new { success = true, data = cats });
        }

        /// <summary>Get a category by ID.</summary>
        [HttpGet("categories/{id:int}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var cat = await _categoryService.GetByIdAsync(id);
            return cat == null ? NotFound() : Ok(new { success = true, data = cat });
        }

        /// <summary>Create a report category.</summary>
        [HttpPost("categories")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateReportCategoryRequest request)
        {
            var cat = await _categoryService.CreateAsync(request, GetTenantId());
            return Ok(new { success = true, data = cat });
        }

        /// <summary>Update a report category.</summary>
        [HttpPut("categories/{id:int}")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateReportCategoryRequest request)
        {
            var cat = await _categoryService.UpdateAsync(id, request);
            return Ok(new { success = true, data = cat });
        }

        /// <summary>Delete a report category.</summary>
        [HttpDelete("categories/{id:int}")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteAsync(id);
            return Ok(new { success = true, message = "Category deleted." });
        }

        // ─── Templates ────────────────────────────────────────────────────────

        /// <summary>List report templates with optional category filter and search.</summary>
        [HttpGet("templates")]
        public async Task<IActionResult> GetTemplates(
            [FromQuery] int? categoryId,
            [FromQuery] string? search,
            [FromQuery] string? module)
        {
            var templates = await _templateService.GetAllAsync(GetTenantId(), categoryId, search);
            if (!string.IsNullOrWhiteSpace(module))
                templates = templates.Where(t =>
                    string.Equals(t.ModuleTag, module, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(new { success = true, data = templates, count = templates.Count });
        }

        /// <summary>Get a template with its parameter definitions.</summary>
        [HttpGet("templates/{id:int}")]
        public async Task<IActionResult> GetTemplate(int id)
        {
            var template = await _templateService.GetByIdAsync(id);
            return template == null ? NotFound() : Ok(new { success = true, data = template });
        }

        /// <summary>Get a template by its code.</summary>
        [HttpGet("templates/by-code/{code}")]
        public async Task<IActionResult> GetTemplateByCode(string code)
        {
            var template = await _templateService.GetByCodeAsync(code, GetTenantId());
            return template == null ? NotFound() : Ok(new { success = true, data = template });
        }

        /// <summary>Create a new report template.</summary>
        [HttpPost("templates")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> CreateTemplate([FromBody] CreateReportTemplateRequest request)
        {
            var template = await _templateService.CreateAsync(request, GetTenantId());
            return Ok(new { success = true, data = template });
        }

        /// <summary>Update a report template.</summary>
        [HttpPut("templates/{id:int}")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> UpdateTemplate(int id, [FromBody] CreateReportTemplateRequest request)
        {
            var template = await _templateService.UpdateAsync(id, request);
            return Ok(new { success = true, data = template });
        }

        /// <summary>Delete a report template.</summary>
        [HttpDelete("templates/{id:int}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteTemplate(int id)
        {
            await _templateService.DeleteAsync(id);
            return Ok(new { success = true, message = "Template deleted." });
        }

        /// <summary>Upload a custom RDLC file for a template.</summary>
        [HttpPost("templates/{id:int}/upload-rdlc")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> UploadRdlc(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { success = false, message = "No file uploaded." });

            if (!file.FileName.EndsWith(".rdlc", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { success = false, message = "Only .rdlc files are accepted." });

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var content = ms.ToArray();

            var success = await _templateService.UploadRdlcAsync(id, content, file.FileName);
            return success
                ? Ok(new { success = true, message = "RDLC uploaded successfully." })
                : BadRequest(new { success = false, message = "Invalid RDLC file. Please verify the XML structure." });
        }

        /// <summary>Download the RDLC template file.</summary>
        [HttpGet("templates/{id:int}/download-rdlc")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> DownloadRdlc(int id)
        {
            var template = await _templateService.GetByIdAsync(id);
            if (template == null) return NotFound();

            var bytes = await _templateService.DownloadRdlcAsync(id);
            if (bytes == null) return NotFound(new { message = "RDLC file not found." });

            return File(bytes, "application/xml",
                template.RdlcFileName ?? $"{template.ReportCode}.rdlc");
        }

        /// <summary>Toggle favorite status for a report template.</summary>
        [HttpPost("templates/{id:int}/favorite")]
        public async Task<IActionResult> ToggleFavorite(int id, [FromQuery] bool value = true)
        {
            await _engine.SetFavoriteAsync(id, value, GetTenantId());
            return Ok(new { success = true, isFavorite = value });
        }

        // ─── Parameters ───────────────────────────────────────────────────────

        /// <summary>Get parameter definitions for a report by code.</summary>
        [HttpGet("parameters/{reportCode}")]
        public async Task<IActionResult> GetParameters(string reportCode)
        {
            var parameters = await _engine.GetParametersAsync(reportCode, GetTenantId());
            return Ok(new { success = true, data = parameters });
        }

        // ─── Report Execution ─────────────────────────────────────────────────

        /// <summary>
        /// Generate a report and stream the file (PDF, Excel, Word, CSV, etc.).
        /// </summary>
        [HttpPost("execute")]
        public async Task<IActionResult> Execute([FromBody] ReportExecutionRequest request)
        {
            var tenantId = GetTenantId();
            var generatedBy = GetCurrentUser();

            var fileBytes = await _engine.GenerateAsync(request, generatedBy, tenantId);

            var (contentType, ext) = GetContentTypeAndExt(request.Format);
            var reportName = request.ReportCode.Replace("_", " ");
            return File(fileBytes, contentType, $"{reportName}_{DateTime.Now:yyyyMMdd_HHmm}{ext}");
        }

        /// <summary>
        /// Generate a report preview and return as base64 PDF for embedding in an iframe.
        /// </summary>
        [HttpPost("preview")]
        public async Task<IActionResult> Preview([FromBody] ReportExecutionRequest request)
        {
            var tenantId = GetTenantId();
            var base64 = await _engine.GeneratePreviewBase64Async(request, tenantId);
            return Ok(new { success = true, base64Pdf = base64 });
        }

        // ─── Format-specific export endpoints ────────────────────────────────

        /// <summary>Export report as PDF.</summary>
        [HttpPost("export/pdf")]
        public async Task<IActionResult> ExportPdf([FromBody] ReportExecutionRequest request)
        {
            request.Format = "PDF";
            return await Execute(request);
        }

        /// <summary>Export report as Excel.</summary>
        [HttpPost("export/excel")]
        public async Task<IActionResult> ExportExcel([FromBody] ReportExecutionRequest request)
        {
            request.Format = "EXCEL";
            return await Execute(request);
        }

        /// <summary>Export report as Word.</summary>
        [HttpPost("export/word")]
        public async Task<IActionResult> ExportWord([FromBody] ReportExecutionRequest request)
        {
            request.Format = "WORD";
            return await Execute(request);
        }

        /// <summary>Export report as CSV.</summary>
        [HttpPost("export/csv")]
        public async Task<IActionResult> ExportCsv([FromBody] ReportExecutionRequest request)
        {
            // CSV uses ReportExportService for better streaming support
            request.Format = "CSV";
            return await Execute(request);
        }

        // ─── Email ────────────────────────────────────────────────────────────

        /// <summary>Generate and email a report as an attachment.</summary>
        [HttpPost("email")]
        public async Task<IActionResult> EmailReport([FromBody] ReportEmailRequest request)
        {
            var tenantId = GetTenantId();
            var triggeredBy = GetCurrentUser();

            if (!request.ToEmails.Any())
                return BadRequest(new { success = false, message = "At least one recipient is required." });

            var success = await _emailService.EmailReportAsync(request, triggeredBy, tenantId);
            return success
                ? Ok(new { success = true, message = $"Report queued for delivery to {request.ToEmails.Count} recipient(s)." })
                : StatusCode(500, new { success = false, message = "Failed to generate or queue the report email." });
        }

        // ─── Print ────────────────────────────────────────────────────────────

        /// <summary>Log a print event for a report execution.</summary>
        [HttpPost("print")]
        public async Task<IActionResult> LogPrint([FromBody] ReportPrintRequest request)
        {
            await _historyService.LogPrintAsync(
                request.ReportHistoryId, request.PrintedBy, request.PrinterName);
            return Ok(new { success = true, message = "Print event logged." });
        }

        // ─── History ──────────────────────────────────────────────────────────

        /// <summary>Get paginated report execution history.</summary>
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory(
            [FromQuery] int? templateId,
            [FromQuery] string? generatedBy,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _historyService.GetHistoryAsync(
                GetTenantId(), templateId, generatedBy, fromDate, toDate, page, pageSize);
            return Ok(new { success = true, data = result });
        }

        /// <summary>Log a download event for a history record.</summary>
        [HttpPost("history/{id:int}/download")]
        public async Task<IActionResult> LogDownload(int id)
        {
            await _historyService.LogDownloadAsync(id, GetCurrentUser());
            return Ok(new { success = true });
        }

        /// <summary>Delete a history record.</summary>
        [HttpDelete("history/{id:int}")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> DeleteHistory(int id)
        {
            await _historyService.DeleteAsync(id);
            return Ok(new { success = true, message = "History record deleted." });
        }

        /// <summary>Trigger cleanup of old history records.</summary>
        [HttpPost("history/cleanup")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CleanupHistory([FromQuery] int daysToKeep = 90)
        {
            await _historyService.CleanupOldAsync(daysToKeep);
            return Ok(new { success = true, message = $"History older than {daysToKeep} days cleaned up." });
        }

        // ─── Schedules ────────────────────────────────────────────────────────

        /// <summary>Get all report schedules.</summary>
        [HttpGet("schedules")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> GetSchedules()
        {
            var schedules = await _scheduleService.GetAllAsync(GetTenantId());
            return Ok(new { success = true, data = schedules });
        }

        /// <summary>Get a schedule by ID.</summary>
        [HttpGet("schedules/{id:int}")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> GetSchedule(int id)
        {
            var schedule = await _scheduleService.GetByIdAsync(id);
            return schedule == null ? NotFound() : Ok(new { success = true, data = schedule });
        }

        /// <summary>Create a new report schedule.</summary>
        [HttpPost("schedules")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateReportScheduleRequest request)
        {
            var schedule = await _scheduleService.CreateAsync(request, GetTenantId());
            return Ok(new { success = true, data = schedule });
        }

        /// <summary>Update a report schedule.</summary>
        [HttpPut("schedules/{id:int}")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> UpdateSchedule(int id, [FromBody] CreateReportScheduleRequest request)
        {
            var schedule = await _scheduleService.UpdateAsync(id, request);
            return Ok(new { success = true, data = schedule });
        }

        /// <summary>Delete a report schedule.</summary>
        [HttpDelete("schedules/{id:int}")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            await _scheduleService.DeleteAsync(id);
            return Ok(new { success = true, message = "Schedule deleted." });
        }

        /// <summary>Toggle a schedule's active state.</summary>
        [HttpPatch("schedules/{id:int}/toggle")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> ToggleSchedule(int id, [FromQuery] bool active)
        {
            await _scheduleService.ToggleActiveAsync(id, active);
            return Ok(new { success = true, isActive = active });
        }

        /// <summary>Manually trigger a scheduled report execution.</summary>
        [HttpPost("schedules/{id:int}/run")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> RunScheduleNow(int id)
        {
            await _scheduleService.ExecuteScheduledAsync(id);
            return Ok(new { success = true, message = "Schedule executed manually." });
        }

        // ─── Branding ─────────────────────────────────────────────────────────

        /// <summary>Get the tenant's report branding configuration.</summary>
        [HttpGet("branding")]
        public async Task<IActionResult> GetBranding()
        {
            var branding = await _brandingService.GetBrandingAsync(GetTenantId());
            return Ok(new { success = true, data = branding });
        }

        /// <summary>Create or update the tenant's report branding.</summary>
        [HttpPost("branding")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> SaveBranding([FromBody] ReportBrandingDto dto)
        {
            dto.TenantId = GetTenantId() ?? dto.TenantId;
            var saved = await _brandingService.SaveBrandingAsync(dto);
            return Ok(new { success = true, data = saved });
        }

        /// <summary>Upload a logo or signature image for branding.</summary>
        [HttpPost("branding/upload-image")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> UploadBrandingImage(
            IFormFile file,
            [FromQuery] string logoType = "HeaderLogo")
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { success = false, message = "No file provided." });

            var tenantId = GetTenantId() ?? 1;
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var path = await _brandingService.UploadLogoAsync(
                tenantId, ms.ToArray(), file.FileName, logoType);

            return path != null
                ? Ok(new { success = true, path, message = $"{logoType} uploaded." })
                : StatusCode(500, new { success = false, message = "Upload failed." });
        }

        // ─── Permissions ──────────────────────────────────────────────────────

        /// <summary>Get the permission matrix for all reports or a specific report.</summary>
        [HttpGet("permissions")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> GetPermissions([FromQuery] int? templateId)
        {
            var perms = await _permissionService.GetAllAsync(GetTenantId(), templateId);
            return Ok(new { success = true, data = perms });
        }

        /// <summary>Save a report permission (create or update).</summary>
        [HttpPost("permissions")]
        [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
        public async Task<IActionResult> SavePermission([FromBody] ReportPermissionDto dto)
        {
            await _permissionService.SaveAsync(dto, GetTenantId());
            return Ok(new { success = true, message = "Permission saved." });
        }

        // ─── QR Code ─────────────────────────────────────────────────────────

        /// <summary>Generate a QR code for any data string.</summary>
        [HttpPost("qr/generate")]
        public async Task<IActionResult> GenerateQr([FromBody] QrCodeRequest request)
        {
            if (request.AsBase64)
            {
                var b64 = await _qrBarcodeService.GenerateQrBase64Async(request.Data, request.Size);
                return Ok(new { success = true, base64 = b64 });
            }
            var bytes = await _qrBarcodeService.GenerateQrAsync(request.Data, request.Size);
            return File(bytes, "image/png", "qrcode.png");
        }

        /// <summary>Generate a student QR code.</summary>
        [HttpGet("qr/student/{studentId:int}")]
        public async Task<IActionResult> StudentQr(int studentId)
        {
            var bytes = await _qrBarcodeService.GenerateStudentQrAsync(studentId, GetTenantId());
            return File(bytes, "image/png", $"student_{studentId}_qr.png");
        }

        /// <summary>Generate an employee QR code.</summary>
        [HttpGet("qr/employee/{employeeId:int}")]
        public async Task<IActionResult> EmployeeQr(int employeeId)
        {
            var bytes = await _qrBarcodeService.GenerateEmployeeQrAsync(employeeId, GetTenantId());
            return File(bytes, "image/png", $"employee_{employeeId}_qr.png");
        }

        /// <summary>Generate a fee receipt QR code.</summary>
        [HttpGet("qr/fee-receipt/{paymentId:int}")]
        public async Task<IActionResult> FeeReceiptQr(int paymentId)
        {
            var bytes = await _qrBarcodeService.GenerateFeeReceiptQrAsync(paymentId, GetTenantId());
            return File(bytes, "image/png", $"fee_receipt_{paymentId}_qr.png");
        }

        /// <summary>Generate a certificate verification QR code.</summary>
        [HttpGet("qr/certificate/{certNo}")]
        public async Task<IActionResult> CertificateQr(string certNo)
        {
            var bytes = await _qrBarcodeService.GenerateCertificateQrAsync(certNo, GetTenantId());
            return File(bytes, "image/png", $"certificate_{certNo}_qr.png");
        }

        // ─── Barcodes ─────────────────────────────────────────────────────────

        /// <summary>Generate a barcode for any data.</summary>
        [HttpPost("barcode/generate")]
        public async Task<IActionResult> GenerateBarcode([FromBody] BarcodeRequest request)
        {
            if (request.AsBase64)
            {
                var b64 = await _qrBarcodeService.GenerateBarcodeBase64Async(
                    request.Data, request.Format, request.Width, request.Height);
                return Ok(new { success = true, base64 = b64 });
            }
            var bytes = await _qrBarcodeService.GenerateBarcodeAsync(
                request.Data, request.Format, request.Width, request.Height);
            return File(bytes, "image/png", "barcode.png");
        }

        /// <summary>Generate a library book barcode.</summary>
        [HttpGet("barcode/book/{isbn}")]
        public async Task<IActionResult> BookBarcode(string isbn)
        {
            var bytes = await _qrBarcodeService.GenerateBookBarcodeAsync(isbn, GetTenantId());
            return File(bytes, "image/png", $"book_{isbn}_barcode.png");
        }

        /// <summary>Generate a student ID barcode.</summary>
        [HttpGet("barcode/student/{studentId}")]
        public async Task<IActionResult> StudentBarcode(string studentId)
        {
            var bytes = await _qrBarcodeService.GenerateStudentIdBarcodeAsync(studentId, GetTenantId());
            return File(bytes, "image/png", $"student_{studentId}_barcode.png");
        }

        // ─── Private Helpers ──────────────────────────────────────────────────

        private int? GetTenantId()
        {
            var claim = User.FindFirst("TenantId") ?? User.FindFirst("schoolRegistrationId");
            return claim != null && int.TryParse(claim.Value, out var id) ? id : null;
        }

        private string GetCurrentUser()
            => User.FindFirst(ClaimTypes.Name)?.Value
            ?? User.FindFirst(ClaimTypes.Email)?.Value
            ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? "System";

        private static (string ContentType, string Ext) GetContentTypeAndExt(string format)
            => format.ToUpperInvariant() switch
            {
                "EXCEL" => ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ".xlsx"),
                "WORD" => ("application/vnd.openxmlformats-officedocument.wordprocessingml.document", ".docx"),
                "CSV" => ("text/csv", ".csv"),
                "XML" => ("application/xml", ".xml"),
                "JSON" => ("application/json", ".json"),
                _ => ("application/pdf", ".pdf")
            };
    }
}
