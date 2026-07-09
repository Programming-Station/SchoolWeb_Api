using System;

namespace School_DTOs.Fee
{
    public class FineRuleDto
    {
        public int Id { get; set; }
        public int FeeTypeId { get; set; }
        public string? FeeTypeName { get; set; }
        public int GraceDays { get; set; }
        public string FineType { get; set; } = "Fixed"; // Fixed or Percentage
        public decimal FineAmount { get; set; }
        public decimal MaxFine { get; set; }
        public bool IsActive { get; set; } = true;
        public int SchoolRegistrationId { get; set; }
    }
}
