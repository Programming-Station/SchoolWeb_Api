using School.Models.Module;
using School_DTOs;
using School_DTOs.Module;

namespace School.Services.Interfaces
{
    public interface IModuleService
    {
        Task<APIResponse<ModuleDto>> AddModuleAsync(ModuleModel model);
        Task<APIResponse<ModuleDto>> GetModuleByIdAsync(int id);
        Task<APIResponse<IEnumerable<ModuleDto>>> GetAllModulesAsync();
        Task<APIResponse> UpdateModuleAsync(ModuleModel model);
        Task<APIResponse> DeleteModuleAsync(int id);
        Task<APIResponse> ToggleModuleStatusAsync(int id);
        Task<APIResponse<IEnumerable<ModuleDto>>> GetModulesByUserIdAsync(string userId);
        Task<APIResponse> AssignModulesToUserAsync(AssignModulesToUserModel model);
        Task<APIResponse> RemoveModulePermissionAsync(int moduleId, string userId);
        Task<APIResponse<IEnumerable<ModulePermissionDto>>> GetModulePermissionsByUserIdAsync(string userId);
    }
}

