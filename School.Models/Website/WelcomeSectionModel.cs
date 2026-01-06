using System.ComponentModel.DataAnnotations;

namespace School.Models.Website
{
    public class WelcomeSectionModel
    {
        public int? Id { get; set; }

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

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
