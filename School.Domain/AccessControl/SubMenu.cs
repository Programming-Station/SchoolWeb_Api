using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using static School.Domain.BaseEntity;

namespace School.Domain.AccessControl
{
    public class SubMenu : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string SubMenuName { get; set; }
        public string URL { get; set; }
        public int Priority { get; set; }
        public string? Icon { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsVisible { get; set; }
        public string? AccesibleFor { get; set; }
        public int MenuId { get; set; }
        [ForeignKey("MenuId")]
        public virtual Menu? MainMenu { get; set; }
    }
}
