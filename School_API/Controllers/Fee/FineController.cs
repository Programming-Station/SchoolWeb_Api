using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School_DTOs.Fee;
using System.Threading.Tasks;
using School.Infrastructure.Interfaces;

namespace School_API.Controllers.Fee
{
    public class FineController : BaseController
    {
        private readonly IFineCalculationService _fineService;
        private readonly ITenantService _tenant;

        public FineController(
            ICurrentUserService currentUser,
            IFineCalculationService fineService,
            ITenantService tenant)
            : base(currentUser)
        {
            _fineService = fineService;
            _tenant = tenant;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRule([FromBody] FineRuleDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var (success, message, rule) = await _fineService.CreateRuleAsync(dto, UserName, schoolId);
            return success ? Ok(new { message, rule }) : BadRequest(new { message });
        }

        [HttpGet]
        public async Task<IActionResult> GetRules()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var rules = await _fineService.GetAllRulesAsync(schoolId);
            return Ok(rules);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRule([FromBody] FineRuleDto dto)
        {
            var (success, message) = await _fineService.UpdateRuleAsync(dto);
            return success ? Ok(new { message }) : BadRequest(new { message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRule([FromRoute] int id)
        {
            var (success, message) = await _fineService.DeleteRuleAsync(id);
            return success ? Ok(new { message }) : BadRequest(new { message });
        }

        [HttpGet]
        public async Task<IActionResult> GetFines()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var fines = await _fineService.GetFinesAsync(schoolId);
            return Ok(fines);
        }

        [HttpPost]
        public async Task<IActionResult> WaiveFine([FromBody] WaiveFineRequest request)
        {
            var (success, message) = await _fineService.WaiveFineAsync(request.FineId, request.Reason, UserName);
            return success ? Ok(new { message }) : BadRequest(new { message });
        }

        [HttpPost]
        public async Task<IActionResult> RunCalculation()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var (success, message) = await _fineService.RunDailyFineCalculationAsync(schoolId);
            return success ? Ok(new { message }) : BadRequest(new { message });
        }
    }

    public class WaiveFineRequest
    {
        public int FineId { get; set; }
        public string Reason { get; set; } = null!;
    }
}
