using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.Hr;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Transport
{
    [Table("Conductors", Schema = "Transport")]
    public class Conductor : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; } // Link to HR Employee
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [MaxLength(50)]
        public string? LicenseNumber { get; set; }

        [Required, MaxLength(20)]
        public string ContactNumber { get; set; } = null!;

        [MaxLength(200)]
        public string? EmergencyContact { get; set; }

        public bool PoliceVerified { get; set; }

        public bool BackgroundVerified { get; set; }

        [MaxLength(500)]
        public string? DocumentsUrl { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Active"; // Active, Inactive, Suspended

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
