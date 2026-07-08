using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces.Academic;
using School_API.Common.Interface;

namespace School_API.Controllers.Academic
{
    // ══════════════════════════════════════════════════════════════════════════
    // 4.4 HOMEWORK
    // ══════════════════════════════════════════════════════════════════════════
    public class HomeworkController : BaseController
    {
        private readonly IHomeworkService _svc;
        private readonly ITenantService _tenant;
        public HomeworkController(ICurrentUserService cur, IHomeworkService svc, ITenantService tenant) : base(cur) { _svc = svc; _tenant = tenant; }

        [HttpPost] public async Task<IActionResult> Create([FromBody] CreateHomeworkRequest req)
        {
            var (ok, msg, hw) = await _svc.CreateAsync(req, UserName, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { msg, hw }) : BadRequest(new { msg });
        }

        [HttpGet] public async Task<IActionResult> GetByClass([FromQuery] int classId)
            => Ok(await _svc.GetByClassAsync(classId, _tenant.GetTenantId() ?? 0));

        [HttpGet] public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var hw = await _svc.GetByIdAsync(id);
            return hw == null ? NotFound() : Ok(hw);
        }

        [HttpGet] public async Task<IActionResult> GetSubmissions([FromQuery] int homeworkId)
            => Ok(await _svc.GetSubmissionsAsync(homeworkId));

        [HttpPost] public async Task<IActionResult> Submit([FromBody] HomeworkSubmitRequest req)
        {
            var (ok, msg) = await _svc.SubmitAsync(req.HomeworkId, req.StudentId, req.FilePath, req.Remarks, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpPost] public async Task<IActionResult> Grade([FromBody] HomeworkGradeRequest req)
        {
            var (ok, msg) = await _svc.GradeSubmissionAsync(req.SubmissionId, req.Grade, req.Feedback);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpDelete] public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var (ok, msg) = await _svc.DeleteAsync(id);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }
    }

    public record HomeworkSubmitRequest(int HomeworkId, int StudentId, string? FilePath, string? Remarks);
    public record HomeworkGradeRequest(int SubmissionId, string Grade, string Feedback);

    // ══════════════════════════════════════════════════════════════════════════
    // 4.5 ASSIGNMENT
    // ══════════════════════════════════════════════════════════════════════════
    public class AssignmentController : BaseController
    {
        private readonly IAssignmentService _svc;
        private readonly ITenantService _tenant;
        public AssignmentController(ICurrentUserService cur, IAssignmentService svc, ITenantService tenant) : base(cur) { _svc = svc; _tenant = tenant; }

        [HttpPost] public async Task<IActionResult> Create([FromBody] CreateAssignmentRequest req)
        {
            var (ok, msg, a) = await _svc.CreateAsync(req, UserName, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { msg, a }) : BadRequest(new { msg });
        }

        [HttpGet] public async Task<IActionResult> GetByClass([FromQuery] int classId)
            => Ok(await _svc.GetByClassAsync(classId, _tenant.GetTenantId() ?? 0));

        [HttpGet] public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var a = await _svc.GetByIdAsync(id);
            return a == null ? NotFound() : Ok(a);
        }

        [HttpGet] public async Task<IActionResult> GetSubmissions([FromQuery] int assignmentId)
            => Ok(await _svc.GetSubmissionsAsync(assignmentId));

        [HttpPost] public async Task<IActionResult> Submit([FromBody] AssignmentSubmitRequest req)
        {
            var (ok, msg) = await _svc.SubmitAsync(req.AssignmentId, req.StudentId, req.FilePath, req.Remarks, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpPost] public async Task<IActionResult> Grade([FromBody] AssignmentGradeRequest req)
        {
            var (ok, msg) = await _svc.GradeAsync(req.SubmissionId, req.Marks, req.Feedback);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpDelete] public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var (ok, msg) = await _svc.DeleteAsync(id);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }
    }

    public record AssignmentSubmitRequest(int AssignmentId, int StudentId, string? FilePath, string? Remarks);
    public record AssignmentGradeRequest(int SubmissionId, decimal Marks, string Feedback);

    // ══════════════════════════════════════════════════════════════════════════
    // 4.6 ONLINE CLASS
    // ══════════════════════════════════════════════════════════════════════════
    public class OnlineClassController : BaseController
    {
        private readonly IOnlineClassService _svc;
        private readonly ITenantService _tenant;
        public OnlineClassController(ICurrentUserService cur, IOnlineClassService svc, ITenantService tenant) : base(cur) { _svc = svc; _tenant = tenant; }

        [HttpPost] public async Task<IActionResult> Schedule([FromBody] OnlineClassDto dto)
        {
            var (ok, msg, oc) = await _svc.CreateAsync(dto, UserName, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { msg, oc }) : BadRequest(new { msg });
        }

        [HttpGet] public async Task<IActionResult> GetByClass([FromQuery] int classId)
            => Ok(await _svc.GetByClassAsync(classId, _tenant.GetTenantId() ?? 0));

        [HttpGet] public async Task<IActionResult> GetUpcoming()
            => Ok(await _svc.GetUpcomingAsync(_tenant.GetTenantId() ?? 0));

        [HttpGet] public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var oc = await _svc.GetByIdAsync(id);
            return oc == null ? NotFound() : Ok(oc);
        }

        [HttpPut] public async Task<IActionResult> Update([FromBody] OnlineClassDto dto)
        {
            var (ok, msg) = await _svc.UpdateAsync(dto);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpPut] public async Task<IActionResult> UpdateStatus([FromQuery] int id, [FromQuery] string status)
        {
            var (ok, msg) = await _svc.UpdateStatusAsync(id, status);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpDelete] public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var (ok, msg) = await _svc.DeleteAsync(id);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 4.7 SYLLABUS TRACKING
    // ══════════════════════════════════════════════════════════════════════════
    public class SyllabusController : BaseController
    {
        private readonly ISyllabusService _svc;
        private readonly ITenantService _tenant;
        public SyllabusController(ICurrentUserService cur, ISyllabusService svc, ITenantService tenant) : base(cur) { _svc = svc; _tenant = tenant; }

        [HttpPost] public async Task<IActionResult> AddChapter([FromBody] SyllabusChapterDto dto)
        {
            var (ok, msg, c) = await _svc.AddChapterAsync(dto, UserName, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { msg, c }) : BadRequest(new { msg });
        }

        [HttpGet] public async Task<IActionResult> GetChapters([FromQuery] int subjectId, [FromQuery] int classId)
            => Ok(await _svc.GetChaptersAsync(subjectId, classId, _tenant.GetTenantId() ?? 0));

        [HttpPut] public async Task<IActionResult> UpdateProgress([FromQuery] int chapterId, [FromQuery] int completedPeriods)
        {
            var (ok, msg) = await _svc.UpdateProgressAsync(chapterId, completedPeriods, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpDelete] public async Task<IActionResult> DeleteChapter([FromQuery] int id)
        {
            var (ok, msg) = await _svc.DeleteChapterAsync(id);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpPost] public async Task<IActionResult> AddLessonPlan([FromBody] LessonPlanDto dto)
        {
            var (ok, msg, p) = await _svc.AddLessonPlanAsync(dto, UserName, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { msg, p }) : BadRequest(new { msg });
        }

        [HttpGet] public async Task<IActionResult> GetLessonsByChapter([FromQuery] int chapterId)
            => Ok(await _svc.GetLessonPlansByChapterAsync(chapterId));

        [HttpGet] public async Task<IActionResult> GetLessonsByDate([FromQuery] int classId, [FromQuery] DateTime date)
            => Ok(await _svc.GetLessonPlansByDateAsync(classId, date, _tenant.GetTenantId() ?? 0));

        [HttpPut] public async Task<IActionResult> UpdateLessonStatus([FromQuery] int id, [FromQuery] string status, [FromQuery] string? notes = null)
        {
            var (ok, msg) = await _svc.UpdateLessonPlanStatusAsync(id, status, notes);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }
    }
}
