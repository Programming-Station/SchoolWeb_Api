using School_DTOs;

namespace School_DTOs.Menu
{
#nullable disable
    public class MenuDto : BaseDto
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string URL { get; set; }
        public int Priority { get; set; }
        public string MenuIcon { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsVisible { get; set; }
        public int? ModuleId { get; set; }
        public bool IsSelected { get; set; }
        public List<SubMenuDto> SubMenus { get; set; }
    }
    public class SubMenuDto
    {
        public int Id { get; set; }
        public string SubMenuName { get; set; }
        public string URL { get; set; }
        public int Priority { get; set; }
        public string Icon { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsVisible { get; set; }
        public string AccesibleFor { get; set; }
        public bool IsSelected { get; set; }
    }
}
