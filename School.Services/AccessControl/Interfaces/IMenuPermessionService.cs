using School.Models.Menu;
using School_DTOs;
using School_DTOs.Menu;

namespace School.Services.AccessControl.Interfaces
{
    public interface IMenuPermessionService
    {
        Task<APIResponse> GiveMenuPermissionAsync(MenuPermissionModel model);
        Task<APIResponse<MenuPermissionsDto>> GetMenuPermissionAsync(string? roleId);
        
        // Basic CRUD support for MenuPermission entity
        Task<APIResponse> AddMenuPermissionAsync(MenuPermissionModel model);
        Task<APIResponse> DeleteMenuPermissionAsync(int id);
        Task<APIResponse> ToggleMenuPermissionStatusAsync(int id);
    }
}
