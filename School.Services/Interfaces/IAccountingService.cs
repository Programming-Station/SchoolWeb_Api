using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using School_DTOs;
using School_DTOs.Finance;

namespace School.Services.Interfaces
{
    public interface IAccountingService
    {
        // Chart of Accounts
        Task<APIResponse<List<CoaAccountDto>>> GetChartOfAccountsAsync(int schoolId);
        Task<APIResponse<CoaAccountDto>> CreateAccountAsync(int schoolId, CreateCoaAccountDto dto, string user);
        Task<APIResponse<bool>> DeactivateAccountAsync(int schoolId, int accountId, string user);

        // Journal Entries
        Task<APIResponse<List<JournalEntryDto>>> GetJournalEntriesAsync(int schoolId, DateTime? fromDate, DateTime? toDate);
        Task<APIResponse<JournalEntryDto>> PostJournalEntryAsync(int schoolId, CreateJournalEntryDto dto, string user);
        Task<APIResponse<bool>> DeleteJournalEntryAsync(int schoolId, int entryId, string user);

        // Bank / Cash book & Reconciliation
        Task<APIResponse<List<CashBankTransactionDto>>> GetBankTransactionsAsync(int schoolId, int accountId, bool? reconciled);
        Task<APIResponse<bool>> ReconcileTransactionAsync(int schoolId, int txnId, DateTime reconciledDate, string user);

        // Budgets
        Task<APIResponse<List<BudgetPlanDto>>> GetBudgetPlansAsync(int schoolId, string financialYear);
        Task<APIResponse<BudgetPlanDto>> CreateBudgetPlanAsync(int schoolId, CreateBudgetPlanDto dto, string user);

        // Financial Reports
        Task<APIResponse<List<TrialBalanceRowDto>>> GetTrialBalanceAsync(int schoolId, DateTime date);
        Task<APIResponse<LedgerStatementDto>> GetLedgerStatementAsync(int schoolId, int accountId, DateTime fromDate, DateTime toDate);
    }
}
