using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School_DTOs.Administration;

namespace School_API.Controllers.Administration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ComplaintController : BaseController
    {
        private readonly IComplaintService _svc;
        private readonly ITenantService _tenant;

        public ComplaintController(IComplaintService svc, ITenantService tenant, ICurrentUserService cur) : base(cur)
        {
            _svc = svc;
            _tenant = tenant;
        }

        private int SchoolId => _tenant.GetTenantId() ?? 1;

        [HttpGet]
        public async Task<IActionResult> GetComplaints([FromQuery] ComplaintFilterDto filter)
        {
            var r = await _svc.GetComplaintsAsync(filter, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComplaintById(int id)
        {
            var r = await _svc.GetComplaintByIdAsync(id, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComplaint([FromBody] CreateComplaintDto dto)
        {
            var r = await _svc.CreateComplaintAsync(dto, UserId, SchoolId);
            return StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComplaint(int id, [FromBody] ComplaintDto dto)
        {
            var r = await _svc.UpdateComplaintAsync(id, dto, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AssignComplaint(int id, [FromQuery] string assignedToUserId, [FromQuery] string assignedToName)
        {
            var r = await _svc.AssignComplaintAsync(id, assignedToUserId, assignedToName, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> ResolveComplaint(int id, [FromQuery] string resolutionDetails)
        {
            var r = await _svc.ResolveComplaintAsync(id, resolutionDetails, UserName, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> EscalateComplaint(int id, [FromQuery] string notes)
        {
            var r = await _svc.EscalateComplaintAsync(id, notes, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComplaint(int id)
        {
            var r = await _svc.DeleteComplaintAsync(id, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }
    }
}
