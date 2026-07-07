using School.Domain.AccessControl;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.AccessControl
{
    public interface IModuleRepository : IRepository<Module>
    {
        Task<Module> AddModuleAsync(Module entity);
        Task<Module> GetModuleByIdAsync(int id);
        Task<IEnumerable<Module>> GetAllAsync();
        Task<int> UpdateModuleAsync(Module entity);
        Task<int> DeleteModuleAsync(int id);
        Task<int> ToggleModuleStatusAsync(int id);
    }
}
