using System.ComponentModel.DataAnnotations;

namespace School.Models.Module
{
    public class ModulePermissionModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Module ID is required")]
        public int ModuleId { get; set; }
        
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; } = null!;
        
        public string? RoleId { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
    
    public class AssignModulesToUserModel
    {
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; } = null!;
        
        [Required(ErrorMessage = "Module IDs are required")]
        public List<int> ModuleIds { get; set; } = new List<int>();
        
        public string? CreatedBy { get; set; }
    }
}

