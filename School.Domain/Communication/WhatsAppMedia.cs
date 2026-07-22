using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("WhatsAppMediaFiles", Schema = "Communication")]
    public class WhatsAppMedia : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string MetaMediaId { get; set; } = null!;

        [Required, MaxLength(255)]
        public string FileName { get; set; } = null!;

        [Required, MaxLength(500)]
        public string FilePath { get; set; } = null!;

        [Required, MaxLength(100)]
        public string MimeType { get; set; } = null!;

        public long FileSize { get; set; }

        public int SchoolRegistrationId { get; set; }
    }
}
