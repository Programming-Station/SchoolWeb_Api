using System.ComponentModel.DataAnnotations;

namespace School.Models.Academic
{
    public class EducationLevelModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Education Level name is required")]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Code is required")]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public string Status { get; set; } = "active";
    }
}
