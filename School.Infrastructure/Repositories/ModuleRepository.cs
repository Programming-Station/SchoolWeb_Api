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
    public class ModuleRepository : Repository<Module>, IModuleRepository
    {
        private readonly SchoolDbContext _context;

        public ModuleRepository(DbFactory dbFactory, SchoolDbContext context) : base(dbFactory)
        {
            _context = context;
        }

        public async Task<APIResponse<ModuleDto>> AddModuleAsync(ModuleModel model)
        {
            try
            {
                // Verify category exists
                var categoryExists = await _context.CategoryModules
                    .AnyAsync(c => c.Id == model.CategoryModuleId && !c.IsDeleted && c.IsActive);

                if (!categoryExists)
                {
                    return new APIResponse<ModuleDto>
                    {
                        Success = false,
                        Message = "Category not found or inactive",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var module = new Module
                {
                    Name = model.Name,
                    Description = model.Description,
                    Route = model.Route,
                    Icon = model.Icon,
                    CategoryModuleId = model.CategoryModuleId,
                    Order = model.Order,
                    IsActive = model.IsActive,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Modules.Add(module);
                await _context.SaveChangesAsync();

                var dto = MapToDto(module);
                return new APIResponse<ModuleDto>
                {
                    Success = true,
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<ModuleDto>
                {
                    Success = false,
                    Message = $"Failed to add module: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<ModuleDto>> GetModuleByIdAsync(int id)
        {
            try
            {
                var module = await _context.Modules
                    .Include(m => m.CategoryModule)
                    .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

                if (module == null)
                {
                    return new APIResponse<ModuleDto>
                    {
                        Success = false,
                        Message = "Module not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                return new APIResponse<ModuleDto>
                {
                    Success = true,
                    Message = CommonResource.FetchSuccess,
                    StatusCode = HttpStatusCode.OK,
                    Data = MapToDto(module)
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<ModuleDto>
                {
                    Success = false,
                    Message = $"Failed to get module: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<IEnumerable<ModuleDto>>> GetAllModulesAsync()
        {
            try
            {
                var modules = await _context.Modules
                    .Where(m => !m.IsDeleted)
                    .Include(m => m.CategoryModule)
                    .OrderBy(m => m.Order)
                    .ThenBy(m => m.Name)
                    .ToListAsync();

                var dtos = modules.Select(MapToDto);
                
                return new APIResponse<IEnumerable<ModuleDto>>
                {
                    Success = true,
                    Message = CommonResource.FetchSuccess,
                    StatusCode = HttpStatusCode.OK,
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<IEnumerable<ModuleDto>>
                {
                    Success = false,
                    Message = $"Failed to get modules: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> UpdateModuleAsync(ModuleModel model)
        {
            try
            {
                var module = await _context.Modules
                    .FirstOrDefaultAsync(m => m.Id == model.Id && !m.IsDeleted);

                if (module == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Module not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                // Verify category exists
                var categoryExists = await _context.CategoryModules
                    .AnyAsync(c => c.Id == model.CategoryModuleId && !c.IsDeleted && c.IsActive);

                if (!categoryExists)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Category not found or inactive",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                module.Name = model.Name;
                module.Description = model.Description;
                module.Route = model.Route;
                module.Icon = model.Icon;
                module.CategoryModuleId = model.CategoryModuleId;
                module.Order = model.Order;
                module.IsActive = model.IsActive;
                module.UpdatedBy = model.UpdatedBy;
                module.UpdatedDate = DateTime.UtcNow;

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
                    Message = $"Failed to update module: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> DeleteModuleAsync(int id)
        {
            try
            {
                var module = await _context.Modules
                    .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

                if (module == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Module not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                module.IsDeleted = true;
                module.UpdatedDate = DateTime.UtcNow;
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
                    Message = $"Failed to delete module: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> ToggleModuleStatusAsync(int id)
        {
            try
            {
                var module = await _context.Modules
                    .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

                if (module == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Module not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                module.IsActive = !module.IsActive;
                module.UpdatedDate = DateTime.UtcNow;
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
                    Message = $"Failed to toggle module status: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<IEnumerable<ModuleDto>>> GetModulesByUserIdAsync(string userId)
        {
            try
            {
                var modules = await _context.ModulePermissions
                    .Where(mp => mp.UserId == userId && mp.IsActive && !mp.IsDeleted)
                    .Include(mp => mp.Module)
                        .ThenInclude(m => m.CategoryModule)
                    .Where(mp => mp.Module != null && !mp.Module.IsDeleted && mp.Module.IsActive)
                    .Select(mp => mp.Module!)
                    .OrderBy(m => m.Order)
                    .ThenBy(m => m.Name)
                    .Distinct()
                    .ToListAsync();

                var dtos = modules.Select(MapToDto);

                return new APIResponse<IEnumerable<ModuleDto>>
                {
                    Success = true,
                    Message = CommonResource.FetchSuccess,
                    StatusCode = HttpStatusCode.OK,
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<IEnumerable<ModuleDto>>
                {
                    Success = false,
                    Message = $"Failed to get user modules: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> AssignModulesToUserAsync(AssignModulesToUserModel model)
        {
            try
            {
                // Remove existing permissions for this user
                var existingPermissions = await _context.ModulePermissions
                    .Where(mp => mp.UserId == model.UserId && !mp.IsDeleted)
                    .ToListAsync();

                _context.ModulePermissions.RemoveRange(existingPermissions);

                // Add new permissions
                var newPermissions = model.ModuleIds.Select(moduleId => new ModulePermission
                {
                    ModuleId = moduleId,
                    UserId = model.UserId,
                    IsActive = true,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = DateTime.UtcNow
                }).ToList();

                _context.ModulePermissions.AddRange(newPermissions);
                await _context.SaveChangesAsync();

                return new APIResponse
                {
                    Success = true,
                    Message = "Modules assigned successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to assign modules: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> RemoveModulePermissionAsync(int moduleId, string userId)
        {
            try
            {
                var permission = await _context.ModulePermissions
                    .FirstOrDefaultAsync(mp => mp.ModuleId == moduleId && mp.UserId == userId && !mp.IsDeleted);

                if (permission == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Permission not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                permission.IsDeleted = true;
                permission.UpdatedDate = DateTime.UtcNow;
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
                    Message = $"Failed to remove permission: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<IEnumerable<ModulePermissionDto>>> GetModulePermissionsByUserIdAsync(string userId)
        {
            try
            {
                var permissions = await _context.ModulePermissions
                    .Where(mp => mp.UserId == userId && !mp.IsDeleted)
                    .Include(mp => mp.Module)
                    .Include(mp => mp.User)
                    .Include(mp => mp.Role)
                    .Select(mp => new ModulePermissionDto
                    {
                        Id = mp.Id,
                        ModuleId = mp.ModuleId,
                        ModuleName = mp.Module != null ? mp.Module.Name : "",
                        ModuleIcon = mp.Module != null ? mp.Module.Icon : null,
                        UserId = mp.UserId,
                        UserName = mp.User != null ? mp.User.UserName ?? "" : "",
                        RoleId = mp.RoleId,
                        RoleName = mp.Role != null ? mp.Role.Name : null,
                        IsActive = mp.IsActive
                    })
                    .ToListAsync();

                return new APIResponse<IEnumerable<ModulePermissionDto>>
                {
                    Success = true,
                    Message = CommonResource.FetchSuccess,
                    StatusCode = HttpStatusCode.OK,
                    Data = permissions
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<IEnumerable<ModulePermissionDto>>
                {
                    Success = false,
                    Message = $"Failed to get permissions: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        private ModuleDto MapToDto(Module module)
        {
            return new ModuleDto
            {
                Id = module.Id,
                Name = module.Name,
                Description = module.Description,
                Route = module.Route,
                Icon = module.Icon,
                CategoryModuleId = module.CategoryModuleId,
                CategoryModuleName = module.CategoryModule?.Name,
                Order = module.Order,
                IsActive = module.IsActive,
                CreatedDate = module.CreatedDate,
                CreatedBy = module.CreatedBy,
                UpdatedDate = module.UpdatedDate,
                UpdatedBy = module.UpdatedBy
            };
        }
    }
}

