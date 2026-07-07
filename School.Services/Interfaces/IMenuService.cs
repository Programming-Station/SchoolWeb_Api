using School.Models.Menu;
using School_DTOs;
using School_DTOs.Menu;

namespace School.Services.Interfaces
{
    public interface IMenuService
    {
        Task<APIResponse<MenuDto>> AddMenuAsync(MenuModel model);
        Task<APIResponse<MenuDto>> GetMenuByIdAsync(int menuId);
        Task<APIResponse<IEnumerable<MenuDto>>> GetAllMenusAsync();
        Task<APIResponse> UpdateMenuAsync(MenuModel model);
        Task<APIResponse> DeleteMenuAsync(int menuId);
        Task<APIResponse> ToggleMenuStatusAsync(int menuId);

        Task<APIResponse<SubMenuDto>> AddSubMenuAsync(SubMenuModel model);
        Task<APIResponse<SubMenuDto>> GetSubMenuByIdAsync(int subMenuId);
        Task<APIResponse<IEnumerable<SubMenuDto>>> GetSubMenusByMenuIdAsync(int menuId);
        Task<APIResponse> UpdateSubMenuAsync(SubMenuModel model);
        Task<APIResponse> DeleteSubMenuAsync(int subMenuId);

        Task<APIResponse> GiveMenuPermissionAsync(MenuPermissionModel model);
        Task<APIResponse<MenuPermissionsDto>> GetMenuPermissionAsync(string? roleId);
    }
}
