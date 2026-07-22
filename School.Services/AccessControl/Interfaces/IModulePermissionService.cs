using School.Models.Module;
using School_DTOs;
using School_DTOs.Module;

namespace School.Services.AccessControl.Interfaces
{
    public interface IModulePermissionService
    {
        Task<APIResponse<IEnumerable<ModuleDto>>> GetModulesByUserIdAsync(string userId);
        Task<APIResponse> AssignModulesToUserAsync(AssignModulesToUserModel model);
        Task<APIResponse> RemoveModulePermissionAsync(int moduleId, string userId);
        Task<APIResponse<IEnumerable<ModulePermissionDto>>> GetModulePermissionsByUserIdAsync(string userId);

        // Basic CRUD support for ModulePermission entity
        Task<APIResponse> AddModulePermissionAsync(ModulePermissionModel model);
        Task<APIResponse> DeleteModulePermissionAsync(int id);
        Task<APIResponse> ToggleModulePermissionStatusAsync(int id);
    }
}
