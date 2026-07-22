using System.ComponentModel.DataAnnotations;

namespace School.Models.Module
{
    public class ModuleModel
    {
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "Module Name is required")]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Route is required")]
        [MaxLength(200)]
        public string Route { get; set; } = null!;

        [Required(ErrorMessage = "Icon is required")]
        [MaxLength(100)]
        public string? Icon { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryModuleId { get; set; }

        public int Order { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

