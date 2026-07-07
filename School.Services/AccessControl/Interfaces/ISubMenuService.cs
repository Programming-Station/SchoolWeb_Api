using School.Models.Menu;
using School_DTOs;
using School_DTOs.Menu;

namespace School.Services.AccessControl.Interfaces
{
    public interface ISubMenuService
    {
        Task<APIResponse<SubMenuDto>> AddSubMenuAsync(SubMenuModel model);
        Task<APIResponse<SubMenuDto>> GetSubMenuByIdAsync(int subMenuId);
        Task<APIResponse<IEnumerable<SubMenuDto>>> GetSubMenusByMenuIdAsync(int menuId);
        Task<APIResponse<IEnumerable<SubMenuDto>>> GetAllSubMenusAsync();
        Task<APIResponse> UpdateSubMenuAsync(SubMenuModel model);
        Task<APIResponse> DeleteSubMenuAsync(int subMenuId);
        Task<APIResponse> ToggleSubMenuStatusAsync(int subMenuId);
    }
}
