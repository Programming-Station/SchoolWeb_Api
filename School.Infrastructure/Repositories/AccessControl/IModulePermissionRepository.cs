using School.Domain.AccessControl;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.AccessControl
{
    public interface IModulePermissionRepository : IRepository<ModulePermission>
    {
        Task<ModulePermission> AddModulePermissionAsync(ModulePermission entity);
        Task<ModulePermission> GetModulePermissionByIdAsync(int id);
        Task<IEnumerable<ModulePermission>> GetAllModulePermissionsAsync();
        Task<int> UpdateModulePermissionAsync(ModulePermission entity);
        Task<int> DeleteModulePermissionAsync(int id);
        Task<int> ToggleModulePermissionStatusAsync(int id);
        
        Task<IEnumerable<Module>> GetModulesByUserIdAsync(string userId);
        Task<int> AssignModulesToUserAsync(string userId, List<int> moduleIds, string? createdBy = null);
        Task<int> RemoveModulePermissionAsync(int moduleId, string userId);
        Task<IEnumerable<ModulePermission>> GetModulePermissionsByUserIdAsync(string userId);
    }
}
