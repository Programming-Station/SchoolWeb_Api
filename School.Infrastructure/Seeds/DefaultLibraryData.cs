using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Library;
using School.Infrastructure;

namespace School.Infrastructure.Seeds
{
    public class DefaultLibraryData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var school = await context.SchoolRegistrations.IgnoreQueryFilters().FirstOrDefaultAsync();
            if (school == null) return;

            int schoolId = school.Id;

            // 1. Book Categories
            if (!await context.BookCategories.IgnoreQueryFilters().AnyAsync(c => c.SchoolRegistrationId == schoolId))
            {
                var science = new BookCategory { Name = "Science & Tech", Description = "Physics, Chemistry, Biology and general science materials", CategoryType = "Book", ColorCode = "#6366f1", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var maths = new BookCategory { Name = "Mathematics", Description = "Calculus, algebra, geometry textbooks", CategoryType = "Book", ColorCode = "#10b981", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var literature = new BookCategory { Name = "Literature & Fiction", Description = "Novels, plays, and poetry collections", CategoryType = "Book", ColorCode = "#f59e0b", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var compSci = new BookCategory { Name = "Computer Science", Description = "Coding, database systems, and networking guides", CategoryType = "Book", ColorCode = "#8b5cf6", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };

                context.BookCategories.AddRange(science, maths, literature, compSci);
                await context.SaveChangesAsync();

                // 2. Book Authors
                var authorPhysics = new BookAuthor { Name = "Dr. H.C. Verma", Biography = "Renowned Indian physicist and writer of introductory physics materials.", Country = "India", BooksCount = 10, Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var authorMaths = new BookAuthor { Name = "Dr. R.D. Sharma", Biography = "Author of widely read high school mathematics textbooks in India.", Country = "India", BooksCount = 8, Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var authorFiction = new BookAuthor { Name = "J.K. Rowling", Biography = "British author best known for writing the Harry Potter fantasy series.", Country = "United Kingdom", BooksCount = 7, Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var authorLiterature = new BookAuthor { Name = "William Shakespeare", Biography = "English playwright, poet, and actor, widely regarded as the greatest writer.", Country = "United Kingdom", BooksCount = 37, Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };

                context.BookAuthors.AddRange(authorPhysics, authorMaths, authorFiction, authorLiterature);
                await context.SaveChangesAsync();

                // 3. Book Publishers
                var pubOxford = new BookPublisher { Name = "Oxford University Press", Email = "support@oup.com", Phone = "011-2365478", GSTNumber = "07PUBGST1234A1Z1", Address = "YMCA Library Building, New Delhi", Website = "https://global.oup.com", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var pubPearson = new BookPublisher { Name = "Pearson Education India", Email = "contact@pearson.com", Phone = "022-9874563", GSTNumber = "27PUBGST5678B2Z2", Address = "Prestige Heights, Mumbai", Website = "https://in.pearson.com", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var pubSchand = new BookPublisher { Name = "S. Chand & Company Ltd", Email = "info@schandpublishing.com", Phone = "011-4569871", GSTNumber = "07PUBGST8901C3Z3", Address = "Ram Nagar, New Delhi", Website = "https://www.schandpublishing.com", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };

                context.BookPublishers.AddRange(pubOxford, pubPearson, pubSchand);
                await context.SaveChangesAsync();

                // 4. Book Vendors
                var vendorCentral = new BookVendor { Name = "Central Book Depot", ContactPerson = "Ramesh Kumar", Email = "orders@centralbooks.com", Phone = "011-8974521", GSTNumber = "07VNDGST1234A1Z1", Address = "Connaught Place, New Delhi", Website = "https://centralbooks.com", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var vendorOxford = new BookVendor { Name = "Oxford Book Store", ContactPerson = "Nisha Sen", Email = "sales@oxfordbookstore.com", Phone = "033-6523145", GSTNumber = "19VNDGST5678B2Z2", Address = "Park Street, Kolkata", Website = "https://www.oxfordbookstore.com", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };

                context.BookVendors.AddRange(vendorCentral, vendorOxford);
                await context.SaveChangesAsync();

                // 5. Books
                var bookPhysics = new Book
                {
                    Title = "Concepts of Physics - Volume 1",
                    ISBN = "978-8177091878",
                    AccessionNumber = "ACC-PHY-001",
                    Barcode = "BAR-PHY-001",
                    QRCode = "QR-PHY-001",
                    Edition = "1st Edition",
                    Volume = "Vol 1",
                    Language = "English",
                    SubjectCategory = "Physics",
                    Author = authorPhysics.Name,
                    AuthorId = authorPhysics.Id,
                    Publisher = pubSchand.Name,
                    PublisherId = pubSchand.Id,
                    VendorId = vendorCentral.Id,
                    CategoryId = science.Id,
                    BookType = "TextBook",
                    Status = "Available",
                    PurchaseDate = DateTime.UtcNow.AddDays(-60),
                    PurchasePrice = 450.00m,
                    Shelf = "A",
                    Rack = "3",
                    Row = "2",
                    Cupboard = "C1",
                    RackLocation = "Science-A3-2",
                    TotalCopies = 5,
                    AvailableCopies = 5,
                    MinimumStock = 1,
                    MaximumStock = 10,
                    Keywords = "Physics, Mechanics, Optics, HC Verma",
                    Description = "Comprehensive textbook covering classical mechanics, wave motion and basic optics for high school students.",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-60)
                };

                var bookMaths = new Book
                {
                    Title = "Mathematics for Class 12 - Volume 1",
                    ISBN = "978-8193297678",
                    AccessionNumber = "ACC-MTH-001",
                    Barcode = "BAR-MTH-001",
                    QRCode = "QR-MTH-001",
                    Edition = "12th Edition",
                    Volume = "Vol 1",
                    Language = "English",
                    SubjectCategory = "Mathematics",
                    Author = authorMaths.Name,
                    AuthorId = authorMaths.Id,
                    Publisher = pubSchand.Name,
                    PublisherId = pubSchand.Id,
                    VendorId = vendorCentral.Id,
                    CategoryId = maths.Id,
                    BookType = "TextBook",
                    Status = "Available",
                    PurchaseDate = DateTime.UtcNow.AddDays(-45),
                    PurchasePrice = 550.00m,
                    Shelf = "B",
                    Rack = "2",
                    Row = "1",
                    Cupboard = "C2",
                    RackLocation = "Maths-B2-1",
                    TotalCopies = 8,
                    AvailableCopies = 7,
                    MinimumStock = 2,
                    MaximumStock = 15,
                    Keywords = "Maths, Calculus, Matrices, RD Sharma",
                    Description = "Comprehensive mathematics practice book for higher secondary CBSE students.",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-45)
                };

                var bookMacbeth = new Book
                {
                    Title = "Macbeth",
                    ISBN = "978-0198324003",
                    AccessionNumber = "ACC-LIT-001",
                    Barcode = "BAR-LIT-001",
                    QRCode = "QR-LIT-001",
                    Edition = "Oxford School Ed.",
                    Volume = "N/A",
                    Language = "English",
                    SubjectCategory = "English Literature",
                    Author = authorLiterature.Name,
                    AuthorId = authorLiterature.Id,
                    Publisher = pubOxford.Name,
                    PublisherId = pubOxford.Id,
                    VendorId = vendorOxford.Id,
                    CategoryId = literature.Id,
                    BookType = "Reference",
                    Status = "Available",
                    PurchaseDate = DateTime.UtcNow.AddDays(-90),
                    PurchasePrice = 300.00m,
                    Shelf = "C",
                    Rack = "1",
                    Row = "3",
                    Cupboard = "C3",
                    RackLocation = "Lit-C1-3",
                    TotalCopies = 3,
                    AvailableCopies = 2,
                    MinimumStock = 1,
                    MaximumStock = 5,
                    Keywords = "Shakespeare, Play, Drama, Tragedy",
                    Description = "The classic tragedy of ambition and guilt written by William Shakespeare.",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-90)
                };

                context.Books.AddRange(bookPhysics, bookMaths, bookMacbeth);
                await context.SaveChangesAsync();

                // 6. Fine Rules
                var fineDefault = new LibraryFineRule
                {
                    RuleName = "Standard Circulation Fine",
                    PerDayFine = 5.00m,
                    MaxFine = 200.00m,
                    GraceDays = 3,
                    HolidayExemption = true,
                    CategoryWise = false,
                    Status = "Active",
                    IsDefault = true,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow
                };
                context.LibraryFineRules.Add(fineDefault);
                await context.SaveChangesAsync();

                // 7. Library Members
                var student = await context.Students.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.SchoolRegistrationId == schoolId);
                var memberStudent = new LibraryMember
                {
                    MembershipNumber = "LIB-ST-2026-001",
                    MemberType = "Student",
                    MemberName = student != null ? student.Name : "Aryan Sharma",
                    Email = "aryan@example.com",
                    Phone = "9876543210",
                    StudentId = student?.Id,
                    JoiningDate = DateTime.UtcNow.AddDays(-30),
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    BorrowLimit = 3,
                    CurrentBorrowCount = 1,
                    MembershipBarcode = "MB-ST-001",
                    MembershipQRCode = "MQ-ST-001",
                    Status = "Active",
                    Remarks = "Standard student membership.",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-30)
                };
                context.LibraryMembers.Add(memberStudent);
                await context.SaveChangesAsync();

                // 8. Book Issue Logs
                var issue1 = new BookIssueLog
                {
                    BookId = bookMaths.Id,
                    StudentId = student?.Id ?? 1,
                    MemberId = memberStudent.Id,
                    IssueDate = DateTime.UtcNow.AddDays(-10),
                    DueDate = DateTime.UtcNow.AddDays(4),
                    OriginalDueDate = DateTime.UtcNow.AddDays(4),
                    FineAmount = 0.00m,
                    FinePaid = false,
                    Status = "Issued",
                    BookConditionOnIssue = "Excellent",
                    IsRenewed = false,
                    RenewalCount = 0,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-10)
                };
                var issue2 = new BookIssueLog
                {
                    BookId = bookMacbeth.Id,
                    StudentId = student?.Id ?? 1,
                    MemberId = memberStudent.Id,
                    IssueDate = DateTime.UtcNow.AddDays(-20),
                    DueDate = DateTime.UtcNow.AddDays(-6),
                    OriginalDueDate = DateTime.UtcNow.AddDays(-6),
                    ReturnDate = DateTime.UtcNow.AddDays(-1),
                    FineAmount = 25.00m,
                    FinePaid = true,
                    Status = "Returned",
                    BookConditionOnIssue = "Good",
                    BookConditionOnReturn = "Good",
                    IsRenewed = false,
                    RenewalCount = 0,
                    Remarks = "Overdue fine paid at return desk.",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-20)
                };

                context.BookIssueLogs.AddRange(issue1, issue2);

                // 9. Book Reservations
                var resv = new BookReservation
                {
                    BookId = bookPhysics.Id,
                    MemberId = memberStudent.Id,
                    ReservationDate = DateTime.UtcNow.AddDays(-1),
                    ExpiryDate = DateTime.UtcNow.AddDays(6),
                    QueuePosition = 1,
                    Status = "Pending",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-1)
                };
                context.BookReservations.Add(resv);

                // 10. Digital Resources
                var digital = new DigitalResource
                {
                    Title = "CBSE Mathematics Class 12 Revision Notes",
                    Description = "Summarized formulas, shortcuts, and key points for class 12 algebra & calculus units.",
                    ResourceType = "PDF",
                    FilePath = "/uploads/library/maths_class12_revision.pdf",
                    FileSize = "4.2 MB",
                    FileExtension = "pdf",
                    DownloadAllowed = true,
                    DownloadCount = 14,
                    ViewCount = 45,
                    BookId = bookMaths.Id,
                    SubjectCategory = "Mathematics",
                    Language = "English",
                    ThumbnailPath = "/uploads/library/thumbs/maths_thumb.jpg",
                    Tags = "Maths, CBSE, Revision, Class 12",
                    Status = "Active",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-15)
                };
                context.DigitalResources.Add(digital);

                await context.SaveChangesAsync();
            }
        }
    }
}
