using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hostel
{
    [Table("HostelVisitors", Schema = "Hostel")]
    public class HostelVisitor : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }

        [Required, MaxLength(200)]
        public string VisitorName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Relation { get; set; } = null!; // Mother, Father, Friend, etc.

        [Required, MaxLength(50)]
        public string ContactNumber { get; set; } = null!;

        [MaxLength(100)]
        public string? IdProofType { get; set; }

        [MaxLength(100)]
        public string? IdProofNumber { get; set; }

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        [MaxLength(500)]
        public string? Purpose { get; set; }

        public DateTime EntryTime { get; set; }

        public DateTime? ExitTime { get; set; }

        [MaxLength(200)]
        public string? ApprovedBy { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "pending"; // pending, approved, rejected, out

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual global::School.Domain.Student.Student Student { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
