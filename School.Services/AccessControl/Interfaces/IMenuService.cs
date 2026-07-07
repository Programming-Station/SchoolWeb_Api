using School.Models.Menu;
using School_DTOs;
using School_DTOs.Menu;

namespace School.Services.AccessControl.Interfaces
{
    public interface IMenuService
    {
        Task<APIResponse<MenuDto>> AddMenuAsync(MenuModel model);
        Task<APIResponse<MenuDto>> GetMenuByIdAsync(int menuId);
        Task<APIResponse<IEnumerable<MenuDto>>> GetAllMenusAsync();
        Task<APIResponse> UpdateMenuAsync(MenuModel model);
        Task<APIResponse> DeleteMenuAsync(int menuId);
        Task<APIResponse> ToggleMenuStatusAsync(int menuId);
    }
}
