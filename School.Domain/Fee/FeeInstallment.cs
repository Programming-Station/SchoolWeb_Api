using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.FeeManagnment;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.FeeManagnment
{
    #nullable disable
    public class FeeInstallment : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FeeStructureId { get; set; }
        [ForeignKey(nameof(FeeStructureId))]
        public virtual FeeStructure FeeStructure { get; set; }

        [Required]
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student.Student Student { get; set; }

        [Required]
        public int InstallmentNo { get; set; } // 1, 2, 3...

        [Required, MaxLength(100)]
        public string InstallmentName { get; set; } // e.g. "1st Installment", "Quarterly"

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal FineAmount { get; set; } = 0;

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0; // Scholarship/concession

        [Required]
        public DateTime DueDate { get; set; }

        /// <summary>Pending / Paid / Overdue / Waived / PartiallyPaid</summary>
        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime? PaidDate { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; }
    }
}
