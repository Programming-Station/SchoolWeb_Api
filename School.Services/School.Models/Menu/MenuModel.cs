using School.Models.CustomeVailidation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace School.Models.Menu
{
    public class MenuModel
    {
        public int MenuId { get; set; } = 0;
        [DisplayName("Menu name")]
        [Required(ErrorMessage = "{0} is required")]
        [NoScript]
        public required string MenuName { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public string? URL { get; set; }
        public int Priority { get; set; }
        public string? MenuIcon { get; set; }
        public bool IsVisible { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        [DisplayName("Module Id")]
        [Required]
        public int ModuleId { get; set; } = 1;
        public List<SubMenuModel>? SubMenus { get; set; }
    }
}
