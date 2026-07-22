using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces.Academic;
using School_API.Common.Interface;
using School_DTOs.Academic;
namespace School_API.Controllers.Academic
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : BaseController
    {
        private readonly IExamService _svc;
        public ExamController(IExamService svc, ICurrentUserService cur) : base(cur) { _svc = svc; }
        [HttpGet] public async Task<IActionResult> GetAll() { var r = await _svc.GetAllAsync(); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) { var r = await _svc.GetByIdAsync(id); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPost] public async Task<IActionResult> Create([FromBody] CreateExamDto m) { var r = await _svc.CreateAsync(m, UserName); return StatusCode((int)r.StatusCode, r); }
        [HttpPut] public async Task<IActionResult> Update([FromBody] UpdateExamDto m) { var r = await _svc.UpdateAsync(m.Id, m, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var r = await _svc.DeleteAsync(id, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPost("PublishResult")]
        public async Task<IActionResult> PublishResult([FromQuery] int examId)
        {
            var r = await _svc.PublishResultAsync(examId, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);

        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ExamResultController : BaseController
    {
        private readonly IExamResultService _svc;
        public ExamResultController(IExamResultService svc, ICurrentUserService cur) : base(cur) { _svc = svc; }
        [HttpGet("exam/{examId}")] public async Task<IActionResult> GetByExam(int examId) { var r = await _svc.GetAllByExamIdAsync(examId); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) { var r = await _svc.GetByIdAsync(id); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPost] public async Task<IActionResult> Create([FromBody] CreateExamResultDto m) { var r = await _svc.CreateAsync(m, UserName); return StatusCode((int)r.StatusCode, r); }
        [HttpPut] public async Task<IActionResult> Update([FromBody] UpdateExamResultDto m) { var r = await _svc.UpdateAsync(m.Id, m, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var r = await _svc.DeleteAsync(id, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
    }
}

