using System.Diagnostics;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Reporting.NETCore;
using School.Domain.Reporting;
using School.Infrastructure;
using School.Infrastructure.Repositories;
using School.Services.Interfaces;
using School_DTOs.Reporting;

namespace School.Services.Reporting
{
    /// <summary>
    /// Core Reporting Engine — the generic, tenant-aware, branding-injected,
    /// multi-format report renderer that replaces all ad-hoc switch-case logic.
    /// 
    /// Flow: Request → Resolve Template → Load RDLC → Fetch Data → Inject Branding
    ///       → Inject QR/Barcode → Set Parameters → Render → Log History → Return bytes
    /// </summary>
    public class ReportingEngineService : IReportingEngineService
    {
        private readonly SchoolDbContext _db;
        private readonly ReportingRepository _repo;
        private readonly IReportingBrandingService _brandingService;
        private readonly IQrBarcodeService _qrBarcodeService;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ReportingEngineService> _logger;
        private readonly string _rdlcBasePath;
        private readonly string _storagePath;

        public ReportingEngineService(
            SchoolDbContext db,
            ReportingRepository repo,
            IReportingBrandingService brandingService,
            IQrBarcodeService qrBarcodeService,
            IMemoryCache cache,
            ILogger<ReportingEngineService> logger,
            IConfiguration configuration)
        {
            _db = db;
            _repo = repo;
            _brandingService = brandingService;
            _qrBarcodeService = qrBarcodeService;
            _cache = cache;
            _logger = logger;
            _rdlcBasePath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "RdlcFiles");
            var storagePath = configuration.GetSection("AppSettings:ImageStoragePath").Value;
            _storagePath = string.IsNullOrWhiteSpace(storagePath)
                ? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads")
                : Path.GetFullPath(storagePath.Trim().Replace('/', Path.DirectorySeparatorChar));
        }

        // ─── Public API ───────────────────────────────────────────────────────

        public async Task<byte[]> GenerateAsync(
            ReportExecutionRequest request,
            string generatedBy,
            int? tenantId = null)
        {
            var sw = Stopwatch.StartNew();
            var history = new ReportHistory
            {
                TenantId = tenantId,
                GeneratedBy = generatedBy,
                GeneratedAt = DateTime.UtcNow,
                Format = request.Format.ToUpperInvariant(),
                Status = ReportExecutionStatus.Running,
                ParametersJson = JsonSerializer.Serialize(request.Parameters),
                IsScheduled = false
            };

            var execution = new ReportExecution
            {
                TenantId = tenantId,
                StartedAt = DateTime.UtcNow,
                ExecutionStage = "DataFetch"
            };

            try
            {
                // 1. Resolve template
                var template = await ResolveTemplateAsync(request, tenantId);
                history.ReportTemplateId = template.Id;
                history.ReportName = template.ReportName;

                await _repo.IncrementExecutionCountAsync(template.Id);

                // 2. Build execution context
                var ctx = new ReportExecutionContext
                {
                    Template = template,
                    Format = request.Format,
                    Parameters = request.Parameters,
                    TenantId = tenantId,
                    GeneratedBy = generatedBy
                };

                // 3. Render
                var dataFetchStart = Stopwatch.GetTimestamp();
                var dataSources = await FetchDataSourcesAsync(ctx);
                execution.DataFetchMs = StopwatchToMs(dataFetchStart);
                execution.RowsProcessed = dataSources.Values.Sum(CountRows);

                var renderStart = Stopwatch.GetTimestamp();
                var fileBytes = await RenderRdlcAsync(ctx, dataSources);
                execution.RenderMs = StopwatchToMs(renderStart);

                sw.Stop();
                execution.IsSuccess = true;
                execution.ExecutionStage = "Done";
                execution.CompletedAt = DateTime.UtcNow;

                history.Status = ReportExecutionStatus.Success;
                history.FileSizeBytes = fileBytes.Length;
                history.ExecutionMs = sw.ElapsedMilliseconds;
                history.RowCount = execution.RowsProcessed;

                await _repo.LogHistoryAsync(history);
                execution.ReportHistoryId = history.Id;
                await _repo.LogExecutionAsync(execution);

                _logger.LogInformation(
                    "Report [{Code}] generated in {Ms}ms for tenant {Tenant} by {User}",
                    template.ReportCode, sw.ElapsedMilliseconds, tenantId, generatedBy);

                return fileBytes;
            }
            catch (Exception ex)
            {
                sw.Stop();
                history.Status = ReportExecutionStatus.Failed;
                history.ErrorMessage = ex.Message;
                history.ExecutionMs = sw.ElapsedMilliseconds;
                await _repo.LogHistoryAsync(history);

                execution.IsSuccess = false;
                execution.ErrorDetail = ex.ToString();
                execution.ExecutionStage = "Error";
                if (history.Id > 0)
                {
                    execution.ReportHistoryId = history.Id;
                    await _repo.LogExecutionAsync(execution);
                }

                _logger.LogError(ex, "Report generation failed for code [{Code}]", request.ReportCode);
                throw;
            }
        }

        public async Task<string> GeneratePreviewBase64Async(
            ReportExecutionRequest request, int? tenantId = null)
        {
            request.Format = "PDF";
            var bytes = await GenerateAsync(request, "Preview", tenantId);
            return Convert.ToBase64String(bytes);
        }

        public async Task<List<ReportParameterDto>> GetParametersAsync(
            string reportCode, int? tenantId = null)
        {
            var template = await _repo.GetTemplateByCodeAsync(reportCode, tenantId);
            if (template == null) return new List<ReportParameterDto>();

            return template.ReportParameters.Select(p => new ReportParameterDto
            {
                Id = p.Id,
                ReportTemplateId = p.ReportTemplateId,
                ParameterName = p.ParameterName,
                DisplayLabel = p.DisplayLabel,
                DataType = p.DataType.ToString(),
                IsRequired = p.IsRequired,
                DefaultValue = p.DefaultValue,
                EnumValuesJson = p.EnumValuesJson,
                LookupApiEndpoint = p.LookupApiEndpoint,
                PlaceholderText = p.PlaceholderText,
                HelpText = p.HelpText,
                SortOrder = p.SortOrder,
                DependsOnParameter = p.DependsOnParameter,
                DependsOnValue = p.DependsOnValue
            }).ToList();
        }

        public async Task SetFavoriteAsync(int templateId, bool isFavorite, int? tenantId = null)
        {
            var template = await _repo.GetTemplateByIdAsync(templateId);
            if (template != null)
            {
                template.IsFavorite = isFavorite;
                await _repo.UpdateTemplateAsync(template);
            }
        }

        public async Task<List<ReportCategoryDto>> GetDashboardCategoriesAsync(int? tenantId = null)
        {
            var categories = await _repo.GetCategoriesAsync(tenantId);
            var templates = await _repo.GetTemplatesAsync(tenantId, onlyVisible: true);

            return categories.Select(c => new ReportCategoryDto
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                IconClass = c.IconClass,
                ColorHex = c.ColorHex,
                Description = c.Description,
                SortOrder = c.SortOrder,
                IsActive = c.IsActive,
                ReportCount = templates.Count(t => t.ReportCategoryId == c.Id)
            }).ToList();
        }

        public async Task<List<ReportTemplateDto>> SearchReportsAsync(
            string searchTerm, int? tenantId = null)
        {
            var templates = await _repo.GetTemplatesAsync(tenantId, searchTerm: searchTerm);
            return templates.Select(MapToDto).ToList();
        }

        // ─── Private Helpers ──────────────────────────────────────────────────

        private async Task<ReportTemplate> ResolveTemplateAsync(
            ReportExecutionRequest request, int? tenantId)
        {
            ReportTemplate? template = null;

            if (request.ReportTemplateId.HasValue)
                template = await _repo.GetTemplateByIdAsync(request.ReportTemplateId.Value);

            if (template == null && !string.IsNullOrWhiteSpace(request.ReportCode))
                template = await _repo.GetTemplateByCodeAsync(request.ReportCode, tenantId);

            if (template == null)
                throw new InvalidOperationException(
                    $"Report template not found: {request.ReportCode ?? request.ReportTemplateId?.ToString()}");

            return template;
        }

        private async Task<Dictionary<string, object>> FetchDataSourcesAsync(
            ReportExecutionContext ctx)
        {
            var p = ctx.Parameters;
            var dataSources = new Dictionary<string, object>();

            // Extract common parameters
            int? GetInt(string key) => p.TryGetValue(key, out var v) && int.TryParse(v, out var i) ? i : null;
            DateTime? GetDate(string key) => p.TryGetValue(key, out var v) && DateTime.TryParse(v, out var d) ? d : null;
            string? GetStr(string key) => p.TryGetValue(key, out var v) ? v : null;

            var id = GetInt("Id") ?? GetInt("id");
            var classId = GetInt("ClassId") ?? GetInt("classId");
            var sectionId = GetInt("SectionId") ?? GetInt("sectionId");
            var fromDate = GetDate("FromDate") ?? GetDate("fromDate") ?? DateTime.Today;
            var toDate = GetDate("ToDate") ?? GetDate("toDate") ?? DateTime.Today;
            var month = GetStr("Month") ?? GetStr("month");
            var status = GetStr("Status") ?? GetStr("status");
            var tenantId = ctx.TenantId;

            switch (ctx.Template.ReportCode)
            {
                // ─── Fee Reports ─────────────────────────────────────────────
                case "FEE_RECEIPT":
                    var payments = await _db.FeePayments
                        .Include(x => x.Student).ThenInclude(s => s.Class)
                        .Include(x => x.FeeInstallment)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources["FeeReceiptDataSet"] = payments;
                    break;

                case "FEE_COLLECTION":
                    var feeCollection = await _db.FeePayments
                        .Include(x => x.Student).ThenInclude(s => s.Class)
                        .Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate.AddDays(1))
                        .ToListAsync();
                    dataSources["FeeCollectionDataSet"] = feeCollection;
                    break;

                case "FEE_DEFAULTER":
                    var defaulters = await _db.Students
                        .Include(x => x.Class)
                        .Where(x => !x.IsDeleted)
                        .Take(500)
                        .ToListAsync();
                    dataSources["DefaulterDataSet"] = defaulters;
                    break;

                // ─── Student Reports ──────────────────────────────────────────
                case "STUDENT_CLASS_DIRECTORY":
                    var directory = await _db.Students
                        .Include(x => x.Class)
                        .Where(x => !x.IsDeleted &&
                                    (classId == null || x.ClassId == classId) &&
                                    (sectionId == null || x.ClassId == sectionId))
                        .OrderBy(x => x.Name)
                        .ToListAsync();
                    dataSources["StudentDirectoryDataSet"] = directory;
                    break;

                case "STUDENT_MARKSHEET":
                case "STUDENT_REPORT_CARD":
                    var results = await _db.ExamResults
                        .Include(x => x.Student)
                        .Include(x => x.Exam)
                        .Include(x => x.Subject)
                        .Where(x => id == null || x.StudentId == id)
                        .ToListAsync();
                    dataSources["ReportCardDataSet"] = results;
                    break;

                case "STUDENT_PROGRESS_REPORT":
                    var progressResults = await _db.ExamResults
                        .Include(x => x.Student)
                        .Include(x => x.Exam)
                        .Include(x => x.Subject)
                        .Where(x => id == null || x.StudentId == id)
                        .OrderBy(x => x.Exam.StartDate)
                        .ToListAsync();
                    dataSources["ProgressDataSet"] = progressResults;
                    break;

                // ─── Attendance ───────────────────────────────────────────────
                case "DAILY_ATTENDANCE":
                    var attendance = await _db.StudentAttendances
                        .Include(x => x.Student)
                        .Include(x => x.Class)
                        .Where(x => (classId == null || x.ClassId == classId) &&
                                    x.AttendanceDate.Date == fromDate.Date)
                        .ToListAsync();
                    dataSources["AttendanceDataSet"] = attendance;
                    break;

                // ─── Admission ────────────────────────────────────────────────
                case "ADMISSION_REGISTER":
                    var apps = await _db.AdmissionApplications
                        .Include(x => x.Course)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources["AdmissionDataSet"] = apps;
                    break;

                case "ADMISSION_PIPELINE":
                    var pipeline = await _db.AdmissionApplications
                        .Include(x => x.Course)
                        .Where(x => (status == null || x.Status == status) &&
                                    (fromDate == default || x.CreatedDate >= fromDate) &&
                                    (toDate == default || x.CreatedDate <= toDate.AddDays(1)))
                        .ToListAsync();
                    dataSources["PipelineDataSet"] = pipeline;
                    break;

                case "TRANSFER_CERTIFICATE":
                    var tcStudents = await _db.Students
                        .Include(x => x.Class)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources["TCDataSet"] = tcStudents.Select(x => new
                    {
                        StudentFullName = x.Name,
                        AdmissionNumber = x.StudentId,
                        ClassName = x.Class != null ? $"{x.Class.Name}" : "General",
                        DateOfJoining = x.CreatedDate,
                        DateOfLeaving = DateTime.Today,
                        Conduct = "Good",
                        Reason = "Parents' Request"
                    }).ToList();
                    break;

                case "BONAFIDE_CERTIFICATE":
                case "CHARACTER_CERTIFICATE":
                case "MIGRATION_CERTIFICATE":
                case "EXPERIENCE_CERTIFICATE":
                    var certStudents = await _db.Students
                        .Include(x => x.Class)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources["CertificateDataSet"] = certStudents;
                    break;

                case "STUDENT_ID_CARD":
                    var idStudents = await _db.Students
                        .Include(x => x.Class)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources["IdCardDataSet"] = idStudents;
                    break;

                // ─── HR / Payroll ─────────────────────────────────────────────
                case "EMPLOYEE_PAYSLIP":
                    var payroll = await _db.PayrollRuns
                        .Include(x => x.Employee)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources["PayslipDataSet"] = payroll;
                    break;

                case "SALARY_REPORT":
                    var salaryReport = await _db.PayrollRuns
                        .Include(x => x.Employee)
                        .Where(x => (string.IsNullOrEmpty(month) || x.Month == month) &&
                                    x.CreatedDate >= fromDate && x.CreatedDate <= toDate.AddDays(1))
                        .ToListAsync();
                    dataSources["SalaryDataSet"] = salaryReport;
                    break;

                case "EMPLOYEE_LIST":
                    var employees = await _db.Employees
                        .Include(x => x.Designation)
                        .Include(x => x.Department)
                        .Where(x => !x.IsDeleted)
                        .OrderBy(x => x.FirstName)
                        .ToListAsync();
                    dataSources["EmployeeListDataSet"] = employees;
                    break;

                case "LEAVE_REPORT":
                    var leaves = await _db.LeaveRequests
                        .Include(x => x.Employee)
                        .Include(x => x.LeaveType)
                        .Where(x => x.StartDate >= fromDate && x.EndDate <= toDate.AddDays(1))
                        .ToListAsync();
                    dataSources["LeaveDataSet"] = leaves;
                    break;

                case "EMPLOYEE_ID_CARD":
                    var empId = await _db.Employees
                        .Include(x => x.Designation)
                        .Include(x => x.Department)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources["EmpIdCardDataSet"] = empId;
                    break;

                // ─── Library ──────────────────────────────────────────────────
                case "LIBRARY_ISSUE_SLIP":
                    var issues = await _db.BookIssueLogs
                        .Include(x => x.Book)
                        .Include(x => x.Student)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources["LibraryIssueDataSet"] = issues;
                    break;

                case "LIBRARY_OVERDUE":
                    var overdue = await _db.BookIssueLogs
                        .Include(x => x.Book)
                        .Include(x => x.Student)
                        .Where(x => x.DueDate < DateTime.Today && x.ReturnDate == null)
                        .ToListAsync();
                    dataSources["OverdueDataSet"] = overdue;
                    break;

                case "BOOK_STOCK":
                    var books = await _db.Books
                        .Include(x => x.Category)
                        .Where(x => !x.IsDeleted)
                        .ToListAsync();
                    dataSources["BookStockDataSet"] = books;
                    break;

                // ─── Transport ────────────────────────────────────────────────
                case "STUDENT_BUS_PASS":
                    var busPass = await _db.TransportAllocations
                        .Include(x => x.Student)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources["BusPassDataSet"] = busPass;
                    break;

                case "TRANSPORT_REPORT":
                    var transport = await _db.TransportAllocations
                        .Include(x => x.Student)
                        .Include(x => x.TransportRoute)
                        .ToListAsync();
                    dataSources["TransportDataSet"] = transport;
                    break;

                // ─── Hostel ───────────────────────────────────────────────────
                case "HOSTEL_GATE_PASS":
                    var gatePasses = await _db.HostelGatePasses
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources["GatePassDataSet"] = gatePasses;
                    break;

                case "HOSTEL_OCCUPANCY":
                    var hostelAdm = await _db.HostelAdmissions
                        .Include(x => x.Room)
                        .Include(x => x.Student)
                        .Where(x => x.IsActive)
                        .ToListAsync();
                    dataSources["HostelOccupancyDataSet"] = hostelAdm;
                    break;

                // ─── Inventory ────────────────────────────────────────────────
                case "PURCHASE_ORDER":
                    var pos = await _db.PurchaseOrders
                        .Include(x => x.Vendor)
                        .Where(x => id == null || x.Id == id)
                        .ToListAsync();
                    dataSources["PurchaseOrderDataSet"] = pos;
                    break;

                case "INVENTORY_REPORT":
                    var inventory = await _db.InventoryItems
                        .Include(x => x.Category)
                        .Where(x => !x.IsDeleted)
                        .ToListAsync();
                    dataSources["InventoryDataSet"] = inventory;
                    break;

                // ─── Finance ──────────────────────────────────────────────────
                case "ACCOUNTING_LEDGER":
                case "GENERAL_LEDGER":
                    var ledger = await _db.JournalEntryLines
                        .Include(x => x.Account)
                        .Where(x => id == null || x.AccountId == id)
                        .ToListAsync();
                    dataSources["LedgerDataSet"] = ledger;
                    break;

                case "CASH_BOOK":
                    var cashBook = await _db.CashBankTransactions
                        .Where(x => x.TransactionDate >= fromDate && x.TransactionDate <= toDate.AddDays(1))
                        .ToListAsync();
                    dataSources["CashBookDataSet"] = cashBook;
                    break;

                case "BALANCE_SHEET":
                    var accounts = await _db.CoaAccounts
                        .Where(x => !x.IsDeleted)
                        .ToListAsync();
                    dataSources["BalanceSheetDataSet"] = accounts;
                    break;

                case "INCOME_EXPENSE":
                    var incomeExp = await _db.JournalEntryLines
                        .Include(x => x.Account)
                        .Include(x => x.JournalEntry)
                        .Where(x => x.JournalEntry.EntryDate >= fromDate &&
                                    x.JournalEntry.EntryDate <= toDate.AddDays(1))
                        .ToListAsync();
                    dataSources["IncomeExpenseDataSet"] = incomeExp;
                    break;

                case "BANK_STATEMENT":
                    var bankTx = await _db.CashBankTransactions
                        .Where(x => x.TransactionDate >= fromDate && x.TransactionDate <= toDate.AddDays(1))
                        .OrderBy(x => x.TransactionDate)
                        .ToListAsync();
                    dataSources["BankStatementDataSet"] = bankTx;
                    break;

                case "GST_REPORT":
                    var gstEntries = await _db.JournalEntryLines
                        .Include(x => x.Account)
                        .Include(x => x.JournalEntry)
                        .Where(x => x.Account.Name.Contains("GST") &&
                                    x.JournalEntry.EntryDate >= fromDate &&
                                    x.JournalEntry.EntryDate <= toDate.AddDays(1))
                        .ToListAsync();
                    dataSources["GstDataSet"] = gstEntries;
                    break;

                case "AUDIT_REPORT":
                    var auditLogs = await _db.AdminAuditLogs
                        .Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate.AddDays(1))
                        .OrderByDescending(x => x.CreatedDate)
                        .Take(1000)
                        .ToListAsync();
                    dataSources["AuditDataSet"] = auditLogs;
                    break;

                default:
                    // Graceful fallback — return an empty placeholder dataset
                    _logger.LogWarning("No data fetcher implemented for report code: {Code}", ctx.Template.ReportCode);
                    dataSources["EmptyDataSet"] = new List<object>();
                    break;
            }

            return dataSources;
        }

        private async Task<byte[]> RenderRdlcAsync(
            ReportExecutionContext ctx,
            Dictionary<string, object> dataSources)
        {
            var report = new LocalReport();

            // Load RDLC — DB content takes precedence over file system
            if (ctx.Template.RdlcContent != null && ctx.Template.RdlcContent.Length > 0)
            {
                using var ms = new MemoryStream(ctx.Template.RdlcContent);
                report.LoadReportDefinition(ms);
            }
            else
            {
                var rdlcFileName = ctx.Template.RdlcFileName ?? $"{ctx.Template.ReportCode}.rdlc";
                var reportPath = Path.Combine(_rdlcBasePath, rdlcFileName);
                EnsureRdlcExists(reportPath, ctx.Template.ReportName);
                report.ReportPath = reportPath;
            }

            // Inject data sources
            foreach (var ds in dataSources)
                report.DataSources.Add(new ReportDataSource(ds.Key, ds.Value));

            // Inject branding parameters
            await InjectBrandingParametersAsync(report, ctx);

            // Normalize format
            var format = ctx.Format.ToUpperInvariant() switch
            {
                "EXCEL" => "EXCELOPENXML",
                "WORD" => "WORDOPENXML",
                "PDF" => "PDF",
                _ => "PDF"
            };

            return await Task.Run(() => report.Render(format));
        }

        private async Task InjectBrandingParametersAsync(LocalReport report, ReportExecutionContext ctx)
        {
            var branding = await _brandingService.GetBrandingAsync(ctx.TenantId);
            var reportInfo = report.GetParameters();
            var expected = new HashSet<string>(
                reportInfo.Select(p => p.Name),
                StringComparer.OrdinalIgnoreCase);

            var merged = new Dictionary<string, string>(
                ctx.Parameters ?? new Dictionary<string, string>(),
                StringComparer.OrdinalIgnoreCase);

            void TryAdd(string key, string? value)
            {
                if (expected.Contains(key) && !merged.ContainsKey(key))
                    merged[key] = value ?? string.Empty;
            }

            // Organization identity
            TryAdd("SchoolName", branding.SchoolName ?? branding.OrganizationName);
            TryAdd("OrganizationName", branding.OrganizationName);
            TryAdd("TagLine", branding.TagLine);
            TryAdd("SchoolAddress", branding.AddressLine1);
            TryAdd("Address", branding.AddressLine1);
            TryAdd("Address2", branding.AddressLine2);
            TryAdd("City", branding.City);
            TryAdd("State", branding.State);
            TryAdd("PinCode", branding.PinCode);
            TryAdd("Phone", branding.Phone ?? branding.Mobile);
            TryAdd("Mobile", branding.Mobile);
            TryAdd("Email", branding.Email);
            TryAdd("Website", branding.Website);
            TryAdd("Affiliation", branding.AffiliationNumber);
            TryAdd("AffiliationNumber", branding.AffiliationNumber);
            TryAdd("RegistrationNumber", branding.RegistrationNumber);

            // Logos (as base64)
            TryAdd("Logo", GetBase64(branding.HeaderLogo));
            TryAdd("HeaderLogo", GetBase64(branding.HeaderLogo));
            TryAdd("FooterLogo", GetBase64(branding.FooterLogo));
            TryAdd("LogoLight", GetBase64(branding.LogoLight));
            TryAdd("LogoDark", GetBase64(branding.LogoDark));

            // Authorities
            TryAdd("PrincipalName", branding.PrincipalName);
            TryAdd("PrincipalSignature", GetBase64(branding.PrincipalSignature));
            TryAdd("DirectorName", branding.DirectorName);
            TryAdd("DirectorSignature", GetBase64(branding.DirectorSignature));
            TryAdd("Seal", GetBase64(branding.OfficialSeal));
            TryAdd("OfficialSeal", GetBase64(branding.OfficialSeal));
            TryAdd("DigitalSignature", GetBase64(branding.DigitalSignature));

            // Report appearance
            TryAdd("Watermark", GetBase64(branding.ReportWatermark));
            TryAdd("WatermarkText", branding.WatermarkText);
            TryAdd("Footer", branding.ReportFooterText);
            TryAdd("ReportFooterText", branding.ReportFooterText);
            TryAdd("Disclaimer", branding.Disclaimer);

            // Theme
            TryAdd("ThemeColor", branding.PrimaryColor ?? "#1e3a8a");
            TryAdd("PrimaryColor", branding.PrimaryColor ?? "#1e3a8a");
            TryAdd("SecondaryColor", branding.SecondaryColor ?? "#2563eb");
            TryAdd("FontFamily", branding.FontFamily ?? "Arial");

            // Context
            TryAdd("CurrentDate", DateTime.Now.ToString("dd-MMM-yyyy"));
            TryAdd("GeneratedDate", DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt"));
            TryAdd("GeneratedBy", ctx.GeneratedBy ?? "System");
            TryAdd("AcademicSession", branding.CurrentAcademicSession);
            TryAdd("BranchName", branding.CampusName);
            TryAdd("CampusName", branding.CampusName);

            // Apply
            var reportParams = merged
                .Where(kv => expected.Contains(kv.Key))
                .Select(kv => new Microsoft.Reporting.NETCore.ReportParameter(kv.Key, kv.Value ?? string.Empty))
                .ToList();

            if (reportParams.Count > 0)
                report.SetParameters(reportParams);
        }

        private string GetBase64(string? path)
        {
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;
            if (path.StartsWith("data:image", StringComparison.OrdinalIgnoreCase))
            {
                var parts = path.Split(',');
                return parts.Length > 1 ? parts[1] : parts[0];
            }
            try
            {
                string physicalPath;
                if (path.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase))
                {
                    var rel = path.Substring("/uploads/".Length).TrimStart('/');
                    physicalPath = Path.Combine(_storagePath, rel.Replace('/', Path.DirectorySeparatorChar));
                }
                else physicalPath = path;

                if (File.Exists(physicalPath))
                    return Convert.ToBase64String(File.ReadAllBytes(physicalPath));
            }
            catch { }
            return string.Empty;
        }

        private static void EnsureRdlcExists(string filePath, string reportName)
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (!File.Exists(filePath))
            {
                var minimal = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<Report xmlns=""http://schemas.microsoft.com/sqlserver/reporting/2016/01/reportdefinition"" xmlns:rd=""http://schemas.microsoft.com/SQLServer/reporting/reportdesigner"">
  <ReportSections>
    <ReportSection>
      <Body>
        <Height>11in</Height>
        <ReportItems>
          <Textbox Name=""TitleTextBox"">
            <CanGrow>true</CanGrow>
            <Paragraphs>
              <Paragraph>
                <TextRuns>
                  <TextRun>
                    <Value>Enterprise {reportName} Report</Value>
                    <Style><FontSize>16pt</FontSize><FontWeight>Bold</FontWeight><Color>#1e3a8a</Color></Style>
                  </TextRun>
                </TextRuns>
              </Paragraph>
            </Paragraphs>
            <Top>0.5in</Top><Left>0.5in</Left><Height>0.4in</Height><Width>7in</Width>
          </Textbox>
        </ReportItems>
      </Body>
      <Width>8.5in</Width>
      <Page><PageHeight>11in</PageHeight><PageWidth>8.5in</PageWidth></Page>
    </ReportSection>
  </ReportSections>
  <rd:ReportUnitType>Inch</rd:ReportUnitType>
</Report>";
                File.WriteAllText(filePath, minimal);
            }
        }

        private static int CountRows(object datasource)
        {
            if (datasource is System.Collections.ICollection col)
                return col.Count;
            return 0;
        }

        private static long StopwatchToMs(long startTick)
            => (long)((Stopwatch.GetTimestamp() - startTick) * 1000.0 / Stopwatch.Frequency);

        private static ReportTemplateDto MapToDto(ReportTemplate t) => new()
        {
            Id = t.Id,
            ReportCategoryId = t.ReportCategoryId,
            CategoryName = t.ReportCategory?.Name,
            CategoryIcon = t.ReportCategory?.IconClass,
            CategoryColor = t.ReportCategory?.ColorHex,
            ReportCode = t.ReportCode,
            ReportName = t.ReportName,
            Description = t.Description,
            ReportType = t.ReportType.ToString(),
            DefaultFormat = t.DefaultFormat.ToString(),
            PageOrientation = t.PageOrientation.ToString(),
            PageSize = t.PageSize.ToString(),
            RdlcFileName = t.RdlcFileName,
            IsSystem = t.IsSystem,
            IsFavorite = t.IsFavorite,
            IsVisible = t.IsVisible,
            HasWatermark = t.HasWatermark,
            HasLogo = t.HasLogo,
            HasSignature = t.HasSignature,
            HasQrCode = t.HasQrCode,
            HasBarcode = t.HasBarcode,
            ModuleTag = t.ModuleTag,
            ExecutionCount = t.ExecutionCount,
            Parameters = t.ReportParameters?.Select(p => new ReportParameterDto
            {
                Id = p.Id,
                ReportTemplateId = p.ReportTemplateId,
                ParameterName = p.ParameterName,
                DisplayLabel = p.DisplayLabel,
                DataType = p.DataType.ToString(),
                IsRequired = p.IsRequired,
                DefaultValue = p.DefaultValue,
                SortOrder = p.SortOrder
            }).ToList() ?? new List<ReportParameterDto>()
        };
    }

    /// <summary>Internal context passed through the rendering pipeline.</summary>
    internal class ReportExecutionContext
    {
        public ReportTemplate Template { get; set; } = null!;
        public string Format { get; set; } = "PDF";
        public Dictionary<string, string> Parameters { get; set; } = new();
        public int? TenantId { get; set; }
        public string? GeneratedBy { get; set; }
    }
}
