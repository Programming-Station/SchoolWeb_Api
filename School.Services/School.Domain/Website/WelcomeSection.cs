using System.ComponentModel.DataAnnotations;
using static School.Domain.BaseEntity;

namespace School.Domain.Website
{
    public class WelcomeSection : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string TitlePart1 { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string TitlePart2 { get; set; } = null!;

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
