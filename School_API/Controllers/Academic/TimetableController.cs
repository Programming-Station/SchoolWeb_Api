using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces.Academic;
using School.Infrastructure.Interfaces;
using School_API.Common.Interface;
using School_DTOs.Academic;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace School_API.Controllers.Academic
{
    [Route("api/[controller]")][ApiController]
    public class TimetableController : BaseController
    {
        private readonly ITimetableSlotService _svc;
        private readonly ITimetableService _timetableSvc;
        private readonly ITenantService _tenant;

        public TimetableController(ITimetableSlotService svc, ITimetableService timetableSvc, ITenantService tenant, ICurrentUserService cur) : base(cur)
        {
            _svc = svc;
            _timetableSvc = timetableSvc;
            _tenant = tenant;
        }

        [HttpGet] public async Task<IActionResult> GetAll(){var r=await _svc.GetAllAsync();return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpGet("{id}")] public async Task<IActionResult> GetById(int id){var r=await _svc.GetByIdAsync(id);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpPost] public async Task<IActionResult> Create([FromBody]CreateTimetableSlotDto m){var r=await _svc.CreateAsync(m,UserName);return StatusCode((int)r.StatusCode,r);}
        [HttpPut] public async Task<IActionResult> Update([FromBody]UpdateTimetableSlotDto m){var r=await _svc.UpdateAsync(m.Id,m,UserName);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id){var r=await _svc.DeleteAsync(id,UserName);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}

        // Weekly Timetable Endpoints
        [HttpPost("SaveWeekly")]
        public async Task<IActionResult> SaveWeekly([FromBody] SaveTimetableRequest req)
        {
            var (ok, msg) = await _timetableSvc.SaveTimetableAsync(req, UserName, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { message = msg }) : BadRequest(new { message = msg });
        }

        [HttpGet("GetWeekly")]
        public async Task<IActionResult> GetWeekly([FromQuery] int classId, [FromQuery] int academicYearId)
        {
            var r = await _timetableSvc.GetWeeklyTimetableAsync(classId, academicYearId, _tenant.GetTenantId() ?? 0);
            return Ok(r);
        }

        [HttpGet("GetByClass")]
        public async Task<IActionResult> GetByClass([FromQuery] int classId, [FromQuery] int academicYearId)
        {
            var r = await _timetableSvc.GetByClassAsync(classId, academicYearId, _tenant.GetTenantId() ?? 0);
            return Ok(r);
        }
    }
}

