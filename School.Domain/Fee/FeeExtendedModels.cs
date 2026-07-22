using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.FeeManagnment
{
#nullable disable

    /// <summary>6.4 — Late Fine per installment</summary>
    public class FeeFine : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required] public int FeeInstallmentId { get; set; }
        [ForeignKey(nameof(FeeInstallmentId))] public virtual FeeInstallment FeeInstallment { get; set; }

        [Required] public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))] public virtual Student.Student Student { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")] public decimal FineAmount { get; set; }

        /// <summary>PerDay / Fixed / Percentage</summary>
        [MaxLength(20)] public string FineType { get; set; } = "Fixed";

        public int? DaysLate { get; set; }
        [MaxLength(500)] public string Reason { get; set; }

        /// <summary>Pending / Waived / Paid</summary>
        [MaxLength(20)] public string Status { get; set; } = "Pending";

        [MaxLength(450)] public string AppliedBy { get; set; }

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }

    /// <summary>6.5 — Student Scholarship / Concession</summary>
    public class StudentScholarship : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required] public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))] public virtual Student.Student Student { get; set; }

        [Required, MaxLength(200)] public string ScholarshipName { get; set; }
        [MaxLength(500)] public string Description { get; set; }

        /// <summary>Fixed / Percentage</summary>
        [MaxLength(20)] public string DiscountType { get; set; } = "Fixed";
        [Column(TypeName = "decimal(10,2)")] public decimal DiscountValue { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        /// <summary>Active / Expired / Cancelled</summary>
        [MaxLength(20)] public string Status { get; set; } = "Active";

        [MaxLength(450)] public string ApprovedBy { get; set; }
        [MaxLength(500)] public string Remarks { get; set; }

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }

    /// <summary>6.6 — Fee Refund record</summary>
    public class FeeRefund : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required] public int FeePaymentId { get; set; }
        [ForeignKey(nameof(FeePaymentId))] public virtual FeePayment FeePayment { get; set; }

        [Required] public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))] public virtual Student.Student Student { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")] public decimal RefundAmount { get; set; }
        [MaxLength(200)] public string Reason { get; set; }

        /// <summary>Cash / Cheque / Online / NEFT</summary>
        [MaxLength(30)] public string RefundMode { get; set; } = "Cash";
        [MaxLength(100)] public string RefundRef { get; set; }

        [Required] public DateTime RefundDate { get; set; } = DateTime.Today;

        /// <summary>Pending / Approved / Processed / Rejected</summary>
        [MaxLength(20)] public string Status { get; set; } = "Pending";

        [MaxLength(450)] public string ApprovedBy { get; set; }
        [MaxLength(500)] public string Remarks { get; set; }

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }

    /// <summary>6.4 — Fine Calculation Rules per Fee Type</summary>
    public class FineRule : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required] public int FeeTypeId { get; set; }
        [ForeignKey(nameof(FeeTypeId))] public virtual FeeType FeeType { get; set; }

        [Required] public int GraceDays { get; set; } = 0;

        /// <summary>Fixed / Percentage</summary>
        [Required, MaxLength(20)] public string FineType { get; set; } = "Fixed";

        [Required, Column(TypeName = "decimal(10,2)")] public decimal FineAmount { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")] public decimal MaxFine { get; set; } = 1000;

        public bool IsActive { get; set; } = true;

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }
}
