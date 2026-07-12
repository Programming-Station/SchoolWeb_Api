using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces;
using School.Infrastructure.Interfaces;
using School_API.Common.Interface;
using School_DTOs.Finance;

namespace School_API.Controllers.Finance
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountingController : BaseController
    {
        private readonly IAccountingService _svc;
        private readonly ITenantService _tenantSvc;

        public AccountingController(IAccountingService svc, ITenantService tenantSvc, ICurrentUserService cur) : base(cur)
        {
            _svc = svc;
            _tenantSvc = tenantSvc;
        }

        private int SchoolId => _tenantSvc.GetTenantId() ?? 1;

        [HttpGet]
        public async Task<IActionResult> GetChartOfAccounts()
        {
            var r = await _svc.GetChartOfAccountsAsync(SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateCoaAccountDto dto)
        {
            var r = await _svc.CreateAccountAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{accountId}")]
        public async Task<IActionResult> DeactivateAccount(int accountId)
        {
            var r = await _svc.DeactivateAccountAsync(SchoolId, accountId, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetJournalEntries([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var r = await _svc.GetJournalEntriesAsync(SchoolId, fromDate, toDate);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> PostJournalEntry([FromBody] CreateJournalEntryDto dto)
        {
            var r = await _svc.PostJournalEntryAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{entryId}")]
        public async Task<IActionResult> DeleteJournalEntry(int entryId)
        {
            var r = await _svc.DeleteJournalEntryAsync(SchoolId, entryId, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetBankTransactions([FromQuery] int accountId, [FromQuery] bool? reconciled)
        {
            var r = await _svc.GetBankTransactionsAsync(SchoolId, accountId, reconciled);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> ReconcileTransaction([FromQuery] int txnId, [FromQuery] DateTime reconciledDate)
        {
            var r = await _svc.ReconcileTransactionAsync(SchoolId, txnId, reconciledDate, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetBudgetPlans([FromQuery] string financialYear)
        {
            var r = await _svc.GetBudgetPlansAsync(SchoolId, financialYear);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBudgetPlan([FromBody] CreateBudgetPlanDto dto)
        {
            var r = await _svc.CreateBudgetPlanAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetTrialBalance([FromQuery] DateTime date)
        {
            var r = await _svc.GetTrialBalanceAsync(SchoolId, date);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetLedgerStatement([FromQuery] int accountId, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            var r = await _svc.GetLedgerStatementAsync(SchoolId, accountId, fromDate, toDate);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }
    }
}
