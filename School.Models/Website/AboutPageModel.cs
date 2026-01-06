using System.ComponentModel.DataAnnotations;

namespace School.Models.Website
{
    public class AboutPageModel
    {
        public int? Id { get; set; }

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

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
