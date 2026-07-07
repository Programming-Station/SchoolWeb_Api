using School.Models.Module;
using School_DTOs;
using School_DTOs.Module;

namespace School.Services.AccessControl.Interfaces
{
    public interface ICategoryModuleService
    {
        Task<APIResponse<CategoryModuleDto>> AddCategoryModuleAsync(CategoryModuleModel model);
        Task<APIResponse<CategoryModuleDto>> GetCategoryModuleByIdAsync(int id);
        Task<APIResponse<IEnumerable<CategoryModuleDto>>> GetAllCategoryModulesAsync();
        Task<APIResponse> UpdateCategoryModuleAsync(CategoryModuleModel model);
        Task<APIResponse> DeleteCategoryModuleAsync(int id);
        Task<APIResponse> ToggleCategoryModuleStatusAsync(int id);
    }
}
