namespace School.Models.Fee
{
    public class FeeModel
    {
        public int FeeId { get; set; }

        public int StudentId { get; set; }

        public int ClassId { get; set; }
        public int AcademicYearId { get; set; }

        public decimal TotalFee { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }

        public string FeeType { get; set; }   // Monthly / Quarterly / Yearly

        public string Status { get; set; }    // Paid / Partial / Due

        public DateTime FeeGeneratedDate { get; set; }
        public DateTime? DueDate { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
