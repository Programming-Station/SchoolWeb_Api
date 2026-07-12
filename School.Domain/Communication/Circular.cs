using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("Circulars", Schema = "Communication")]
    public class Circular : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string CircularNo { get; set; } = null!; // e.g. CIR-2026-0001

        [Required, MaxLength(200)]
        public string Subject { get; set; } = null!;

        [Required, MaxLength(8000)]
        public string Content { get; set; } = null!;

        [MaxLength(500)]
        public string? AttachmentPath { get; set; }

        [Required, MaxLength(50)]
        public string TargetAudience { get; set; } = "All"; // All, Students, Parents, Employees

        public DateTime PublishDate { get; set; } = DateTime.UtcNow;

        public int SchoolRegistrationId { get; set; }
    }
}
