using System.ComponentModel.DataAnnotations;
using static School.Domain.BaseEntity;

namespace School.Domain.Website
{
    public class NoticeBar : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string NoticeText { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string ContactInfo { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
