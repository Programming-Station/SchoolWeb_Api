using System.ComponentModel.DataAnnotations;

namespace School.Models.Website
{
    public class SliderImageModel
    {
        public int? Id { get; set; }

        [Required]
        public string ImageUrl { get; set; } = null!;

        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(200)]
        public string? AltText { get; set; }

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
