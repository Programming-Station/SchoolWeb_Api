using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Models.Module;
using School_DTOs;
using School_DTOs.Module;
using School.Utilities.Resources;
using Microsoft.EntityFrameworkCore;
using System.Net;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories
{
    public class CategoryModuleRepository : Repository<CategoryModule>, ICategoryModuleRepository
    {
        private readonly SchoolDbContext _context;

        public CategoryModuleRepository(DbFactory dbFactory, SchoolDbContext context) : base(dbFactory)
        {
            _context = context;
        }

        public async Task<APIResponse<CategoryModuleDto>> AddCategoryModuleAsync(CategoryModuleModel model)
        {
            try
            {
                var category = new CategoryModule
                {
                    Name = model.Name,
                    Description = model.Description,
                    Order = model.Order,
                    IsActive = model.IsActive,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = DateTime.UtcNow
                };

                _context.CategoryModules.Add(category);
                await _context.SaveChangesAsync();

                var dto = MapToDto(category);
                return new APIResponse<CategoryModuleDto>
                {
                    Success = true,
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<CategoryModuleDto>
                {
                    Success = false,
                    Message = $"Failed to add category: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<CategoryModuleDto>> GetCategoryModuleByIdAsync(int id)
        {
            try
            {
                var category = await _context.CategoryModules
                    .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

                if (category == null)
                {
                    return new APIResponse<CategoryModuleDto>
                    {
                        Success = false,
                        Message = "Category not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                return new APIResponse<CategoryModuleDto>
                {
                    Success = true,
                    Message = CommonResource.FetchSuccess,
                    StatusCode = HttpStatusCode.OK,
                    Data = MapToDto(category)
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<CategoryModuleDto>
                {
                    Success = false,
                    Message = $"Failed to get category: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<IEnumerable<CategoryModuleDto>>> GetAllCategoryModulesAsync()
        {
            try
            {
                var categories = await _context.CategoryModules
                    .Where(c => !c.IsDeleted)
                    .OrderBy(c => c.Order)
                    .ThenBy(c => c.Name)
                    .ToListAsync();

                var dtos = categories.Select(MapToDto);
                
                return new APIResponse<IEnumerable<CategoryModuleDto>>
                {
                    Success = true,
                    Message = CommonResource.FetchSuccess,
                    StatusCode = HttpStatusCode.OK,
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<IEnumerable<CategoryModuleDto>>
                {
                    Success = false,
                    Message = $"Failed to get categories: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> UpdateCategoryModuleAsync(CategoryModuleModel model)
        {
            try
            {
                var category = await _context.CategoryModules
                    .FirstOrDefaultAsync(c => c.Id == model.Id && !c.IsDeleted);

                if (category == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Category not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                category.Name = model.Name;
                category.Description = model.Description;
                category.Order = model.Order;
                category.IsActive = model.IsActive;
                category.UpdatedBy = model.UpdatedBy;
                category.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.UpdateSuccess,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to update category: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> DeleteCategoryModuleAsync(int id)
        {
            try
            {
                var category = await _context.CategoryModules
                    .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

                if (category == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Category not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var modulesUsingCategory = await _context.Modules
                    .AnyAsync(m => m.CategoryModuleId == id && !m.IsDeleted);

                if (modulesUsingCategory)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Cannot delete category. It is being used by one or more modules.",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                category.IsDeleted = true;
                category.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.DeleteSuccess,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to delete category: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> ToggleCategoryModuleStatusAsync(int id)
        {
            try
            {
                var category = await _context.CategoryModules
                    .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

                if (category == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Category not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                category.IsActive = !category.IsActive;
                category.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.UpdateSuccess,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to toggle category status: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        private CategoryModuleDto MapToDto(CategoryModule category)
        {
            return new CategoryModuleDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Order = category.Order,
                IsActive = category.IsActive,
                CreatedDate = category.CreatedDate,
                CreatedBy = category.CreatedBy,
                UpdatedDate = category.UpdatedDate,
                UpdatedBy = category.UpdatedBy
            };
        }
    }
}

