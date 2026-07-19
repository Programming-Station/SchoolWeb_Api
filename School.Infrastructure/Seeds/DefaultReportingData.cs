using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Reporting;

namespace School.Infrastructure.Seeds
{
    public static class DefaultReportingData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var school = await context.SchoolRegistrations.FirstOrDefaultAsync();
            if (school == null) return;

            int schoolId = school.Id;

            // ════════════════════════════════════════════════════════════════════
            // 1. SEED REPORT CATEGORIES (12 Categories)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.ReportCategories.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var categories = new List<ReportCategory>
                {
                    new() { Code = "ADMISSION", Name = "Admission Reports", IconClass = "pi pi-user-plus", ColorHex = "#0ea5e9", Description = "Student admission forms, merit lists, and enrollment statistics", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Code = "STUDENT", Name = "Student Reports", IconClass = "pi pi-users", ColorHex = "#8b5cf6", Description = "Student directory, profiles, ID cards, and demographic analytics", SortOrder = 2, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Code = "FEE", Name = "Fee & Finance Reports", IconClass = "pi pi-indian-rupee", ColorHex = "#10b981", Description = "Fee receipts, collection summaries, defaulter lists, and refund reports", SortOrder = 3, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Code = "ACADEMIC", Name = "Academic Reports", IconClass = "pi pi-book", ColorHex = "#f59e0b", Description = "Mark sheets, transcripts, report cards, and subject analysis", SortOrder = 4, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Code = "EXAMINATION", Name = "Examination Reports", IconClass = "pi pi-file-edit", ColorHex = "#ef4444", Description = "Exam schedules, hall tickets, result tabulation, and grade distribution", SortOrder = 5, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Code = "ATTENDANCE", Name = "Attendance Reports", IconClass = "pi pi-calendar-clock", ColorHex = "#06b6d4", Description = "Daily, monthly, and term-wise attendance reports for students and staff", SortOrder = 6, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Code = "HR", Name = "HR & Staff Reports", IconClass = "pi pi-id-card", ColorHex = "#d946ef", Description = "Employee directory, leave balances, appraisals, and staff analytics", SortOrder = 7, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Code = "PAYROLL", Name = "Payroll Reports", IconClass = "pi pi-wallet", ColorHex = "#14b8a6", Description = "Salary slips, PF statements, tax declarations, and payroll summaries", SortOrder = 8, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Code = "TRANSPORT", Name = "Transport Reports", IconClass = "pi pi-car", ColorHex = "#f97316", Description = "Route manifests, vehicle logs, fuel reports, and allocation lists", SortOrder = 9, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Code = "LIBRARY", Name = "Library Reports", IconClass = "pi pi-bookmark", ColorHex = "#a855f7", Description = "Book catalogs, issue/return logs, overdue lists, and accession registers", SortOrder = 10, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Code = "HOSTEL", Name = "Hostel Reports", IconClass = "pi pi-building", ColorHex = "#64748b", Description = "Room allocation, mess menus, hostel attendance, and maintenance logs", SortOrder = 11, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Code = "CERTIFICATE", Name = "Certificates & Letters", IconClass = "pi pi-verified", ColorHex = "#059669", Description = "Transfer certificates, bonafides, character certificates, and experience letters", SortOrder = 12, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }
                };
                context.ReportCategories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 2. SEED REPORT TEMPLATES (25 Templates — matching RDLC files)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.ReportTemplates.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                // Fetch category IDs
                var admissionCat = await context.ReportCategories.FirstOrDefaultAsync(c => c.Code == "ADMISSION" && c.SchoolRegistrationId == schoolId);
                var studentCat = await context.ReportCategories.FirstOrDefaultAsync(c => c.Code == "STUDENT" && c.SchoolRegistrationId == schoolId);
                var feeCat = await context.ReportCategories.FirstOrDefaultAsync(c => c.Code == "FEE" && c.SchoolRegistrationId == schoolId);
                var academicCat = await context.ReportCategories.FirstOrDefaultAsync(c => c.Code == "ACADEMIC" && c.SchoolRegistrationId == schoolId);
                var examCat = await context.ReportCategories.FirstOrDefaultAsync(c => c.Code == "EXAMINATION" && c.SchoolRegistrationId == schoolId);
                var attendanceCat = await context.ReportCategories.FirstOrDefaultAsync(c => c.Code == "ATTENDANCE" && c.SchoolRegistrationId == schoolId);
                var hrCat = await context.ReportCategories.FirstOrDefaultAsync(c => c.Code == "HR" && c.SchoolRegistrationId == schoolId);
                var payrollCat = await context.ReportCategories.FirstOrDefaultAsync(c => c.Code == "PAYROLL" && c.SchoolRegistrationId == schoolId);
                var transportCat = await context.ReportCategories.FirstOrDefaultAsync(c => c.Code == "TRANSPORT" && c.SchoolRegistrationId == schoolId);
                var libraryCat = await context.ReportCategories.FirstOrDefaultAsync(c => c.Code == "LIBRARY" && c.SchoolRegistrationId == schoolId);
                var hostelCat = await context.ReportCategories.FirstOrDefaultAsync(c => c.Code == "HOSTEL" && c.SchoolRegistrationId == schoolId);
                var certCat = await context.ReportCategories.FirstOrDefaultAsync(c => c.Code == "CERTIFICATE" && c.SchoolRegistrationId == schoolId);

                var templates = new List<ReportTemplate>
                {
                    // Fee Reports
                    new() { ReportCategoryId = feeCat?.Id, ReportCode = "FEE_RECEIPT", ReportName = "Fee Payment Receipt", Description = "Individual student fee payment receipt with installment breakup, payment mode, and QR code verification", ReportType = ReportType.Invoice, DefaultFormat = ReportFormat.PDF, RdlcFileName = "FeeReceipt.rdlc", IsSystem = true, HasQrCode = true, HasLogo = true, ModuleTag = "Fee", SearchTags = "fee,receipt,payment,challan", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { ReportCategoryId = feeCat?.Id, ReportCode = "FEE_COLLECTION_DAILY", ReportName = "Daily Fee Collection Report", Description = "Date-wise fee collection summary with payment mode breakdown and grand totals", ReportType = ReportType.Tabular, DefaultFormat = ReportFormat.PDF, RdlcFileName = "DailyFeeCollection.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "Fee", SearchTags = "fee,collection,daily,cash,online", SortOrder = 2, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { ReportCategoryId = feeCat?.Id, ReportCode = "FEE_DEFAULTER", ReportName = "Fee Defaulter List", Description = "Students with outstanding fee dues grouped by class and section with aging analysis", ReportType = ReportType.Tabular, DefaultFormat = ReportFormat.PDF, RdlcFileName = "FeeDefaulterList.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "Fee", SearchTags = "fee,defaulter,outstanding,dues,pending", SortOrder = 3, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { ReportCategoryId = feeCat?.Id, ReportCode = "BALANCE_SHEET", ReportName = "Consolidated Balance Sheet", Description = "Complete financial balance sheet with account heads, ledger types, and net balance amounts", ReportType = ReportType.Summary, DefaultFormat = ReportFormat.PDF, PageOrientation = PageOrientation.Landscape, RdlcFileName = "BalanceSheet.rdlc", IsSystem = true, HasLogo = true, HasSignature = true, ModuleTag = "Finance", SearchTags = "balance,sheet,finance,ledger,accounts", SortOrder = 4, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // Student Reports
                    new() { ReportCategoryId = studentCat?.Id, ReportCode = "STUDENT_LIST", ReportName = "Student Directory", Description = "Comprehensive student list with admission number, class, section, contact details, and parent information", ReportType = ReportType.Tabular, DefaultFormat = ReportFormat.PDF, PageOrientation = PageOrientation.Landscape, RdlcFileName = "StudentList.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "Student", SearchTags = "student,list,directory,class,section", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { ReportCategoryId = studentCat?.Id, ReportCode = "STUDENT_ID_CARD", ReportName = "Student Identity Card", Description = "Student photo ID card with barcode, QR code, emergency contact, and blood group", ReportType = ReportType.IdCard, DefaultFormat = ReportFormat.PDF, PageSize = PageSize.Custom, CustomPageWidth = 3.375m, CustomPageHeight = 2.125m, RdlcFileName = "StudentIDCard.rdlc", IsSystem = true, HasQrCode = true, HasBarcode = true, HasLogo = true, ModuleTag = "Student", SearchTags = "student,id,card,identity,badge", SortOrder = 2, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { ReportCategoryId = studentCat?.Id, ReportCode = "STUDENT_STRENGTH", ReportName = "Student Strength Report", Description = "Class and section wise student count with gender-wise breakup and vacancy analysis", ReportType = ReportType.Summary, DefaultFormat = ReportFormat.PDF, RdlcFileName = "StudentStrength.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "Student", SearchTags = "student,strength,count,vacancy,class", SortOrder = 3, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // Academic Reports
                    new() { ReportCategoryId = academicCat?.Id, ReportCode = "ACADEMIC_TRANSCRIPT", ReportName = "Official Academic Transcript", Description = "Semester-wise academic transcript with subject grades, credits, CGPA, and cumulative grade points", ReportType = ReportType.Summary, DefaultFormat = ReportFormat.PDF, RdlcFileName = "AcademicTranscript.rdlc", IsSystem = true, HasLogo = true, HasSignature = true, HasQrCode = true, ModuleTag = "Academic", SearchTags = "transcript,grade,cgpa,semester,marksheet", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { ReportCategoryId = academicCat?.Id, ReportCode = "TIMETABLE", ReportName = "Class Timetable", Description = "Weekly class timetable with period-wise subject allocation and teacher assignments", ReportType = ReportType.Matrix, DefaultFormat = ReportFormat.PDF, PageOrientation = PageOrientation.Landscape, RdlcFileName = "Timetable.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "Academic", SearchTags = "timetable,schedule,period,class,teacher", SortOrder = 2, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // Examination Reports
                    new() { ReportCategoryId = examCat?.Id, ReportCode = "MARKSHEET", ReportName = "Student Marksheet", Description = "Term-end marksheet with subject-wise marks, grades, class rank, and teacher remarks", ReportType = ReportType.Summary, DefaultFormat = ReportFormat.PDF, RdlcFileName = "Marksheet.rdlc", IsSystem = true, HasLogo = true, HasSignature = true, ModuleTag = "Examination", SearchTags = "marksheet,marks,grade,result,rank", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { ReportCategoryId = examCat?.Id, ReportCode = "EXAM_SCHEDULE", ReportName = "Examination Schedule", Description = "Date-sheet with exam dates, timings, subjects, and venue details", ReportType = ReportType.Tabular, DefaultFormat = ReportFormat.PDF, RdlcFileName = "ExamSchedule.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "Examination", SearchTags = "exam,schedule,datesheet,timetable", SortOrder = 2, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // Attendance Reports
                    new() { ReportCategoryId = attendanceCat?.Id, ReportCode = "ATTENDANCE_MONTHLY", ReportName = "Monthly Attendance Register", Description = "Day-by-day attendance register with present/absent/leave count and percentage for the month", ReportType = ReportType.Matrix, DefaultFormat = ReportFormat.PDF, PageOrientation = PageOrientation.Landscape, RdlcFileName = "MonthlyAttendance.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "Attendance", SearchTags = "attendance,monthly,register,present,absent", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { ReportCategoryId = attendanceCat?.Id, ReportCode = "ATTENDANCE_BELOW_75", ReportName = "Low Attendance Alert Report", Description = "Students with attendance below 75% threshold for intervention and notice issuance", ReportType = ReportType.Tabular, DefaultFormat = ReportFormat.PDF, RdlcFileName = "LowAttendance.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "Attendance", SearchTags = "attendance,low,below,alert,shortage", SortOrder = 2, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // HR Reports
                    new() { ReportCategoryId = hrCat?.Id, ReportCode = "EMPLOYEE_LIST", ReportName = "Employee Directory", Description = "Complete staff directory with designation, department, contact info, and joining date", ReportType = ReportType.Tabular, DefaultFormat = ReportFormat.PDF, PageOrientation = PageOrientation.Landscape, RdlcFileName = "EmployeeList.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "HR", SearchTags = "employee,staff,directory,list,department", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { ReportCategoryId = hrCat?.Id, ReportCode = "LEAVE_BALANCE", ReportName = "Leave Balance Report", Description = "Employee-wise leave balance with leave type breakup, consumed, and remaining days", ReportType = ReportType.Tabular, DefaultFormat = ReportFormat.PDF, RdlcFileName = "LeaveBalance.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "HR", SearchTags = "leave,balance,remaining,consumed,type", SortOrder = 2, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // Payroll Reports
                    new() { ReportCategoryId = payrollCat?.Id, ReportCode = "SALARY_SLIP", ReportName = "Monthly Salary Slip", Description = "Individual employee salary slip with earnings, deductions, net pay, and PF/tax breakup", ReportType = ReportType.Invoice, DefaultFormat = ReportFormat.PDF, RdlcFileName = "SalarySlip.rdlc", IsSystem = true, HasLogo = true, HasSignature = true, ModuleTag = "Payroll", SearchTags = "salary,slip,payslip,earnings,deductions", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { ReportCategoryId = payrollCat?.Id, ReportCode = "PAYROLL_SUMMARY", ReportName = "Payroll Summary Report", Description = "Department-wise payroll summary with total gross, deductions, net pay, and statutory contributions", ReportType = ReportType.Summary, DefaultFormat = ReportFormat.Excel, PageOrientation = PageOrientation.Landscape, RdlcFileName = "PayrollSummary.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "Payroll", SearchTags = "payroll,summary,department,gross,net", SortOrder = 2, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // Transport Reports
                    new() { ReportCategoryId = transportCat?.Id, ReportCode = "TRANSPORT_ROUTE_LIST", ReportName = "Transport Route & Vehicle Manifest", Description = "Route-wise stop locations, arrival times, allocated students, vehicle details, and driver assignment", ReportType = ReportType.Tabular, DefaultFormat = ReportFormat.PDF, RdlcFileName = "TransportRouteList.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "Transport", SearchTags = "transport,route,vehicle,bus,stop,driver", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { ReportCategoryId = transportCat?.Id, ReportCode = "FUEL_LOG", ReportName = "Vehicle Fuel & Maintenance Log", Description = "Vehicle-wise fuel consumption, maintenance history, and cost analysis", ReportType = ReportType.Tabular, DefaultFormat = ReportFormat.PDF, RdlcFileName = "FuelLog.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "Transport", SearchTags = "fuel,vehicle,maintenance,log,cost", SortOrder = 2, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // Library Reports
                    new() { ReportCategoryId = libraryCat?.Id, ReportCode = "LIBRARY_DUE_LIST", ReportName = "Library Book Defaulters & Dues", Description = "Overdue books with borrower details, due dates, fine amounts, and return status", ReportType = ReportType.Tabular, DefaultFormat = ReportFormat.PDF, RdlcFileName = "LibraryDueList.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "Library", SearchTags = "library,overdue,book,fine,due,defaulter", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { ReportCategoryId = libraryCat?.Id, ReportCode = "BOOK_CATALOG", ReportName = "Book Accession Register", Description = "Complete library book catalog with accession number, ISBN, author, publisher, and shelf location", ReportType = ReportType.Tabular, DefaultFormat = ReportFormat.Excel, PageOrientation = PageOrientation.Landscape, RdlcFileName = "BookCatalog.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "Library", SearchTags = "book,catalog,accession,isbn,author", SortOrder = 2, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // Hostel Reports
                    new() { ReportCategoryId = hostelCat?.Id, ReportCode = "HOSTEL_ALLOCATION", ReportName = "Hostel Room Allocation Report", Description = "Building, floor, room, and bed-wise allocation with student details and occupancy status", ReportType = ReportType.Tabular, DefaultFormat = ReportFormat.PDF, RdlcFileName = "HostelAllocation.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "Hostel", SearchTags = "hostel,room,allocation,bed,occupancy", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // Certificate Reports
                    new() { ReportCategoryId = certCat?.Id, ReportCode = "TRANSFER_CERT", ReportName = "Transfer Certificate (TC)", Description = "Official transfer certificate with student academic history, conduct, and principal signature", ReportType = ReportType.Certificate, DefaultFormat = ReportFormat.PDF, RdlcFileName = "TransferCertificate.rdlc", IsSystem = true, HasLogo = true, HasSignature = true, HasQrCode = true, ModuleTag = "Certificate", SearchTags = "transfer,certificate,tc,leaving,migration", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { ReportCategoryId = certCat?.Id, ReportCode = "BONAFIDE_CERT", ReportName = "Bonafide Certificate", Description = "Bonafide student certificate confirming enrollment status for official purposes", ReportType = ReportType.Certificate, DefaultFormat = ReportFormat.PDF, RdlcFileName = "BonafideCertificate.rdlc", IsSystem = true, HasLogo = true, HasSignature = true, HasQrCode = true, ModuleTag = "Certificate", SearchTags = "bonafide,certificate,enrollment,student", SortOrder = 2, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // Admission Reports
                    new() { ReportCategoryId = admissionCat?.Id, ReportCode = "ADMISSION_FORM", ReportName = "Admission Application Form", Description = "Printed admission application form with student details, parent information, and document checklist", ReportType = ReportType.Custom, DefaultFormat = ReportFormat.PDF, RdlcFileName = "AdmissionForm.rdlc", IsSystem = true, HasLogo = true, ModuleTag = "Admission", SearchTags = "admission,form,application,enroll", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }
                };
                context.ReportTemplates.AddRange(templates);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 3. SEED REPORT BRANDING (1 per tenant)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.ReportBrandings.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var branding = new ReportBranding
                {
                    SchoolRegistrationId = schoolId,
                    SchoolName = school.SchoolName,
                    OrganizationName = school.SchoolName,
                    TagLine = "Empowering Education Through Technology",
                    EstablishedYear = "2010",
                    AddressLine1 = "123 Education Lane, Knowledge Park",
                    City = "New Delhi",
                    State = "Delhi",
                    PinCode = "110001",
                    Phone = "+91-11-23456789",
                    Mobile = "+919900112233",
                    Email = "info@school.in",
                    Website = "https://www.school.in",
                    AffiliationNumber = "CBSE/AFF/2630001",
                    RegistrationNumber = "SCH/2010/DEL/00123",
                    PrincipalName = "Dr. Ramesh Chandra Mishra",
                    DirectorName = "Shri Arun Kumar Agarwal",
                    ChairmanName = "Shri Arun Kumar Agarwal",
                    PrimaryColor = "#1e3a8a",
                    SecondaryColor = "#3b82f6",
                    AccentColor = "#f59e0b",
                    FontFamily = "Arial",
                    WatermarkText = "OFFICIAL COPY",
                    ReportFooterText = "This is a computer-generated document. No signature is required.",
                    Disclaimer = "The information in this report is confidential and intended for the addressee only.",
                    CopyrightText = "© 2026 School ERP. All Rights Reserved.",
                    QrVerificationBaseUrl = "https://www.school.in/verify",
                    BarcodePrefix = "SCH001",
                    CurrentAcademicSession = "2026-27",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                };
                context.ReportBrandings.Add(branding);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 4. SEED REPORT PARAMETERS (Parameters for top reports)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.ReportParameters.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var feeReceipt = await context.ReportTemplates.FirstOrDefaultAsync(t => t.ReportCode == "FEE_RECEIPT" && t.SchoolRegistrationId == schoolId);
                var dailyFee = await context.ReportTemplates.FirstOrDefaultAsync(t => t.ReportCode == "FEE_COLLECTION_DAILY" && t.SchoolRegistrationId == schoolId);
                var studentList = await context.ReportTemplates.FirstOrDefaultAsync(t => t.ReportCode == "STUDENT_LIST" && t.SchoolRegistrationId == schoolId);
                var marksheet = await context.ReportTemplates.FirstOrDefaultAsync(t => t.ReportCode == "MARKSHEET" && t.SchoolRegistrationId == schoolId);

                var parameters = new List<ReportParameter>();

                if (feeReceipt != null)
                {
                    parameters.Add(new() { ReportTemplateId = feeReceipt.Id, ParameterName = "ReceiptNo", DisplayLabel = "Receipt Number", DataType = ParameterDataType.String, IsRequired = true, PlaceholderText = "e.g. REC/2026/0045", HelpText = "Enter the unique receipt reference number", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" });
                }

                if (dailyFee != null)
                {
                    parameters.Add(new() { ReportTemplateId = dailyFee.Id, ParameterName = "FromDate", DisplayLabel = "From Date", DataType = ParameterDataType.Date, IsRequired = true, DefaultValue = DateTime.UtcNow.ToString("yyyy-MM-dd"), HelpText = "Start date for collection log", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" });
                    parameters.Add(new() { ReportTemplateId = dailyFee.Id, ParameterName = "ToDate", DisplayLabel = "To Date", DataType = ParameterDataType.Date, IsRequired = true, DefaultValue = DateTime.UtcNow.ToString("yyyy-MM-dd"), HelpText = "End date for collection log", SortOrder = 2, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" });
                }

                if (studentList != null)
                {
                    parameters.Add(new() { ReportTemplateId = studentList.Id, ParameterName = "ClassId", DisplayLabel = "Class / Standard", DataType = ParameterDataType.EntityLookup, IsRequired = false, LookupApiEndpoint = "/api/class/list", PlaceholderText = "All Classes", HelpText = "Filter by class", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" });
                    parameters.Add(new() { ReportTemplateId = studentList.Id, ParameterName = "Status", DisplayLabel = "Student Status", DataType = ParameterDataType.Enum, IsRequired = true, DefaultValue = "Active", EnumValuesJson = "[{\"label\":\"Active\",\"value\":\"Active\"},{\"label\":\"Inactive\",\"value\":\"Inactive\"},{\"label\":\"Alumni\",\"value\":\"Alumni\"},{\"label\":\"Suspended\",\"value\":\"Suspended\"}]", HelpText = "Filter by enrollment status", SortOrder = 2, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" });
                }

                if (marksheet != null)
                {
                    parameters.Add(new() { ReportTemplateId = marksheet.Id, ParameterName = "StudentId", DisplayLabel = "Student Name", DataType = ParameterDataType.EntityLookup, IsRequired = true, LookupApiEndpoint = "/api/student/list", PlaceholderText = "Search Student", HelpText = "Select student profile for mark list", SortOrder = 1, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" });
                    parameters.Add(new() { ReportTemplateId = marksheet.Id, ParameterName = "Term", DisplayLabel = "Exam Term", DataType = ParameterDataType.Enum, IsRequired = true, DefaultValue = "Term1", EnumValuesJson = "[{\"label\":\"Term 1 (Midterm)\",\"value\":\"Term1\"},{\"label\":\"Term 2 (Final)\",\"value\":\"Term2\"}]", HelpText = "Select exam term", SortOrder = 2, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" });
                }

                if (parameters.Count > 0)
                {
                    context.ReportParameters.AddRange(parameters);
                    await context.SaveChangesAsync();
                }
            }

            // ════════════════════════════════════════════════════════════════════
            // 5. SEED REPORT PERMISSIONS (Control access based on Roles)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.ReportPermissions.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var templates = await context.ReportTemplates.Where(t => t.SchoolRegistrationId == schoolId).ToListAsync();
                var permissions = new List<ReportPermission>();

                foreach (var template in templates)
                {
                    // SuperAdmin gets all capabilities
                    permissions.Add(new()
                    {
                        SchoolRegistrationId = schoolId,
                        ReportTemplateId = template.Id,
                        RoleName = "SuperAdmin",
                        CanView = true,
                        CanExportPdf = true,
                        CanExportExcel = true,
                        CanExportWord = true,
                        CanExportCsv = true,
                        CanPrint = true,
                        CanEmail = true,
                        CanSchedule = true,
                        CanManageTemplate = true,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    });

                    // Admin (SchoolAdmin) gets most capabilities
                    permissions.Add(new()
                    {
                        SchoolRegistrationId = schoolId,
                        ReportTemplateId = template.Id,
                        RoleName = "Admin",
                        CanView = true,
                        CanExportPdf = true,
                        CanExportExcel = template.ReportCode.Contains("DAILY") || template.ReportCode.Contains("CATALOG") || template.ReportCode.Contains("LIST"),
                        CanExportWord = template.ReportCode.Contains("CERT") || template.ReportCode.Contains("LETTER"),
                        CanExportCsv = false,
                        CanPrint = true,
                        CanEmail = true,
                        CanSchedule = true,
                        CanManageTemplate = false,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    });

                    // Employee gets view/print/pdf on relevant modules
                    bool isHROrPayroll = template.ModuleTag == "HR" || template.ModuleTag == "Payroll" || template.ModuleTag == "Finance";
                    permissions.Add(new()
                    {
                        SchoolRegistrationId = schoolId,
                        ReportTemplateId = template.Id,
                        RoleName = "Employee",
                        CanView = !isHROrPayroll || template.ReportCode == "SALARY_SLIP" || template.ReportCode == "LEAVE_BALANCE",
                        CanExportPdf = !isHROrPayroll || template.ReportCode == "SALARY_SLIP" || template.ReportCode == "LEAVE_BALANCE",
                        CanExportExcel = false,
                        CanExportWord = false,
                        CanExportCsv = false,
                        CanPrint = !isHROrPayroll,
                        CanEmail = false,
                        CanSchedule = false,
                        CanManageTemplate = false,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    });

                    // Student/Parent gets to view only transcript/marksheet/ID card/timetable/receipts
                    bool isStudentAccessible = template.ReportCode == "STUDENT_ID_CARD" ||
                                               template.ReportCode == "TIMETABLE" ||
                                               template.ReportCode == "FEE_RECEIPT" ||
                                               template.ReportCode == "MARKSHEET" ||
                                               template.ReportCode == "ACADEMIC_TRANSCRIPT";

                    permissions.Add(new()
                    {
                        SchoolRegistrationId = schoolId,
                        ReportTemplateId = template.Id,
                        RoleName = "Student",
                        CanView = isStudentAccessible,
                        CanExportPdf = isStudentAccessible,
                        CanExportExcel = false,
                        CanExportWord = false,
                        CanExportCsv = false,
                        CanPrint = isStudentAccessible,
                        CanEmail = false,
                        CanSchedule = false,
                        CanManageTemplate = false,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    });
                }

                context.ReportPermissions.AddRange(permissions);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 6. SEED REPORT SCHEDULES (Automated report generation)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.ReportSchedules.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var dailyFee = await context.ReportTemplates.FirstOrDefaultAsync(t => t.ReportCode == "FEE_COLLECTION_DAILY" && t.SchoolRegistrationId == schoolId);
                var leaveBalance = await context.ReportTemplates.FirstOrDefaultAsync(t => t.ReportCode == "LEAVE_BALANCE" && t.SchoolRegistrationId == schoolId);
                var defaulterList = await context.ReportTemplates.FirstOrDefaultAsync(t => t.ReportCode == "FEE_DEFAULTER" && t.SchoolRegistrationId == schoolId);

                var schedules = new List<ReportSchedule>();

                if (dailyFee != null)
                {
                    schedules.Add(new()
                    {
                        SchoolRegistrationId = schoolId,
                        ReportTemplateId = dailyFee.Id,
                        ScheduleName = "End of Day Fee Collection Summary",
                        CronExpression = "0 18 * * *", // 6:00 PM Daily
                        RecipientEmails = "finance.head@school.in;admin@school.in",
                        CcEmails = "principal@school.in",
                        Format = "PDF",
                        ParametersJson = "{\"FromDate\":\"TODAY\",\"ToDate\":\"TODAY\"}",
                        EmailSubjectTemplate = "Daily Fee Collection Report - {Date}",
                        EmailBodyTemplate = "Dear Finance Team,<br/><br/>Please find attached the daily fee collection summary for {Date}.<br/><br/>Regards,<br/>School ERP Automation",
                        LastRunStatus = "Success",
                        LastRunAt = DateTime.UtcNow.AddDays(-1),
                        NextRunAt = DateTime.UtcNow.AddHours(2),
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    });
                }

                if (leaveBalance != null)
                {
                    schedules.Add(new()
                    {
                        SchoolRegistrationId = schoolId,
                        ReportTemplateId = leaveBalance.Id,
                        ScheduleName = "Monthly Employee Leave Balances",
                        CronExpression = "0 9 1 * *", // 9:00 AM on 1st of every month
                        RecipientEmails = "hr@school.in",
                        Format = "Excel",
                        EmailSubjectTemplate = "Monthly Leave Balance Sheet - {MonthName}",
                        EmailBodyTemplate = "Dear HR Office,<br/><br/>Please find attached the consolidated employee leave balance register for this month.<br/><br/>Regards,<br/>School ERP Automation",
                        LastRunStatus = "Success",
                        LastRunAt = DateTime.UtcNow.AddDays(-19),
                        NextRunAt = DateTime.UtcNow.AddDays(12),
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    });
                }

                if (defaulterList != null)
                {
                    schedules.Add(new()
                    {
                        SchoolRegistrationId = schoolId,
                        ReportTemplateId = defaulterList.Id,
                        ScheduleName = "Weekly Dues Defaulters Report",
                        CronExpression = "0 8 * * 1", // 8:00 AM every Monday
                        RecipientEmails = "accounts.receivable@school.in",
                        Format = "PDF",
                        EmailSubjectTemplate = "Weekly Fee Defaulter List - Week {WeekOfYear}",
                        EmailBodyTemplate = "Dear Team,<br/><br/>Attached is the list of outstanding fee accounts for this week. Please follow up on outstanding dues.<br/><br/>Regards,<br/>Accounts Department",
                        LastRunStatus = "Pending",
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    });
                }

                context.ReportSchedules.AddRange(schedules);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 7. SEED REPORT HISTORY & EXECUTIONS (Audit logs / performance telemetries)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.ReportHistories.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var feeReceipt = await context.ReportTemplates.FirstOrDefaultAsync(t => t.ReportCode == "FEE_RECEIPT" && t.SchoolRegistrationId == schoolId);
                var studentList = await context.ReportTemplates.FirstOrDefaultAsync(t => t.ReportCode == "STUDENT_LIST" && t.SchoolRegistrationId == schoolId);

                var historyList = new List<ReportHistory>();

                if (feeReceipt != null)
                {
                    var hist1 = new ReportHistory
                    {
                        SchoolRegistrationId = schoolId,
                        ReportTemplateId = feeReceipt.Id,
                        ReportName = "Fee Payment Receipt",
                        GeneratedBy = "accounts.clerk@school.in",
                        GeneratedAt = DateTime.UtcNow.AddHours(-3),
                        Format = "PDF",
                        FileSizeBytes = 184520,
                        ExecutionMs = 450,
                        ParametersJson = "{\"ReceiptNo\":\"REC/2026/0014\"}",
                        Status = ReportExecutionStatus.Success,
                        IsDownloaded = true,
                        DownloadCount = 1,
                        LastDownloadAt = DateTime.UtcNow.AddHours(-3),
                        IsPrinted = true,
                        PrintCount = 1,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed",
                        IsActive = true
                    };

                    hist1.Executions.Add(new ReportExecution
                    {
                        SchoolRegistrationId = schoolId,
                        StartedAt = DateTime.UtcNow.AddHours(-3).AddMilliseconds(-450),
                        CompletedAt = DateTime.UtcNow.AddHours(-3),
                        ExecutionStage = "Done",
                        RowsProcessed = 12,
                        PeakMemoryBytes = 14500000,
                        DataFetchMs = 150,
                        RenderMs = 210,
                        ExportMs = 90,
                        CorrelationId = Guid.NewGuid().ToString(),
                        ServerInstance = "API_SRV_01",
                        IsSuccess = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed",
                        IsActive = true
                    });

                    historyList.Add(hist1);
                }

                if (studentList != null)
                {
                    var hist2 = new ReportHistory
                    {
                        SchoolRegistrationId = schoolId,
                        ReportTemplateId = studentList.Id,
                        ReportName = "Student Directory",
                        GeneratedBy = "principal@school.in",
                        GeneratedAt = DateTime.UtcNow.AddHours(-1),
                        Format = "PDF",
                        FileSizeBytes = 1205300,
                        ExecutionMs = 2800,
                        ParametersJson = "{\"ClassId\":\"10\",\"Status\":\"Active\"}",
                        Status = ReportExecutionStatus.Success,
                        IsDownloaded = true,
                        DownloadCount = 2,
                        LastDownloadAt = DateTime.UtcNow.AddMinutes(-45),
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed",
                        IsActive = true
                    };

                    hist2.Executions.Add(new ReportExecution
                    {
                        SchoolRegistrationId = schoolId,
                        StartedAt = DateTime.UtcNow.AddHours(-1).AddMilliseconds(-2800),
                        CompletedAt = DateTime.UtcNow.AddHours(-1),
                        ExecutionStage = "Done",
                        RowsProcessed = 840,
                        PeakMemoryBytes = 85000000,
                        DataFetchMs = 950,
                        RenderMs = 1250,
                        ExportMs = 600,
                        CorrelationId = Guid.NewGuid().ToString(),
                        ServerInstance = "API_SRV_02",
                        IsSuccess = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed",
                        IsActive = true
                    });

                    historyList.Add(hist2);
                }

                if (historyList.Count > 0)
                {
                    context.ReportHistories.AddRange(historyList);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
