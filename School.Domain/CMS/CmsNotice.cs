using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.CMS
{
    [Table("CmsNotices", Schema = "CMS")]
    public class CmsNotice : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int SchoolRegistrationId { get; set; }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Category { get; set; } = "General"; // General, Academic, Admission, Event, Exam

        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ExpiryDate { get; set; }

        public string? AttachmentUrl { get; set; }

        public bool IsImportant { get; set; } = false;

        public bool IsActive { get; set; } = true;
    }
}
