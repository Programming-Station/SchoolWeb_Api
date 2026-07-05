using School.Domain.AccessControl;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Models.Module;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IModuleRepository : IRepository<Module>
    {
        Task<Module> AddModuleAsync(Module entity);
        Task<Module> GetModuleByIdAsync(int id);
        Task<IEnumerable<Module>> GetAllAsync();
        Task<int> UpdateModuleAsync(Module entity);
        Task<int> DeleteModuleAsync(int id);
        Task<int> ToggleModuleStatusAsync(int id);
        Task<IEnumerable<Module>> GetModulesByUserIdAsync(string userId);
        Task<int> AssignModulesToUserAsync(string userId, List<int> moduleIds, string? createdBy = null);
        Task<int> RemoveModulePermissionAsync(int moduleId, string userId);
        Task<IEnumerable<ModulePermission>> GetModulePermissionsByUserIdAsync(string userId);
    }
}
