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

        // ─── Chart of Accounts (COA) ───
        [HttpGet]
        public async Task<IActionResult> GetChartOfAccounts([FromQuery] string? accountType, [FromQuery] bool? isActive, [FromQuery] string? search)
        {
            var r = await _svc.GetChartOfAccountsAsync(SchoolId, accountType, isActive, search);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var r = await _svc.GetAccountByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateCoaAccountDto dto)
        {
            var r = await _svc.CreateAccountAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] CreateCoaAccountDto dto)
        {
            var r = await _svc.UpdateAccountAsync(SchoolId, id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{accountId}")]
        public async Task<IActionResult> DeactivateAccount(int accountId)
        {
            var r = await _svc.DeactivateAccountAsync(SchoolId, accountId, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{accountId}")]
        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            var r = await _svc.DeleteAccountAsync(SchoolId, accountId, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportAccounts([FromQuery] string? accountType, [FromQuery] bool? isActive, [FromQuery] string? search)
        {
            var r = await _svc.ExportAccountsAsync(SchoolId, accountType, isActive, search);
            if (!r.Success || r.Data == null) return BadRequest(r);
            return File(r.Data, "text/csv", "coa_accounts.csv");
        }

        // ─── Journal Entries ───
        [HttpGet]
        public async Task<IActionResult> GetJournalEntries([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string? status, [FromQuery] string? search)
        {
            var r = await _svc.GetJournalEntriesAsync(SchoolId, fromDate, toDate, status, search);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJournalEntryById(int id)
        {
            var r = await _svc.GetJournalEntryByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> PostJournalEntry([FromBody] CreateJournalEntryDto dto)
        {
            var r = await _svc.PostJournalEntryAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJournalEntry(int id, [FromBody] CreateJournalEntryDto dto)
        {
            var r = await _svc.UpdateJournalEntryAsync(SchoolId, id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{entryId}")]
        public async Task<IActionResult> DeleteJournalEntry(int entryId)
        {
            var r = await _svc.DeleteJournalEntryAsync(SchoolId, entryId, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportJournalEntries([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string? status, [FromQuery] string? search)
        {
            var r = await _svc.ExportJournalEntriesAsync(SchoolId, fromDate, toDate, status, search);
            if (!r.Success || r.Data == null) return BadRequest(r);
            return File(r.Data, "text/csv", "journal_entries.csv");
        }

        // ─── Bank Transactions ───
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

        // ─── Cheque Books ───
        [HttpGet]
        public async Task<IActionResult> GetChequeBooks([FromQuery] int? accountId, [FromQuery] bool? isExhausted)
        {
            var r = await _svc.GetChequeBooksAsync(SchoolId, accountId, isExhausted);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChequeBookById(int id)
        {
            var r = await _svc.GetChequeBookByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChequeBook([FromBody] CreateChequeBookDto dto)
        {
            var r = await _svc.CreateChequeBookAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChequeBook(int id, [FromBody] CreateChequeBookDto dto)
        {
            var r = await _svc.UpdateChequeBookAsync(SchoolId, id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChequeBook(int id)
        {
            var r = await _svc.DeleteChequeBookAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportChequeBooks([FromQuery] int? accountId, [FromQuery] bool? isExhausted)
        {
            var r = await _svc.ExportChequeBooksAsync(SchoolId, accountId, isExhausted);
            if (!r.Success || r.Data == null) return BadRequest(r);
            return File(r.Data, "text/csv", "cheque_books.csv");
        }

        // ─── Budgets ───
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
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
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
            if (!r.Success || r.Data == null) return BadRequest(r);
            return File(r.Data, "text/csv", "budget_plans.csv");
        }

        // ─── Cost Centers ───
        [HttpGet]
        public async Task<IActionResult> GetCostCenters([FromQuery] bool? isActive, [FromQuery] string? search)
        {
            var r = await _svc.GetCostCentersAsync(SchoolId, isActive, search);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCostCenterById(int id)
        {
            var r = await _svc.GetCostCenterByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCostCenter([FromBody] CreateCostCenterDto dto)
        {
            var r = await _svc.CreateCostCenterAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCostCenter(int id, [FromBody] CreateCostCenterDto dto)
        {
            var r = await _svc.UpdateCostCenterAsync(SchoolId, id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCostCenter(int id)
        {
            var r = await _svc.DeleteCostCenterAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportCostCenters([FromQuery] bool? isActive, [FromQuery] string? search)
        {
            var r = await _svc.ExportCostCentersAsync(SchoolId, isActive, search);
            if (!r.Success || r.Data == null) return BadRequest(r);
            return File(r.Data, "text/csv", "cost_centers.csv");
        }

        // ─── Financial Years ───
        [HttpGet]
        public async Task<IActionResult> GetFinancialYears([FromQuery] bool? isClosed, [FromQuery] bool? isLocked, [FromQuery] string? search)
        {
            var r = await _svc.GetFinancialYearsAsync(SchoolId, isClosed, isLocked, search);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFinancialYearById(int id)
        {
            var r = await _svc.GetFinancialYearByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFinancialYear([FromBody] CreateFinancialYearDto dto)
        {
            var r = await _svc.CreateFinancialYearAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFinancialYear(int id, [FromBody] CreateFinancialYearDto dto)
        {
            var r = await _svc.UpdateFinancialYearAsync(SchoolId, id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinancialYear(int id)
        {
            var r = await _svc.DeleteFinancialYearAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportFinancialYears([FromQuery] bool? isClosed, [FromQuery] bool? isLocked, [FromQuery] string? search)
        {
            var r = await _svc.ExportFinancialYearsAsync(SchoolId, isClosed, isLocked, search);
            if (!r.Success || r.Data == null) return BadRequest(r);
            return File(r.Data, "text/csv", "financial_years.csv");
        }

        // ─── Tax Configs ───
        [HttpGet]
        public async Task<IActionResult> GetTaxConfigs([FromQuery] bool? isActive, [FromQuery] string? search)
        {
            var r = await _svc.GetTaxConfigsAsync(SchoolId, isActive, search);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaxConfigById(int id)
        {
            var r = await _svc.GetTaxConfigByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaxConfig([FromBody] SaveTaxConfigDto dto)
        {
            var r = await _svc.CreateTaxConfigAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaxConfig(int id, [FromBody] SaveTaxConfigDto dto)
        {
            var r = await _svc.UpdateTaxConfigAsync(SchoolId, id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaxConfig(int id)
        {
            var r = await _svc.DeleteTaxConfigAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportTaxConfigs([FromQuery] bool? isActive, [FromQuery] string? search)
        {
            var r = await _svc.ExportTaxConfigsAsync(SchoolId, isActive, search);
            if (!r.Success || r.Data == null) return BadRequest(r);
            return File(r.Data, "text/csv", "tax_configs.csv");
        }

        // ─── Reporting & Transactions ───
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
