using Microsoft.EntityFrameworkCore;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using School.Infrastructure;
using School.Services.Interfaces;
using School.Services.School.ISchoolServices;

namespace School.Services.Fee
{
    public class ReceiptService : IReceiptService
    {
        private readonly SchoolDbContext _dbContext;
        private readonly IBrandingService _brandingService;

        public ReceiptService(SchoolDbContext dbContext, IBrandingService brandingService)
        {
            _dbContext = dbContext;
            _brandingService = brandingService;
        }

        public async Task<byte[]> GenerateReceiptPdfAsync(int paymentId, string baseUrl)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var branding = await _brandingService.GetProfileAsync();

            var payment = await _dbContext.FeePayments
                .Include(p => p.Student)
                    .ThenInclude(s => s.Class)
                .Include(p => p.FeeInstallment)
                .FirstOrDefaultAsync(p => p.Id == paymentId && !p.IsDeleted);

            if (payment == null) return Array.Empty<byte>();

            var qrCodeUrl = $"{baseUrl}/fees/receipt/{paymentId}";
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
    }
}
