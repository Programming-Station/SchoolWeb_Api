using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace School.Models.Menu
{
    public class MenuPermissionModel
    {
        [Required]
        [DisplayName("Role Id")]
        public required string RoleId { get; set; }
        [Required]
        [DisplayName("Please add menus permissions records.")]
        public required List<MenuPermissions> menuPermissions { get; set; }
        public string? UserName { get; set; }
        public string? CreateedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
    public class MenuPermissions
    {
        public int Id { get; set; }
        public string? MenuName { get; set; }
        public bool IsSelected { get; set; }
        public List<SubMenuPermissionModel>? SubMenus { get; set; }
    }

    public class SubMenuPermissionModel
    {
        public int Id { get; set; }
        public string? SubMenuName { get; set; }
        public bool IsSelected { get; set; }
    }
}
