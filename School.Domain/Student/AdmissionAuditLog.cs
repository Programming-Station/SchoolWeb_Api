using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Student
{
    public class AdmissionAuditLog : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AdmissionApplicationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Action { get; set; } = string.Empty; // DraftSaved, Submitted, DocumentVerified, FeeAssigned, Enrolled, etc.

        [Required]
        [MaxLength(50)]
        public string StatusFrom { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string StatusTo { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string PerformedBy { get; set; } = string.Empty;

        public DateTime PerformedDate { get; set; } = DateTime.Now;

        public string? DetailsJson { get; set; } // Detailed logs of changes

        [ForeignKey(nameof(AdmissionApplicationId))]
        public virtual AdmissionApplication AdmissionApplication { get; set; } = null!;
    }
}
