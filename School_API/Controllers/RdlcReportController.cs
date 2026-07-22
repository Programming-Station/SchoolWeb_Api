using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Infrastructure;
using School.Services.Interfaces;

namespace School_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RdlcReportController : ControllerBase
    {
        private readonly IRdlcCertificateService _rdlcService;
        private readonly IAdmissionService _admissionService;
        private readonly IRdlcReportManager _reportManager;
        private readonly SchoolDbContext _dbContext;
        private readonly IMessageService _messageService;
        private readonly IEmailService _emailService;

        public RdlcReportController(
            IRdlcCertificateService rdlcService,
            IAdmissionService admissionService,
            IRdlcReportManager reportManager,
            SchoolDbContext dbContext,
            IMessageService messageService,
            IEmailService emailService)
        {
            _rdlcService = rdlcService;
            _admissionService = admissionService;
            _reportManager = reportManager;
            _dbContext = dbContext;
            _messageService = messageService;
            _emailService = emailService;
        }

        /// <summary>Generates and downloads the student registration certificate PDF via RDLC.</summary>
        [HttpGet("RegistrationCertificate/{registrationId}")]
        public async Task<IActionResult> DownloadRegistrationCertificate([FromRoute] int registrationId)
        {
            var admissionRes = await _admissionService.GetApplicationByIdAsync(registrationId);
            if (admissionRes == null || !admissionRes.Success || admissionRes.Data == null)
            {
                return NotFound(new { message = "Admission application not found." });
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var pdfBytes = await _rdlcService.GenerateRegistrationCertificateAsync(admissionRes.Data, baseUrl);

            if (pdfBytes == null || pdfBytes.Length == 0)
                return BadRequest(new { message = "Failed to generate certificate." });

            return File(pdfBytes, "application/pdf", $"Registration_Certificate_{registrationId}.pdf");
        }

        /// <summary>Generates and downloads the fee payment receipt PDF via RDLC.</summary>
        [HttpGet("FeeReceipt/{paymentId}")]
        public async Task<IActionResult> DownloadFeeReceipt([FromRoute] int paymentId)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var pdfBytes = await _rdlcService.GenerateFeeReceiptPdfAsync(paymentId, baseUrl);

            if (pdfBytes == null || pdfBytes.Length == 0)
                return NotFound(new { message = "Payment receipt not found or failed to generate." });

            return File(pdfBytes, "application/pdf", $"Fee_Receipt_{paymentId}.pdf");
        }

        /// <summary>Generates and downloads the comprehensive admission application summary PDF via RDLC.</summary>
        [HttpGet("AdmissionApplicationSummary/{applicationId}")]
        public async Task<IActionResult> DownloadAdmissionApplicationSummary([FromRoute] int applicationId)
        {
            var pdfBytes = await _rdlcService.GenerateAdmissionApplicationSummaryAsync(applicationId);
            if (pdfBytes == null || pdfBytes.Length == 0)
                return NotFound(new { message = "Admission application summary not found or failed to generate." });

            return File(pdfBytes, "application/pdf", $"Admission_Application_Summary_{applicationId}.pdf");
        }

        /// <summary>Generates and downloads the admissions pipeline status analytics report via RDLC.</summary>
        [HttpGet("AdmissionsPipeline")]
        public async Task<IActionResult> DownloadAdmissionsPipelineReport(
            [FromQuery] string? status,
            [FromQuery] string? courseName,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var pdfBytes = await _rdlcService.GenerateAdmissionsPipelineReportAsync(status, courseName, fromDate, toDate);
            if (pdfBytes == null || pdfBytes.Length == 0)
                return BadRequest(new { message = "Failed to generate admissions pipeline report." });

            return File(pdfBytes, "application/pdf", "Admissions_Pipeline_Report.pdf");
        }

        /// <summary>Generates and downloads the student class directory report via RDLC.</summary>
        [HttpGet("StudentClassDirectory")]
        public async Task<IActionResult> DownloadStudentClassDirectory(
            [FromQuery] int classId,
            [FromQuery] int? sectionId)
        {
            var pdfBytes = await _rdlcService.GenerateStudentClassDirectoryAsync(classId, sectionId);
            if (pdfBytes == null || pdfBytes.Length == 0)
                return BadRequest(new { message = "Failed to generate student class directory." });

            return File(pdfBytes, "application/pdf", $"Student_Class_Directory_Class_{classId}.pdf");
        }

        /// <summary>Generates and downloads the admission verification audit trail report via RDLC.</summary>
        [HttpGet("AdmissionAuditTrail")]
        public async Task<IActionResult> DownloadAdmissionAuditTrail(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var pdfBytes = await _rdlcService.GenerateAdmissionAuditTrailReportAsync(fromDate, toDate);
            if (pdfBytes == null || pdfBytes.Length == 0)
                return BadRequest(new { message = "Failed to generate admission audit trail report." });

            return File(pdfBytes, "application/pdf", "Admission_Audit_Trail_Report.pdf");
        }

        /// <summary>Generates and downloads the academic qualification analysis report via RDLC.</summary>
        [HttpGet("AcademicQualificationAnalysis")]
        public async Task<IActionResult> DownloadAcademicQualificationAnalysis(
            [FromQuery] string? boardName,
            [FromQuery] string? passingYear)
        {
            var pdfBytes = await _rdlcService.GenerateAcademicQualificationAnalysisAsync(boardName, passingYear);
            if (pdfBytes == null || pdfBytes.Length == 0)
                return BadRequest(new { message = "Failed to generate academic qualification analysis report." });

            return File(pdfBytes, "application/pdf", "Academic_Qualification_Analysis_Report.pdf");
        }

        /// <summary>Generates and downloads the parent-student emergency contacts directory via RDLC.</summary>
        [HttpGet("ParentStudentEmergencyDirectory")]
        public async Task<IActionResult> DownloadParentStudentEmergencyDirectory([FromQuery] int classId)
        {
            var pdfBytes = await _rdlcService.GenerateParentStudentEmergencyDirectoryAsync(classId);
            if (pdfBytes == null || pdfBytes.Length == 0)
                return BadRequest(new { message = "Failed to generate parent-student emergency directory." });

            return File(pdfBytes, "application/pdf", $"PTA_Emergency_Directory_Class_{classId}.pdf");
        }

        /// <summary>Generates and downloads the admission rule eligibility deviation exception report via RDLC.</summary>
        [HttpGet("AdmissionDeviation")]
        public async Task<IActionResult> DownloadAdmissionDeviationReport()
        {
            var pdfBytes = await _rdlcService.GenerateAdmissionDeviationReportAsync();
            if (pdfBytes == null || pdfBytes.Length == 0)
                return BadRequest(new { message = "Failed to generate admission deviation exception report." });

            return File(pdfBytes, "application/pdf", "Admission_Deviation_Exception_Report.pdf");
        }

        /// <summary>Generates and downloads reports dynamically across all ERP modules.</summary>
        [HttpGet("Export/{module}/{reportName}")]
        public async Task<IActionResult> ExportReport(
            [FromRoute] string module,
            [FromRoute] string reportName,
            [FromQuery] string format = "PDF",
            [FromQuery] int? id = null,
            [FromQuery] int? classId = null,
            [FromQuery] int? sectionId = null,
            [FromQuery] string? status = null,
            [FromQuery] string? boardName = null,
            [FromQuery] string? passingYear = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] string? month = null)
        {
            var defaultSchool = await _dbContext.SchoolRegistrations.FirstOrDefaultAsync();
            var reportParams = new Dictionary<string, string>
            {
                { "SchoolName", defaultSchool?.SchoolName ?? "SchoolSaaS Academy" },
                { "SchoolAddress", defaultSchool?.Address ?? "City Campus, Metro Center" },
                { "ReportGeneratedDate", DateTime.Now.ToString("dd-MMM-yyyy HH:mm") },
                { "ReportGeneratedBy", "System Administrator" },
                { "ReportTitle", $"{reportName} Document Summary" }
            };

            var dataSources = new Dictionary<string, object>();

            // Query resolving switch based on report layout name
            switch (reportName)
            {
                case "LibraryIssueSlip":
                    var issues = await _dbContext.BookIssueLogs
                        .Include(x => x.Book)
                        .Include(x => x.Student)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources.Add("LibraryIssueDataSet", issues);
                    break;

                case "BookStockRegister":
                    var books = await _dbContext.Books
                        .Include(x => x.Category)
                        .ToListAsync();
                    dataSources.Add("BookStockDataSet", books);
                    break;

                case "EmployeePayslip":
                    var payroll = await _dbContext.PayrollRuns
                        .Include(x => x.Employee)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources.Add("PayslipDataSet", payroll);
                    break;

                case "StudentBusPass":
                    var trans = await _dbContext.TransportAllocations
                        .Include(x => x.Student)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources.Add("BusPassDataSet", trans);
                    break;

                case "HostelGatePass":
                    var gates = await _dbContext.HostelGatePasses
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources.Add("GatePassDataSet", gates);
                    break;

                case "PurchaseOrder":
                    var pos = await _dbContext.PurchaseOrders
                        .Include(x => x.Vendor)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources.Add("PurchaseOrderDataSet", pos);
                    break;

                case "AdmissionRegister":
                    var apps = await _dbContext.AdmissionApplications
                        .Include(x => x.Course)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources.Add("AdmissionDataSet", apps);
                    break;

                case "FeeReceipt":
                    var payments = await _dbContext.FeePayments
                        .Include(x => x.Student)
                            .ThenInclude(s => s.Class)
                        .Include(x => x.FeeInstallment)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources.Add("FeeReceiptDataSet", payments);
                    break;

                case "AccountingLedger":
                    var lines = await _dbContext.JournalEntryLines
                        .Include(x => x.Account)
                        .Where(x => id == null || x.AccountId == id)
                        .ToListAsync();
                    dataSources.Add("LedgerDataSet", lines);
                    break;

                case "StudentReportCard":
                    var results = await _dbContext.ExamResults
                        .Include(x => x.Student)
                        .Include(x => x.Exam)
                        .Include(x => x.Subject)
                        .Where(x => id == null || x.StudentId == id)
                        .ToListAsync();
                    dataSources.Add("ReportCardDataSet", results);
                    break;

                case "TransferCertificate":
                    var st = await _dbContext.Students
                        .Include(x => x.Class)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();

                    var mappedTC = st.Select(x => new
                    {
                        StudentFullName = x.Name,
                        AdmissionNumber = x.StudentId,
                        ClassName = x.Class != null ? $"{x.Class.Name}{(string.IsNullOrEmpty(x.Class.Section) ? "" : " - " + x.Class.Section)}" : "General",
                        DateOfJoining = x.CreatedDate,
                        DateOfLeaving = DateTime.Today,
                        Conduct = "Good"
                    }).ToList();

                    dataSources.Add("TCDataSet", mappedTC);
                    break;

                case "DailyAttendanceRegister":
                    var attendances = await _dbContext.StudentAttendances
                        .Include(x => x.Student)
                        .Include(x => x.Class)
                        .Where(x => (classId == null || x.ClassId == classId) && x.AttendanceDate.Date == (fromDate ?? DateTime.Today).Date)
                        .ToListAsync();
                    dataSources.Add("AttendanceDataSet", attendances);
                    break;

                case "FeeDefaulter":
                    var students = await _dbContext.Students
                        .Include(x => x.Class)
                        .Take(30)
                        .ToListAsync();
                    dataSources.Add("DefaulterDataSet", students);
                    break;

                default:
                    // Fallback to generic listing from Student directory
                    var directoryList = await _dbContext.Students.Take(10).ToListAsync();
                    dataSources.Add("DefaultDataSet", directoryList);
                    break;
            }

            try
            {
                var fileBytes = await _reportManager.RenderReportAsync(reportName, format, dataSources, reportParams);

                string contentType = "application/pdf";
                string fileExt = ".pdf";

                if (format.Equals("EXCEL", StringComparison.OrdinalIgnoreCase))
                {
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileExt = ".xlsx";
                }
                else if (format.Equals("WORD", StringComparison.OrdinalIgnoreCase))
                {
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    fileExt = ".docx";
                }

                return File(fileBytes, contentType, $"{reportName}_Document{fileExt}");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error rendering report: {ex.Message}" });
            }
        }

        /// <summary>Queues a dynamically generated PDF report attachment email.</summary>
        [HttpPost("EmailReport")]
        public async Task<IActionResult> EmailReport([FromBody] EmailReportRequest request)
        {
            var defaultSchool = await _dbContext.SchoolRegistrations.FirstOrDefaultAsync();
            var reportParams = new Dictionary<string, string>
            {
                { "SchoolName", defaultSchool?.SchoolName ?? "SchoolSaaS Academy" },
                { "SchoolAddress", defaultSchool?.Address ?? "City Campus, Metro Center" },
                { "ReportGeneratedDate", DateTime.Now.ToString("dd-MMM-yyyy HH:mm") },
                { "ReportGeneratedBy", "System Administrator" },
                { "ReportTitle", $"{request.ReportName} Document Summary" }
            };

            var dataSources = new Dictionary<string, object>();

            // Compile the requested report bytes
            switch (request.ReportName)
            {
                case "LibraryIssueSlip":
                    var issues = await _dbContext.BookIssueLogs
                        .Include(x => x.Book)
                        .Include(x => x.Student)
                        .Where(x => request.Id == null || x.Id == request.Id)
                        .ToListAsync();
                    dataSources.Add("LibraryIssueDataSet", issues);
                    break;

                case "BookStockRegister":
                    var books = await _dbContext.Books.Include(x => x.Category).ToListAsync();
                    dataSources.Add("BookStockDataSet", books);
                    break;

                case "EmployeePayslip":
                    var payroll = await _dbContext.PayrollRuns
                        .Include(x => x.Employee)
                        .Where(x => request.Id == null || x.Id == request.Id)
                        .ToListAsync();
                    dataSources.Add("PayslipDataSet", payroll);
                    break;

                case "StudentBusPass":
                    var trans = await _dbContext.TransportAllocations
                        .Include(x => x.Student)
                        .Where(x => request.Id == null || x.Id == request.Id)
                        .ToListAsync();
                    dataSources.Add("BusPassDataSet", trans);
                    break;

                case "HostelGatePass":
                    var gates = await _dbContext.HostelGatePasses
                        .Where(x => request.Id == null || x.Id == request.Id)
                        .ToListAsync();
                    dataSources.Add("GatePassDataSet", gates);
                    break;

                case "PurchaseOrder":
                    var pos = await _dbContext.PurchaseOrders
                        .Include(x => x.Vendor)
                        .Where(x => request.Id == null || x.Id == request.Id)
                        .ToListAsync();
                    dataSources.Add("PurchaseOrderDataSet", pos);
                    break;

                case "AdmissionRegister":
                    var apps = await _dbContext.AdmissionApplications
                        .Include(x => x.Course)
                        .Where(x => request.Id == null || x.Id == request.Id)
                        .ToListAsync();
                    dataSources.Add("AdmissionDataSet", apps);
                    break;

                case "FeeReceipt":
                    var payments = await _dbContext.FeePayments
                        .Include(x => x.Student)
                            .ThenInclude(s => s.Class)
                        .Include(x => x.FeeInstallment)
                        .Where(x => request.Id == null || x.Id == request.Id)
                        .ToListAsync();
                    dataSources.Add("FeeReceiptDataSet", payments);
                    break;

                case "AccountingLedger":
                    var lines = await _dbContext.JournalEntryLines
                        .Include(x => x.Account)
                        .Where(x => request.Id == null || x.AccountId == request.Id)
                        .ToListAsync();
                    dataSources.Add("LedgerDataSet", lines);
                    break;

                case "StudentReportCard":
                    var results = await _dbContext.ExamResults
                        .Include(x => x.Student)
                        .Include(x => x.Exam)
                        .Include(x => x.Subject)
                        .Where(x => request.Id == null || x.StudentId == request.Id)
                        .ToListAsync();
                    dataSources.Add("ReportCardDataSet", results);
                    break;

                case "TransferCertificate":
                    var st = await _dbContext.Students
                        .Include(x => x.Class)
                        .Where(x => request.Id == null || x.Id == request.Id)
                        .ToListAsync();

                    var mappedTC = st.Select(x => new
                    {
                        StudentFullName = x.Name,
                        AdmissionNumber = x.StudentId,
                        ClassName = x.Class != null ? $"{x.Class.Name}{(string.IsNullOrEmpty(x.Class.Section) ? "" : " - " + x.Class.Section)}" : "General",
                        DateOfJoining = x.CreatedDate,
                        DateOfLeaving = DateTime.Today,
                        Conduct = "Good"
                    }).ToList();

                    dataSources.Add("TCDataSet", mappedTC);
                    break;

                case "DailyAttendanceRegister":
                    var attendances = await _dbContext.StudentAttendances
                        .Include(x => x.Student)
                        .Include(x => x.Class)
                        .Where(x => (request.ClassId == null || x.ClassId == request.ClassId) && x.AttendanceDate.Date == DateTime.Today)
                        .ToListAsync();
                    dataSources.Add("AttendanceDataSet", attendances);
                    break;

                case "FeeDefaulter":
                    var students = await _dbContext.Students.Include(x => x.Class).Take(30).ToListAsync();
                    dataSources.Add("DefaulterDataSet", students);
                    break;

                default:
                    var directoryList = await _dbContext.Students.Take(10).ToListAsync();
                    dataSources.Add("DefaultDataSet", directoryList);
                    break;
            }

            try
            {
                var fileBytes = await _reportManager.RenderReportAsync(request.ReportName, "PDF", dataSources, reportParams);

                // Queue the template email with the compiled PDF attached
                _emailService.QueueTemplateEmail(
                    request.RecipientEmail,
                    request.TemplateName,
                    request.Placeholders,
                    fileBytes,
                    $"{request.ReportName}_Document.pdf");

                return Ok(new { success = true, message = "Report compiled and queued for background email dispatch successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Failed to render and queue report: {ex.Message}" });
            }
        }

        /// <summary>Sends a simulated WhatsApp transaction log alert.</summary>
        [HttpPost("SendWhatsApp")]
        public async Task<IActionResult> SendWhatsApp([FromBody] WhatsAppRequest request)
        {
            var res = await _messageService.SendWhatsAppAsync(request.RecipientPhone, request.Message);
            if (res)
                return Ok(new { success = true, message = "WhatsApp notification dispatched." });

            return BadRequest(new { message = "Failed to dispatch WhatsApp." });
        }

        /// <summary>Sends a simulated SMS notification.</summary>
        [HttpPost("SendSMS")]
        public async Task<IActionResult> SendSMS([FromBody] SmsRequest request)
        {
            var res = await _messageService.SendSmsAsync(request.RecipientNo, request.Message);
            if (res)
                return Ok(new { success = true, message = "SMS alert dispatched." });

            return BadRequest(new { message = "Failed to dispatch SMS." });
        }
    }

    public record EmailReportRequest(
        string RecipientEmail,
        string TemplateName,
        string Module,
        string ReportName,
        int? Id,
        int? ClassId,
        Dictionary<string, string>? Placeholders);

    public record WhatsAppRequest(string RecipientPhone, string Message);
    public record SmsRequest(string RecipientNo, string Message);
}
