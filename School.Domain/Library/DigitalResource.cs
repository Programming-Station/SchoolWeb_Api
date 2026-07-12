using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Library
{
    [Table("DigitalResources", Schema = "Library")]
    public class DigitalResource : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>EBook | PDF | Notes | QuestionPaper | AudioBook | VideoLecture | ResearchPaper | Magazine</summary>
        [Required, MaxLength(30)]
        public string ResourceType { get; set; } = "PDF";

        [MaxLength(500)]
        public string? FilePath { get; set; }

        [MaxLength(500)]
        public string? StreamingUrl { get; set; }

        [MaxLength(50)]
        public string? FileSize { get; set; }

        [MaxLength(20)]
        public string? FileExtension { get; set; }

        public bool DownloadAllowed { get; set; } = false;

        public int DownloadCount { get; set; } = 0;
        public int ViewCount { get; set; } = 0;

        // Optional link to a physical book
        public int? BookId { get; set; }
        [ForeignKey(nameof(BookId))]
        public virtual Book? Book { get; set; }

        [MaxLength(100)]
        public string? SubjectCategory { get; set; }

        [MaxLength(50)]
        public string? Language { get; set; }

        [MaxLength(500)]
        public string? ThumbnailPath { get; set; }

        [MaxLength(200)]
        public string? Tags { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "Active";

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration? SchoolRegistration { get; set; }
    }
}
