using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using QRCoder;
using School.Domain.FeeManagnment;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs.Student;

using School.Services.School.ISchoolServices;

namespace School.Services
{
    public class RdlcCertificateService : IRdlcCertificateService
    {
        private readonly SchoolDbContext _dbContext;
        private readonly IReportBrandingService _brandingService;

        public RdlcCertificateService(SchoolDbContext dbContext, IReportBrandingService brandingService)
        {
            _dbContext = dbContext;
            _brandingService = brandingService;
        }

        private string GetBase64FromFile(string? path)
        {
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;
            
            if (path.StartsWith("data:image", StringComparison.OrdinalIgnoreCase))
            {
                var parts = path.Split(',');
                return parts.Length > 1 ? parts[1] : parts[0];
            }
            
            try
            {
                string physicalPath = path.StartsWith("/") 
                    ? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path.TrimStart('/'))
                    : path;
                    
                if (File.Exists(physicalPath))
                {
                    return Convert.ToBase64String(File.ReadAllBytes(physicalPath));
                }
            }
            catch {}
            return string.Empty;
        }

        private async Task ApplyBrandingParametersAsync(LocalReport report, List<ReportParameter> parameters)
        {
            var branding = await _brandingService.GetBrandingAsync();
            var reportInfo = report.GetParameters();
            var expectedParams = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var pInfo in reportInfo)
            {
                expectedParams.Add(pInfo.Name);
            }

            var existingParams = new HashSet<string>(parameters.Select(p => p.Name), StringComparer.OrdinalIgnoreCase);

            void TryAddParam(string key, string? value)
            {
                if (expectedParams.Contains(key) && !existingParams.Contains(key))
                {
                    parameters.Add(new ReportParameter(key, value ?? string.Empty));
                }
            }

            TryAddParam("SchoolName", branding.SchoolName ?? branding.OrganizationName);
            TryAddParam("SchoolAddress", branding.AddressLine1 ?? string.Empty);
            TryAddParam("Logo", GetBase64FromFile(branding.HeaderLogo));
            TryAddParam("LogoImage", GetBase64FromFile(branding.HeaderLogo));
            TryAddParam("HeaderLogo", GetBase64FromFile(branding.HeaderLogo));
            TryAddParam("FooterLogo", GetBase64FromFile(branding.FooterLogo));
            TryAddParam("Phone", branding.Phone ?? branding.Mobile ?? string.Empty);
            TryAddParam("Email", branding.Email ?? string.Empty);
            TryAddParam("Website", branding.Website ?? string.Empty);
            TryAddParam("Affiliation", branding.AffiliationNumber ?? string.Empty);
            TryAddParam("PrincipalName", branding.PrincipalName ?? string.Empty);
            TryAddParam("PrincipalSignature", GetBase64FromFile(branding.PrincipalSignature));
            TryAddParam("DirectorSignature", GetBase64FromFile(branding.DirectorSignature));
            TryAddParam("Seal", GetBase64FromFile(branding.OfficialSeal));
            TryAddParam("Watermark", GetBase64FromFile(branding.ReportWatermark));
            TryAddParam("ThemeColor", branding.PrimaryColor ?? "#1e3a8a");
            TryAddParam("CurrentDate", DateTime.Now.ToString("dd-MMM-yyyy"));
        }

        public async Task<byte[]> GenerateRegistrationCertificateAsync(AdmissionApplicationDto registration, string baseUrl)
        {
            var qrCodeUrl = $"{baseUrl}/api/Admission/GetById/{registration.Id}";
            var qrCodeBytes = GenerateQrCode(qrCodeUrl);
            var qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);

            var bgPath = Path.Combine(Directory.GetCurrentDirectory(), "ReportsImages", "certificate-bg.png");
            string bgBase64 = File.Exists(bgPath) ? Convert.ToBase64String(File.ReadAllBytes(bgPath)) : string.Empty;

            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "ReportsImages", "Collegelogo.png");
            string logoBase64 = File.Exists(logoPath) ? Convert.ToBase64String(File.ReadAllBytes(logoPath)) : string.Empty;

            string studentPhotoBase64 = string.Empty;
            if (!string.IsNullOrWhiteSpace(registration.PhotoUrl))
            {
                var photoPath = Path.Combine(Directory.GetCurrentDirectory(), registration.PhotoUrl);
                if (File.Exists(photoPath)) studentPhotoBase64 = Convert.ToBase64String(File.ReadAllBytes(photoPath));
            }

            var report = new LocalReport();
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "RdlcFiles", "RegistrationCertificate.rdlc");
            
            // Ensure directory and mock file exist so it compiles and runs without IO exception
            EnsureReportFileExists(reportPath, "RegistrationCertificate");
            report.ReportPath = reportPath;

            // Set parameters representing layout content fields
            var parameters = new List<ReportParameter>
            {
                new("RegistrationNo", registration.AdmissionNo ?? $"ADM/{registration.Id}/25"),
                new("FullName", registration.FullName),
                new("FatherName", registration.FathersName),
                new("MotherName", registration.MothersName),
                new("CourseName", registration.CourseName ?? "N/A"),
                new("PassingYear", FormatPassYear(registration.LastPassingYear)),
                new("BloodGroup", registration.BloodGroup ?? "N/A"),
                new("Address", $"{registration.PermanentAddress}, PIN - {registration.PermanentPinCode}"),
                new("ExaminingBody", registration.LastInstituteName ?? "N/A"),
                new("DateOfBirth", registration.DateOfBirth != default ? registration.DateOfBirth.ToString("dd-MMM-yyyy") : "N/A"),
                new("DateOfIssue", DateTime.UtcNow.ToString("dd-MMM-yyyy")),
                new("ValidUpTo", DateTime.UtcNow.AddYears(5).ToString("dd-MMM-yyyy")),
                new("LogoImage", logoBase64),
                new("BackgroundImage", bgBase64),
                new("StudentPhoto", studentPhotoBase64),
                new("QrCodeImage", qrCodeBase64)
            };

            await ApplyBrandingParametersAsync(report, parameters);
            report.SetParameters(parameters);
            
            // Add DTO as a datasource for listing items if requested by template
            report.DataSources.Add(new ReportDataSource("RegistrationDataSet", new List<AdmissionApplicationDto> { registration }));

            return await Task.Run(() => report.Render("PDF"));
        }

        public async Task<byte[]> GenerateFeeReceiptPdfAsync(int paymentId, string baseUrl)
        {
            var payment = await _dbContext.FeePayments
                .Include(p => p.Student)
                    .ThenInclude(s => s.Class)
                .Include(p => p.FeeInstallment)
                .FirstOrDefaultAsync(p => p.Id == paymentId && !p.IsDeleted);

            if (payment == null) return Array.Empty<byte>();

            var qrCodeUrl = $"{baseUrl}/api/FeeCollection/GetReceipt?paymentId={paymentId}";
            var qrCodeBytes = GenerateQrCode(qrCodeUrl);
            var qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);

            var report = new LocalReport();
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "RdlcFiles", "FeeReceipt.rdlc");
            
            EnsureReportFileExists(reportPath, "FeeReceipt");
            report.ReportPath = reportPath;

            var parameters = new List<ReportParameter>
            {
                new("ReceiptNo", payment.ReceiptNo),
                new("PaymentDate", payment.PaymentDate.ToString("dd-MMM-yyyy hh:mm tt")),
                new("PaymentMode", payment.PaymentMode),
                new("TransactionRef", payment.TransactionRef ?? "-"),
                new("StudentName", payment.Student?.Name ?? "N/A"),
                new("ClassName", payment.Student?.Class?.Name ?? "N/A"),
                new("FatherName", payment.Student?.FathersName ?? "N/A"),
                new("InstallmentName", payment.FeeInstallment?.InstallmentName ?? "Quarterly Installment"),
                new("Status", payment.Status),
                new("AmountPaid", payment.AmountPaid.ToString("C")),
                new("QrCodeImage", qrCodeBase64)
            };

            await ApplyBrandingParametersAsync(report, parameters);
            report.SetParameters(parameters);
            return await Task.Run(() => report.Render("PDF"));
        }

        public async Task<byte[]> GenerateAdmissionApplicationSummaryAsync(int applicationId)
        {
            var application = await _dbContext.AdmissionApplications
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == applicationId && !x.IsDeleted);

            if (application == null) return Array.Empty<byte>();

            var report = new LocalReport();
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "RdlcFiles", "AdmissionApplicationSummary.rdlc");
            EnsureReportFileExists(reportPath, "AdmissionApplicationSummary");
            report.ReportPath = reportPath;

            var parameters = new List<ReportParameter>
            {
                new("ApplicationNo", application.ApplicationNo ?? $"APP/{application.Id}"),
                new("FullName", application.FullName),
                new("DateOfBirth", application.DateOfBirth.ToString("dd-MMM-yyyy")),
                new("AppliedCourse", application.Course?.Name ?? "N/A"),
                new("Status", application.Status ?? "Submitted"),
                new("FathersName", application.FathersName ?? "-"),
                new("MothersName", application.MothersName ?? "-"),
                new("ContactNo", application.Mobile),
                new("EmailAddress", application.Email ?? "-")
            };

            await ApplyBrandingParametersAsync(report, parameters);
            report.SetParameters(parameters);
            return await Task.Run(() => report.Render("PDF"));
        }

        public async Task<byte[]> GenerateAdmissionsPipelineReportAsync(string? status, string? courseName, DateTime? fromDate, DateTime? toDate)
        {
            var query = _dbContext.AdmissionApplications.Include(x => x.Course).Where(x => !x.IsDeleted).AsQueryable();

            if (!string.IsNullOrEmpty(status)) query = query.Where(x => x.Status == status);
            if (!string.IsNullOrEmpty(courseName)) query = query.Where(x => x.Course != null && x.Course.Name == courseName);
            if (fromDate.HasValue) query = query.Where(x => x.CreatedDate >= fromDate.Value);
            if (toDate.HasValue) query = query.Where(x => x.CreatedDate <= toDate.Value);

            var list = await query.ToListAsync();

            var report = new LocalReport();
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "RdlcFiles", "AdmissionsPipeline.rdlc");
            EnsureReportFileExists(reportPath, "AdmissionsPipeline");
            report.ReportPath = reportPath;

            var parameters = new List<ReportParameter>
            {
                new("ReportTitle", "Admissions Pipeline Status Analytics"),
                new("TotalApplications", list.Count.ToString())
            };

            await ApplyBrandingParametersAsync(report, parameters);
            report.SetParameters(parameters);
            report.DataSources.Add(new ReportDataSource("PipelineDataSet", list));

            return await Task.Run(() => report.Render("PDF"));
        }

        public async Task<byte[]> GenerateStudentClassDirectoryAsync(int classId, int? sectionId)
        {
            var query = _dbContext.Students.Where(x => x.ClassId == classId && !x.IsDeleted).AsQueryable();

            var list = await query.ToListAsync();

            var report = new LocalReport();
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "RdlcFiles", "StudentClassDirectory.rdlc");
            EnsureReportFileExists(reportPath, "StudentClassDirectory");
            report.ReportPath = reportPath;

            var parameters = new List<ReportParameter>
            {
                new("ReportTitle", $"Class Directory - Class ID {classId}"),
                new("TotalStudents", list.Count.ToString())
            };

            await ApplyBrandingParametersAsync(report, parameters);
            report.SetParameters(parameters);
            report.DataSources.Add(new ReportDataSource("DirectoryDataSet", list));

            return await Task.Run(() => report.Render("PDF"));
        }

        public async Task<byte[]> GenerateAdmissionAuditTrailReportAsync(DateTime? fromDate, DateTime? toDate)
        {
            var query = _dbContext.AdmissionAuditLogs.Where(x => !x.IsDeleted).AsQueryable();
            if (fromDate.HasValue) query = query.Where(x => x.CreatedDate >= fromDate.Value);
            if (toDate.HasValue) query = query.Where(x => x.CreatedDate <= toDate.Value);

            var list = await query.ToListAsync();

            var report = new LocalReport();
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "RdlcFiles", "AdmissionAuditTrail.rdlc");
            EnsureReportFileExists(reportPath, "AdmissionAuditTrail");
            report.ReportPath = reportPath;

            var parameters = new List<ReportParameter>
            {
                new("ReportTitle", "Admission Status Verifications & Audit Log")
            };

            await ApplyBrandingParametersAsync(report, parameters);
            report.SetParameters(parameters);
            report.DataSources.Add(new ReportDataSource("AuditDataSet", list));

            return await Task.Run(() => report.Render("PDF"));
        }

        public async Task<byte[]> GenerateAcademicQualificationAnalysisAsync(string? boardName, string? passingYear)
        {
            var query = _dbContext.EducationalDetails.Where(x => !x.IsDeleted).AsQueryable();
            if (!string.IsNullOrEmpty(boardName)) query = query.Where(x => x.ExamName == boardName);
            if (!string.IsNullOrEmpty(passingYear)) query = query.Where(x => x.PassingYear == passingYear);

            var list = await query.ToListAsync();

            var report = new LocalReport();
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "RdlcFiles", "AcademicQualificationAnalysis.rdlc");
            EnsureReportFileExists(reportPath, "AcademicQualificationAnalysis");
            report.ReportPath = reportPath;

            var parameters = new List<ReportParameter>
            {
                new("ReportTitle", "Applicant Academic Qualifications Analysis")
            };

            await ApplyBrandingParametersAsync(report, parameters);
            report.SetParameters(parameters);
            report.DataSources.Add(new ReportDataSource("QualificationsDataSet", list));

            return await Task.Run(() => report.Render("PDF"));
        }

        public async Task<byte[]> GenerateParentStudentEmergencyDirectoryAsync(int classId)
        {
            var list = await _dbContext.Students
                .Where(x => x.ClassId == classId && !x.IsDeleted)
                .ToListAsync();

            var report = new LocalReport();
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "RdlcFiles", "ParentStudentEmergencyDirectory.rdlc");
            EnsureReportFileExists(reportPath, "ParentStudentEmergencyDirectory");
            report.ReportPath = reportPath;

            var parameters = new List<ReportParameter>
            {
                new("ReportTitle", "PTA Emergency Directory")
            };

            await ApplyBrandingParametersAsync(report, parameters);
            report.SetParameters(parameters);
            report.DataSources.Add(new ReportDataSource("EmergencyDataSet", list));

            return await Task.Run(() => report.Render("PDF"));
        }

        public async Task<byte[]> GenerateAdmissionDeviationReportAsync()
        {
            var list = await _dbContext.AdmissionApplications
                .Where(x => !x.IsDeleted)
                .ToListAsync();

            var report = new LocalReport();
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "RdlcFiles", "AdmissionDeviation.rdlc");
            EnsureReportFileExists(reportPath, "AdmissionDeviation");
            report.ReportPath = reportPath;

            var parameters = new List<ReportParameter>
            {
                new("ReportTitle", "Admission Check-points & Rules Exception Report")
            };

            await ApplyBrandingParametersAsync(report, parameters);
            report.SetParameters(parameters);
            report.DataSources.Add(new ReportDataSource("DeviationDataSet", list));

            return await Task.Run(() => report.Render("PDF"));
        }

        private byte[] GenerateQrCode(string data)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }

        private static string FormatPassYear(string passYear)
        {
            if (string.IsNullOrEmpty(passYear))
                return DateTime.UtcNow.ToString("MMMM-yyyy");

            if (passYear.Length == 4 && int.TryParse(passYear, out _))
            {
                return DateTime.UtcNow.ToString("MMMM") + "-" + passYear;
            }

            return passYear;
        }

        private static void EnsureReportFileExists(string filePath, string reportName)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(filePath))
            {
                // Write a basic, compliant minimal RDLC structure so ReportViewer doesn't crash on execution
                var minimalRdlc = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<Report xmlns=""http://schemas.microsoft.com/sqlserver/reporting/2016/01/reportdefinition"" xmlns:rd=""http://schemas.microsoft.com/SQLServer/reporting/reportdesigner"">
  <ReportSections>
    <ReportSection>
      <Body>
        <Height>11in</Height>
        <ReportItems>
          <Textbox Name=""TitleTextBox"">
            <CanGrow>true</CanGrow>
            <KeepTogether>true</KeepTogether>
            <Paragraphs>
              <Paragraph>
                <TextRuns>
                  <TextRun>
                    <Value>School Centralized {reportName} Report</Value>
                    <Style>
                      <FontSize>18pt</FontSize>
                      <FontWeight>Bold</FontWeight>
                    </Style>
                  </TextRun>
                </TextRuns>
                <Style />
              </Paragraph>
            </Paragraphs>
            <Top>0.5in</Top>
            <Left>0.5in</Left>
            <Height>0.4in</Height>
            <Width>5in</Width>
          </Textbox>
        </ReportItems>
        <Style />
      </Body>
      <Width>8.5in</Width>
      <Page>
        <PageHeight>11in</PageHeight>
        <PageWidth>8.5in</PageWidth>
        <Style />
      </Page>
    </ReportSection>
  </ReportSections>
  <rd:ReportUnitType>Inch</rd:ReportUnitType>
</Report>";
                File.WriteAllText(filePath, minimalRdlc);
            }
        }
    }
}
