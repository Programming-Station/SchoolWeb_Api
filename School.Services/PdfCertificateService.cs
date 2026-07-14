using Microsoft.EntityFrameworkCore;
using School.Domain.FeeManagnment;
using School.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using School.Services.Interfaces;
using School_DTOs.Student;
using QRCoder;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using School.Services.School.ISchoolServices;

namespace School.Services
{
    public class PdfCertificateService : IPdfCertificateService
    {
        private readonly SchoolDbContext _dbContext;
        private readonly IBrandingService _brandingService;

        public PdfCertificateService(SchoolDbContext dbContext, IBrandingService brandingService)
        {
            _dbContext = dbContext;
            _brandingService = brandingService;
        }

        public async Task<byte[]> GenerateRegistrationCertificateAsync(AdmissionApplicationDto registration, string baseUrl)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var branding = await _brandingService.GetProfileAsync();

            var qrCodeUrl = $"{baseUrl}/api/Admission/GetById/{registration.Id}";
            var qrCodeBytes = GenerateQrCode(qrCodeUrl);

            var bgPath = !string.IsNullOrEmpty(branding.ReportBackground)
                ? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", branding.ReportBackground.TrimStart('/'))
                : Path.Combine(Directory.GetCurrentDirectory(), "ReportsImages", "certificate-bg.png");
            if (!File.Exists(bgPath)) bgPath = Path.Combine(Directory.GetCurrentDirectory(), "ReportsImages", "certificate-bg.png");
            var backgroundImage = File.Exists(bgPath) ? File.ReadAllBytes(bgPath) : Array.Empty<byte>();

            var logoPath = !string.IsNullOrEmpty(branding.HeaderLogo)
                ? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", branding.HeaderLogo.TrimStart('/'))
                : Path.Combine(Directory.GetCurrentDirectory(), "ReportsImages", "Collegelogo.png");
            if (!File.Exists(logoPath)) logoPath = Path.Combine(Directory.GetCurrentDirectory(), "ReportsImages", "Collegelogo.png");
            var logoImage = File.Exists(logoPath) ? File.ReadAllBytes(logoPath) : Array.Empty<byte>();

            byte[] studentPhoto = null;
            if (!string.IsNullOrWhiteSpace(registration.PhotoUrl))
            {
                var photoPath = Path.Combine(Directory.GetCurrentDirectory(), registration.PhotoUrl);
                if (File.Exists(photoPath)) studentPhoto = File.ReadAllBytes(photoPath);
            }

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    if (backgroundImage.Length > 0)
                    {
                        page.Background().Image(backgroundImage).FitArea();
                    }

                    page.Header().PaddingBottom(10).Column(col =>
                    {
                        col.Item().AlignCenter().Text(branding.SchoolName ?? branding.OrganizationName)
                            .Bold().FontSize(26).FontColor(Color.FromHex(branding.PrimaryColor ?? "#d9261c"));

                        col.Item().AlignCenter().Text($"AFFILIATED TO {branding.Board ?? "BOARD"} | REC. NO: {branding.RecognitionNumber ?? "AN ISO 9001:2015 CERTIFIED"}")
                            .FontColor(Color.FromHex(branding.SecondaryColor ?? "#78c3e6")).FontSize(16);

                        if (logoImage.Length > 0)
                        {
                            col.Item().AlignCenter().Container().Width(80).Height(80).Image(logoImage).FitArea();
                        }

                        col.Item().PaddingTop(5).AlignCenter()
                            .Text("CERTIFICATE OF REGISTRATION / MEMBERSHIP").Bold().FontSize(18);
                    });

                    page.Content().Column(column =>
                    {
                        column.Spacing(6);

                        column.Item().Text(
                            $"On the recommendation of the Academic Council & Governing Body of {(branding.SchoolName ?? branding.OrganizationName)} hereby confers upon"
                        ).AlignCenter().FontSize(9);

                        column.Item().AlignCenter()
                            .Text(registration.CourseName ?? "N/A")
                            .Bold().FontSize(12);

                        column.Item().PaddingTop(8).Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(2);   // Label
                                c.RelativeColumn(3);   // Value
                                c.ConstantColumn(100); // Photo
                            });

                            table.Cell().Element(CellStyle).Text("Registration No").Bold();
                            table.Cell().Element(CellStyle).Text(registration.AdmissionNo ?? $"ADM/{registration.Id}/25");
                            table.Cell().RowSpan(4)
                                .AlignCenter().AlignMiddle()
                                .Width(90)
                                .Height(100)        // 3:4 ratio
                                .Padding(4)
                                .Image(studentPhoto ?? Array.Empty<byte>()).FitArea();

                            table.Cell().Element(CellStyle).Text("Name").Bold();
                            table.Cell().Element(CellStyle).Text(registration.FullName);

                            table.Cell().Element(CellStyle).Text("Father Name").Bold();
                            table.Cell().Element(CellStyle).Text(registration.FathersName);

                            table.Cell().Element(CellStyle).Text("Mother Name").Bold();
                            table.Cell().Element(CellStyle).Text(registration.MothersName);

                            table.Cell().Element(CellStyle).Text("Month / Year of Passing").Bold();
                            table.Cell().Element(CellStyle).Text(FormatPassYear(registration.LastPassingYear));
                            table.Cell().Element(CellStyle); // blank

                            table.Cell().Element(CellStyle).Text("Blood Group").Bold();
                            table.Cell().Element(CellStyle).Text(registration.BloodGroup ?? "N/A");
                            table.Cell().Element(CellStyle); // blank
                        });

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(2);
                                c.RelativeColumn(6);
                            });

                            table.Cell().Element(CellStyle).Text("Address").Bold();
                            table.Cell().Element(CellStyle)
                                .Text($"{registration.PermanentAddress}, PIN - {registration.PermanentPinCode}");

                            table.Cell().Element(CellStyle).Text("Examining Body").Bold();
                            table.Cell().Element(CellStyle).Text(registration.LastInstituteName ?? "N/A");

                            table.Cell().Element(CellStyle).Text("Date of Birth").Bold();
                            table.Cell().Element(CellStyle)
                                .Text(registration.DateOfBirth != default(DateTime) ? registration.DateOfBirth.ToString("dd-MMM-yyyy") : "N/A");

                            var issueDate = DateTime.UtcNow;
                            var expiryDate = issueDate.AddYears(5);

                            table.Cell().Element(CellStyle).Text("Date of Issue").Bold();
                            table.Cell().Element(CellStyle).Text(issueDate.ToString("dd-MMM-yyyy"));

                            table.Cell().Element(CellStyle).Text("Up to Valid Date").Bold();
                            table.Cell().Element(CellStyle).Text(expiryDate.ToString("dd-MMM-yyyy"));

                            table.Cell().Element(CellStyle).Text("Place").Bold();
                            table.Cell().Element(CellStyle).Text(branding.City ?? "DELHI");
                        });

                        column.Item().PaddingTop(6)
                            .Text(branding.Disclaimer ?? $"{branding.SchoolName ?? branding.OrganizationName} has the right to cancel the certificate if any information is found incorrect.")
                            .FontSize(8).Italic();

                        column.Item().PaddingTop(8).AlignRight().Column(qr =>
                        {
                            qr.Item().Width(85).Height(85)
                                .Image(qrCodeBytes).FitArea();

                            qr.Item().AlignCenter()
                                .Text("Scan to Verify").FontSize(7);
                        });
                    });

                    page.Footer().AlignCenter().PaddingTop(10).Column(col =>
                    {
                        col.Item().Text(branding.ChairmanName ?? "CHAIRPERSON").Bold().FontSize(10);
                        col.Item().Text($"{(branding.Website ?? "https://Schoolvns.org")} | {(branding.Email ?? "Schoolvns@gmail.com")}").FontSize(8);
                    });
                });
            });

            return await Task.Run(() => document.GeneratePdf());
        }

        private byte[] GenerateQrCode(string data)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }

        private static IContainer CellStyle(IContainer container)
        {
            return container
                .PaddingVertical(8)
                .PaddingHorizontal(5); 
        }

        public async Task<byte[]> GenerateFeeReceiptPdfAsync(int paymentId, string baseUrl)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var branding = await _brandingService.GetProfileAsync();

            var payment = await _dbContext.FeePayments
                .Include(p => p.Student)
                    .ThenInclude(s => s.Class)
                .Include(p => p.FeeInstallment)
                .FirstOrDefaultAsync(p => p.Id == paymentId && !p.IsDeleted);

            if (payment == null) return Array.Empty<byte>();

            var qrCodeUrl = $"{baseUrl}/api/FeeCollection/GetReceipt?paymentId={paymentId}";
            var qrCodeBytes = GenerateQrCode(qrCodeUrl);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(35);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    page.Header().PaddingBottom(20).Column(col =>
                    {
                        col.Item().AlignCenter().Text(branding.SchoolName ?? branding.OrganizationName)
                            .Bold().FontSize(22).FontColor(Color.FromHex(branding.PrimaryColor ?? "#1E3A8A"));

                        col.Item().AlignCenter().Text("Fee Payment Receipt")
                            .FontColor(Colors.Grey.Darken2).FontSize(14).Bold();
                    });

                    page.Content().Column(column =>
                    {
                        column.Spacing(10);

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text($"Receipt No: {payment.ReceiptNo}").Bold();
                                c.Item().Text($"Date: {payment.PaymentDate:dd-MMM-yyyy hh:mm tt}");
                                c.Item().Text($"Mode: {payment.PaymentMode}");
                                c.Item().Text($"Ref: {payment.TransactionRef ?? "-"}");
                            });

                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text($"Student: {payment.Student?.Name}").Bold();
                                c.Item().Text($"Class: {payment.Student?.Class?.Name ?? "N/A"}");
                                c.Item().Text($"Fathers Name: {payment.Student?.FathersName ?? "N/A"}");
                            });
                        });

                        column.Item().PaddingTop(15).Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(3);   // Description
                                c.RelativeColumn(1);   // Status
                                c.RelativeColumn(1);   // Amount
                            });

                            table.Cell().Background(Colors.Grey.Lighten3).Element(CellStyle).Text("Installment Description").Bold();
                            table.Cell().Background(Colors.Grey.Lighten3).Element(CellStyle).Text("Status").Bold();
                            table.Cell().Background(Colors.Grey.Lighten3).Element(CellStyle).Text("Amount Paid").Bold();

                            table.Cell().Element(CellStyle).Text(payment.FeeInstallment?.InstallmentName ?? "Quarterly Installment");
                            table.Cell().Element(CellStyle).Text(payment.Status);
                            table.Cell().Element(CellStyle).Text(payment.AmountPaid.ToString("C"));
                        });

                        column.Item().PaddingTop(20).Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Terms & Conditions:").Bold().FontSize(8);
                                if (!string.IsNullOrEmpty(branding.TermsAndConditions))
                                {
                                    c.Item().Text(branding.TermsAndConditions).FontSize(7);
                                }
                                else
                                {
                                    c.Item().Text("1. Fees once paid are non-refundable.").FontSize(7);
                                    c.Item().Text("2. This is a computer-generated document and requires no physical signature.").FontSize(7);
                                }
                            });

                            row.ConstantItem(85).Column(qr =>
                            {
                                qr.Item().Width(70).Height(70).Image(qrCodeBytes).FitArea();
                                qr.Item().AlignCenter().Text("Verify Receipt").FontSize(6);
                            });
                        });
                    });

                    page.Footer().AlignCenter().PaddingTop(15).Text(t =>
                    {
                        t.Span(branding.ReportFooterText ?? "Generated via School SAAS Billing Gateway").FontSize(8).Italic();
                    });
                });
            });

            return await Task.Run(() => document.GeneratePdf());
        }

        private static string FormatPassYear(string passYear)
        {
            if (string.IsNullOrEmpty(passYear))
                return DateTime.UtcNow.ToString("MMMM-yyyy");

            if (passYear.Length == 4 && int.TryParse(passYear, out int year))
            {
                return DateTime.UtcNow.ToString("MMMM") + "-" + passYear;
            }

            return passYear;
        }
    }
}
