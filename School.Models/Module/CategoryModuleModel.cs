using System.ComponentModel.DataAnnotations;

namespace School.Models.Module
{
    public class CategoryModuleModel
    {
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "Category Name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        public int Order { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

