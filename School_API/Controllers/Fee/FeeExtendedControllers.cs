using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Fee;
using School_API.Common.Interface;

namespace School_API.Controllers.Fee
{
    // ══════════════════════════════════════════════════════════════════════════
    // 6.4 FEE FINE
    // ══════════════════════════════════════════════════════════════════════════
    public class FeeFineController : BaseController
    {
        private readonly IFeeFineService _svc;
        private readonly ITenantService _tenant;
        public FeeFineController(ICurrentUserService cur, IFeeFineService svc, ITenantService tenant) : base(cur) { _svc = svc; _tenant = tenant; }

        [HttpPost] public async Task<IActionResult> Apply([FromBody] FeeFineDto dto)
        {
            var (ok, msg, fine) = await _svc.ApplyFineAsync(dto, UserName, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { msg, fine }) : BadRequest(new { msg });
        }

        [HttpGet] public async Task<IActionResult> GetByStudent([FromQuery] int studentId)
            => Ok(await _svc.GetByStudentAsync(studentId, _tenant.GetTenantId() ?? 0));

        [HttpGet] public async Task<IActionResult> GetPending()
            => Ok(await _svc.GetPendingAsync(_tenant.GetTenantId() ?? 0));

        [HttpPut] public async Task<IActionResult> Waive([FromQuery] int id)
        {
            var (ok, msg) = await _svc.WaiveFineAsync(id);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpPut] public async Task<IActionResult> MarkPaid([FromQuery] int id)
        {
            var (ok, msg) = await _svc.MarkPaidAsync(id);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpDelete] public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var (ok, msg) = await _svc.DeleteAsync(id);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 6.5 SCHOLARSHIP
    // ══════════════════════════════════════════════════════════════════════════
    public class ScholarshipController : BaseController
    {
        private readonly IScholarshipService _svc;
        private readonly ITenantService _tenant;
        public ScholarshipController(ICurrentUserService cur, IScholarshipService svc, ITenantService tenant) : base(cur) { _svc = svc; _tenant = tenant; }

        [HttpPost] public async Task<IActionResult> Create([FromBody] ScholarshipDto dto)
        {
            var (ok, msg, s) = await _svc.CreateAsync(dto, UserName, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { msg, s }) : BadRequest(new { msg });
        }

        [HttpGet] public async Task<IActionResult> GetByStudent([FromQuery] int studentId)
            => Ok(await _svc.GetByStudentAsync(studentId, _tenant.GetTenantId() ?? 0));

        [HttpGet] public async Task<IActionResult> GetActive()
            => Ok(await _svc.GetActiveAsync(_tenant.GetTenantId() ?? 0));

        [HttpPut] public async Task<IActionResult> Update([FromBody] ScholarshipDto dto)
        {
            var (ok, msg) = await _svc.UpdateAsync(dto);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpPut] public async Task<IActionResult> Revoke([FromQuery] int id)
        {
            var (ok, msg) = await _svc.RevokeAsync(id);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 6.6 FEE REFUND
    // ══════════════════════════════════════════════════════════════════════════
    public class FeeRefundController : BaseController
    {
        private readonly IFeeRefundService _svc;
        private readonly ITenantService _tenant;
        public FeeRefundController(ICurrentUserService cur, IFeeRefundService svc, ITenantService tenant) : base(cur) { _svc = svc; _tenant = tenant; }

        [HttpPost] public async Task<IActionResult> Request([FromBody] FeeRefundDto dto)
        {
            var (ok, msg, r) = await _svc.RequestRefundAsync(dto, UserName, _tenant.GetTenantId() ?? 0);
            return ok ? Ok(new { msg, r }) : BadRequest(new { msg });
        }

        [HttpGet] public async Task<IActionResult> GetByStudent([FromQuery] int studentId)
            => Ok(await _svc.GetByStudentAsync(studentId, _tenant.GetTenantId() ?? 0));

        [HttpGet] public async Task<IActionResult> GetPending()
            => Ok(await _svc.GetPendingAsync(_tenant.GetTenantId() ?? 0));

        [HttpPut] public async Task<IActionResult> Approve([FromQuery] int id)
        {
            var (ok, msg) = await _svc.ApproveAsync(id, UserName);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpPut] public async Task<IActionResult> Process([FromQuery] int id, [FromQuery] string? refundRef = null)
        {
            var (ok, msg) = await _svc.ProcessAsync(id, UserName, refundRef);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpPut] public async Task<IActionResult> Reject([FromQuery] int id)
        {
            var (ok, msg) = await _svc.RejectAsync(id, UserName);
            return ok ? Ok(new { msg }) : BadRequest(new { msg });
        }

        [HttpGet] public async Task<IActionResult> GetTotalRefunded([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var total = await _svc.GetTotalRefundedAsync(from, to, _tenant.GetTenantId() ?? 0);
            return Ok(new { from, to, totalRefunded = total });
        }
    }
}
