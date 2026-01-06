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
        //public async Task<byte[]> GenerateRegistrationCertificateAsync(StudentRegistrationDto registration, string baseUrl)
        //{
        //    QuestPDF.Settings.License = LicenseType.Community;

        //    // Generate QR code
        //    var qrCodeUrl = $"{baseUrl}/api/StudentRegistration/Verify/{registration.Id}";
        //    var qrCodeBytes = GenerateQrCode(qrCodeUrl);
        //    var bgPath = Path.Combine(Directory.GetCurrentDirectory(), "ReportsImages","certificate-bg.png" );
        //    var backgroundImage = File.ReadAllBytes(bgPath);

        //    var document = Document.Create(container =>
        //    {
        //        container.Page(page =>
        //        {
        //            page.Size(PageSizes.A4);
        //            page.Margin(40);
        //            page.PageColor(Colors.White);
        //            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));
        //            // ✅ BACKGROUND IMAGE
        //            page.Background().Image(backgroundImage).FitArea(); // ya .Stretch()

        //            byte[] studentPhoto = null;
        //            if (!string.IsNullOrEmpty(registration.PassportPhoto))
        //            {
        //                var photoPath = Path.Combine(Directory.GetCurrentDirectory(),registration.PassportPhoto                        );
        //                if (File.Exists(photoPath))
        //                    studentPhoto = File.ReadAllBytes(photoPath);
        //            }


        //            page.Header()
        //                .Column(column =>
        //                {
        //                    column.Item().AlignCenter().Text("School PARA MEDICAL COUNCIL OF INDIA").Bold().FontSize(16);
        //                    column.Item().AlignCenter().Text("AN ISO 9001:2015 CERTIFIED").FontSize(10);
        //                    column.Item().PaddingTop(10).AlignCenter().Text("CERTIFICATE OF REGISTRATION/MEMBERSHIP").Bold().FontSize(14);
        //                    column.Item().PaddingTop(5).AlignCenter().Text("On the recommendation of the Academic Council & Governing Body of Paramedical Council of India hereby confers upon").FontSize(10);
        //                    column.Item().PaddingTop(10).AlignCenter().Text(registration.CourseName ?? registration.CourseType).Bold().FontSize(14);
        //                });

        //            page.Content()
        //                .PaddingVertical(20)
        //                .Column(column =>
        //                {
        //                    // Registration Details Table
        //                    column.Item().Table(table =>
        //                    {
        //                        table.ColumnsDefinition(columns =>
        //                        {
        //                            columns.RelativeColumn(2);
        //                            columns.RelativeColumn(3);
        //                        });

        //                        table.Cell().Element(CellStyle).Text("Registration No").Bold();
        //                        table.Cell().Element(CellStyle).Text(registration.CouncilEnrollmentNo ?? $"PMCI/{registration.Id}/25");

        //                        table.Cell().Element(CellStyle).Text("Name").Bold();
        //                        table.Cell().Element(CellStyle).Text(registration.FullName);

        //                        table.Cell().Element(CellStyle).Text("Father Name").Bold();
        //                        table.Cell().Element(CellStyle).Text(registration.FathersName);

        //                        table.Cell().Element(CellStyle).Text("Mother Name").Bold();
        //                        table.Cell().Element(CellStyle).Text(registration.MothersName);

        //                        // PHOTO cell (row-span effect by repeating empty cells)
        //                        table.Cell().RowSpan(3).Element(CellStyle).AlignCenter().AlignMiddle()
        //                            .Height(140)
        //                            .Border(1)
        //                            .Image(studentPhoto ?? Array.Empty<byte>())
        //                            .FitArea();

        //                        table.Cell().Element(CellStyle).Text("Month/Year of Passing").Bold();
        //                        table.Cell().Element(CellStyle).Text(FormatPassYear(registration.PassYear));

        //                        table.Cell().Element(CellStyle).Text("Blood Group").Bold();
        //                        table.Cell().Element(CellStyle).Text(registration.BloodGroup ?? "N/A");

        //                        table.Cell().Element(CellStyle).Text("Address").Bold();
        //                        table.Cell().Element(CellStyle).Text($"{registration.PermanentAddress}, PIN-{registration.PinCode}");

        //                        table.Cell().Element(CellStyle).Text("Examining Body").Bold();
        //                        table.Cell().Element(CellStyle).Text($"{registration.InstituteName}");

        //                        table.Cell().Element(CellStyle).Text("Date of Birth").Bold();
        //                        table.Cell().Element(CellStyle).Text(registration.DateOfBirth != DateTime.MinValue 
        //                            ? registration.DateOfBirth.ToString("dd-MMM-yyyy") 
        //                            : "N/A");

        //                        var issueDate = DateTime.UtcNow;
        //                        table.Cell().Element(CellStyle).Text("Date of Issue").Bold();
        //                        table.Cell().Element(CellStyle).Text(issueDate.ToString("dd-MMM-yyyy"));

        //                        //table.Cell().Element(CellStyle).Text("Date of Renew").Bold();
        //                       // table.Cell().Element(CellStyle).Text("-");

        //                        var expiryDate = issueDate.AddYears(5);
        //                        table.Cell().Element(CellStyle).Text("Date of Expiry").Bold();
        //                        table.Cell().Element(CellStyle).Text(expiryDate.ToString("dd-MMM-yyyy"));

        //                       // table.Cell().Element(CellStyle).Text("Valid Date").Bold();
        //                      //  table.Cell().Element(CellStyle).Text(issueDate.ToString("yyyy-MM-dd"));

        //                        table.Cell().Element(CellStyle).Text("Place").Bold();
        //                        table.Cell().Element(CellStyle).Text("DELHI");
        //                    });

        //                    column.Item().PaddingTop(20).Text("Para Medical Council of India has the right to cancel the certificate. If any information is found to be wrong or false.").FontSize(9).Italic();

        //                    // Add QR Code
        //                    column.Item().PaddingTop(20).Row(row =>
        //                    {
        //                        row.RelativeItem().AlignRight().Width(100).Height(100).Image(qrCodeBytes);
        //                        row.RelativeItem(2);
        //                    });
        //                    column.Item().PaddingTop(5).AlignRight().Text("Scan QR Code to verify certificate").FontSize(8).Italic();
        //                });

        //            page.Footer()
        //                .AlignCenter()
        //                .Column(column =>
        //                {
        //                    column.Item().PaddingTop(30).Text("CHAIRPERSON").Bold();
        //                    column.Item().PaddingTop(5).Text("https://Schoolvns.org, EMail:Schoolvns@gmail.com").FontSize(8);
        //                });
        //        });
        //    });

        //    return await Task.Run(() => document.GeneratePdf());
        //}
        //      public async Task<byte[]> GenerateRegistrationCertificateAsync(StudentRegistrationDto registration, string baseUrl)
        //      {
        //          QuestPDF.Settings.License = LicenseType.Community;

        //          // Generate QR code
        //          var qrCodeUrl = $"{baseUrl}/api/StudentRegistration/Verify/{registration.Id}";
        //          var qrCodeBytes = GenerateQrCode(qrCodeUrl);
        //          var bgPath = Path.Combine(Directory.GetCurrentDirectory(), "ReportsImages", "certificate-bg.png");
        //          var backgroundImage = File.ReadAllBytes(bgPath);

        //          var document = Document.Create(container =>
        //          {
        //              container.Page(page =>
        //              {
        //                  page.Size(PageSizes.A4);
        //                  page.Margin(40);
        //                  page.PageColor(Colors.White);
        //                  page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));
        //                  // ✅ BACKGROUND IMAGE
        //                  page.Background().Image(backgroundImage).FitArea(); // ya .Stretch()

        //                  byte[] studentPhoto = null;
        //                  if (!string.IsNullOrEmpty(registration.PassportPhoto))
        //                  {
        //                      var photoPath = Path.Combine(Directory.GetCurrentDirectory(), registration.PassportPhoto);
        //                      if (File.Exists(photoPath))
        //                          studentPhoto = File.ReadAllBytes(photoPath);
        //                  }


        //                  page.Header()
        //                      .Column(column =>
        //                      {
        //                          column.Item().AlignCenter().Text("School PARA MEDICAL COUNCIL OF INDIA").Bold().FontSize(16);
        //                          column.Item().AlignCenter().Text("AN ISO 9001:2015 CERTIFIED").FontSize(10);
        //                          column.Item().PaddingTop(10).AlignCenter().Text("CERTIFICATE OF REGISTRATION/MEMBERSHIP").Bold().FontSize(14);
        //                          column.Item().PaddingTop(5).AlignCenter().Text("On the recommendation of the Academic Council & Governing Body of Paramedical Council of India hereby confers upon").FontSize(10);
        //                          column.Item().PaddingTop(10).AlignCenter().Text(registration.CourseName ?? registration.CourseType).Bold().FontSize(14);
        //                      });

        //                  page.Content()
        //.PaddingVertical(25)
        //.Column(column =>
        //{
        //    column.Spacing(15);

        //    // 🔹 DETAILS + PHOTO GRID
        //    column.Item().Table(table =>
        //    {
        //        table.ColumnsDefinition(columns =>
        //        {
        //            columns.RelativeColumn(2);   // Label
        //            columns.RelativeColumn(3);   // Value
        //            columns.ConstantColumn(120); // Photo
        //        });

        //        // 🔹 Name
        //        table.Cell().Element(CellStyle).Text("Name").Bold();
        //        table.Cell().Element(CellStyle).Text(registration.FullName);
        //        table.Cell().RowSpan(4)
        //            .AlignCenter()
        //            .AlignMiddle()
        //            .Height(160)
        //            .Border(1)
        //            .BorderColor(Colors.Grey.Lighten1)
        //            .Padding(5)
        //            .Image(studentPhoto ?? Array.Empty<byte>())
        //            .FitArea();

        //        // 🔹 Father
        //        table.Cell().Element(CellStyle).Text("Father Name").Bold();
        //        table.Cell().Element(CellStyle).Text(registration.FathersName);

        //        // 🔹 Mother
        //        table.Cell().Element(CellStyle).Text("Mother Name").Bold();
        //        table.Cell().Element(CellStyle).Text(registration.MothersName);

        //        // 🔹 DOB
        //        table.Cell().Element(CellStyle).Text("Date of Birth").Bold();
        //        table.Cell().Element(CellStyle)
        //            .Text(registration.DateOfBirth != DateTime.MinValue
        //                ? registration.DateOfBirth.ToString("dd-MMM-yyyy")
        //                : "N/A");
        //    });

        //    // 🔹 SECOND DETAILS TABLE
        //    column.Item().Table(table =>
        //    {
        //        table.ColumnsDefinition(columns =>
        //        {
        //            columns.RelativeColumn(2);
        //            columns.RelativeColumn(3);
        //        });

        //        table.Cell().Element(CellStyle).Text("Registration No").Bold();
        //        table.Cell().Element(CellStyle)
        //            .Text(registration.CouncilEnrollmentNo ?? $"PMCI/{registration.Id}/25");

        //        table.Cell().Element(CellStyle).Text("Course").Bold();
        //        table.Cell().Element(CellStyle)
        //            .Text(registration.CourseName ?? registration.CourseType);

        //        table.Cell().Element(CellStyle).Text("Month / Year of Passing").Bold();
        //        table.Cell().Element(CellStyle).Text(FormatPassYear(registration.PassYear));

        //        table.Cell().Element(CellStyle).Text("Examining Body").Bold();
        //        table.Cell().Element(CellStyle).Text(registration.InstituteName);

        //        table.Cell().Element(CellStyle).Text("Blood Group").Bold();
        //        table.Cell().Element(CellStyle).Text(registration.BloodGroup ?? "N/A");

        //        table.Cell().Element(CellStyle).Text("Address").Bold();
        //        table.Cell().Element(CellStyle)
        //            .Text($"{registration.PermanentAddress}, PIN-{registration.PinCode}");

        //        var issueDate = DateTime.UtcNow;
        //        var expiryDate = issueDate.AddYears(5);

        //        table.Cell().Element(CellStyle).Text("Date of Issue").Bold();
        //        table.Cell().Element(CellStyle).Text(issueDate.ToString("dd-MMM-yyyy"));

        //        table.Cell().Element(CellStyle).Text("Date of Expiry").Bold();
        //        table.Cell().Element(CellStyle).Text(expiryDate.ToString("dd-MMM-yyyy"));

        //        table.Cell().Element(CellStyle).Text("Place").Bold();
        //        table.Cell().Element(CellStyle).Text("DELHI");
        //    });

        //    // 🔹 DECLARATION
        //    column.Item()
        //        .PaddingTop(10)
        //        .Text("Para Medical Council of India reserves the right to cancel this certificate if any information is found to be incorrect or misleading.")
        //        .FontSize(9)
        //        .Italic()
        //        .FontColor(Colors.Grey.Darken2);

        //    // 🔹 QR + TEXT
        //    column.Item().Row(row =>
        //    {
        //        row.RelativeItem();

        //        row.ConstantItem(110)
        //            .AlignCenter()
        //            .Column(qr =>
        //            {
        //                qr.Item()
        //                  .Width(90)
        //                  .Height(90)
        //                  .Image(qrCodeBytes)
        //                  .FitArea();

        //                qr.Item()
        //                  .PaddingTop(4)
        //                  .Text("Scan to Verify")
        //                  .FontSize(8)
        //                  .AlignCenter();
        //            });
        //    });

        //});

        //                  page.Footer()
        //                      .AlignCenter()
        //                      .Column(column =>
        //                      {
        //                          column.Item().PaddingTop(30).Text("CHAIRPERSON").Bold();
        //                          column.Item().PaddingTop(5).Text("https://Schoolvns.org, EMail:Schoolvns@gmail.com").FontSize(8);
        //                      });
        //              });
        //          });

        //          return await Task.Run(() => document.GeneratePdf());
        //      }

        public async Task<byte[]> GenerateRegistrationCertificateAsync(StudentRegistrationDto registration,string baseUrl)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var qrCodeUrl = $"{baseUrl}/api/StudentRegistration/Verify/{registration.Id}";
            var qrCodeBytes = GenerateQrCode(qrCodeUrl);

            //============= Backgroudpath=============
            var bgPath = Path.Combine(Directory.GetCurrentDirectory(), "ReportsImages", "certificate-bg.png");
            var backgroundImage = File.ReadAllBytes(bgPath);

            //============= logo=============
            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "ReportsImages", "Collegelogo.png");
            var logoImage = File.ReadAllBytes(logoPath);

            //============= Photopath=============
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


                    // =================  // BACKGROUND IMAGE =================
                    page.Background().Image(backgroundImage).FitArea(); // ya .Stretch() 
                    // ================= HEADER =================
                    page.Header().PaddingBottom(10).Column(col =>
                    {
                        col.Item().AlignCenter().Text("School PARAMEDICAL COUNCIL OF INDIA")
                            .Bold().FontSize(26).FontColor(Color.FromHex("#d9261c"));

                        col.Item().AlignCenter().Text("AN ISO 9001:2015 CERTIFIED")
                            .FontColor(Color.FromHex("#78c3e6")).FontSize(16);

                        //==============college logo=================
                        col.Item().AlignCenter().Container().Width(80).Height(80).Image(logoImage).FitArea();

                        //==========================================
                        col.Item().PaddingTop(5).AlignCenter()
                            .Text("CERTIFICATE OF REGISTRATION / MEMBERSHIP").Bold().FontSize(18);
                    });

                    // ================= CONTENT =================
                    page.Content().Column(column =>
                    {
                        column.Spacing(6);

                        // Intro Text
                        column.Item().Text(
                            "On the recommendation of the Academic Council & Governing Body of Paramedical Council of India hereby confers upon"
                        ).AlignCenter().FontSize(9);

                        // Course Name
                        column.Item().AlignCenter()
                            .Text(registration.CourseName ?? registration.CourseType)
                            .Bold().FontSize(12);

                        // -------------------- DETAILS + PHOTO --------------------
                        column.Item().PaddingTop(8).Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(2);   // Label
                                c.RelativeColumn(3);   // Value
                                c.ConstantColumn(100); // Photo
                            });

                            // Helper to add label + value cells
                            void Cell(string title, string value)
                            {
                                table.Cell().Element(CellStyle).Text(title).Bold();
                                table.Cell().Element(CellStyle).Text(value);
                                table.Cell().Element(CellStyle); // leave photo cell blank
                            }

                            // Row 1
                            table.Cell().Element(CellStyle).Text("Registration No").Bold();
                            table.Cell().Element(CellStyle).Text(registration.CouncilEnrollmentNo ?? $"PMCI/{registration.Id}/25");
                            table.Cell().RowSpan(4)
                                .AlignCenter().AlignMiddle()
                                .Width(90)
                                .Height(100)        // 3:4 ratio
                               // .Border(1)
                                .Padding(4)
                                .Image(studentPhoto ?? Array.Empty<byte>()).FitArea();

                            // Row 2
                            table.Cell().Element(CellStyle).Text("Name").Bold();
                            table.Cell().Element(CellStyle).Text(registration.FullName);

                            // Row 3
                            table.Cell().Element(CellStyle).Text("Father Name").Bold();
                            table.Cell().Element(CellStyle).Text(registration.FathersName);

                            // Row 4
                            table.Cell().Element(CellStyle).Text("Mother Name").Bold();
                            table.Cell().Element(CellStyle).Text(registration.MothersName);

                            // Additional info without photo
                            table.Cell().Element(CellStyle).Text("Month / Year of Passing").Bold();
                            table.Cell().Element(CellStyle).Text(FormatPassYear(registration.PassYear));
                            table.Cell().Element(CellStyle); // blank

                            table.Cell().Element(CellStyle).Text("Blood Group").Bold();
                            table.Cell().Element(CellStyle).Text(registration.BloodGroup ?? "N/A");
                            table.Cell().Element(CellStyle); // blank
                        });

                        // -------------------- ADDRESS + DOB --------------------
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

                        // -------------------- DISCLAIMER --------------------
                        column.Item().PaddingTop(6)
                            .Text("Para Medical Council of India has the right to cancel the certificate if any information is found incorrect.")
                            .FontSize(8).Italic();

                        // -------------------- QR CODE --------------------
                        column.Item().PaddingTop(8).AlignRight().Column(qr =>
                        {
                            qr.Item().Width(85).Height(85)
                                .Image(qrCodeBytes).FitArea();

                            qr.Item().AlignCenter()
                                .Text("Scan to Verify").FontSize(7);
                        });
                    });

                    // ================= FOOTER =================
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

        //private static IContainer CellStyle(IContainer container)
        //{
        //    return container
        //        .BorderBottom(0.5f)
        //        .BorderColor(Colors.Grey.Lighten2)
        //        .PaddingVertical(8)
        //        .PaddingHorizontal(5);
        //}
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

            // Try to parse and format the year
            if (passYear.Length == 4 && int.TryParse(passYear, out int year))
            {
                return DateTime.UtcNow.ToString("MMMM") + "-" + passYear;
            }

            return passYear;
        }
    }
}

