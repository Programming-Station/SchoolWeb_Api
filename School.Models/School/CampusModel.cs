using System.ComponentModel.DataAnnotations;

namespace School.Models.School
{
    public class CampusModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Campus name is required")]
        [MaxLength(200, ErrorMessage = "Campus name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campus code is required")]
        [MaxLength(50, ErrorMessage = "Campus code cannot exceed 50 characters")]
        public string Code { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(15)]
        public string? Phone { get; set; }

        [MaxLength(200)]
        public string? Email { get; set; }

        public string Status { get; set; } = "active";
    }
}
