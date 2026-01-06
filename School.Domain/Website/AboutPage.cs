using System.ComponentModel.DataAnnotations;
using static School.Domain.BaseEntity;

namespace School.Domain.Website
{
    public class AboutPage : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string MainTitle { get; set; } = null!;

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(2000)]
        public string Mission { get; set; } = null!;

        [Required]
        [MaxLength(2000)]
        public string Vision { get; set; } = null!;

        [MaxLength(2000)]
        public string? Values { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
