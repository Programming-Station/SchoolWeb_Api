using System;
using System.Collections.Generic;

namespace School_DTOs.Finance
{
    public class ExpenseLineDto
    {
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
    }

    public class ExpenseDto : BaseDto
    {
        public int Id { get; set; }
        public string ExpenseNumber { get; set; } = null!;
        public int CoaAccountId { get; set; }
        public string CoaAccountName { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int? VendorId { get; set; }
        public string? VendorName { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? ReceiptUrl { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = null!;
        public int SchoolRegistrationId { get; set; }

        // Mappings for frontend compatibility
        public string Reference => ReferenceNumber ?? ExpenseNumber;
        public DateTime EntryDate => Date;
        public string Narration => Description ?? $"Expense transaction {ExpenseNumber}";
        public List<ExpenseLineDto> Lines => new() { new ExpenseLineDto { DebitAmount = Amount, CreditAmount = 0 } };
    }
}
