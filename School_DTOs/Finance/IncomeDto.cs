using System;
using System.Collections.Generic;

namespace School_DTOs.Finance
{
    public class IncomeLineDto
    {
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
    }

    public class IncomeDto : BaseDto
    {
        public int Id { get; set; }
        public string IncomeNumber { get; set; } = null!;
        public int CoaAccountId { get; set; }
        public string CoaAccountName { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string ReceivedFrom { get; set; } = null!;
        public string? ReferenceNumber { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = null!;
        public int SchoolRegistrationId { get; set; }

        // Mappings for frontend compatibility
        public string Reference => ReferenceNumber ?? IncomeNumber;
        public DateTime EntryDate => Date;
        public string Narration => Description ?? $"Income from {ReceivedFrom}";
        public List<IncomeLineDto> Lines => new() { new IncomeLineDto { DebitAmount = Amount, CreditAmount = 0 } };
    }
}
