using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using School.Models.Finance;
using School.Services.Interfaces;
using School.Infrastructure.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers.Finance
{
    public class IncomeController : BaseController
    {
        private readonly IIncomeService _incomeService;
        private readonly ITenantService _tenantService;

        public IncomeController(
            IIncomeService incomeService,
            ITenantService tenantService,
            ICurrentUserService currentUser) : base(currentUser)
        {
            _incomeService = incomeService;
            _tenantService = tenantService;
        }

        private int SchoolId => _tenantService.GetTenantId() ?? 1;

        [HttpPost]
        public async Task<IActionResult> CreateIncome([FromBody] IncomeModel model)
        {
            var result = await _incomeService.CreateIncomeAsync(model, SchoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIncomeById(int id)
        {
            var result = await _incomeService.GetIncomeByIdAsync(id, SchoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetIncomes()
        {
            var result = await _incomeService.GetAllIncomesAsync(SchoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateIncome([FromBody] IncomeModel model)
        {
            var result = await _incomeService.UpdateIncomeAsync(model, SchoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncome(int id)
        {
            var result = await _incomeService.DeleteIncomeAsync(id, SchoolId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
