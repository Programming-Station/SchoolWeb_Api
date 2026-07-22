using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hostel
{
    [Table("HostelFeeAllocations", Schema = "Hostel")]
    public class HostelFeeAllocation : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int AdmissionId { get; set; }

        public int StudentId { get; set; }

        [Required, MaxLength(200)]
        public string FeePlanName { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime DueDate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "pending"; // pending, paid, overdue

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(AdmissionId))]
        public virtual HostelAdmission Admission { get; set; } = null!;

        [ForeignKey(nameof(StudentId))]
        public virtual global::School.Domain.Student.Student Student { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
