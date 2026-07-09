using System;

namespace School_DTOs.Fee
{
    public class FeeFineDto
    {
        public int Id { get; set; }
        public int FeeInstallmentId { get; set; }
        public string? InstallmentName { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public decimal FineAmount { get; set; }
        public string FineType { get; set; } = "Fixed";
        public int? DaysLate { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Waived, Paid
        public string? AppliedBy { get; set; }
        public int SchoolRegistrationId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
