using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("SharedDocuments", Schema = "Communication")]
    public class SharedDocument : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string FileName { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required, MaxLength(500)]
        public string FilePath { get; set; } = null!;

        public long FileSize { get; set; } // in bytes

        [MaxLength(100)]
        public string? FileType { get; set; } // e.g. pdf, docx, jpeg

        [Required, MaxLength(50)]
        public string TargetAudience { get; set; } = "All"; // All, Students, Parents, Employees

        public DateTime? ExpiryDate { get; set; }

        public int DownloadCount { get; set; } = 0;

        public bool IsPublicLink { get; set; } = false;

        public int SchoolRegistrationId { get; set; }
    }
}
