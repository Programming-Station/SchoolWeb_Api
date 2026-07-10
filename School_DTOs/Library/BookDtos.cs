using System;
using System.Collections.Generic;

namespace School_DTOs.Library
{
    #nullable disable

    // ════════════════════════════════════════════════════════════════════════
    // BOOK DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string AccessionNumber { get; set; }
        public string Barcode { get; set; }
        public string QRCode { get; set; }
        public string Edition { get; set; }
        public string Volume { get; set; }
        public string Language { get; set; }
        public string Author { get; set; }
        public string CoAuthor { get; set; }
        public int? AuthorId { get; set; }
        public string Publisher { get; set; }
        public int? PublisherId { get; set; }
        public int? VendorId { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string BookType { get; set; }
        public string Status { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public string Shelf { get; set; }
        public string Rack { get; set; }
        public string Row { get; set; }
        public string Cupboard { get; set; }
        public string RackLocation { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public int MinimumStock { get; set; }
        public int MaximumStock { get; set; }
        public string BookImagePath { get; set; }
        public string PdfAttachmentPath { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string SubjectCategory { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateBookDto
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string AccessionNumber { get; set; }
        public string Barcode { get; set; }
        public string Edition { get; set; }
        public string Volume { get; set; }
        public string Language { get; set; }
        public string Author { get; set; }
        public string CoAuthor { get; set; }
        public int? AuthorId { get; set; }
        public string Publisher { get; set; }
        public int? PublisherId { get; set; }
        public int? VendorId { get; set; }
        public int? CategoryId { get; set; }
        public string SubjectCategory { get; set; }
        public string BookType { get; set; }
        public string Status { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public string Shelf { get; set; }
        public string Rack { get; set; }
        public string Row { get; set; }
        public string Cupboard { get; set; }
        public string RackLocation { get; set; }
        public int TotalCopies { get; set; }
        public int MinimumStock { get; set; }
        public int MaximumStock { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // ISSUE LOG DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class BookIssueLogDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string Barcode { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string ClassName { get; set; }
        public int? MemberId { get; set; }
        public string MemberName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal FineAmount { get; set; }
        public bool FinePaid { get; set; }
        public string Status { get; set; }
        public string BookConditionOnIssue { get; set; }
        public string BookConditionOnReturn { get; set; }
        public bool IsRenewed { get; set; }
        public int RenewalCount { get; set; }
        public int OverdueDays { get; set; }
    }

    public class CreateBookIssueDto
    {
        public int BookId { get; set; }
        public int StudentId { get; set; }
        public int? MemberId { get; set; }
        public int DaysToBorrow { get; set; } = 14;
        public string BookConditionOnIssue { get; set; } = "Good";
    }

    // ════════════════════════════════════════════════════════════════════════
    // CATEGORY DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class BookCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryType { get; set; }
        public string ColorCode { get; set; }
        public int? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public string Status { get; set; }
    }

    public class CreateBookCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryType { get; set; }
        public string ColorCode { get; set; }
        public int? ParentCategoryId { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // AUTHOR DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class BookAuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public string Country { get; set; }
        public string PhotoPath { get; set; }
        public string Website { get; set; }
        public int BooksCount { get; set; }
        public string Status { get; set; }
    }

    public class CreateBookAuthorDto
    {
        public string Name { get; set; }
        public string Biography { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // PUBLISHER DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class BookPublisherDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string GSTNumber { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string Status { get; set; }
    }

    public class CreateBookPublisherDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string GSTNumber { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // VENDOR DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class BookVendorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string GSTNumber { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
    }

    public class CreateBookVendorDto
    {
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string GSTNumber { get; set; }
        public string Address { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // MEMBERSHIP DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class LibraryMemberDto
    {
        public int Id { get; set; }
        public string MembershipNumber { get; set; }
        public string MemberType { get; set; }
        public string MemberName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? StudentId { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int BorrowLimit { get; set; }
        public int CurrentBorrowCount { get; set; }
        public string MembershipBarcode { get; set; }
        public string Status { get; set; }
        public bool IsExpired { get; set; }
        public int DaysToExpiry { get; set; }
    }

    public class CreateLibraryMemberDto
    {
        public string MemberType { get; set; }
        public string MemberName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? StudentId { get; set; }
        public string EmployeeUserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int BorrowLimit { get; set; } = 3;
    }

    // ════════════════════════════════════════════════════════════════════════
    // RESERVATION DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class BookReservationDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int QueuePosition { get; set; }
        public string Status { get; set; }
    }

    public class CreateBookReservationDto
    {
        public int BookId { get; set; }
        public int MemberId { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // FINE RULE DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class FineRuleDto
    {
        public int Id { get; set; }
        public string RuleName { get; set; }
        public decimal PerDayFine { get; set; }
        public decimal MaxFine { get; set; }
        public int GraceDays { get; set; }
        public bool HolidayExemption { get; set; }
        public bool CategoryWise { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsDefault { get; set; }
        public string Status { get; set; }
    }

    public class CreateFineRuleDto
    {
        public string RuleName { get; set; }
        public decimal PerDayFine { get; set; }
        public decimal MaxFine { get; set; }
        public int GraceDays { get; set; }
        public bool HolidayExemption { get; set; }
        public bool CategoryWise { get; set; }
        public int? CategoryId { get; set; }
        public bool IsDefault { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // DIGITAL RESOURCE DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class DigitalResourceDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ResourceType { get; set; }
        public string FilePath { get; set; }
        public string StreamingUrl { get; set; }
        public string FileSize { get; set; }
        public bool DownloadAllowed { get; set; }
        public int DownloadCount { get; set; }
        public int ViewCount { get; set; }
        public int? BookId { get; set; }
        public string BookTitle { get; set; }
        public string SubjectCategory { get; set; }
        public string Language { get; set; }
        public string ThumbnailPath { get; set; }
        public string Tags { get; set; }
        public string Status { get; set; }
    }

    public class CreateDigitalResourceDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ResourceType { get; set; }
        public string FilePath { get; set; }
        public string StreamingUrl { get; set; }
        public bool DownloadAllowed { get; set; }
        public int? BookId { get; set; }
        public string SubjectCategory { get; set; }
        public string Language { get; set; }
        public string Tags { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // DASHBOARD & ANALYTICS DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class LibraryDashboardDto
    {
        public int TotalBooks { get; set; }
        public int AvailableBooks { get; set; }
        public int IssuedBooks { get; set; }
        public int ReservedBooks { get; set; }
        public int OverdueBooks { get; set; }
        public int LostBooks { get; set; }
        public int DamagedBooks { get; set; }
        public int TodayIssues { get; set; }
        public int TodayReturns { get; set; }
        public decimal TodayFine { get; set; }
        public int ActiveMembers { get; set; }
        public int InactiveMembers { get; set; }
        public int PendingReservations { get; set; }
        public int BooksNearDueDate { get; set; }
        public int ExpiredMemberships { get; set; }
        public List<TopReaderDto> TopReaders { get; set; } = new();
        public List<MostBorrowedBookDto> MostBorrowedBooks { get; set; } = new();
        public List<BookDto> NewArrivals { get; set; } = new();
        public List<MonthlyStatDto> MonthlyIssues { get; set; } = new();
        public List<MonthlyStatDto> MonthlyReturns { get; set; } = new();
        public List<CategoryStatDto> CategoryStats { get; set; } = new();
    }

    public class TopReaderDto
    {
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public int BooksRead { get; set; }
    }

    public class MostBorrowedBookDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int BorrowCount { get; set; }
    }

    public class MonthlyStatDto
    {
        public string Month { get; set; }
        public int Count { get; set; }
    }

    public class CategoryStatDto
    {
        public string Category { get; set; }
        public int BookCount { get; set; }
        public string ColorCode { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // PAGINATION
    // ════════════════════════════════════════════════════════════════════════
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
