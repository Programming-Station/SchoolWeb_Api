using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("NoticeBoards", Schema = "Communication")]
    public class NoticeBoard : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; } = null!;

        [Required, MaxLength(4000)]
        public string Content { get; set; } = null!;

        [Required, MaxLength(50)]
        public string TargetAudience { get; set; } = "All"; // All, Students, Parents, Employees

        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ExpiryDate { get; set; }

        public bool IsPinned { get; set; } = false;

        public int SchoolRegistrationId { get; set; }
    }
}
