using Microsoft.EntityFrameworkCore;
using School.Domain.Library;
using School.Domain.Student;

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
            var categories = await context.BookCategories.IgnoreQueryFilters().Where(c => c.SchoolRegistrationId == schoolId).ToListAsync();
            if (!categories.Any())
            {
                var science = new BookCategory { Name = "Science & Tech", Description = "Physics, Chemistry, Biology and general science materials", CategoryType = "Book", ColorCode = "#6366f1", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var maths = new BookCategory { Name = "Mathematics", Description = "Calculus, algebra, geometry textbooks", CategoryType = "Book", ColorCode = "#10b981", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var literature = new BookCategory { Name = "Literature & Fiction", Description = "Novels, plays, and poetry collections", CategoryType = "Book", ColorCode = "#f59e0b", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var compSci = new BookCategory { Name = "Computer Science", Description = "Coding, database systems, and networking guides", CategoryType = "Book", ColorCode = "#8b5cf6", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };

                categories.AddRange(new[] { science, maths, literature, compSci });
                context.BookCategories.AddRange(science, maths, literature, compSci);
                await context.SaveChangesAsync();
            }

            // 2. Book Authors
            var authors = await context.BookAuthors.IgnoreQueryFilters().Where(a => a.SchoolRegistrationId == schoolId).ToListAsync();
            if (!authors.Any())
            {
                var authorPhysics = new BookAuthor { Name = "Dr. H.C. Verma", Biography = "Renowned Indian physicist and writer of introductory physics materials.", Country = "India", BooksCount = 10, Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var authorMaths = new BookAuthor { Name = "Dr. R.D. Sharma", Biography = "Author of widely read high school mathematics textbooks in India.", Country = "India", BooksCount = 8, Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var authorFiction = new BookAuthor { Name = "J.K. Rowling", Biography = "British author best known for writing the Harry Potter fantasy series.", Country = "United Kingdom", BooksCount = 7, Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var authorLiterature = new BookAuthor { Name = "William Shakespeare", Biography = "English playwright, poet, and actor, widely regarded as the greatest writer.", Country = "United Kingdom", BooksCount = 37, Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var authorHawking = new BookAuthor { Name = "Stephen Hawking", Biography = "English theoretical physicist, cosmologist, and author.", Country = "United Kingdom", BooksCount = 5, Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var authorDawkins = new BookAuthor { Name = "Richard Dawkins", Biography = "English evolutionary biologist and author.", Country = "United Kingdom", BooksCount = 4, Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var authorUncleBob = new BookAuthor { Name = "Robert C. Martin", Biography = "American software engineer and author, founder of Clean Coders.", Country = "United States", BooksCount = 6, Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var authorKernighan = new BookAuthor { Name = "Brian W. Kernighan", Biography = "Canadian computer scientist who worked at Bell Labs and co-designed awk/AMPL.", Country = "Canada", BooksCount = 5, Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var authorOrwell = new BookAuthor { Name = "George Orwell", Biography = "English novelist, essayist, journalist, and critic.", Country = "United Kingdom", BooksCount = 6, Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var authorLee = new BookAuthor { Name = "Harper Lee", Biography = "American novelist best known for her 1960 novel To Kill a Mockingbird.", Country = "United States", BooksCount = 2, Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var authorGrewal = new BookAuthor { Name = "Dr. B.S. Grewal", Biography = "Author of widely used higher engineering mathematics texts.", Country = "India", BooksCount = 5, Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };

                authors.AddRange(new[] { authorPhysics, authorMaths, authorFiction, authorLiterature, authorHawking, authorDawkins, authorUncleBob, authorKernighan, authorOrwell, authorLee, authorGrewal });
                context.BookAuthors.AddRange(authors);
                await context.SaveChangesAsync();
            }

            // 3. Book Publishers
            var publishers = await context.BookPublishers.IgnoreQueryFilters().Where(p => p.SchoolRegistrationId == schoolId).ToListAsync();
            if (!publishers.Any())
            {
                var pubOxford = new BookPublisher { Name = "Oxford University Press", Email = "support@oup.com", Phone = "011-2365478", GSTNumber = "07PUBGST1234A1Z1", Address = "YMCA Library Building, New Delhi", Website = "https://global.oup.com", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var pubPearson = new BookPublisher { Name = "Pearson Education India", Email = "contact@pearson.com", Phone = "022-9874563", GSTNumber = "27PUBGST5678B2Z2", Address = "Prestige Heights, Mumbai", Website = "https://in.pearson.com", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var pubSchand = new BookPublisher { Name = "S. Chand & Company Ltd", Email = "info@schandpublishing.com", Phone = "011-4569871", GSTNumber = "07PUBGST8901C3Z3", Address = "Ram Nagar, New Delhi", Website = "https://www.schandpublishing.com", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var pubBantam = new BookPublisher { Name = "Bantam Books", Email = "orders@bantam.com", Phone = "212-782-9000", GSTNumber = "USA12345678", Address = "1745 Broadway, New York, NY", Website = "https://www.penguinrandomhouse.com", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var pubHarper = new BookPublisher { Name = "HarperCollins Publishers India", Email = "info@harpercollins.co.in", Phone = "0120-4044800", GSTNumber = "09PUBGST5678X1Z9", Address = "A-75, Sector 57, Noida", Website = "https://harpercollins.co.in", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var pubPenguin = new BookPublisher { Name = "Penguin Books India", Email = "query@penguinrandomhouse.in", Phone = "0124-4785600", GSTNumber = "06PUBGST9012D4Z4", Address = "Infinity Tower C, DLF Cyber City, Gurgaon", Website = "https://penguin.co.in", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };

                publishers.AddRange(new[] { pubOxford, pubPearson, pubSchand, pubBantam, pubHarper, pubPenguin });
                context.BookPublishers.AddRange(publishers);
                await context.SaveChangesAsync();
            }

            // 4. Book Vendors
            var vendors = await context.BookVendors.IgnoreQueryFilters().Where(v => v.SchoolRegistrationId == schoolId).ToListAsync();
            if (!vendors.Any())
            {
                var vendorCentral = new BookVendor { Name = "Central Book Depot", ContactPerson = "Ramesh Kumar", Email = "orders@centralbooks.com", Phone = "011-8974521", GSTNumber = "07VNDGST1234A1Z1", Address = "Connaught Place, New Delhi", Website = "https://centralbooks.com", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var vendorOxford = new BookVendor { Name = "Oxford Book Store", ContactPerson = "Nisha Sen", Email = "sales@oxfordbookstore.com", Phone = "033-6523145", GSTNumber = "19VNDGST5678B2Z2", Address = "Park Street, Kolkata", Website = "https://www.oxfordbookstore.com", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var vendorLandmark = new BookVendor { Name = "Landmark Book Distributors", ContactPerson = "Vikram Aditya", Email = "landmark@books.com", Phone = "044-6655443", GSTNumber = "33VNDGST9876C3Z3", Address = "Apex Plaza, Nungambakkam, Chennai", Website = "https://landmarkonthenet.com", Status = "Active", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };

                vendors.AddRange(new[] { vendorCentral, vendorOxford, vendorLandmark });
                context.BookVendors.AddRange(vendors);
                await context.SaveChangesAsync();
            }

            // 5. Books
            var books = await context.Books.IgnoreQueryFilters().Where(b => b.SchoolRegistrationId == schoolId).ToListAsync();
            if (!books.Any())
            {
                var science = categories.FirstOrDefault(c => c.Name == "Science & Tech") ?? await context.BookCategories.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Name == "Science & Tech" && c.SchoolRegistrationId == schoolId);
                var maths = categories.FirstOrDefault(c => c.Name == "Mathematics") ?? await context.BookCategories.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Name == "Mathematics" && c.SchoolRegistrationId == schoolId);
                var literature = categories.FirstOrDefault(c => c.Name == "Literature & Fiction") ?? await context.BookCategories.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Name == "Literature & Fiction" && c.SchoolRegistrationId == schoolId);
                var compSci = categories.FirstOrDefault(c => c.Name == "Computer Science") ?? await context.BookCategories.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Name == "Computer Science" && c.SchoolRegistrationId == schoolId);

                var authorPhysics = authors.FirstOrDefault(a => a.Name == "Dr. H.C. Verma") ?? await context.BookAuthors.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Name == "Dr. H.C. Verma" && a.SchoolRegistrationId == schoolId);
                var authorMaths = authors.FirstOrDefault(a => a.Name == "Dr. R.D. Sharma") ?? await context.BookAuthors.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Name == "Dr. R.D. Sharma" && a.SchoolRegistrationId == schoolId);
                var authorLiterature = authors.FirstOrDefault(a => a.Name == "William Shakespeare") ?? await context.BookAuthors.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Name == "William Shakespeare" && a.SchoolRegistrationId == schoolId);
                var authorHawking = authors.FirstOrDefault(a => a.Name == "Stephen Hawking") ?? await context.BookAuthors.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Name == "Stephen Hawking" && a.SchoolRegistrationId == schoolId);
                var authorDawkins = authors.FirstOrDefault(a => a.Name == "Richard Dawkins") ?? await context.BookAuthors.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Name == "Richard Dawkins" && a.SchoolRegistrationId == schoolId);
                var authorUncleBob = authors.FirstOrDefault(a => a.Name == "Robert C. Martin") ?? await context.BookAuthors.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Name == "Robert C. Martin" && a.SchoolRegistrationId == schoolId);
                var authorKernighan = authors.FirstOrDefault(a => a.Name == "Brian W. Kernighan") ?? await context.BookAuthors.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Name == "Brian W. Kernighan" && a.SchoolRegistrationId == schoolId);
                var authorFiction = authors.FirstOrDefault(a => a.Name == "J.K. Rowling") ?? await context.BookAuthors.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Name == "J.K. Rowling" && a.SchoolRegistrationId == schoolId);
                var authorLee = authors.FirstOrDefault(a => a.Name == "Harper Lee") ?? await context.BookAuthors.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Name == "Harper Lee" && a.SchoolRegistrationId == schoolId);
                var authorOrwell = authors.FirstOrDefault(a => a.Name == "George Orwell") ?? await context.BookAuthors.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Name == "George Orwell" && a.SchoolRegistrationId == schoolId);
                var authorGrewal = authors.FirstOrDefault(a => a.Name == "Dr. B.S. Grewal") ?? await context.BookAuthors.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Name == "Dr. B.S. Grewal" && a.SchoolRegistrationId == schoolId);

                var pubSchand = publishers.FirstOrDefault(p => p.Name == "S. Chand & Company Ltd") ?? await context.BookPublishers.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Name == "S. Chand & Company Ltd" && p.SchoolRegistrationId == schoolId);
                var pubOxford = publishers.FirstOrDefault(p => p.Name == "Oxford University Press") ?? await context.BookPublishers.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Name == "Oxford University Press" && p.SchoolRegistrationId == schoolId);
                var pubPearson = publishers.FirstOrDefault(p => p.Name == "Pearson Education India") ?? await context.BookPublishers.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Name == "Pearson Education India" && p.SchoolRegistrationId == schoolId);
                var pubBantam = publishers.FirstOrDefault(p => p.Name == "Bantam Books") ?? await context.BookPublishers.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Name == "Bantam Books" && p.SchoolRegistrationId == schoolId);
                var pubHarper = publishers.FirstOrDefault(p => p.Name == "HarperCollins Publishers India") ?? await context.BookPublishers.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Name == "HarperCollins Publishers India" && p.SchoolRegistrationId == schoolId);
                var pubPenguin = publishers.FirstOrDefault(p => p.Name == "Penguin Books India") ?? await context.BookPublishers.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Name == "Penguin Books India" && p.SchoolRegistrationId == schoolId);

                var vendorCentral = vendors.FirstOrDefault(v => v.Name == "Central Book Depot") ?? await context.BookVendors.IgnoreQueryFilters().FirstOrDefaultAsync(v => v.Name == "Central Book Depot" && v.SchoolRegistrationId == schoolId);
                var vendorOxford = vendors.FirstOrDefault(v => v.Name == "Oxford Book Store") ?? await context.BookVendors.IgnoreQueryFilters().FirstOrDefaultAsync(v => v.Name == "Oxford Book Store" && v.SchoolRegistrationId == schoolId);
                var vendorLandmark = vendors.FirstOrDefault(v => v.Name == "Landmark Book Distributors") ?? await context.BookVendors.IgnoreQueryFilters().FirstOrDefaultAsync(v => v.Name == "Landmark Book Distributors" && v.SchoolRegistrationId == schoolId);

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
                    Author = authorPhysics?.Name ?? "Dr. H.C. Verma",
                    AuthorId = authorPhysics?.Id,
                    Publisher = pubSchand?.Name ?? "S. Chand & Company Ltd",
                    PublisherId = pubSchand?.Id,
                    VendorId = vendorCentral?.Id,
                    CategoryId = science?.Id,
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
                    Author = authorMaths?.Name ?? "Dr. R.D. Sharma",
                    AuthorId = authorMaths?.Id,
                    Publisher = pubSchand?.Name ?? "S. Chand & Company Ltd",
                    PublisherId = pubSchand?.Id,
                    VendorId = vendorCentral?.Id,
                    CategoryId = maths?.Id,
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
                    Author = authorLiterature?.Name ?? "William Shakespeare",
                    AuthorId = authorLiterature?.Id,
                    Publisher = pubOxford?.Name ?? "Oxford University Press",
                    PublisherId = pubOxford?.Id,
                    VendorId = vendorOxford?.Id,
                    CategoryId = literature?.Id,
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

                var bookBriefHistory = new Book
                {
                    Title = "A Brief History of Time",
                    ISBN = "978-0553380163",
                    AccessionNumber = "ACC-SCI-002",
                    Barcode = "BAR-SCI-002",
                    QRCode = "QR-SCI-002",
                    Edition = "10th Anniv. Ed.",
                    Volume = "N/A",
                    Language = "English",
                    SubjectCategory = "Cosmology",
                    Author = authorHawking?.Name ?? "Stephen Hawking",
                    AuthorId = authorHawking?.Id,
                    Publisher = pubBantam?.Name ?? "Bantam Books",
                    PublisherId = pubBantam?.Id,
                    VendorId = vendorLandmark?.Id,
                    CategoryId = science?.Id,
                    BookType = "General",
                    Status = "Available",
                    PurchaseDate = DateTime.UtcNow.AddDays(-120),
                    PurchasePrice = 399.00m,
                    Shelf = "A",
                    Rack = "4",
                    Row = "1",
                    Cupboard = "C1",
                    RackLocation = "Science-A4-1",
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    MinimumStock = 1,
                    MaximumStock = 5,
                    Keywords = "Universe, Black Holes, Space, Time, Hawking",
                    Description = "Stephen Hawking's landmark book about the origin and fate of the universe.",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-120)
                };

                var bookSelfishGene = new Book
                {
                    Title = "The Selfish Gene",
                    ISBN = "978-0198788607",
                    AccessionNumber = "ACC-SCI-003",
                    Barcode = "BAR-SCI-003",
                    QRCode = "QR-SCI-003",
                    Edition = "40th Anniv. Ed.",
                    Volume = "N/A",
                    Language = "English",
                    SubjectCategory = "Biology",
                    Author = authorDawkins?.Name ?? "Richard Dawkins",
                    AuthorId = authorDawkins?.Id,
                    Publisher = pubOxford?.Name ?? "Oxford University Press",
                    PublisherId = pubOxford?.Id,
                    VendorId = vendorOxford?.Id,
                    CategoryId = science?.Id,
                    BookType = "General",
                    Status = "Available",
                    PurchaseDate = DateTime.UtcNow.AddDays(-150),
                    PurchasePrice = 450.00m,
                    Shelf = "A",
                    Rack = "2",
                    Row = "3",
                    Cupboard = "C1",
                    RackLocation = "Science-A2-3",
                    TotalCopies = 3,
                    AvailableCopies = 3,
                    MinimumStock = 1,
                    MaximumStock = 5,
                    Keywords = "Evolution, Biology, Genetics, Dawkins",
                    Description = "A brilliant exposition of the gene-centric view of evolution.",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-150)
                };

                var bookCleanCode = new Book
                {
                    Title = "Clean Code",
                    ISBN = "978-0132350884",
                    AccessionNumber = "ACC-CS-001",
                    Barcode = "BAR-CS-001",
                    QRCode = "QR-CS-001",
                    Edition = "1st Edition",
                    Volume = "N/A",
                    Language = "English",
                    SubjectCategory = "Software Engineering",
                    Author = authorUncleBob?.Name ?? "Robert C. Martin",
                    AuthorId = authorUncleBob?.Id,
                    Publisher = pubPearson?.Name ?? "Pearson Education India",
                    PublisherId = pubPearson?.Id,
                    VendorId = vendorLandmark?.Id,
                    CategoryId = compSci?.Id,
                    BookType = "Reference",
                    Status = "Available",
                    PurchaseDate = DateTime.UtcNow.AddDays(-30),
                    PurchasePrice = 850.00m,
                    Shelf = "D",
                    Rack = "1",
                    Row = "1",
                    Cupboard = "C4",
                    RackLocation = "CS-D1-1",
                    TotalCopies = 6,
                    AvailableCopies = 6,
                    MinimumStock = 1,
                    MaximumStock = 10,
                    Keywords = "Programming, Software, Refactoring, Clean Code, Uncle Bob",
                    Description = "A handbook of agile software craftsmanship including code examples.",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-30)
                };

                var bookCProg = new Book
                {
                    Title = "The C Programming Language",
                    ISBN = "978-0131103627",
                    AccessionNumber = "ACC-CS-002",
                    Barcode = "BAR-CS-002",
                    QRCode = "QR-CS-002",
                    Edition = "2nd Edition",
                    Volume = "N/A",
                    Language = "English",
                    SubjectCategory = "Programming Languages",
                    Author = authorKernighan?.Name ?? "Brian W. Kernighan",
                    AuthorId = authorKernighan?.Id,
                    Publisher = pubPearson?.Name ?? "Pearson Education India",
                    PublisherId = pubPearson?.Id,
                    VendorId = vendorCentral?.Id,
                    CategoryId = compSci?.Id,
                    BookType = "TextBook",
                    Status = "Available",
                    PurchaseDate = DateTime.UtcNow.AddDays(-80),
                    PurchasePrice = 399.00m,
                    Shelf = "D",
                    Rack = "2",
                    Row = "2",
                    Cupboard = "C4",
                    RackLocation = "CS-D2-2",
                    TotalCopies = 5,
                    AvailableCopies = 5,
                    MinimumStock = 1,
                    MaximumStock = 8,
                    Keywords = "C Language, Dennis Ritchie, Programming, Kernighan",
                    Description = "The definitive guide to ANSI standard C programming.",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-80)
                };

                var bookHarryPotter = new Book
                {
                    Title = "Harry Potter and the Sorcerer's Stone",
                    ISBN = "978-0439708180",
                    AccessionNumber = "ACC-FIC-001",
                    Barcode = "BAR-FIC-001",
                    QRCode = "QR-FIC-001",
                    Edition = "Special Edition",
                    Volume = "Vol 1",
                    Language = "English",
                    SubjectCategory = "Fantasy Fiction",
                    Author = authorFiction?.Name ?? "J.K. Rowling",
                    AuthorId = authorFiction?.Id,
                    Publisher = pubPenguin?.Name ?? "Penguin Books India",
                    PublisherId = pubPenguin?.Id,
                    VendorId = vendorOxford?.Id,
                    CategoryId = literature?.Id,
                    BookType = "General",
                    Status = "Available",
                    PurchaseDate = DateTime.UtcNow.AddDays(-200),
                    PurchasePrice = 299.00m,
                    Shelf = "E",
                    Rack = "1",
                    Row = "1",
                    Cupboard = "C5",
                    RackLocation = "Fiction-E1-1",
                    TotalCopies = 10,
                    AvailableCopies = 10,
                    MinimumStock = 2,
                    MaximumStock = 20,
                    Keywords = "Magic, Harry Potter, Hogwarts, Rowling",
                    Description = "The first book in the legendary Harry Potter series about the boy wizard.",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-200)
                };

                var bookMockingbird = new Book
                {
                    Title = "To Kill a Mockingbird",
                    ISBN = "978-0446310789",
                    AccessionNumber = "ACC-FIC-002",
                    Barcode = "BAR-FIC-002",
                    QRCode = "QR-FIC-002",
                    Edition = "Classic Edition",
                    Volume = "N/A",
                    Language = "English",
                    SubjectCategory = "Classic Literature",
                    Author = authorLee?.Name ?? "Harper Lee",
                    AuthorId = authorLee?.Id,
                    Publisher = pubHarper?.Name ?? "HarperCollins Publishers India",
                    PublisherId = pubHarper?.Id,
                    VendorId = vendorLandmark?.Id,
                    CategoryId = literature?.Id,
                    BookType = "General",
                    Status = "Available",
                    PurchaseDate = DateTime.UtcNow.AddDays(-110),
                    PurchasePrice = 350.00m,
                    Shelf = "E",
                    Rack = "2",
                    Row = "2",
                    Cupboard = "C5",
                    RackLocation = "Fiction-E2-2",
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    MinimumStock = 1,
                    MaximumStock = 8,
                    Keywords = "Harper Lee, Racism, Classic Novel, Mockingbird",
                    Description = "Harper Lee's Pulitzer Prize-winning masterpiece about honor and injustice.",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-110)
                };

                var book1984 = new Book
                {
                    Title = "1984",
                    ISBN = "978-0451524935",
                    AccessionNumber = "ACC-FIC-003",
                    Barcode = "BAR-FIC-003",
                    QRCode = "QR-FIC-003",
                    Edition = "Classic Edition",
                    Volume = "N/A",
                    Language = "English",
                    SubjectCategory = "Dystopian Fiction",
                    Author = authorOrwell?.Name ?? "George Orwell",
                    AuthorId = authorOrwell?.Id,
                    Publisher = pubPenguin?.Name ?? "Penguin Books India",
                    PublisherId = pubPenguin?.Id,
                    VendorId = vendorOxford?.Id,
                    CategoryId = literature?.Id,
                    BookType = "General",
                    Status = "Available",
                    PurchaseDate = DateTime.UtcNow.AddDays(-180),
                    PurchasePrice = 280.00m,
                    Shelf = "E",
                    Rack = "3",
                    Row = "1",
                    Cupboard = "C5",
                    RackLocation = "Fiction-E3-1",
                    TotalCopies = 5,
                    AvailableCopies = 5,
                    MinimumStock = 1,
                    MaximumStock = 10,
                    Keywords = "Dystopia, Big Brother, Orwell, 1984",
                    Description = "The classic dystopian novel warning against totalitarianism.",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-180)
                };

                var bookBriefAnswers = new Book
                {
                    Title = "Brief Answers to the Big Questions",
                    ISBN = "978-1473560932",
                    AccessionNumber = "ACC-SCI-004",
                    Barcode = "BAR-SCI-004",
                    QRCode = "QR-SCI-004",
                    Edition = "1st Edition",
                    Volume = "N/A",
                    Language = "English",
                    SubjectCategory = "Cosmology",
                    Author = authorHawking?.Name ?? "Stephen Hawking",
                    AuthorId = authorHawking?.Id,
                    Publisher = pubBantam?.Name ?? "Bantam Books",
                    PublisherId = pubBantam?.Id,
                    VendorId = vendorCentral?.Id,
                    CategoryId = science?.Id,
                    BookType = "General",
                    Status = "Available",
                    PurchaseDate = DateTime.UtcNow.AddDays(-75),
                    PurchasePrice = 499.00m,
                    Shelf = "A",
                    Rack = "4",
                    Row = "2",
                    Cupboard = "C1",
                    RackLocation = "Science-A4-2",
                    TotalCopies = 3,
                    AvailableCopies = 3,
                    MinimumStock = 1,
                    MaximumStock = 5,
                    Keywords = "Hawking, Future, Science, Cosmology, Universe",
                    Description = "Stephen Hawking's final book offering his views on the universe's biggest mysteries.",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-75)
                };

                var bookHigherMaths = new Book
                {
                    Title = "Higher Engineering Mathematics",
                    ISBN = "978-8174091956",
                    AccessionNumber = "ACC-MTH-002",
                    Barcode = "BAR-MTH-002",
                    QRCode = "QR-MTH-002",
                    Edition = "43rd Edition",
                    Volume = "N/A",
                    Language = "English",
                    SubjectCategory = "Mathematics",
                    Author = authorGrewal?.Name ?? "Dr. B.S. Grewal",
                    AuthorId = authorGrewal?.Id,
                    Publisher = pubSchand?.Name ?? "S. Chand & Company Ltd",
                    PublisherId = pubSchand?.Id,
                    VendorId = vendorCentral?.Id,
                    CategoryId = maths?.Id,
                    BookType = "TextBook",
                    Status = "Available",
                    PurchaseDate = DateTime.UtcNow.AddDays(-140),
                    PurchasePrice = 850.00m,
                    Shelf = "B",
                    Rack = "3",
                    Row = "1",
                    Cupboard = "C2",
                    RackLocation = "Maths-B3-1",
                    TotalCopies = 5,
                    AvailableCopies = 5,
                    MinimumStock = 1,
                    MaximumStock = 10,
                    Keywords = "Engineering, Math, BS Grewal, Calculus",
                    Description = "A comprehensive text for engineering and science students.",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-140)
                };

                books.AddRange(new[] { bookPhysics, bookMaths, bookMacbeth, bookBriefHistory, bookSelfishGene, bookCleanCode, bookCProg, bookHarryPotter, bookMockingbird, book1984, bookBriefAnswers, bookHigherMaths });
                context.Books.AddRange(bookPhysics, bookMaths, bookMacbeth, bookBriefHistory, bookSelfishGene, bookCleanCode, bookCProg, bookHarryPotter, bookMockingbird, book1984, bookBriefAnswers, bookHigherMaths);
                await context.SaveChangesAsync();
            }

            // 6. Fine Rules
            var fineRules = await context.LibraryFineRules.IgnoreQueryFilters().Where(fr => fr.SchoolRegistrationId == schoolId).ToListAsync();
            if (!fineRules.Any())
            {
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
            }

            // 7. Library Members
            var members = await context.LibraryMembers.IgnoreQueryFilters().Where(m => m.SchoolRegistrationId == schoolId).ToListAsync();
            if (!members.Any())
            {
                var student = await context.Students.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.SchoolRegistrationId == schoolId)
                              ?? await context.Students.IgnoreQueryFilters().FirstOrDefaultAsync();

                if (student == null)
                {
                    var course = await context.Courses.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.SchoolRegistrationId == schoolId)
                                 ?? await context.Courses.IgnoreQueryFilters().FirstOrDefaultAsync();
                    var classEntity = await context.Classes.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.SchoolRegistrationId == schoolId)
                                      ?? await context.Classes.IgnoreQueryFilters().FirstOrDefaultAsync();
                    var status = await context.Statuses.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.Name == "Active")
                                 ?? await context.Statuses.IgnoreQueryFilters().FirstOrDefaultAsync();

                    if (course != null && classEntity != null && status != null)
                    {
                        student = new Student
                        {
                            StudentId = "STU_LIB_TEMP",
                            EnrollmentNumber = "ENR_LIB_TEMP",
                            CourseType = "School",
                            CourseId = course.Id,
                            CourseOpted = course.Name,
                            Name = "Library Seed Student",
                            Gender = "Male",
                            ClassId = classEntity.Id,
                            StatusId = status.Id,
                            SchoolRegistrationId = schoolId,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = "seed"
                        };
                        context.Students.Add(student);
                        await context.SaveChangesAsync();
                    }
                }

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
                members.Add(memberStudent);
                context.LibraryMembers.Add(memberStudent);
                await context.SaveChangesAsync();
            }

            // 8. Book Issue Logs
            var issueLogs = await context.BookIssueLogs.IgnoreQueryFilters().Where(il => il.SchoolRegistrationId == schoolId).ToListAsync();
            if (!issueLogs.Any())
            {
                var bookMaths = books.FirstOrDefault(b => b.Title.Contains("Mathematics")) ?? await context.Books.IgnoreQueryFilters().FirstOrDefaultAsync(b => b.Title.Contains("Mathematics") && b.SchoolRegistrationId == schoolId);
                var bookMacbeth = books.FirstOrDefault(b => b.Title.Contains("Macbeth")) ?? await context.Books.IgnoreQueryFilters().FirstOrDefaultAsync(b => b.Title.Contains("Macbeth") && b.SchoolRegistrationId == schoolId);
                var memberStudent = members.FirstOrDefault() ?? await context.LibraryMembers.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.SchoolRegistrationId == schoolId);
                var student = await context.Students.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.SchoolRegistrationId == schoolId)
                              ?? await context.Students.IgnoreQueryFilters().FirstOrDefaultAsync();

                if (bookMaths != null && memberStudent != null && student != null)
                {
                    var issue1 = new BookIssueLog
                    {
                        BookId = bookMaths.Id,
                        StudentId = student.Id,
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
                    context.BookIssueLogs.Add(issue1);
                }

                if (bookMacbeth != null && memberStudent != null && student != null)
                {
                    var issue2 = new BookIssueLog
                    {
                        BookId = bookMacbeth.Id,
                        StudentId = student.Id,
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
                    context.BookIssueLogs.Add(issue2);
                }

                await context.SaveChangesAsync();
            }

            // 9. Book Reservations
            var reservations = await context.BookReservations.IgnoreQueryFilters().Where(r => r.SchoolRegistrationId == schoolId).ToListAsync();
            if (!reservations.Any())
            {
                var bookPhysics = books.FirstOrDefault(b => b.Title.Contains("Physics")) ?? await context.Books.IgnoreQueryFilters().FirstOrDefaultAsync(b => b.Title.Contains("Physics") && b.SchoolRegistrationId == schoolId);
                var memberStudent = members.FirstOrDefault() ?? await context.LibraryMembers.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.SchoolRegistrationId == schoolId);

                if (bookPhysics != null && memberStudent != null)
                {
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
                    await context.SaveChangesAsync();
                }
            }

            // 10. Digital Resources
            var digitalResources = await context.DigitalResources.IgnoreQueryFilters().Where(dr => dr.SchoolRegistrationId == schoolId).ToListAsync();
            if (!digitalResources.Any())
            {
                var bookMaths = books.FirstOrDefault(b => b.Title.Contains("Mathematics")) ?? await context.Books.IgnoreQueryFilters().FirstOrDefaultAsync(b => b.Title.Contains("Mathematics") && b.SchoolRegistrationId == schoolId);

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
                    BookId = bookMaths?.Id,
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
