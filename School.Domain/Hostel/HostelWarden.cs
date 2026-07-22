using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hostel
{
    [Table("HostelWardens", Schema = "Hostel")]
    public class HostelWarden : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int HostelId { get; set; }

        [Required, MaxLength(50)]
        public string RoleType { get; set; } = null!; // ChiefWarden, AssistantWarden, Supervisor

        [MaxLength(200)]
        public string? EmergencyContact { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "active"; // active, inactive

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual global::School.Domain.Hr.Employee Employee { get; set; } = null!;

        [ForeignKey(nameof(HostelId))]
        public virtual Hostel Hostel { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
