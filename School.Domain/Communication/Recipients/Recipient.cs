using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Communication.Recipients
{
    [Table("Recipients", Schema = "Communication")]
    public class Recipient : BaseEntity.AuditEntity<int>, BaseEntity.ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [Required, StringLength(100)]
        public string RecipientType { get; set; } = null!; // Student, Parent, Employee...

        [StringLength(50)]
        public string? RecipientCode { get; set; }

        [Required, StringLength(150)]
        public string FullName { get; set; } = null!;

        [StringLength(100)]
        public string? DisplayName { get; set; }

        [StringLength(500)]
        public string? PhotoUrl { get; set; }

        [StringLength(20)]
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public int? TargetEntityId { get; set; } // Link to StudentId, EmployeeId etc.

        // Contact Info
        [StringLength(100)]
        public string? Email { get; set; }
        [StringLength(100)]
        public string? AlternateEmail { get; set; }

        [StringLength(20)]
        public string? Mobile { get; set; }
        [StringLength(20)]
        public string? AlternateMobile { get; set; }
        [StringLength(20)]
        public string? WhatsAppNumber { get; set; }
        [StringLength(20)]
        public string? EmergencyContact { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }
        [StringLength(100)]
        public string? City { get; set; }
        [StringLength(100)]
        public string? State { get; set; }
        [StringLength(100)]
        public string? Country { get; set; }
        [StringLength(20)]
        public string? Pincode { get; set; }

        // Additional Meta
        [StringLength(50)]
        public string? Language { get; set; }
        [StringLength(50)]
        public string? TimeZone { get; set; }
        [StringLength(50)]
        public string? PreferredChannel { get; set; }

        public bool IsVerified { get; set; } = false;
        public bool IsBlocked { get; set; } = false;

        public virtual ICollection<RecipientGroupMember> GroupMembers { get; set; } = new List<RecipientGroupMember>();
        public virtual ICollection<RecipientTag> Tags { get; set; } = new List<RecipientTag>();
        public virtual ICollection<RecipientPreference> Preferences { get; set; } = new List<RecipientPreference>();
    }
}
