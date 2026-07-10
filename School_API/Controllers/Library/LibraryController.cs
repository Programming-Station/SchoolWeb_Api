using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces;
using School.Infrastructure.Interfaces;
using School_API.Common.Interface;
using School_DTOs.Library;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School_API.Controllers.Library
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : BaseController
    {
        private readonly ILibraryService _lib;
        private readonly ITenantService _tenant;

        public LibraryController(ICurrentUserService currentUser, ILibraryService lib, ITenantService tenant)
            : base(currentUser)
        {
            _lib = lib;
            _tenant = tenant;
        }

        private int SchoolId => _tenant.GetTenantId() ?? 0;

        // ════════════════════════════════════════════════════════════════════
        // DASHBOARD
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var data = await _lib.GetDashboardAsync(SchoolId);
            return Ok(new { success = true, data });
        }

        // ════════════════════════════════════════════════════════════════════
        // BOOKS CATALOG
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Books")]
        public async Task<IActionResult> GetBooks([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? status = null, [FromQuery] string? category = null)
        {
            var result = await _lib.GetBooksAsync(SchoolId, search, page, pageSize, status, category);
            return Ok(new { success = true, data = result });
        }

        [HttpGet("Books/{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _lib.GetBookByIdAsync(id, SchoolId);
            return book == null ? NotFound(new { success = false, message = "Book not found." }) : Ok(new { success = true, data = book });
        }

        [HttpPost("Books")]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDto dto)
        {
            var book = await _lib.CreateBookAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = book });
        }

        [HttpPut("Books/{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] CreateBookDto dto)
        {
            var ok = await _lib.UpdateBookAsync(id, dto, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Book updated." }) : BadRequest(new { success = false, message = "Update failed." });
        }

        [HttpDelete("Books/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var ok = await _lib.DeleteBookAsync(id, SchoolId);
            return ok ? Ok(new { success = true, message = "Book deleted." }) : BadRequest(new { success = false, message = "Delete failed." });
        }

        [HttpPost("Books/Restore/{id}")]
        public async Task<IActionResult> RestoreBook(int id)
        {
            var ok = await _lib.RestoreBookAsync(id, SchoolId);
            return ok ? Ok(new { success = true, message = "Book restored." }) : BadRequest(new { success = false, message = "Restore failed." });
        }

        [HttpPost("Books/BulkDelete")]
        public async Task<IActionResult> BulkDeleteBooks([FromBody] List<int> ids)
        {
            var ok = await _lib.BulkDeleteBooksAsync(ids, SchoolId);
            return ok ? Ok(new { success = true, message = $"{ids.Count} books deleted." }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // ISSUE LOGS
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Issues")]
        public async Task<IActionResult> GetIssueLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? status = null)
        {
            var result = await _lib.GetIssueLogsAsync(SchoolId, page, pageSize, status);
            return Ok(new { success = true, data = result });
        }

        [HttpGet("Issues/Student/{studentId}")]
        public async Task<IActionResult> GetStudentIssueLogs(int studentId)
        {
            var list = await _lib.GetStudentIssueLogsAsync(studentId, SchoolId);
            return Ok(new { success = true, data = list });
        }

        [HttpPost("Issue")]
        public async Task<IActionResult> IssueBook([FromBody] CreateBookIssueDto dto)
        {
            try { var log = await _lib.IssueBookAsync(dto, SchoolId, UserId); return Ok(new { success = true, data = log }); }
            catch (System.Exception ex) { return BadRequest(new { success = false, message = ex.Message }); }
        }

        [HttpPost("Return/{issueLogId}")]
        public async Task<IActionResult> ReturnBook(int issueLogId)
        {
            try { var ok = await _lib.ReturnBookAsync(issueLogId, SchoolId, UserId); return ok ? Ok(new { success = true, message = "Book returned." }) : BadRequest(new { success = false }); }
            catch (System.Exception ex) { return BadRequest(new { success = false, message = ex.Message }); }
        }

        [HttpPost("Renew/{issueLogId}")]
        public async Task<IActionResult> RenewBook(int issueLogId, [FromQuery] int days = 7)
        {
            try { var ok = await _lib.RenewBookAsync(issueLogId, days, SchoolId, UserId); return ok ? Ok(new { success = true, message = "Book renewed." }) : BadRequest(new { success = false }); }
            catch (System.Exception ex) { return BadRequest(new { success = false, message = ex.Message }); }
        }

        // ════════════════════════════════════════════════════════════════════
        // CATEGORIES
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategories() => Ok(new { success = true, data = await _lib.GetCategoriesAsync(SchoolId) });

        [HttpPost("Categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateBookCategoryDto dto) => Ok(new { success = true, data = await _lib.CreateCategoryAsync(dto, SchoolId, UserId) });

        [HttpPut("Categories/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateBookCategoryDto dto) => (await _lib.UpdateCategoryAsync(id, dto, SchoolId, UserId)) ? Ok(new { success = true }) : BadRequest(new { success = false });

        [HttpDelete("Categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id) => (await _lib.DeleteCategoryAsync(id, SchoolId)) ? Ok(new { success = true }) : BadRequest(new { success = false });

        // ════════════════════════════════════════════════════════════════════
        // AUTHORS
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Authors")]
        public async Task<IActionResult> GetAuthors() => Ok(new { success = true, data = await _lib.GetAuthorsAsync(SchoolId) });

        [HttpPost("Authors")]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateBookAuthorDto dto) => Ok(new { success = true, data = await _lib.CreateAuthorAsync(dto, SchoolId, UserId) });

        [HttpPut("Authors/{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] CreateBookAuthorDto dto) => (await _lib.UpdateAuthorAsync(id, dto, SchoolId, UserId)) ? Ok(new { success = true }) : BadRequest(new { success = false });

        [HttpDelete("Authors/{id}")]
        public async Task<IActionResult> DeleteAuthor(int id) => (await _lib.DeleteAuthorAsync(id, SchoolId)) ? Ok(new { success = true }) : BadRequest(new { success = false });

        // ════════════════════════════════════════════════════════════════════
        // PUBLISHERS
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Publishers")]
        public async Task<IActionResult> GetPublishers() => Ok(new { success = true, data = await _lib.GetPublishersAsync(SchoolId) });

        [HttpPost("Publishers")]
        public async Task<IActionResult> CreatePublisher([FromBody] CreateBookPublisherDto dto) => Ok(new { success = true, data = await _lib.CreatePublisherAsync(dto, SchoolId, UserId) });

        [HttpPut("Publishers/{id}")]
        public async Task<IActionResult> UpdatePublisher(int id, [FromBody] CreateBookPublisherDto dto) => (await _lib.UpdatePublisherAsync(id, dto, SchoolId, UserId)) ? Ok(new { success = true }) : BadRequest(new { success = false });

        [HttpDelete("Publishers/{id}")]
        public async Task<IActionResult> DeletePublisher(int id) => (await _lib.DeletePublisherAsync(id, SchoolId)) ? Ok(new { success = true }) : BadRequest(new { success = false });

        // ════════════════════════════════════════════════════════════════════
        // VENDORS
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Vendors")]
        public async Task<IActionResult> GetVendors() => Ok(new { success = true, data = await _lib.GetVendorsAsync(SchoolId) });

        [HttpPost("Vendors")]
        public async Task<IActionResult> CreateVendor([FromBody] CreateBookVendorDto dto) => Ok(new { success = true, data = await _lib.CreateVendorAsync(dto, SchoolId, UserId) });

        [HttpPut("Vendors/{id}")]
        public async Task<IActionResult> UpdateVendor(int id, [FromBody] CreateBookVendorDto dto) => (await _lib.UpdateVendorAsync(id, dto, SchoolId, UserId)) ? Ok(new { success = true }) : BadRequest(new { success = false });

        [HttpDelete("Vendors/{id}")]
        public async Task<IActionResult> DeleteVendor(int id) => (await _lib.DeleteVendorAsync(id, SchoolId)) ? Ok(new { success = true }) : BadRequest(new { success = false });

        // ════════════════════════════════════════════════════════════════════
        // MEMBERS
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Members")]
        public async Task<IActionResult> GetMembers([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? memberType = null, [FromQuery] string? status = null)
            => Ok(new { success = true, data = await _lib.GetMembersAsync(SchoolId, page, pageSize, memberType, status) });

        [HttpGet("Members/{id}")]
        public async Task<IActionResult> GetMember(int id)
        {
            var m = await _lib.GetMemberByIdAsync(id, SchoolId);
            return m == null ? NotFound(new { success = false }) : Ok(new { success = true, data = m });
        }

        [HttpPost("Members")]
        public async Task<IActionResult> CreateMember([FromBody] CreateLibraryMemberDto dto) => Ok(new { success = true, data = await _lib.CreateMemberAsync(dto, SchoolId, UserId) });

        [HttpPut("Members/{id}")]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] CreateLibraryMemberDto dto) => (await _lib.UpdateMemberAsync(id, dto, SchoolId, UserId)) ? Ok(new { success = true }) : BadRequest(new { success = false });

        [HttpDelete("Members/{id}")]
        public async Task<IActionResult> DeleteMember(int id) => (await _lib.DeleteMemberAsync(id, SchoolId)) ? Ok(new { success = true }) : BadRequest(new { success = false });

        // ════════════════════════════════════════════════════════════════════
        // RESERVATIONS
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Reservations")]
        public async Task<IActionResult> GetReservations() => Ok(new { success = true, data = await _lib.GetReservationsAsync(SchoolId) });

        [HttpPost("Reservations")]
        public async Task<IActionResult> CreateReservation([FromBody] CreateBookReservationDto dto) => Ok(new { success = true, data = await _lib.CreateReservationAsync(dto, SchoolId, UserId) });

        [HttpPost("Reservations/Cancel/{id}")]
        public async Task<IActionResult> CancelReservation(int id) => (await _lib.CancelReservationAsync(id, SchoolId)) ? Ok(new { success = true }) : BadRequest(new { success = false });

        // ════════════════════════════════════════════════════════════════════
        // FINE RULES
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("FineRules")]
        public async Task<IActionResult> GetFineRules() => Ok(new { success = true, data = await _lib.GetFineRulesAsync(SchoolId) });

        [HttpPost("FineRules")]
        public async Task<IActionResult> CreateFineRule([FromBody] CreateFineRuleDto dto) => Ok(new { success = true, data = await _lib.CreateFineRuleAsync(dto, SchoolId, UserId) });

        [HttpPut("FineRules/{id}")]
        public async Task<IActionResult> UpdateFineRule(int id, [FromBody] CreateFineRuleDto dto) => (await _lib.UpdateFineRuleAsync(id, dto, SchoolId, UserId)) ? Ok(new { success = true }) : BadRequest(new { success = false });

        [HttpDelete("FineRules/{id}")]
        public async Task<IActionResult> DeleteFineRule(int id) => (await _lib.DeleteFineRuleAsync(id, SchoolId)) ? Ok(new { success = true }) : BadRequest(new { success = false });

        // ════════════════════════════════════════════════════════════════════
        // DIGITAL LIBRARY
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Digital")]
        public async Task<IActionResult> GetDigital([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? resourceType = null)
            => Ok(new { success = true, data = await _lib.GetDigitalResourcesAsync(SchoolId, page, pageSize, resourceType) });

        [HttpPost("Digital")]
        public async Task<IActionResult> CreateDigital([FromBody] CreateDigitalResourceDto dto) => Ok(new { success = true, data = await _lib.CreateDigitalResourceAsync(dto, SchoolId, UserId) });

        [HttpPut("Digital/{id}")]
        public async Task<IActionResult> UpdateDigital(int id, [FromBody] CreateDigitalResourceDto dto) => (await _lib.UpdateDigitalResourceAsync(id, dto, SchoolId, UserId)) ? Ok(new { success = true }) : BadRequest(new { success = false });

        [HttpDelete("Digital/{id}")]
        public async Task<IActionResult> DeleteDigital(int id) => (await _lib.DeleteDigitalResourceAsync(id, SchoolId)) ? Ok(new { success = true }) : BadRequest(new { success = false });
    }
}
