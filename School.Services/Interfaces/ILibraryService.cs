using System.Collections.Generic;
using System.Threading.Tasks;
using School_DTOs.Library;

namespace School.Services.Interfaces
{
    public interface ILibraryService
    {
        // ── Books Catalog ──────────────────────────────────────────────────────
        Task<PagedResultDto<BookDto>> GetBooksAsync(int schoolId, string search, int page, int pageSize, string status, string category);
        Task<BookDto> GetBookByIdAsync(int id, int schoolId);
        Task<BookDto> CreateBookAsync(CreateBookDto dto, int schoolId, string userId);
        Task<bool> UpdateBookAsync(int id, CreateBookDto dto, int schoolId, string userId);
        Task<bool> DeleteBookAsync(int id, int schoolId);
        Task<bool> RestoreBookAsync(int id, int schoolId);
        Task<bool> BulkDeleteBooksAsync(List<int> ids, int schoolId);

        // ── Issue / Return ─────────────────────────────────────────────────────
        Task<PagedResultDto<BookIssueLogDto>> GetIssueLogsAsync(int schoolId, int page, int pageSize, string status);
        Task<IEnumerable<BookIssueLogDto>> GetStudentIssueLogsAsync(int studentId, int schoolId);
        Task<BookIssueLogDto> IssueBookAsync(CreateBookIssueDto dto, int schoolId, string userId);
        Task<bool> ReturnBookAsync(int issueLogId, int schoolId, string userId);
        Task<bool> RenewBookAsync(int issueLogId, int additionalDays, int schoolId, string userId);

        // ── Categories ────────────────────────────────────────────────────────
        Task<IEnumerable<BookCategoryDto>> GetCategoriesAsync(int schoolId);
        Task<BookCategoryDto> CreateCategoryAsync(CreateBookCategoryDto dto, int schoolId, string userId);
        Task<bool> UpdateCategoryAsync(int id, CreateBookCategoryDto dto, int schoolId, string userId);
        Task<bool> DeleteCategoryAsync(int id, int schoolId);

        // ── Authors ───────────────────────────────────────────────────────────
        Task<IEnumerable<BookAuthorDto>> GetAuthorsAsync(int schoolId);
        Task<BookAuthorDto> CreateAuthorAsync(CreateBookAuthorDto dto, int schoolId, string userId);
        Task<bool> UpdateAuthorAsync(int id, CreateBookAuthorDto dto, int schoolId, string userId);
        Task<bool> DeleteAuthorAsync(int id, int schoolId);

        // ── Publishers ────────────────────────────────────────────────────────
        Task<IEnumerable<BookPublisherDto>> GetPublishersAsync(int schoolId);
        Task<BookPublisherDto> CreatePublisherAsync(CreateBookPublisherDto dto, int schoolId, string userId);
        Task<bool> UpdatePublisherAsync(int id, CreateBookPublisherDto dto, int schoolId, string userId);
        Task<bool> DeletePublisherAsync(int id, int schoolId);

        // ── Vendors ───────────────────────────────────────────────────────────
        Task<IEnumerable<BookVendorDto>> GetVendorsAsync(int schoolId);
        Task<BookVendorDto> CreateVendorAsync(CreateBookVendorDto dto, int schoolId, string userId);
        Task<bool> UpdateVendorAsync(int id, CreateBookVendorDto dto, int schoolId, string userId);
        Task<bool> DeleteVendorAsync(int id, int schoolId);

        // ── Members ───────────────────────────────────────────────────────────
        Task<PagedResultDto<LibraryMemberDto>> GetMembersAsync(int schoolId, int page, int pageSize, string memberType, string status);
        Task<LibraryMemberDto> GetMemberByIdAsync(int id, int schoolId);
        Task<LibraryMemberDto> CreateMemberAsync(CreateLibraryMemberDto dto, int schoolId, string userId);
        Task<bool> UpdateMemberAsync(int id, CreateLibraryMemberDto dto, int schoolId, string userId);
        Task<bool> DeleteMemberAsync(int id, int schoolId);

        // ── Reservations ──────────────────────────────────────────────────────
        Task<IEnumerable<BookReservationDto>> GetReservationsAsync(int schoolId);
        Task<BookReservationDto> CreateReservationAsync(CreateBookReservationDto dto, int schoolId, string userId);
        Task<bool> CancelReservationAsync(int id, int schoolId);

        // ── Fine Rules ────────────────────────────────────────────────────────
        Task<IEnumerable<FineRuleDto>> GetFineRulesAsync(int schoolId);
        Task<FineRuleDto> CreateFineRuleAsync(CreateFineRuleDto dto, int schoolId, string userId);
        Task<bool> UpdateFineRuleAsync(int id, CreateFineRuleDto dto, int schoolId, string userId);
        Task<bool> DeleteFineRuleAsync(int id, int schoolId);

        // ── Digital Library ───────────────────────────────────────────────────
        Task<PagedResultDto<DigitalResourceDto>> GetDigitalResourcesAsync(int schoolId, int page, int pageSize, string resourceType);
        Task<DigitalResourceDto> CreateDigitalResourceAsync(CreateDigitalResourceDto dto, int schoolId, string userId);
        Task<bool> UpdateDigitalResourceAsync(int id, CreateDigitalResourceDto dto, int schoolId, string userId);
        Task<bool> DeleteDigitalResourceAsync(int id, int schoolId);

        // ── Dashboard ─────────────────────────────────────────────────────────
        Task<LibraryDashboardDto> GetDashboardAsync(int schoolId);
    }
}
