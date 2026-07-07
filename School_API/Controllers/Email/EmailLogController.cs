using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces.Email;
using School_API.Common.Interface;

namespace School_API.Controllers.Email
{
    public class EmailLogController : BaseController
    {
        private readonly IEmailLogService _service;

        public EmailLogController(IEmailLogService service, ICurrentUserService currentUser)
            : base(currentUser)
        {
            _service = service;
        }

        /// <summary>Get paginated email logs for a school</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int schoolId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _service.GetAllBySchoolIdAsync(schoolId, page, pageSize);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Get single email log by ID (includes BodyHtml and SmtpResponse)</summary>
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Delete a specific email log record</summary>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _service.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Purge old email logs older than specified days (default: 90 days)</summary>
        [HttpDelete]
        public async Task<IActionResult> PurgeOldLogs([FromQuery] int schoolId, [FromQuery] int daysOlderThan = 90)
        {
            var result = await _service.DeleteOldLogsAsync(schoolId, daysOlderThan);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
