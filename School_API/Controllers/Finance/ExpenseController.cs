using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using School.Models.Finance;
using School.Services.Interfaces;
using School.Infrastructure.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers.Finance
{
    public class ExpenseController : BaseController
    {
        private readonly IExpenseService _expenseService;
        private readonly ITenantService _tenantService;

        public ExpenseController(
            IExpenseService expenseService,
            ITenantService tenantService,
            ICurrentUserService currentUser) : base(currentUser)
        {
            _expenseService = expenseService;
            _tenantService = tenantService;
        }

        private int SchoolId => _tenantService.GetTenantId() ?? 1;

        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] ExpenseModel model)
        {
            var result = await _expenseService.CreateExpenseAsync(model, SchoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenseById(int id)
        {
            var result = await _expenseService.GetExpenseByIdAsync(id, SchoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenses()
        {
            var result = await _expenseService.GetAllExpensesAsync(SchoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExpense([FromBody] ExpenseModel model)
        {
            var result = await _expenseService.UpdateExpenseAsync(model, SchoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var result = await _expenseService.DeleteExpenseAsync(id, SchoolId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
