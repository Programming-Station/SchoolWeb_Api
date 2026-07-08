using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces.Academic;
using School_API.Common.Interface;

namespace School_API.Controllers.Academic
{
    public class SubjectEnrollmentController : BaseController
    {
        private readonly ISubjectEnrollmentService _service;
        private readonly ITenantService _tenant;

        public SubjectEnrollmentController(ICurrentUserService currentUser, ISubjectEnrollmentService service, ITenantService tenant)
            : base(currentUser)
        {
            _service = service;
            _tenant  = tenant;
        }

        [HttpPost]
        public async Task<IActionResult> Enroll([FromBody] EnrollSubjectRequest request)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var (success, message, enrolled) = await _service.EnrollSubjectsAsync(request, UserName, schoolId);
            return success ? Ok(new { message, enrolled }) : BadRequest(new { message });
        }

        [HttpPost]
        public async Task<IActionResult> Drop([FromBody] DropSubjectRequest request)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var (success, message) = await _service.DropSubjectAsync(request.EnrollmentId, request.Remarks ?? "", schoolId);
            return success ? Ok(new { message }) : BadRequest(new { message });
        }

        [HttpGet]
        public async Task<IActionResult> GetByStudent([FromQuery] int studentId)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var result = await _service.GetByStudentAsync(studentId, schoolId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByClass([FromQuery] int classId)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var result = await _service.GetByClassAsync(classId, schoolId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByBatch([FromQuery] int batchId)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var result = await _service.GetByBatchAsync(batchId, schoolId);
            return Ok(result);
        }
    }

    public class DropSubjectRequest
    {
        public int EnrollmentId { get; set; }
        public string? Remarks { get; set; }
    }
}
