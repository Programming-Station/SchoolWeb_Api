using School_DTOs;
using School_DTOs.Finance;

namespace School.Services.Interfaces
{
    public interface IAccountingService
    {
        // Chart of Accounts
        Task<APIResponse<List<CoaAccountDto>>> GetChartOfAccountsAsync(int schoolId, string? accountType = null, bool? isActive = null, string? search = null);
        Task<APIResponse<CoaAccountDto>> GetAccountByIdAsync(int schoolId, int id);
        Task<APIResponse<CoaAccountDto>> CreateAccountAsync(int schoolId, CreateCoaAccountDto dto, string user);
        Task<APIResponse<CoaAccountDto>> UpdateAccountAsync(int schoolId, int id, CreateCoaAccountDto dto, string user);
        Task<APIResponse<bool>> DeactivateAccountAsync(int schoolId, int accountId, string user);
        Task<APIResponse<bool>> DeleteAccountAsync(int schoolId, int accountId, string user);
        Task<APIResponse<byte[]>> ExportAccountsAsync(int schoolId, string? accountType = null, bool? isActive = null, string? search = null);

        // Journal Entries & Maker Checker
        Task<APIResponse<List<JournalEntryDto>>> GetJournalEntriesAsync(int schoolId, DateTime? fromDate, DateTime? toDate, string? status = null, string? search = null);
        Task<APIResponse<JournalEntryDto>> GetJournalEntryByIdAsync(int schoolId, int id);
        Task<APIResponse<JournalEntryDto>> PostJournalEntryAsync(int schoolId, CreateJournalEntryDto dto, string user);
        Task<APIResponse<JournalEntryDto>> UpdateJournalEntryAsync(int schoolId, int id, CreateJournalEntryDto dto, string user);
        Task<APIResponse<bool>> DeleteJournalEntryAsync(int schoolId, int entryId, string user);
        Task<APIResponse<bool>> SubmitJournalForApprovalAsync(int schoolId, int entryId, string user);
        Task<APIResponse<bool>> ApproveJournalEntryAsync(int schoolId, int entryId, string user);
        Task<APIResponse<bool>> RejectJournalEntryAsync(int schoolId, int entryId, string notes, string user);
        Task<APIResponse<byte[]>> ExportJournalEntriesAsync(int schoolId, DateTime? fromDate, DateTime? toDate, string? status = null, string? search = null);

        // Bank / Cash book & Reconciliation & Cheque Books
        Task<APIResponse<List<CashBankTransactionDto>>> GetBankTransactionsAsync(int schoolId, int accountId, bool? reconciled);
        Task<APIResponse<bool>> ReconcileTransactionAsync(int schoolId, int txnId, DateTime reconciledDate, string user);

        // Cheque Books
        Task<APIResponse<List<ChequeBookDto>>> GetChequeBooksAsync(int schoolId, int? accountId = null, bool? isExhausted = null);
        Task<APIResponse<ChequeBookDto>> GetChequeBookByIdAsync(int schoolId, int id);
        Task<APIResponse<ChequeBookDto>> CreateChequeBookAsync(int schoolId, CreateChequeBookDto dto, string user);
        Task<APIResponse<ChequeBookDto>> UpdateChequeBookAsync(int schoolId, int id, CreateChequeBookDto dto, string user);
        Task<APIResponse<bool>> DeleteChequeBookAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportChequeBooksAsync(int schoolId, int? accountId = null, bool? isExhausted = null);

        // Budgets
        Task<APIResponse<List<BudgetPlanDto>>> GetBudgetPlansAsync(int schoolId, string? financialYear = null, int? departmentId = null, int? accountId = null);
        Task<APIResponse<BudgetPlanDto>> GetBudgetPlanByIdAsync(int schoolId, int id);
        Task<APIResponse<BudgetPlanDto>> CreateBudgetPlanAsync(int schoolId, CreateBudgetPlanDto dto, string user);
        Task<APIResponse<BudgetPlanDto>> UpdateBudgetPlanAsync(int schoolId, int id, CreateBudgetPlanDto dto, string user);
        Task<APIResponse<bool>> DeleteBudgetPlanAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportBudgetPlansAsync(int schoolId, string? financialYear = null, int? departmentId = null, int? accountId = null);

        // Cost Centers
        Task<APIResponse<List<CostCenterDto>>> GetCostCentersAsync(int schoolId, bool? isActive = null, string? search = null);
        Task<APIResponse<CostCenterDto>> GetCostCenterByIdAsync(int schoolId, int id);
        Task<APIResponse<CostCenterDto>> CreateCostCenterAsync(int schoolId, CreateCostCenterDto dto, string user);
        Task<APIResponse<CostCenterDto>> UpdateCostCenterAsync(int schoolId, int id, CreateCostCenterDto dto, string user);
        Task<APIResponse<bool>> DeleteCostCenterAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportCostCentersAsync(int schoolId, bool? isActive = null, string? search = null);

        // Financial Years
        Task<APIResponse<List<FinancialYearDto>>> GetFinancialYearsAsync(int schoolId, bool? isClosed = null, bool? isLocked = null, string? search = null);
        Task<APIResponse<FinancialYearDto>> GetFinancialYearByIdAsync(int schoolId, int id);
        Task<APIResponse<FinancialYearDto>> CreateFinancialYearAsync(int schoolId, CreateFinancialYearDto dto, string user);
        Task<APIResponse<FinancialYearDto>> UpdateFinancialYearAsync(int schoolId, int id, CreateFinancialYearDto dto, string user);
        Task<APIResponse<bool>> DeleteFinancialYearAsync(int schoolId, int id, string user);
        Task<APIResponse<bool>> UpdateYearStateAsync(int schoolId, int id, bool isClosed, bool isLocked, string user);
        Task<APIResponse<bool>> CarryForwardBalancesAsync(int schoolId, int fromYearId, int toYearId, string user);
        Task<APIResponse<byte[]>> ExportFinancialYearsAsync(int schoolId, bool? isClosed = null, bool? isLocked = null, string? search = null);

        // Tax Configs
        Task<APIResponse<List<TaxConfigDto>>> GetTaxConfigsAsync(int schoolId, bool? isActive = null, string? search = null);
        Task<APIResponse<TaxConfigDto>> GetTaxConfigByIdAsync(int schoolId, int id);
        Task<APIResponse<TaxConfigDto>> CreateTaxConfigAsync(int schoolId, SaveTaxConfigDto dto, string user);
        Task<APIResponse<TaxConfigDto>> UpdateTaxConfigAsync(int schoolId, int id, SaveTaxConfigDto dto, string user);
        Task<APIResponse<bool>> DeleteTaxConfigAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportTaxConfigsAsync(int schoolId, bool? isActive = null, string? search = null);

        // Financial Reports
        Task<APIResponse<List<TrialBalanceRowDto>>> GetTrialBalanceAsync(int schoolId, DateTime date);
        Task<APIResponse<LedgerStatementDto>> GetLedgerStatementAsync(int schoolId, int accountId, DateTime fromDate, DateTime toDate);
        Task<APIResponse<BalanceSheetDto>> GetBalanceSheetAsync(int schoolId, DateTime date);
        Task<APIResponse<ProfitLossDto>> GetProfitLossAsync(int schoolId, DateTime fromDate, DateTime toDate);
        Task<APIResponse<CashFlowDto>> GetCashFlowStatementAsync(int schoolId, DateTime fromDate, DateTime toDate);
        Task<APIResponse<List<AgeingReportRowDto>>> GetReceivablesAgeingAsync(int schoolId, string type);
        Task<APIResponse<DashboardSummaryDto>> GetDashboardSummaryAsync(int schoolId);

        // AI Features
        Task<APIResponse<List<CashFlowForecastRowDto>>> GetAiCashFlowForecastAsync(int schoolId);
        Task<APIResponse<List<AnomalyRecordDto>>> DetectExpenseAnomaliesAsync(int schoolId);
        Task<APIResponse<List<string>>> ScanDuplicateVouchersAsync(int schoolId);

        // Bulk processing
        Task<APIResponse<bool>> BulkInsertJournalsAsync(int schoolId, List<CreateJournalEntryDto> dtos, string user);
    }
}
