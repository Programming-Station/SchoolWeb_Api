using System;
using System.Collections.Generic;
using System.Text;

namespace School.Models.Fee
{
    public class FeeModel
    {
        public int FeeId { get; set; }

        // Student Info
        public int StudentId { get; set; }
      //  public Student Student { get; set; }

        // Academic Info
        public int ClassId { get; set; }
        public int AcademicYearId { get; set; }

        // Fee Details
        public decimal TotalFee { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }

        // Fee Type
        public string FeeType { get; set; }   // Monthly / Quarterly / Yearly

        // Status
        public string Status { get; set; }    // Paid / Partial / Due

        // Dates
        public DateTime FeeGeneratedDate { get; set; }
        public DateTime? DueDate { get; set; }

        // Audit
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
