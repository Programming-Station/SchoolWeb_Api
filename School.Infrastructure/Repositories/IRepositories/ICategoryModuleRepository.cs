using School.Domain;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ICategoryModuleRepository : IRepository<CategoryModule>
    {
        Task<CategoryModule> AddCategoryModuleAsync(CategoryModule entity);
        Task<CategoryModule> GetCategoryModuleByIdAsync(int id);
        Task<IEnumerable<CategoryModule>> GetAllAsync();
        Task<int> UpdateCategoryModuleAsync(CategoryModule entity);
        Task<int> DeleteCategoryModuleAsync(int id);
        Task<int> ToggleCategoryModuleStatusAsync(int id);
        Task<bool> IsCategoryInUseAsync(int categoryId);
    }
}
