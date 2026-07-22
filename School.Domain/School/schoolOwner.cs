using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.Auth;
using static School.Domain.BaseEntity;

namespace School.Domain.School
{
    [Table("SchoolOwners", Schema = "School")]
    public class SchoolOwner : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int SchoolRegistrationId { get; set; }
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        public int? SchoolSubscriptionId { get; set; }
        public virtual SchoolSubscription? SchoolSubscription { get; set; }

        public string ApplicationUserId { get; set; } = string.Empty;
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public string? ProfilePhoto { get; set; }

        public int StatusId { get; set; }
        public virtual Status Status { get; set; } = null!;

        public bool EmailVerified { get; set; }

        public bool MobileVerified { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public string? LastLoginIp { get; set; }

        public int FailedLoginAttempt { get; set; }

        public bool IsLocked { get; set; }
    }
}

