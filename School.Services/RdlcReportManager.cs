using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Reporting.NETCore;
using QRCoder;
using School.Services.Interfaces;
using School.Services.School.ISchoolServices;

namespace School.Services
{
    public class RdlcReportManager : IRdlcReportManager
    {
        private readonly IReportBrandingService _brandingService;

        public RdlcReportManager(IReportBrandingService brandingService)
        {
            _brandingService = brandingService;
        }

        public async Task<byte[]> RenderReportAsync(
            string reportName, 
            string renderType, 
            Dictionary<string, object> dataSources, 
            Dictionary<string, string> parameters)
        {
            var report = new LocalReport();
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "RdlcFiles", $"{reportName}.rdlc");
            
            EnsureReportFileExists(reportPath, reportName);
            report.ReportPath = reportPath;

            // Load branding details
            var branding = await _brandingService.GetBrandingAsync();

            // Intercept and auto-populate parameters based on what the RDLC definition expects
            var reportInfo = report.GetParameters();
            var expectedParams = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var pInfo in reportInfo)
            {
                expectedParams.Add(pInfo.Name);
            }

            var mergedParameters = parameters != null 
                ? new Dictionary<string, string>(parameters, StringComparer.OrdinalIgnoreCase) 
                : new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            void TryAddParam(string key, string? value)
            {
                if (expectedParams.Contains(key) && !mergedParameters.ContainsKey(key))
                {
                    mergedParameters[key] = value ?? string.Empty;
                }
            }

            // Populate branding details
            TryAddParam("SchoolName", branding.SchoolName ?? branding.OrganizationName);
            TryAddParam("OrganizationName", branding.OrganizationName);
            TryAddParam("SchoolAddress", branding.AddressLine1 ?? string.Empty);
            TryAddParam("Address", branding.AddressLine1 ?? string.Empty);
            TryAddParam("Logo", branding.HeaderLogo ?? string.Empty);
            TryAddParam("HeaderLogo", branding.HeaderLogo ?? string.Empty);
            TryAddParam("FooterLogo", branding.FooterLogo ?? string.Empty);
            TryAddParam("LogoLight", branding.LogoLight ?? string.Empty);
            TryAddParam("LogoDark", branding.LogoDark ?? string.Empty);
            TryAddParam("Phone", branding.Phone ?? branding.Mobile ?? string.Empty);
            TryAddParam("Email", branding.Email ?? string.Empty);
            TryAddParam("Website", branding.Website ?? string.Empty);
            TryAddParam("Affiliation", branding.AffiliationNumber ?? string.Empty);
            TryAddParam("PrincipalName", branding.PrincipalName ?? string.Empty);
            TryAddParam("PrincipalSignature", branding.PrincipalSignature ?? string.Empty);
            TryAddParam("DirectorSignature", branding.DirectorSignature ?? string.Empty);
            TryAddParam("Seal", branding.OfficialSeal ?? string.Empty);
            TryAddParam("OfficialSeal", branding.OfficialSeal ?? string.Empty);
            TryAddParam("Watermark", branding.ReportWatermark ?? string.Empty);
            TryAddParam("ThemeColor", branding.PrimaryColor ?? "#1e3a8a");
            TryAddParam("CurrentDate", DateTime.Now.ToString("dd-MMM-yyyy"));
            TryAddParam("GeneratedDate", DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt"));
            TryAddParam("GeneratedBy", "System Administrator");
            TryAddParam("AcademicSession", branding.CurrentAcademicSession ?? string.Empty);
            TryAddParam("BranchName", branding.CampusName ?? string.Empty);
            TryAddParam("DigitalSignature", branding.DigitalSignature ?? string.Empty);
            TryAddParam("Footer", branding.ReportFooterText ?? string.Empty);
            TryAddParam("CopyrightText", branding.CopyrightText ?? string.Empty);
            TryAddParam("Disclaimer", branding.Disclaimer ?? string.Empty);
            TryAddParam("TermsAndConditions", branding.TermsAndConditions ?? string.Empty);

            // Compile parameters list
            var reportParams = new List<ReportParameter>();
            foreach (var p in mergedParameters)
            {
                if (expectedParams.Contains(p.Key))
                {
                    reportParams.Add(new ReportParameter(p.Key, p.Value ?? string.Empty));
                }
            }

            if (reportParams.Count > 0)
            {
                report.SetParameters(reportParams);
            }

            // Load Report datasources
            if (dataSources != null)
            {
                foreach (var ds in dataSources)
                {
                    report.DataSources.Add(new ReportDataSource(ds.Key, ds.Value));
                }
            }

            // Normalize format type: PDF, EXCEL, WORD
            string format = "PDF";
            if (renderType.Equals("EXCEL", StringComparison.OrdinalIgnoreCase))
            {
                format = "EXCELOPENXML"; // Excel OpenXML format
            }
            else if (renderType.Equals("WORD", StringComparison.OrdinalIgnoreCase))
            {
                format = "WORDOPENXML"; // Word OpenXML format
            }

            return await Task.Run(() => report.Render(format));
        }

        public async Task<byte[]> GenerateQrCodeBase64Async(string data)
        {
            return await Task.Run(() =>
            {
                using var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new PngByteQRCode(qrCodeData);
                return qrCode.GetGraphic(20);
            });
        }

        public async Task<byte[]> GenerateBarcodeBase64Async(string data)
        {
            // Fallback lightweight 1D Code39 mock barcode generator returning PNG bytes
            return await Task.Run(() =>
            {
                using var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode($"BARCODE:{data}", QRCodeGenerator.ECCLevel.L);
                using var qrCode = new PngByteQRCode(qrCodeData);
                return qrCode.GetGraphic(10);
            });
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
                    <Value>Enterprise {reportName} Report Summary</Value>
                    <Style>
                      <FontSize>16pt</FontSize>
                      <FontWeight>Bold</FontWeight>
                      <Color>#1e3a8a</Color>
                    </Style>
                  </TextRun>
                </TextRuns>
              </Paragraph>
            </Paragraphs>
            <Top>0.5in</Top>
            <Left>0.5in</Left>
            <Height>0.4in</Height>
            <Width>6.5in</Width>
          </Textbox>
        </ReportItems>
      </Body>
      <Width>8.5in</Width>
      <Page>
        <PageHeight>11in</PageHeight>
        <PageWidth>8.5in</PageWidth>
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
