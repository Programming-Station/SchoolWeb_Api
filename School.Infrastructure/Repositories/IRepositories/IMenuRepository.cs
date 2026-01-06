using School.Domain;
using School_DTOs.Menu;
using School.Models.Menu;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IMenuRepository
    {
        Task<Menu> AddMenuAsync(Menu entity);
        Task<Menu> GetMenuByIdAsync(int menuId);
        Task<IEnumerable<Menu>> GetAllMenusAsync();
        Task<int> UpdateMenuAsync(Menu entity);
        Task<int> DeleteMenuAsync(int menuId);
        Task<int> ToggleMenuStatusAsync(int menuId);
        
        // SubMenu operations
        Task<SubMenu> AddSubMenuAsync(SubMenu entity);
        Task<SubMenu> GetSubMenuByIdAsync(int subMenuId);
        Task<IEnumerable<SubMenu>> GetSubMenusByMenuIdAsync(int menuId);
        Task<int> UpdateSubMenuAsync(SubMenu entity);
        Task<int> DeleteSubMenuAsync(int subMenuId);
        
        // Menu Permission operations
        Task<MenuPermissionsDto> GetMenuPermissionAsync(string? roleId);
        Task<int> GiveMenuPermissionAsync(MenuPermissionModel model);
    }
}
