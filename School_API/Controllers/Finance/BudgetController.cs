using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School_DTOs.Finance;

namespace School_API.Controllers.Finance
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BudgetController : BaseController
    {
        private readonly IAccountingService _svc;
        private readonly ITenantService _tenant;

        public BudgetController(IAccountingService svc, ITenantService tenant, ICurrentUserService cur) : base(cur)
        {
            _svc = svc;
            _tenant = tenant;
        }

        private int SchoolId => _tenant.GetTenantId() ?? 1;

        [HttpGet]
        public async Task<IActionResult> GetBudgetPlans([FromQuery] string? financialYear, [FromQuery] int? departmentId, [FromQuery] int? accountId)
        {
            var r = await _svc.GetBudgetPlansAsync(SchoolId, financialYear, departmentId, accountId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBudgetPlanById(int id)
        {
            var r = await _svc.GetBudgetPlanByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBudgetPlan([FromBody] CreateBudgetPlanDto dto)
        {
            var r = await _svc.CreateBudgetPlanAsync(SchoolId, dto, UserName);
            return StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBudgetPlan(int id, [FromBody] CreateBudgetPlanDto dto)
        {
            var r = await _svc.UpdateBudgetPlanAsync(SchoolId, id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudgetPlan(int id)
        {
            var r = await _svc.DeleteBudgetPlanAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportBudgetPlans([FromQuery] string? financialYear, [FromQuery] int? departmentId, [FromQuery] int? accountId)
        {
            var r = await _svc.ExportBudgetPlansAsync(SchoolId, financialYear, departmentId, accountId);
            if (!r.Success || r.Data == null)
            {
                return BadRequest(r);
            }
            return File(r.Data, "text/csv", "budget_plans.csv");
        }
    }
}
