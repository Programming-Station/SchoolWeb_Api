using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces.Academic;
using School_API.Common.Interface;

namespace School_API.Controllers.Academic
{
    public class StudentAttendanceController : BaseController
    {
        private readonly IStudentAttendanceService _service;
        private readonly ITenantService _tenant;

        public StudentAttendanceController(ICurrentUserService currentUser, IStudentAttendanceService service, ITenantService tenant)
            : base(currentUser)
        {
            _service = service;
            _tenant = tenant;
        }

        /// <summary>Teacher marks bulk attendance for a class on a given date.</summary>
        [HttpPost]
        public async Task<IActionResult> MarkBulk([FromBody] BulkAttendanceRequest request)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var (success, message, count) = await _service.MarkBulkAttendanceAsync(request, UserName, schoolId);
            return success ? Ok(new { message, markedCount = count }) : BadRequest(new { message });
        }

        /// <summary>Get attendance records for a class on a specific date.</summary>
        [HttpGet]
        public async Task<IActionResult> GetByDateClass([FromQuery] DateTime date, [FromQuery] int classId)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var result = await _service.GetByDateAndClassAsync(date, classId, schoolId);
            return Ok(result);
        }

        /// <summary>Get a student's full attendance records with optional date filter.</summary>
        [HttpGet]
        public async Task<IActionResult> GetByStudent(
            [FromQuery] int studentId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var result = await _service.GetByStudentAsync(studentId, schoolId, from, to);
            return Ok(result);
        }

        /// <summary>Get a student's attendance summary (Present/Absent/%) with optional date filter.</summary>
        [HttpGet]
        public async Task<IActionResult> GetStudentSummary(
            [FromQuery] int studentId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var result = await _service.GetStudentSummaryAsync(studentId, schoolId, from, to);
            return Ok(result);
        }

        /// <summary>Get class-wise attendance summary for a month.</summary>
        [HttpGet]
        public async Task<IActionResult> GetClassSummary(
            [FromQuery] int classId,
            [FromQuery] int month,
            [FromQuery] int year)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var result = await _service.GetClassSummaryAsync(classId, month, year, schoolId);
            return Ok(result);
        }

        /// <summary>Correct an attendance record.</summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] UpdateAttendanceRequest request)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var (success, message) = await _service.UpdateAttendanceAsync(id, request.Status, request.Remarks, schoolId);
            return success ? Ok(new { message }) : BadRequest(new { message });
        }
    }

    public class UpdateAttendanceRequest
    {
        public string Status { get; set; } = "Present";
        public string? Remarks { get; set; }
    }
}
