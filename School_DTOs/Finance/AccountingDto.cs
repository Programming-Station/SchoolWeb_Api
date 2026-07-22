namespace School_DTOs.Finance
{
    // CoaAccount DTOs
    public class CoaAccountDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string AccountType { get; set; } = "Asset";
        public int? ParentAccountId { get; set; }
        public string? ParentAccountName { get; set; }
        public decimal CurrentBalance { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class CreateCoaAccountDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string AccountType { get; set; } = "Asset";
        public int? ParentAccountId { get; set; }
        public bool IsActive { get; set; } = true;
    }

    // Financial Year DTOs
    public class FinancialYearDto
    {
        public int Id { get; set; }
        public string YearName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsClosed { get; set; }
        public bool IsLocked { get; set; }
    }

    public class CreateFinancialYearDto
    {
        public string YearName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    // Cost Center DTOs
    public class CostCenterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class CreateCostCenterDto
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }

    // Cheque Book DTOs
    public class ChequeBookDto
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public string BankAccountName { get; set; } = null!;
        public string SeriesPrefix { get; set; } = null!;
        public int StartChequeNo { get; set; }
        public int EndChequeNo { get; set; }
        public int NextChequeNo { get; set; }
        public bool IsExhausted { get; set; }
    }

    public class CreateChequeBookDto
    {
        public int BankAccountId { get; set; }
        public string SeriesPrefix { get; set; } = null!;
        public int StartChequeNo { get; set; }
        public int EndChequeNo { get; set; }
    }

    // Journal Entry DTOs
    public class JournalEntryDto
    {
        public int Id { get; set; }
        public string VoucherNo { get; set; } = null!;
        public DateTime EntryDate { get; set; }
        public string? Narration { get; set; }
        public string? Reference { get; set; }
        public string Source { get; set; } = "Manual";
        public string VoucherType { get; set; } = "Journal";
        public string Status { get; set; } = "Approved";
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? CostCenterId { get; set; }
        public string? CostCenterName { get; set; }
        public string? AttachmentUrl { get; set; }
        public bool IsPosted { get; set; }
        public List<JournalEntryLineDto> Lines { get; set; } = new();
    }

    public class JournalEntryLineDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public string AccountCode { get; set; } = null!;
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string? Description { get; set; }
    }

    public class CreateJournalEntryDto
    {
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;
        public string? Narration { get; set; }
        public string? Reference { get; set; }
        public string Source { get; set; } = "Manual";
        public string VoucherType { get; set; } = "Journal";
        public int? CostCenterId { get; set; }
        public string? AttachmentUrl { get; set; }
        public List<CreateJournalEntryLineDto> Lines { get; set; } = new();
    }

    public class CreateJournalEntryLineDto
    {
        public int AccountId { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string? Description { get; set; }
    }

    // CashBankTransaction DTOs
    public class CashBankTransactionDto
    {
        public int Id { get; set; }
        public int? JournalEntryId { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public string TransactionType { get; set; } = "Deposit";
        public string PaymentMode { get; set; } = "BankTransfer";
        public string? ReferenceNo { get; set; }
        public string? ChequeNo { get; set; }
        public int? CostCenterId { get; set; }
        public string? CostCenterName { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsReconciled { get; set; }
        public DateTime? ReconciledDate { get; set; }
        public DateTime? ClearedDate { get; set; }
    }

    // Budget DTOs
    public class BudgetPlanDto
    {
        public int Id { get; set; }
        public string FinancialYear { get; set; } = null!;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public int AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public decimal AllocatedAmount { get; set; }
        public decimal UtilizedAmount { get; set; }
    }

    public class CreateBudgetPlanDto
    {
        public string FinancialYear { get; set; } = null!;
        public int DepartmentId { get; set; }
        public int AccountId { get; set; }
        public decimal AllocatedAmount { get; set; }
    }

    // TaxConfig DTOs
    public class TaxConfigDto
    {
        public int Id { get; set; }
        public string TaxName { get; set; } = null!;
        public decimal Percentage { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class SaveTaxConfigDto
    {
        public string TaxName { get; set; } = null!;
        public decimal Percentage { get; set; }
        public int AccountId { get; set; }
        public bool IsActive { get; set; } = true;
    }

    // Reporting DTOs
    public class TrialBalanceRowDto
    {
        public string AccountCode { get; set; } = null!;
        public string AccountName { get; set; } = null!;
        public string AccountType { get; set; } = null!;
        public decimal DebitBalance { get; set; }
        public decimal CreditBalance { get; set; }
    }

    public class BalanceSheetDto
    {
        public DateTime StatementDate { get; set; }
        public List<BalanceSheetRowDto> Assets { get; set; } = new();
        public decimal TotalAssets { get; set; }
        public List<BalanceSheetRowDto> Liabilities { get; set; } = new();
        public decimal TotalLiabilities { get; set; }
        public List<BalanceSheetRowDto> Equity { get; set; } = new();
        public decimal TotalEquity { get; set; }
    }

    public class BalanceSheetRowDto
    {
        public string AccountCode { get; set; } = null!;
        public string AccountName { get; set; } = null!;
        public decimal Balance { get; set; }
    }

    public class ProfitLossDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<ProfitLossRowDto> Incomes { get; set; } = new();
        public decimal TotalIncome { get; set; }
        public List<ProfitLossRowDto> Expenses { get; set; } = new();
        public decimal TotalExpense { get; set; }
        public decimal NetProfitOrLoss { get; set; }
    }

    public class ProfitLossRowDto
    {
        public string AccountCode { get; set; } = null!;
        public string AccountName { get; set; } = null!;
        public decimal Amount { get; set; }
    }

    public class CashFlowDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal BeginningCash { get; set; }
        public decimal EndingCash { get; set; }
        public decimal OperatingInflows { get; set; }
        public decimal OperatingOutflows { get; set; }
        public decimal InvestingInflows { get; set; }
        public decimal InvestingOutflows { get; set; }
        public decimal NetCashChange { get; set; }
    }

    public class LedgerStatementDto
    {
        public string AccountName { get; set; } = null!;
        public string AccountCode { get; set; } = null!;
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public List<LedgerStatementLineDto> Statements { get; set; } = new();
    }

    public class LedgerStatementLineDto
    {
        public DateTime Date { get; set; }
        public string VoucherNo { get; set; } = null!;
        public string Narration { get; set; } = null!;
        public string Reference { get; set; } = null!;
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal RunningBalance { get; set; }
    }

    public class AgeingReportRowDto
    {
        public int TargetId { get; set; }
        public string TargetName { get; set; } = null!;
        public decimal BalanceDue { get; set; }
        public decimal Age0to30 { get; set; }
        public decimal Age31to60 { get; set; }
        public decimal Age61to90 { get; set; }
        public decimal AgeOver90 { get; set; }
    }

    // AI Prediction / Forecaster DTOs
    public class CashFlowForecastRowDto
    {
        public DateTime Date { get; set; }
        public decimal ProjectedInflow { get; set; }
        public decimal ProjectedOutflow { get; set; }
        public decimal NetBalance { get; set; }
        public double ConfidenceScore { get; set; }
    }

    public class AnomalyRecordDto
    {
        public int Id { get; set; }
        public string VoucherNo { get; set; } = null!;
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; } = null!;
        public double AnomalyScore { get; set; }
    }

    public class DashboardSummaryDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal TodayCollection { get; set; }
        public decimal CashInHand { get; set; }
        public decimal BankBalance { get; set; }
        public decimal OutstandingReceivables { get; set; }
        public decimal OutstandingPayables { get; set; }
        public int PendingApprovalsCount { get; set; }
    }
}
