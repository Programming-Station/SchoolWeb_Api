using School.Infrastructure.Repositories.IRepositories;
using School.Models.Module;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Module;

namespace School.Services
{
    public class CategoryModuleService : ICategoryModuleService
    {
        private readonly ICategoryModuleRepository _categoryModuleRepository;

        public CategoryModuleService(ICategoryModuleRepository categoryModuleRepository)
        {
            _categoryModuleRepository = categoryModuleRepository;
        }

        public Task<APIResponse<CategoryModuleDto>> AddCategoryModuleAsync(CategoryModuleModel model)
        {
            return _categoryModuleRepository.AddCategoryModuleAsync(model);
        }

        public Task<APIResponse<CategoryModuleDto>> GetCategoryModuleByIdAsync(int id)
        {
            return _categoryModuleRepository.GetCategoryModuleByIdAsync(id);
        }

        public Task<APIResponse<IEnumerable<CategoryModuleDto>>> GetAllCategoryModulesAsync()
        {
            return _categoryModuleRepository.GetAllCategoryModulesAsync();
        }

        public Task<APIResponse> UpdateCategoryModuleAsync(CategoryModuleModel model)
        {
            return _categoryModuleRepository.UpdateCategoryModuleAsync(model);
        }

        public Task<APIResponse> DeleteCategoryModuleAsync(int id)
        {
            return _categoryModuleRepository.DeleteCategoryModuleAsync(id);
        }

        public Task<APIResponse> ToggleCategoryModuleStatusAsync(int id)
        {
            return _categoryModuleRepository.ToggleCategoryModuleStatusAsync(id);
        }
    }
}

