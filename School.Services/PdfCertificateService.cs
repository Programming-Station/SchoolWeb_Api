using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using School.Services.Interfaces;
using School_DTOs.Student;
using QRCoder;

namespace School.Services
{
    public class PdfCertificateService : IPdfCertificateService
    {























































        public async Task<byte[]> GenerateRegistrationCertificateAsync(StudentRegistrationDto registration,string baseUrl)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var qrCodeUrl = $"{baseUrl}/api/StudentRegistration/Verify/{registration.Id}";
            var qrCodeBytes = GenerateQrCode(qrCodeUrl);

            var bgPath = Path.Combine(Directory.GetCurrentDirectory(), "ReportsImages", "certificate-bg.png");
            var backgroundImage = File.ReadAllBytes(bgPath);

            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "ReportsImages", "Collegelogo.png");
            var logoImage = File.ReadAllBytes(logoPath);

            byte[] studentPhoto = null;
            if (!string.IsNullOrWhiteSpace(registration.PassportPhoto))
            {
                var photoPath = Path.Combine(Directory.GetCurrentDirectory(),registration.PassportPhoto);
                if (File.Exists(photoPath))studentPhoto = File.ReadAllBytes(photoPath);
            }

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));


                    page.Background().Image(backgroundImage).FitArea(); // ya .Stretch() 
                    page.Header().PaddingBottom(10).Column(col =>
                    {
                        col.Item().AlignCenter().Text("School PARAMEDICAL COUNCIL OF INDIA")
                            .Bold().FontSize(26).FontColor(Color.FromHex("#d9261c"));

                        col.Item().AlignCenter().Text("AN ISO 9001:2015 CERTIFIED")
                            .FontColor(Color.FromHex("#78c3e6")).FontSize(16);

                        col.Item().AlignCenter().Container().Width(80).Height(80).Image(logoImage).FitArea();

                        col.Item().PaddingTop(5).AlignCenter()
                            .Text("CERTIFICATE OF REGISTRATION / MEMBERSHIP").Bold().FontSize(18);
                    });

                    page.Content().Column(column =>
                    {
                        column.Spacing(6);

                        column.Item().Text(
                            "On the recommendation of the Academic Council & Governing Body of Paramedical Council of India hereby confers upon"
                        ).AlignCenter().FontSize(9);

                        column.Item().AlignCenter()
                            .Text(registration.CourseName ?? registration.CourseType)
                            .Bold().FontSize(12);

                        column.Item().PaddingTop(8).Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(2);   // Label
                                c.RelativeColumn(3);   // Value
                                c.ConstantColumn(100); // Photo
                            });

                            void Cell(string title, string value)
                            {
                                table.Cell().Element(CellStyle).Text(title).Bold();
                                table.Cell().Element(CellStyle).Text(value);
                                table.Cell().Element(CellStyle); // leave photo cell blank
                            }

                            table.Cell().Element(CellStyle).Text("Registration No").Bold();
                            table.Cell().Element(CellStyle).Text(registration.CouncilEnrollmentNo ?? $"PMCI/{registration.Id}/25");
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
                            table.Cell().Element(CellStyle).Text(FormatPassYear(registration.PassYear));
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
                                .Text($"{registration.PermanentAddress}, PIN - {registration.PinCode}");

                            table.Cell().Element(CellStyle).Text("Examining Body").Bold();
                            table.Cell().Element(CellStyle).Text(registration.InstituteName);

                            table.Cell().Element(CellStyle).Text("Date of Birth").Bold();
                            table.Cell().Element(CellStyle)
                                .Text(registration.DateOfBirth != DateTime.MinValue ? registration.DateOfBirth.ToString("dd-MMM-yyyy") : "N/A");

                            var issueDate = DateTime.UtcNow;
                            var expiryDate = issueDate.AddYears(5);

                            table.Cell().Element(CellStyle).Text("Date of Issue").Bold();
                            table.Cell().Element(CellStyle).Text(issueDate.ToString("dd-MMM-yyyy"));

                            table.Cell().Element(CellStyle).Text("Up to Valid Date").Bold();
                            table.Cell().Element(CellStyle).Text(expiryDate.ToString("dd-MMM-yyyy"));

                            table.Cell().Element(CellStyle).Text("Place").Bold();
                            table.Cell().Element(CellStyle).Text("DELHI");
                        });

                        column.Item().PaddingTop(6)
                            .Text("Para Medical Council of India has the right to cancel the certificate if any information is found incorrect.")
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
                        col.Item().Text("CHAIRPERSON").Bold().FontSize(10);
                        col.Item().Text("https://Schoolvns.org | Schoolvns@gmail.com").FontSize(8);
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

