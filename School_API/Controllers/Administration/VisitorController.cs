using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School_DTOs.Administration;

namespace School_API.Controllers.Administration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VisitorController : BaseController
    {
        private readonly IVisitorService _svc;
        private readonly ITenantService _tenant;

        public VisitorController(IVisitorService svc, ITenantService tenant, ICurrentUserService cur) : base(cur)
        {
            _svc = svc;
            _tenant = tenant;
        }

        private int SchoolId => _tenant.GetTenantId() ?? 1;

        [HttpGet]
        public async Task<IActionResult> GetVisitors([FromQuery] VisitorFilterDto filter)
        {
            var r = await _svc.GetVisitorsAsync(filter, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVisitorById(int id)
        {
            var r = await _svc.GetVisitorByIdAsync(id, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CheckInVisitor([FromBody] CreateVisitorDto dto)
        {
            var r = await _svc.CheckInVisitorAsync(dto, UserId, SchoolId);
            return StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> CheckOutVisitor(int id)
        {
            var r = await _svc.CheckOutVisitorAsync(id, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisitor(int id)
        {
            var r = await _svc.DeleteVisitorAsync(id, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }
    }
}
