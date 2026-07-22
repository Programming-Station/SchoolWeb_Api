using System.ComponentModel.DataAnnotations;

namespace School.Models.Class
{
    public class SectionModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Section name is required")]
        [MaxLength(100, ErrorMessage = "Section name cannot exceed 100 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Class selection is required")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = "active";

        public string? CreatedBy { get; set; }
    }
}
