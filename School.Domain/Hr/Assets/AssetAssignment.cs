using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.Hr;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hr.Assets
{
    public class AssetAssignment : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int SchoolAssetId { get; set; }
        [ForeignKey(nameof(SchoolAssetId))]
        public virtual SchoolAsset SchoolAsset { get; set; } = null!;

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Assigned"; // Assigned, Returned

        [MaxLength(200)]
        public string? Condition { get; set; } // Condition at return/assignment

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
