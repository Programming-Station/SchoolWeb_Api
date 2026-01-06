using School.Models.Module;
using School_DTOs;
using School_DTOs.Module;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ICategoryModuleRepository
    {
        Task<APIResponse<CategoryModuleDto>> AddCategoryModuleAsync(CategoryModuleModel model);
        Task<APIResponse<CategoryModuleDto>> GetCategoryModuleByIdAsync(int id);
        Task<APIResponse<IEnumerable<CategoryModuleDto>>> GetAllCategoryModulesAsync();
        Task<APIResponse> UpdateCategoryModuleAsync(CategoryModuleModel model);
        Task<APIResponse> DeleteCategoryModuleAsync(int id);
        Task<APIResponse> ToggleCategoryModuleStatusAsync(int id);
    }
}

