using System.ComponentModel.DataAnnotations;
using static School.Domain.BaseEntity;

namespace School.Domain
{
    public class Menu : AuditEntity<int>
    {
        public Menu()
        {
            SubMenus = new HashSet<SubMenu>();
        }
        [Key]
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string? URL { get; set; }
        public int Priority { get; set; }
        public string? MenuIcon { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public bool IsVisible { get; set; }
        public virtual ICollection<SubMenu> SubMenus { get; set; }
    }
}
