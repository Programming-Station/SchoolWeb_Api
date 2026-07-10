using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Library;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs.Library;

namespace School.Services.Library
{
    #nullable disable

    public class LibraryService : ILibraryService
    {
        private readonly SchoolDbContext _db;

        public LibraryService(SchoolDbContext db)
        {
            _db = db;
        }

        // ════════════════════════════════════════════════════════════════════
        // BOOKS CATALOG
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<BookDto>> GetBooksAsync(int schoolId, string search, int page, int pageSize, string status, string category)
        {
            var q = _db.Books
                .Include(b => b.Category)
                .Where(b => b.SchoolRegistrationId == schoolId && !b.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower().Trim();
                q = q.Where(b => b.Title.ToLower().Contains(s) ||
                                 b.Author.ToLower().Contains(s) ||
                                 b.ISBN.ToLower().Contains(s) ||
                                 b.AccessionNumber.ToLower().Contains(s));
            }
            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(b => b.Status == status);
            if (!string.IsNullOrWhiteSpace(category))
                q = q.Where(b => b.SubjectCategory == category);

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(b => b.CreatedDate)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            return new PagedResultDto<BookDto>
            {
                Items = items.Select(MapBook).ToList(),
                TotalItems = total, PageNumber = page, PageSize = pageSize
            };
        }

        public async Task<BookDto> GetBookByIdAsync(int id, int schoolId)
        {
            var b = await _db.Books.Include(x => x.Category)
                             .FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            return b == null ? null : MapBook(b);
        }

        public async Task<BookDto> CreateBookAsync(CreateBookDto dto, int schoolId, string userId)
        {
            var entity = new Book
            {
                Title = dto.Title, ISBN = dto.ISBN, AccessionNumber = dto.AccessionNumber,
                Barcode = dto.Barcode ?? Guid.NewGuid().ToString("N")[..12].ToUpper(),
                Edition = dto.Edition, Volume = dto.Volume, Language = dto.Language ?? "English",
                Author = dto.Author, CoAuthor = dto.CoAuthor, AuthorId = dto.AuthorId,
                Publisher = dto.Publisher, PublisherId = dto.PublisherId, VendorId = dto.VendorId,
                CategoryId = dto.CategoryId, SubjectCategory = dto.SubjectCategory,
                BookType = dto.BookType ?? "General", Status = dto.Status ?? "Available",
                PurchaseDate = dto.PurchaseDate, PurchasePrice = dto.PurchasePrice,
                Shelf = dto.Shelf, Rack = dto.Rack, Row = dto.Row, Cupboard = dto.Cupboard,
                RackLocation = dto.RackLocation, TotalCopies = dto.TotalCopies,
                AvailableCopies = dto.TotalCopies, MinimumStock = dto.MinimumStock,
                MaximumStock = dto.MaximumStock, Keywords = dto.Keywords, Description = dto.Description,
                SchoolRegistrationId = schoolId, CreatedBy = userId, CreatedDate = DateTime.UtcNow
            };
            _db.Books.Add(entity);
            await _db.SaveChangesAsync();
            return MapBook(entity);
        }

        public async Task<bool> UpdateBookAsync(int id, CreateBookDto dto, int schoolId, string userId)
        {
            var entity = await _db.Books.FirstOrDefaultAsync(b => b.Id == id && b.SchoolRegistrationId == schoolId && !b.IsDeleted);
            if (entity == null) return false;

            var diff = dto.TotalCopies - entity.TotalCopies;
            entity.Title = dto.Title; entity.ISBN = dto.ISBN; entity.AccessionNumber = dto.AccessionNumber;
            entity.Edition = dto.Edition; entity.Volume = dto.Volume; entity.Language = dto.Language;
            entity.Author = dto.Author; entity.CoAuthor = dto.CoAuthor; entity.AuthorId = dto.AuthorId;
            entity.Publisher = dto.Publisher; entity.PublisherId = dto.PublisherId; entity.VendorId = dto.VendorId;
            entity.CategoryId = dto.CategoryId; entity.SubjectCategory = dto.SubjectCategory;
            entity.BookType = dto.BookType; entity.Status = dto.Status;
            entity.PurchaseDate = dto.PurchaseDate; entity.PurchasePrice = dto.PurchasePrice;
            entity.Shelf = dto.Shelf; entity.Rack = dto.Rack; entity.Row = dto.Row; entity.Cupboard = dto.Cupboard;
            entity.RackLocation = dto.RackLocation; entity.TotalCopies = dto.TotalCopies;
            entity.AvailableCopies = Math.Max(0, entity.AvailableCopies + diff);
            entity.MinimumStock = dto.MinimumStock; entity.MaximumStock = dto.MaximumStock;
            entity.Keywords = dto.Keywords; entity.Description = dto.Description;
            entity.UpdatedBy = userId; entity.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBookAsync(int id, int schoolId)
        {
            var entity = await _db.Books.FirstOrDefaultAsync(b => b.Id == id && b.SchoolRegistrationId == schoolId);
            if (entity == null) return false;
            entity.IsDeleted = true; entity.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreBookAsync(int id, int schoolId)
        {
            var entity = await _db.Books.IgnoreQueryFilters()
                                  .FirstOrDefaultAsync(b => b.Id == id && b.SchoolRegistrationId == schoolId && b.IsDeleted);
            if (entity == null) return false;
            entity.IsDeleted = false; entity.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BulkDeleteBooksAsync(List<int> ids, int schoolId)
        {
            var books = await _db.Books.Where(b => ids.Contains(b.Id) && b.SchoolRegistrationId == schoolId).ToListAsync();
            foreach (var b in books) { b.IsDeleted = true; b.UpdatedDate = DateTime.UtcNow; }
            await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // ISSUE / RETURN / RENEW
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<BookIssueLogDto>> GetIssueLogsAsync(int schoolId, int page, int pageSize, string status)
        {
            var q = _db.BookIssueLogs
                .Include(l => l.Book).Include(l => l.Student)
                .Where(l => l.SchoolRegistrationId == schoolId && !l.IsDeleted);

            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(l => l.Status == status);

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(l => l.IssueDate).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResultDto<BookIssueLogDto>
            {
                Items = items.Select(MapLog).ToList(),
                TotalItems = total, PageNumber = page, PageSize = pageSize
            };
        }

        public async Task<IEnumerable<BookIssueLogDto>> GetStudentIssueLogsAsync(int studentId, int schoolId)
        {
            var logs = await _db.BookIssueLogs
                .Include(l => l.Book).Include(l => l.Student)
                .Where(l => l.StudentId == studentId && l.SchoolRegistrationId == schoolId && !l.IsDeleted)
                .OrderByDescending(l => l.IssueDate).ToListAsync();
            return logs.Select(MapLog);
        }

        public async Task<BookIssueLogDto> IssueBookAsync(CreateBookIssueDto dto, int schoolId, string userId)
        {
            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == dto.BookId && b.SchoolRegistrationId == schoolId && !b.IsDeleted)
                       ?? throw new KeyNotFoundException("Book not found.");

            if (book.AvailableCopies <= 0)
                throw new InvalidOperationException("No copies available for issue.");

            var student = await _db.Students.FindAsync(dto.StudentId)
                          ?? throw new KeyNotFoundException("Student not found.");

            book.AvailableCopies--;
            if (book.AvailableCopies == 0) book.Status = "Issued";

            var dueDate = DateTime.UtcNow.Date.AddDays(dto.DaysToBorrow);
            var log = new BookIssueLog
            {
                BookId = dto.BookId, StudentId = dto.StudentId, MemberId = dto.MemberId,
                IssueDate = DateTime.UtcNow.Date, DueDate = dueDate, OriginalDueDate = dueDate,
                Status = "Issued", BookConditionOnIssue = dto.BookConditionOnIssue ?? "Good",
                SchoolRegistrationId = schoolId, CreatedBy = userId, CreatedDate = DateTime.UtcNow
            };

            _db.BookIssueLogs.Add(log);
            await _db.SaveChangesAsync();

            return await _db.BookIssueLogs
                .Include(l => l.Book).Include(l => l.Student)
                .Where(l => l.Id == log.Id)
                .Select(l => MapLog(l)).FirstAsync();
        }

        public async Task<bool> ReturnBookAsync(int issueLogId, int schoolId, string userId)
        {
            var log = await _db.BookIssueLogs.Include(l => l.Book)
                               .FirstOrDefaultAsync(l => l.Id == issueLogId && l.SchoolRegistrationId == schoolId && !l.IsDeleted);
            if (log == null) return false;
            if (log.Status == "Returned") throw new InvalidOperationException("Book already returned.");

            var returnDate = DateTime.UtcNow.Date;
            log.ReturnDate = returnDate;
            log.Status = "Returned";

            // Calculate fine using rule or default
            var rule = await _db.LibraryFineRules.FirstOrDefaultAsync(r => r.SchoolRegistrationId == schoolId && r.IsDefault && !r.IsDeleted);
            var perDayFine = rule?.PerDayFine ?? 10m;
            var graceDays = rule?.GraceDays ?? 0;
            var maxFine = rule?.MaxFine ?? 500m;

            if (returnDate > log.DueDate.Date)
            {
                var overdueDays = (returnDate - log.DueDate.Date).Days - graceDays;
                if (overdueDays > 0)
                    log.FineAmount = Math.Min(overdueDays * perDayFine, maxFine);
            }

            // Restore book stock
            if (log.Book != null)
            {
                log.Book.AvailableCopies = Math.Min(log.Book.TotalCopies, log.Book.AvailableCopies + 1);
                if (log.Book.AvailableCopies > 0 && log.Book.Status == "Issued")
                    log.Book.Status = "Available";
            }

            log.UpdatedBy = userId; log.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RenewBookAsync(int issueLogId, int additionalDays, int schoolId, string userId)
        {
            var log = await _db.BookIssueLogs.FirstOrDefaultAsync(l => l.Id == issueLogId && l.SchoolRegistrationId == schoolId && !l.IsDeleted);
            if (log == null || log.Status == "Returned") return false;

            log.DueDate = log.DueDate.AddDays(additionalDays);
            log.IsRenewed = true;
            log.RenewalCount++;
            log.Status = "Renewed";
            log.UpdatedBy = userId; log.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // CATEGORIES
        // ════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<BookCategoryDto>> GetCategoriesAsync(int schoolId)
        {
            var list = await _db.BookCategories
                .Where(c => c.SchoolRegistrationId == schoolId && !c.IsDeleted)
                .OrderBy(c => c.Name).ToListAsync();
            return list.Select(c => new BookCategoryDto
            {
                Id = c.Id, Name = c.Name, Description = c.Description,
                CategoryType = c.CategoryType, ColorCode = c.ColorCode,
                ParentCategoryId = c.ParentCategoryId, Status = c.Status
            });
        }

        public async Task<BookCategoryDto> CreateCategoryAsync(CreateBookCategoryDto dto, int schoolId, string userId)
        {
            var entity = new BookCategory
            {
                Name = dto.Name, Description = dto.Description, CategoryType = dto.CategoryType,
                ColorCode = dto.ColorCode, ParentCategoryId = dto.ParentCategoryId,
                SchoolRegistrationId = schoolId, CreatedBy = userId, CreatedDate = DateTime.UtcNow
            };
            _db.BookCategories.Add(entity);
            await _db.SaveChangesAsync();
            return new BookCategoryDto { Id = entity.Id, Name = entity.Name, CategoryType = entity.CategoryType, ColorCode = entity.ColorCode, Status = entity.Status };
        }

        public async Task<bool> UpdateCategoryAsync(int id, CreateBookCategoryDto dto, int schoolId, string userId)
        {
            var e = await _db.BookCategories.FirstOrDefaultAsync(c => c.Id == id && c.SchoolRegistrationId == schoolId && !c.IsDeleted);
            if (e == null) return false;
            e.Name = dto.Name; e.Description = dto.Description; e.CategoryType = dto.CategoryType;
            e.ColorCode = dto.ColorCode; e.ParentCategoryId = dto.ParentCategoryId;
            e.UpdatedBy = userId; e.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id, int schoolId)
        {
            var e = await _db.BookCategories.FirstOrDefaultAsync(c => c.Id == id && c.SchoolRegistrationId == schoolId);
            if (e == null) return false;
            e.IsDeleted = true; await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // AUTHORS
        // ════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<BookAuthorDto>> GetAuthorsAsync(int schoolId)
        {
            var list = await _db.BookAuthors.Where(a => a.SchoolRegistrationId == schoolId && !a.IsDeleted).OrderBy(a => a.Name).ToListAsync();
            return list.Select(a => new BookAuthorDto { Id = a.Id, Name = a.Name, Biography = a.Biography, Country = a.Country, PhotoPath = a.PhotoPath, Website = a.Website, BooksCount = a.BooksCount, Status = a.Status });
        }

        public async Task<BookAuthorDto> CreateAuthorAsync(CreateBookAuthorDto dto, int schoolId, string userId)
        {
            var entity = new BookAuthor { Name = dto.Name, Biography = dto.Biography, Country = dto.Country, Website = dto.Website, SchoolRegistrationId = schoolId, CreatedBy = userId, CreatedDate = DateTime.UtcNow };
            _db.BookAuthors.Add(entity);
            await _db.SaveChangesAsync();
            return new BookAuthorDto { Id = entity.Id, Name = entity.Name, Status = entity.Status };
        }

        public async Task<bool> UpdateAuthorAsync(int id, CreateBookAuthorDto dto, int schoolId, string userId)
        {
            var e = await _db.BookAuthors.FirstOrDefaultAsync(a => a.Id == id && a.SchoolRegistrationId == schoolId && !a.IsDeleted);
            if (e == null) return false;
            e.Name = dto.Name; e.Biography = dto.Biography; e.Country = dto.Country; e.Website = dto.Website;
            e.UpdatedBy = userId; e.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAuthorAsync(int id, int schoolId)
        {
            var e = await _db.BookAuthors.FirstOrDefaultAsync(a => a.Id == id && a.SchoolRegistrationId == schoolId);
            if (e == null) return false;
            e.IsDeleted = true; await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // PUBLISHERS
        // ════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<BookPublisherDto>> GetPublishersAsync(int schoolId)
        {
            var list = await _db.BookPublishers.Where(p => p.SchoolRegistrationId == schoolId && !p.IsDeleted).ToListAsync();
            return list.Select(p => new BookPublisherDto { Id = p.Id, Name = p.Name, Email = p.Email, Phone = p.Phone, GSTNumber = p.GSTNumber, Address = p.Address, Website = p.Website, Status = p.Status });
        }

        public async Task<BookPublisherDto> CreatePublisherAsync(CreateBookPublisherDto dto, int schoolId, string userId)
        {
            var e = new BookPublisher { Name = dto.Name, Email = dto.Email, Phone = dto.Phone, GSTNumber = dto.GSTNumber, Address = dto.Address, Website = dto.Website, SchoolRegistrationId = schoolId, CreatedBy = userId, CreatedDate = DateTime.UtcNow };
            _db.BookPublishers.Add(e);
            await _db.SaveChangesAsync();
            return new BookPublisherDto { Id = e.Id, Name = e.Name, Status = e.Status };
        }

        public async Task<bool> UpdatePublisherAsync(int id, CreateBookPublisherDto dto, int schoolId, string userId)
        {
            var e = await _db.BookPublishers.FirstOrDefaultAsync(p => p.Id == id && p.SchoolRegistrationId == schoolId && !p.IsDeleted);
            if (e == null) return false;
            e.Name = dto.Name; e.Email = dto.Email; e.Phone = dto.Phone; e.GSTNumber = dto.GSTNumber; e.Address = dto.Address; e.Website = dto.Website;
            e.UpdatedBy = userId; e.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePublisherAsync(int id, int schoolId)
        {
            var e = await _db.BookPublishers.FirstOrDefaultAsync(p => p.Id == id && p.SchoolRegistrationId == schoolId);
            if (e == null) return false;
            e.IsDeleted = true; await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // VENDORS
        // ════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<BookVendorDto>> GetVendorsAsync(int schoolId)
        {
            var list = await _db.BookVendors.Where(v => v.SchoolRegistrationId == schoolId && !v.IsDeleted).ToListAsync();
            return list.Select(v => new BookVendorDto { Id = v.Id, Name = v.Name, ContactPerson = v.ContactPerson, Email = v.Email, Phone = v.Phone, GSTNumber = v.GSTNumber, Address = v.Address, Status = v.Status });
        }

        public async Task<BookVendorDto> CreateVendorAsync(CreateBookVendorDto dto, int schoolId, string userId)
        {
            var e = new BookVendor { Name = dto.Name, ContactPerson = dto.ContactPerson, Email = dto.Email, Phone = dto.Phone, GSTNumber = dto.GSTNumber, Address = dto.Address, SchoolRegistrationId = schoolId, CreatedBy = userId, CreatedDate = DateTime.UtcNow };
            _db.BookVendors.Add(e);
            await _db.SaveChangesAsync();
            return new BookVendorDto { Id = e.Id, Name = e.Name, Status = e.Status };
        }

        public async Task<bool> UpdateVendorAsync(int id, CreateBookVendorDto dto, int schoolId, string userId)
        {
            var e = await _db.BookVendors.FirstOrDefaultAsync(v => v.Id == id && v.SchoolRegistrationId == schoolId && !v.IsDeleted);
            if (e == null) return false;
            e.Name = dto.Name; e.ContactPerson = dto.ContactPerson; e.Email = dto.Email; e.Phone = dto.Phone; e.GSTNumber = dto.GSTNumber; e.Address = dto.Address;
            e.UpdatedBy = userId; e.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteVendorAsync(int id, int schoolId)
        {
            var e = await _db.BookVendors.FirstOrDefaultAsync(v => v.Id == id && v.SchoolRegistrationId == schoolId);
            if (e == null) return false;
            e.IsDeleted = true; await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // MEMBERS
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<LibraryMemberDto>> GetMembersAsync(int schoolId, int page, int pageSize, string memberType, string status)
        {
            var q = _db.LibraryMembers.Where(m => m.SchoolRegistrationId == schoolId && !m.IsDeleted);
            if (!string.IsNullOrWhiteSpace(memberType)) q = q.Where(m => m.MemberType == memberType);
            if (!string.IsNullOrWhiteSpace(status)) q = q.Where(m => m.Status == status);

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(m => m.JoiningDate).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var now = DateTime.UtcNow;

            return new PagedResultDto<LibraryMemberDto>
            {
                Items = items.Select(m => new LibraryMemberDto
                {
                    Id = m.Id, MembershipNumber = m.MembershipNumber, MemberType = m.MemberType,
                    MemberName = m.MemberName, Email = m.Email, Phone = m.Phone, StudentId = m.StudentId,
                    JoiningDate = m.JoiningDate, ExpiryDate = m.ExpiryDate, BorrowLimit = m.BorrowLimit,
                    CurrentBorrowCount = m.CurrentBorrowCount, MembershipBarcode = m.MembershipBarcode,
                    Status = m.Status, IsExpired = m.ExpiryDate < now,
                    DaysToExpiry = (m.ExpiryDate - now).Days
                }).ToList(),
                TotalItems = total, PageNumber = page, PageSize = pageSize
            };
        }

        public async Task<LibraryMemberDto> GetMemberByIdAsync(int id, int schoolId)
        {
            var m = await _db.LibraryMembers.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (m == null) return null;
            var now = DateTime.UtcNow;
            return new LibraryMemberDto { Id = m.Id, MembershipNumber = m.MembershipNumber, MemberType = m.MemberType, MemberName = m.MemberName, Email = m.Email, Phone = m.Phone, StudentId = m.StudentId, JoiningDate = m.JoiningDate, ExpiryDate = m.ExpiryDate, BorrowLimit = m.BorrowLimit, CurrentBorrowCount = m.CurrentBorrowCount, Status = m.Status, IsExpired = m.ExpiryDate < now, DaysToExpiry = (m.ExpiryDate - now).Days };
        }

        public async Task<LibraryMemberDto> CreateMemberAsync(CreateLibraryMemberDto dto, int schoolId, string userId)
        {
            var count = await _db.LibraryMembers.CountAsync(m => m.SchoolRegistrationId == schoolId);
            var memberNo = $"LM{schoolId:D3}{(count + 1):D5}";
            var entity = new LibraryMember
            {
                MembershipNumber = memberNo, MemberType = dto.MemberType, MemberName = dto.MemberName,
                Email = dto.Email, Phone = dto.Phone, StudentId = dto.StudentId, EmployeeUserId = dto.EmployeeUserId,
                JoiningDate = DateTime.UtcNow, ExpiryDate = dto.ExpiryDate,
                BorrowLimit = dto.BorrowLimit, MembershipBarcode = memberNo,
                SchoolRegistrationId = schoolId, CreatedBy = userId, CreatedDate = DateTime.UtcNow
            };
            _db.LibraryMembers.Add(entity);
            await _db.SaveChangesAsync();
            return await GetMemberByIdAsync(entity.Id, schoolId);
        }

        public async Task<bool> UpdateMemberAsync(int id, CreateLibraryMemberDto dto, int schoolId, string userId)
        {
            var e = await _db.LibraryMembers.FirstOrDefaultAsync(m => m.Id == id && m.SchoolRegistrationId == schoolId && !m.IsDeleted);
            if (e == null) return false;
            e.MemberName = dto.MemberName; e.Email = dto.Email; e.Phone = dto.Phone;
            e.MemberType = dto.MemberType; e.BorrowLimit = dto.BorrowLimit; e.ExpiryDate = dto.ExpiryDate;
            e.UpdatedBy = userId; e.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMemberAsync(int id, int schoolId)
        {
            var e = await _db.LibraryMembers.FirstOrDefaultAsync(m => m.Id == id && m.SchoolRegistrationId == schoolId);
            if (e == null) return false;
            e.IsDeleted = true; await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // RESERVATIONS
        // ════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<BookReservationDto>> GetReservationsAsync(int schoolId)
        {
            var list = await _db.BookReservations
                .Include(r => r.Book).Include(r => r.Member)
                .Where(r => r.SchoolRegistrationId == schoolId && !r.IsDeleted)
                .OrderBy(r => r.BookId).ThenBy(r => r.QueuePosition).ToListAsync();
            return list.Select(r => new BookReservationDto { Id = r.Id, BookId = r.BookId, BookTitle = r.Book?.Title, BookAuthor = r.Book?.Author, MemberId = r.MemberId, MemberName = r.Member?.MemberName, ReservationDate = r.ReservationDate, ExpiryDate = r.ExpiryDate, QueuePosition = r.QueuePosition, Status = r.Status });
        }

        public async Task<BookReservationDto> CreateReservationAsync(CreateBookReservationDto dto, int schoolId, string userId)
        {
            var queuePos = await _db.BookReservations.CountAsync(r => r.BookId == dto.BookId && r.Status == "Pending" && !r.IsDeleted) + 1;
            var entity = new BookReservation
            {
                BookId = dto.BookId, MemberId = dto.MemberId, ReservationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(7), QueuePosition = queuePos,
                SchoolRegistrationId = schoolId, CreatedBy = userId, CreatedDate = DateTime.UtcNow
            };
            _db.BookReservations.Add(entity);
            await _db.SaveChangesAsync();
            return new BookReservationDto { Id = entity.Id, BookId = entity.BookId, MemberId = entity.MemberId, QueuePosition = entity.QueuePosition, Status = entity.Status };
        }

        public async Task<bool> CancelReservationAsync(int id, int schoolId)
        {
            var e = await _db.BookReservations.FirstOrDefaultAsync(r => r.Id == id && r.SchoolRegistrationId == schoolId);
            if (e == null) return false;
            e.Status = "Cancelled"; e.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // FINE RULES
        // ════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<FineRuleDto>> GetFineRulesAsync(int schoolId)
        {
            var list = await _db.LibraryFineRules.Include(f => f.Category).Where(f => f.SchoolRegistrationId == schoolId && !f.IsDeleted).ToListAsync();
            return list.Select(f => new FineRuleDto { Id = f.Id, RuleName = f.RuleName, PerDayFine = f.PerDayFine, MaxFine = f.MaxFine, GraceDays = f.GraceDays, HolidayExemption = f.HolidayExemption, CategoryWise = f.CategoryWise, CategoryId = f.CategoryId, CategoryName = f.Category?.Name, IsDefault = f.IsDefault, Status = f.Status });
        }

        public async Task<FineRuleDto> CreateFineRuleAsync(CreateFineRuleDto dto, int schoolId, string userId)
        {
            if (dto.IsDefault)
            {
                var existing = await _db.LibraryFineRules.Where(f => f.SchoolRegistrationId == schoolId && f.IsDefault).ToListAsync();
                existing.ForEach(f => f.IsDefault = false);
            }
            var entity = new LibraryFineRule { RuleName = dto.RuleName, PerDayFine = dto.PerDayFine, MaxFine = dto.MaxFine, GraceDays = dto.GraceDays, HolidayExemption = dto.HolidayExemption, CategoryWise = dto.CategoryWise, CategoryId = dto.CategoryId, IsDefault = dto.IsDefault, SchoolRegistrationId = schoolId, CreatedBy = userId, CreatedDate = DateTime.UtcNow };
            _db.LibraryFineRules.Add(entity);
            await _db.SaveChangesAsync();
            return new FineRuleDto { Id = entity.Id, RuleName = entity.RuleName, PerDayFine = entity.PerDayFine, MaxFine = entity.MaxFine, IsDefault = entity.IsDefault };
        }

        public async Task<bool> UpdateFineRuleAsync(int id, CreateFineRuleDto dto, int schoolId, string userId)
        {
            var e = await _db.LibraryFineRules.FirstOrDefaultAsync(f => f.Id == id && f.SchoolRegistrationId == schoolId && !f.IsDeleted);
            if (e == null) return false;
            if (dto.IsDefault) { var others = await _db.LibraryFineRules.Where(f => f.SchoolRegistrationId == schoolId && f.IsDefault && f.Id != id).ToListAsync(); others.ForEach(f => f.IsDefault = false); }
            e.RuleName = dto.RuleName; e.PerDayFine = dto.PerDayFine; e.MaxFine = dto.MaxFine; e.GraceDays = dto.GraceDays; e.HolidayExemption = dto.HolidayExemption; e.CategoryWise = dto.CategoryWise; e.CategoryId = dto.CategoryId; e.IsDefault = dto.IsDefault;
            e.UpdatedBy = userId; e.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFineRuleAsync(int id, int schoolId)
        {
            var e = await _db.LibraryFineRules.FirstOrDefaultAsync(f => f.Id == id && f.SchoolRegistrationId == schoolId);
            if (e == null) return false;
            e.IsDeleted = true; await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // DIGITAL LIBRARY
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<DigitalResourceDto>> GetDigitalResourcesAsync(int schoolId, int page, int pageSize, string resourceType)
        {
            var q = _db.DigitalResources.Include(d => d.Book).Where(d => d.SchoolRegistrationId == schoolId && !d.IsDeleted);
            if (!string.IsNullOrWhiteSpace(resourceType)) q = q.Where(d => d.ResourceType == resourceType);
            var total = await q.CountAsync();
            var items = await q.OrderByDescending(d => d.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedResultDto<DigitalResourceDto> { Items = items.Select(d => new DigitalResourceDto { Id = d.Id, Title = d.Title, Description = d.Description, ResourceType = d.ResourceType, FilePath = d.FilePath, StreamingUrl = d.StreamingUrl, FileSize = d.FileSize, DownloadAllowed = d.DownloadAllowed, DownloadCount = d.DownloadCount, ViewCount = d.ViewCount, BookId = d.BookId, BookTitle = d.Book?.Title, SubjectCategory = d.SubjectCategory, Language = d.Language, ThumbnailPath = d.ThumbnailPath, Tags = d.Tags, Status = d.Status }).ToList(), TotalItems = total, PageNumber = page, PageSize = pageSize };
        }

        public async Task<DigitalResourceDto> CreateDigitalResourceAsync(CreateDigitalResourceDto dto, int schoolId, string userId)
        {
            var entity = new DigitalResource { Title = dto.Title, Description = dto.Description, ResourceType = dto.ResourceType, FilePath = dto.FilePath, StreamingUrl = dto.StreamingUrl, DownloadAllowed = dto.DownloadAllowed, BookId = dto.BookId, SubjectCategory = dto.SubjectCategory, Language = dto.Language, Tags = dto.Tags, SchoolRegistrationId = schoolId, CreatedBy = userId, CreatedDate = DateTime.UtcNow };
            _db.DigitalResources.Add(entity);
            await _db.SaveChangesAsync();
            return new DigitalResourceDto { Id = entity.Id, Title = entity.Title, ResourceType = entity.ResourceType, Status = entity.Status };
        }

        public async Task<bool> UpdateDigitalResourceAsync(int id, CreateDigitalResourceDto dto, int schoolId, string userId)
        {
            var e = await _db.DigitalResources.FirstOrDefaultAsync(d => d.Id == id && d.SchoolRegistrationId == schoolId && !d.IsDeleted);
            if (e == null) return false;
            e.Title = dto.Title; e.Description = dto.Description; e.ResourceType = dto.ResourceType; e.FilePath = dto.FilePath; e.StreamingUrl = dto.StreamingUrl; e.DownloadAllowed = dto.DownloadAllowed; e.BookId = dto.BookId; e.SubjectCategory = dto.SubjectCategory; e.Language = dto.Language; e.Tags = dto.Tags;
            e.UpdatedBy = userId; e.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDigitalResourceAsync(int id, int schoolId)
        {
            var e = await _db.DigitalResources.FirstOrDefaultAsync(d => d.Id == id && d.SchoolRegistrationId == schoolId);
            if (e == null) return false;
            e.IsDeleted = true; await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // DASHBOARD
        // ════════════════════════════════════════════════════════════════════
        public async Task<LibraryDashboardDto> GetDashboardAsync(int schoolId)
        {
            var today = DateTime.UtcNow.Date;
            var books = await _db.Books.Where(b => b.SchoolRegistrationId == schoolId && !b.IsDeleted).ToListAsync();
            var logs = await _db.BookIssueLogs.Include(l => l.Book).Include(l => l.Student)
                .Where(l => l.SchoolRegistrationId == schoolId && !l.IsDeleted).ToListAsync();
            var members = await _db.LibraryMembers.Where(m => m.SchoolRegistrationId == schoolId && !m.IsDeleted).ToListAsync();
            var reservations = await _db.BookReservations.Where(r => r.SchoolRegistrationId == schoolId && !r.IsDeleted && r.Status == "Pending").ToListAsync();

            var activeLoans = logs.Where(l => l.Status != "Returned").ToList();
            var todayLogs = logs.Where(l => l.IssueDate.Date == today).ToList();
            var todayReturns = logs.Where(l => l.ReturnDate?.Date == today).ToList();

            // Monthly stats (last 6 months)
            var monthlyIssues = Enumerable.Range(0, 6).Select(i =>
            {
                var d = today.AddMonths(-i);
                return new MonthlyStatDto { Month = d.ToString("MMM yyyy"), Count = logs.Count(l => l.IssueDate.Year == d.Year && l.IssueDate.Month == d.Month) };
            }).Reverse().ToList();

            var monthlyReturns = Enumerable.Range(0, 6).Select(i =>
            {
                var d = today.AddMonths(-i);
                return new MonthlyStatDto { Month = d.ToString("MMM yyyy"), Count = logs.Count(l => l.ReturnDate?.Year == d.Year && l.ReturnDate?.Month == d.Month) };
            }).Reverse().ToList();

            // Category stats
            var cats = await _db.BookCategories.Where(c => c.SchoolRegistrationId == schoolId && !c.IsDeleted).ToListAsync();
            var catStats = cats.Select(c => new CategoryStatDto { Category = c.Name, BookCount = books.Count(b => b.CategoryId == c.Id), ColorCode = c.ColorCode }).ToList();

            // Top readers
            var topReaders = logs.GroupBy(l => l.StudentId)
                .Select(g => new { StudentId = g.Key, Student = g.First().Student, Count = g.Count() })
                .OrderByDescending(x => x.Count).Take(5)
                .Select(x => new TopReaderDto { StudentName = x.Student?.Name ?? "N/A", StudentCode = x.Student?.StudentId ?? "", BooksRead = x.Count }).ToList();

            // Most borrowed
            var mostBorrowed = logs.GroupBy(l => l.BookId)
                .Select(g => new { Book = g.First().Book, Count = g.Count() })
                .OrderByDescending(x => x.Count).Take(5)
                .Select(x => new MostBorrowedBookDto { Title = x.Book?.Title ?? "N/A", Author = x.Book?.Author ?? "", BorrowCount = x.Count }).ToList();

            // New arrivals (last 30 days)
            var newArrivals = books.Where(b => b.CreatedDate >= today.AddDays(-30)).OrderByDescending(b => b.CreatedDate).Take(10).Select(MapBook).ToList();

            return new LibraryDashboardDto
            {
                TotalBooks = books.Count,
                AvailableBooks = books.Count(b => b.Status == "Available"),
                IssuedBooks = activeLoans.Count,
                ReservedBooks = reservations.Count,
                OverdueBooks = activeLoans.Count(l => l.DueDate.Date < today),
                LostBooks = books.Count(b => b.Status == "Lost"),
                DamagedBooks = books.Count(b => b.Status == "Damaged"),
                TodayIssues = todayLogs.Count,
                TodayReturns = todayReturns.Count,
                TodayFine = todayReturns.Sum(l => l.FineAmount),
                ActiveMembers = members.Count(m => m.Status == "Active"),
                InactiveMembers = members.Count(m => m.Status != "Active"),
                PendingReservations = reservations.Count,
                BooksNearDueDate = activeLoans.Count(l => l.DueDate.Date <= today.AddDays(3) && l.DueDate.Date >= today),
                ExpiredMemberships = members.Count(m => m.ExpiryDate < DateTime.UtcNow),
                TopReaders = topReaders,
                MostBorrowedBooks = mostBorrowed,
                NewArrivals = newArrivals,
                MonthlyIssues = monthlyIssues,
                MonthlyReturns = monthlyReturns,
                CategoryStats = catStats
            };
        }

        // ════════════════════════════════════════════════════════════════════
        // PRIVATE MAPPERS
        // ════════════════════════════════════════════════════════════════════
        private static BookDto MapBook(Book b) => new()
        {
            Id = b.Id, Title = b.Title, ISBN = b.ISBN, AccessionNumber = b.AccessionNumber,
            Barcode = b.Barcode, QRCode = b.QRCode, Edition = b.Edition, Volume = b.Volume,
            Language = b.Language, Author = b.Author, CoAuthor = b.CoAuthor, AuthorId = b.AuthorId,
            Publisher = b.Publisher, PublisherId = b.PublisherId, VendorId = b.VendorId,
            CategoryId = b.CategoryId, CategoryName = b.Category?.Name, SubjectCategory = b.SubjectCategory,
            BookType = b.BookType, Status = b.Status, PurchaseDate = b.PurchaseDate,
            PurchasePrice = b.PurchasePrice, Shelf = b.Shelf, Rack = b.Rack, Row = b.Row,
            Cupboard = b.Cupboard, RackLocation = b.RackLocation, TotalCopies = b.TotalCopies,
            AvailableCopies = b.AvailableCopies, MinimumStock = b.MinimumStock,
            MaximumStock = b.MaximumStock, BookImagePath = b.BookImagePath, PdfAttachmentPath = b.PdfAttachmentPath,
            Keywords = b.Keywords, Description = b.Description, CreatedDate = b.CreatedDate ?? DateTime.MinValue
        };

        private static BookIssueLogDto MapLog(BookIssueLog l)
        {
            var overdue = l.ReturnDate.HasValue ? (int)(l.ReturnDate.Value.Date - l.DueDate.Date).TotalDays : (int)(DateTime.UtcNow.Date - l.DueDate.Date).TotalDays;
            return new BookIssueLogDto
            {
                Id = l.Id, BookId = l.BookId, BookTitle = l.Book?.Title ?? "N/A",
                BookAuthor = l.Book?.Author ?? "N/A", Barcode = l.Book?.Barcode,
                StudentId = l.StudentId, StudentName = l.Student?.Name ?? "N/A",
                StudentCode = l.Student?.StudentId ?? "", MemberId = l.MemberId,
                IssueDate = l.IssueDate, DueDate = l.DueDate, ReturnDate = l.ReturnDate,
                FineAmount = l.FineAmount, FinePaid = l.FinePaid, Status = l.Status,
                BookConditionOnIssue = l.BookConditionOnIssue, BookConditionOnReturn = l.BookConditionOnReturn,
                IsRenewed = l.IsRenewed, RenewalCount = l.RenewalCount,
                OverdueDays = Math.Max(0, overdue)
            };
        }
    }
}
