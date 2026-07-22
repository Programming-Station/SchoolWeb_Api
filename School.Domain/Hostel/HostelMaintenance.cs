using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hostel
{
    [Table("HostelMaintenances", Schema = "Hostel")]
    public class HostelMaintenance : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int? ComplaintId { get; set; }

        public int? AssetId { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; } = null!;

        [Required, MaxLength(200)]
        public string TechnicianName { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, WorkInProgress, Completed

        public DateTime? CompletionDate { get; set; }

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(ComplaintId))]
        public virtual HostelComplaint? Complaint { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
