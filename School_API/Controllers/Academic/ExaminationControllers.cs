using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Academic;
using School_API.Common.Interface;

namespace School_API.Controllers.Academic
{
    // ══════════════════════════════════════════════════════════════════════════
    // 5.1 EXAM SCHEDULE
    // ══════════════════════════════════════════════════════════════════════════
    public class ExamScheduleController : BaseController
    {
        private readonly IExamScheduleService _svc;
        private readonly ITenantService _tenant;
        public ExamScheduleController(ICurrentUserService cur, IExamScheduleService svc, ITenantService tenant) : base(cur) { _svc = svc; _tenant = tenant; }

        [HttpPost] public async Task<IActionResult> Create([FromBody] ExamScheduleDto dto)
        {
            var (ok, msg, s) = await _svc.CreateAsync(dto, UserName, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { msg, s }) : BadRequest(new { msg });
        }

        [HttpPost] public async Task<IActionResult> SaveBulk([FromQuery] int examId, [FromBody] List<ExamScheduleDto> schedules)
        {
            var (ok, msg) = await _svc.SaveBulkAsync(examId, schedules, UserName, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpGet] public async Task<IActionResult> GetByExam([FromQuery] int examId)
            => Ok(await _svc.GetByExamAsync(examId, _tenant.GetTenantId() ?? 0));

        [HttpGet] public async Task<IActionResult> GetByClass([FromQuery] int classId)
            => Ok(await _svc.GetByClassAsync(classId, _tenant.GetTenantId() ?? 0));

        [HttpPut] public async Task<IActionResult> Update([FromBody] ExamScheduleDto dto)
        {
            var (ok, msg) = await _svc.UpdateAsync(dto);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpDelete] public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var (ok, msg) = await _svc.DeleteAsync(id);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 5.3 GRADE CONFIG
    // ══════════════════════════════════════════════════════════════════════════
    public class GradeConfigController : BaseController
    {
        private readonly IGradeConfigService _svc;
        private readonly ITenantService _tenant;
        public GradeConfigController(ICurrentUserService cur, IGradeConfigService svc, ITenantService tenant) : base(cur) { _svc = svc; _tenant = tenant; }

        [HttpPost] public async Task<IActionResult> SaveGrades([FromBody] List<GradeConfigDto> grades)
        {
            var (ok, msg) = await _svc.SaveGradesAsync(grades, UserName, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpGet] public async Task<IActionResult> GetAll()
            => Ok(await _svc.GetBySchoolAsync(_tenant.GetTenantId() ?? 0));

        [HttpGet] public async Task<IActionResult> GetForPercent([FromQuery] decimal percent)
        {
            var g = await _svc.GetForPercentageAsync(percent, _tenant.GetTenantId() ?? 0);
            return g == null ? NotFound(new { msg = "No grade configured for this percentage." }) : Ok(g);
        }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 5.4 REPORT CARD
    // ══════════════════════════════════════════════════════════════════════════
    public class ReportCardController : BaseController
    {
        private readonly IReportCardService _svc;
        private readonly ITenantService _tenant;
        public ReportCardController(ICurrentUserService cur, IReportCardService svc, ITenantService tenant) : base(cur) { _svc = svc; _tenant = tenant; }

        [HttpPost] public async Task<IActionResult> Generate([FromQuery] int examId)
        {
            var (ok, msg, count) = await _svc.GenerateForExamAsync(examId, _tenant.GetTenantId() ?? 0, UserName);
            return ok ? Ok(new { msg, count }) : BadRequest(new { msg });
        }

        [HttpGet] public async Task<IActionResult> GetByExam([FromQuery] int examId)
            => Ok(await _svc.GetByExamAsync(examId, _tenant.GetTenantId() ?? 0));

        [HttpGet] public async Task<IActionResult> GetByStudent([FromQuery] int studentId)
            => Ok(await _svc.GetByStudentAsync(studentId, _tenant.GetTenantId() ?? 0));

        [HttpGet] public async Task<IActionResult> GetStudentExam([FromQuery] int studentId, [FromQuery] int examId)
        {
            var rc = await _svc.GetByStudentExamAsync(studentId, examId);
            return rc == null ? NotFound() : Ok(rc);
        }

        [HttpPost] public async Task<IActionResult> Publish([FromQuery] int examId)
        {
            var (ok, msg) = await _svc.PublishAsync(examId, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpPut] public async Task<IActionResult> AddRemarks([FromQuery] int id, [FromQuery] string remarks)
        {
            var (ok, msg) = await _svc.AddRemarksAsync(id, remarks);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 5.6 STUDENT PROMOTION
    // ══════════════════════════════════════════════════════════════════════════
    public class StudentPromotionController : BaseController
    {
        private readonly IStudentPromotionService _svc;
        private readonly ITenantService _tenant;
        public StudentPromotionController(ICurrentUserService cur, IStudentPromotionService svc, ITenantService tenant) : base(cur) { _svc = svc; _tenant = tenant; }

        [HttpPost] public async Task<IActionResult> BulkPromote([FromBody] BulkPromotionRequest req)
        {
            var (ok, msg, count) = await _svc.BulkPromoteAsync(req, UserName, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { msg, count }) : BadRequest(new { msg });
        }

        [HttpGet] public async Task<IActionResult> GetByClass([FromQuery] int fromClassId)
            => Ok(await _svc.GetByClassAsync(fromClassId, _tenant.GetTenantId() ?? 0));

        [HttpGet] public async Task<IActionResult> GetByStudent([FromQuery] int studentId)
            => Ok(await _svc.GetByStudentAsync(studentId, _tenant.GetTenantId() ?? 0));

        [HttpPut] public async Task<IActionResult> Update([FromBody] PromotionDto dto)
        {
            var (ok, msg) = await _svc.UpdateAsync(dto);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpGet] public async Task<IActionResult> GetEligible([FromQuery] int classId, [FromQuery] int examId)
            => Ok(await _svc.GetEligibleStudentsAsync(classId, examId, _tenant.GetTenantId() ?? 0));
    }
}
