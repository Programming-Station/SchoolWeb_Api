using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("Announcements", Schema = "Communication")]
    public class Announcement : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required, MaxLength(8000)]
        public string Content { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Scope { get; set; } = "School"; // School, Branch, Department, Class, Public

        [MaxLength(100)]
        public string? TargetReferenceId { get; set; } // Specific BranchId, DepartmentId, or ClassId

        [Required, MaxLength(20)]
        public string Priority { get; set; } = "Medium"; // Low, Medium, High, Critical

        [MaxLength(500)]
        public string? AttachmentPath { get; set; }

        [MaxLength(500)]
        public string? ImagePath { get; set; }

        public bool IsPinned { get; set; } = false;

        public DateTime? ExpiryDate { get; set; }

        public int SchoolRegistrationId { get; set; }
    }
}
