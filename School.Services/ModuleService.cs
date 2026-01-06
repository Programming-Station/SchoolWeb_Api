using School.Infrastructure.Repositories.IRepositories;
using School.Models.Module;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Module;

namespace School.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;

        public ModuleService(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        public Task<APIResponse<ModuleDto>> AddModuleAsync(ModuleModel model)
        {
            return _moduleRepository.AddModuleAsync(model);
        }

        public Task<APIResponse<ModuleDto>> GetModuleByIdAsync(int id)
        {
            return _moduleRepository.GetModuleByIdAsync(id);
        }

        public Task<APIResponse<IEnumerable<ModuleDto>>> GetAllModulesAsync()
        {
            return _moduleRepository.GetAllModulesAsync();
        }

        public Task<APIResponse> UpdateModuleAsync(ModuleModel model)
        {
            return _moduleRepository.UpdateModuleAsync(model);
        }

        public Task<APIResponse> DeleteModuleAsync(int id)
        {
            return _moduleRepository.DeleteModuleAsync(id);
        }

        public Task<APIResponse> ToggleModuleStatusAsync(int id)
        {
            return _moduleRepository.ToggleModuleStatusAsync(id);
        }

        public Task<APIResponse<IEnumerable<ModuleDto>>> GetModulesByUserIdAsync(string userId)
        {
            return _moduleRepository.GetModulesByUserIdAsync(userId);
        }

        public Task<APIResponse> AssignModulesToUserAsync(AssignModulesToUserModel model)
        {
            return _moduleRepository.AssignModulesToUserAsync(model);
        }

        public Task<APIResponse> RemoveModulePermissionAsync(int moduleId, string userId)
        {
            return _moduleRepository.RemoveModulePermissionAsync(moduleId, userId);
        }

        public Task<APIResponse<IEnumerable<ModulePermissionDto>>> GetModulePermissionsByUserIdAsync(string userId)
        {
            return _moduleRepository.GetModulePermissionsByUserIdAsync(userId);
        }
    }
}

