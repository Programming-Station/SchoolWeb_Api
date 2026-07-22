using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces.Academic;
using School_API.Common.Interface;
using School_DTOs.Academic;

namespace School_API.Controllers.Academic
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : BaseController
    {
        private readonly ISubjectService _svc;
        public SubjectController(ISubjectService svc, ICurrentUserService cur) : base(cur) { _svc = svc; }

        [HttpGet] public async Task<IActionResult> GetAll() { var r = await _svc.GetAllAsync(); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) { var r = await _svc.GetByIdAsync(id); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPost] public async Task<IActionResult> Create([FromBody] CreateSubjectDto m) { var r = await _svc.CreateAsync(m, UserName); return StatusCode((int)r.StatusCode, r); }
        [HttpPut] public async Task<IActionResult> Update([FromBody] UpdateSubjectDto m) { var r = await _svc.UpdateAsync(m.Id, m, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var r = await _svc.DeleteAsync(id, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
    }
}

