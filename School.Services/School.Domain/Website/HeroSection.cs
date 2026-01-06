using System.ComponentModel.DataAnnotations;
using static School.Domain.BaseEntity;

namespace School.Domain.Website
{
    public class HeroSection : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Subtitle { get; set; } = null!;

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Button1Text { get; set; } = null!;

        [MaxLength(200)]
        public string? Button1Link { get; set; }

        [Required]
        [MaxLength(100)]
        public string Button2Text { get; set; } = null!;

        [MaxLength(200)]
        public string? Button2Link { get; set; }

        [MaxLength(500)]
        public string? BackgroundImageUrl { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
