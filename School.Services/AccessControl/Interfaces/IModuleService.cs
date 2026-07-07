using School.Models.Module;
using School_DTOs;
using School_DTOs.Module;

namespace School.Services.AccessControl.Interfaces
{
    public interface IModuleService
    {
        Task<APIResponse<ModuleDto>> AddModuleAsync(ModuleModel model);
        Task<APIResponse<ModuleDto>> GetModuleByIdAsync(int id);
        Task<APIResponse<IEnumerable<ModuleDto>>> GetAllModulesAsync();
        Task<APIResponse> UpdateModuleAsync(ModuleModel model);
        Task<APIResponse> DeleteModuleAsync(int id);
        Task<APIResponse> ToggleModuleStatusAsync(int id);
    }
}
