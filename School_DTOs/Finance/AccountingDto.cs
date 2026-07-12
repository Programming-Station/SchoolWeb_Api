using System;
using System.Collections.Generic;

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

    // Journal Entry DTOs
    public class JournalEntryDto
    {
        public int Id { get; set; }
        public string VoucherNo { get; set; } = null!;
        public DateTime EntryDate { get; set; }
        public string? Narration { get; set; }
        public string? Reference { get; set; }
        public string Source { get; set; } = "Manual";
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
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsReconciled { get; set; }
        public DateTime? ReconciledDate { get; set; }
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
}
