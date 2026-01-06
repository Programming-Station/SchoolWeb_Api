using System.ComponentModel.DataAnnotations;

namespace School.Models.Menu
{
    public class SubMenuModel
    {
        public int SubMenuId { get; set; }
        [Required]
        public string SubMenuName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string URL { get; set; }
        public int Priority { get; set; }
        public string? Icon { get; set; }
        public bool IsVisible { get; set; }
        public string? AccesibleFor { get; set; }
        [Required]
        public int MenuId { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}