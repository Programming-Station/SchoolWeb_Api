using AutoMapper;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Module;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Module;
using Microsoft.EntityFrameworkCore;
using System.Net;
using School.Infrastructure;

namespace School.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IMapper _mapper;
        private readonly SchoolDbContext _context;

        public ModuleService(IModuleRepository moduleRepository, IMapper mapper, SchoolDbContext context)
        {
            _moduleRepository = moduleRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<APIResponse<ModuleDto>> AddModuleAsync(ModuleModel model)
        {
            var categoryExists = await _context.CategoryModules
                .AnyAsync(c => c.Id == model.CategoryModuleId && !c.IsDeleted && c.IsActive);

            if (!categoryExists)
            {
                return new APIResponse<ModuleDto>
                {
                    Success = false,
                    Message = "Category not found or inactive",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            var entity = _mapper.Map<Module>(model);
            entity.CreatedBy = model.CreatedBy;
            entity.CreatedDate = DateTime.UtcNow;

            entity.Name = entity.Name?.Trim() ?? "";
            entity.Description = entity.Description?.Trim();
            entity.Route = entity.Route?.Trim() ?? "";
            entity.Icon = entity.Icon?.Trim();

            entity = await _moduleRepository.AddModuleAsync(entity);

            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<ModuleDto>
                {
                    Success = false,
                    Data = MapToDto(entity),
                    Message = string.Format(CommonResource.AlreadyExists, typeof(Module).Name, model.Route),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            else if (entity != null && entity.Id > 0)
            {
                var savedEntity = await _moduleRepository.GetModuleByIdAsync(entity.Id);
                return new APIResponse<ModuleDto>
                {
                    Success = true,
                    Data = MapToDto(savedEntity),
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<ModuleDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<ModuleDto>> GetModuleByIdAsync(int id)
        {
            var result = await _moduleRepository.GetModuleByIdAsync(id);

            if (result != null && result.Id > 0)
            {
                return new APIResponse<ModuleDto>
                {
                    Data = MapToDto(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<ModuleDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
        }

        public async Task<APIResponse<IEnumerable<ModuleDto>>> GetAllModulesAsync()
        {
            var result = await _moduleRepository.GetAllAsync();

            if (result != null && result.Any())
            {
                var dtos = result.Select(MapToDto);
                return new APIResponse<IEnumerable<ModuleDto>>
                {
                    Data = dtos,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<ModuleDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> UpdateModuleAsync(ModuleModel model)
        {
            if (model.Id <= 0)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Invalid module ID",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            var existingEntity = await _moduleRepository.GetModuleByIdAsync(model.Id);
            if (existingEntity == null || existingEntity.Id == 0)
            {
                return new APIResponse
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var categoryExists = await _context.CategoryModules
                .AnyAsync(c => c.Id == model.CategoryModuleId && !c.IsDeleted && c.IsActive);

            if (!categoryExists)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Category not found or inactive",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            existingEntity.Name = model.Name.Trim();
            existingEntity.Description = model.Description?.Trim();
            existingEntity.Route = model.Route.Trim();
            existingEntity.Icon = model.Icon?.Trim();
            existingEntity.CategoryModuleId = model.CategoryModuleId;
            existingEntity.Order = model.Order;
            existingEntity.IsActive = model.IsActive;
            existingEntity.UpdatedBy = model.UpdatedBy;
            existingEntity.UpdatedDate = DateTime.UtcNow;

            var result = await _moduleRepository.UpdateModuleAsync(existingEntity);
            if (result > 0)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Message = CommonResource.UpdateSuccess
                };
            }
            else
            {
                return new APIResponse
                {
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
        }

        public async Task<APIResponse> DeleteModuleAsync(int id)
        {
            int changes = await _moduleRepository.DeleteModuleAsync(id);
            if (changes > 0)
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.DeleteSuccess,
                    StatusCode = HttpStatusCode.OK,
                };
            else
                return new APIResponse
                {
                    Message = CommonResource.DeleteFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
        }

        public async Task<APIResponse> ToggleModuleStatusAsync(int id)
        {
            int changes = await _moduleRepository.ToggleModuleStatusAsync(id);
            if (changes > 0)
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.UpdateSuccess,
                    StatusCode = HttpStatusCode.OK,
                };
            else
                return new APIResponse
                {
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
        }

        public async Task<APIResponse<IEnumerable<ModuleDto>>> GetModulesByUserIdAsync(string userId)
        {
            var result = await _moduleRepository.GetModulesByUserIdAsync(userId);

            if (result != null && result.Any())
            {
                var dtos = result.Select(MapToDto);
                return new APIResponse<IEnumerable<ModuleDto>>
                {
                    Data = dtos,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<ModuleDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> AssignModulesToUserAsync(AssignModulesToUserModel model)
        {
            int result = await _moduleRepository.AssignModulesToUserAsync(model.UserId, model.ModuleIds, model.CreatedBy);
            if (result > 0)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = "Modules assigned successfully",
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse
                {
                    Message = "Failed to assign modules",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
        }

        public async Task<APIResponse> RemoveModulePermissionAsync(int moduleId, string userId)
        {
            int changes = await _moduleRepository.RemoveModulePermissionAsync(moduleId, userId);
            if (changes > 0)
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.DeleteSuccess,
                    StatusCode = HttpStatusCode.OK,
                };
            else
                return new APIResponse
                {
                    Message = CommonResource.DeleteFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
        }

        public async Task<APIResponse<IEnumerable<ModulePermissionDto>>> GetModulePermissionsByUserIdAsync(string userId)
        {
            var permissions = await _moduleRepository.GetModulePermissionsByUserIdAsync(userId);

            if (permissions != null && permissions.Any())
            {
                var dtos = permissions.Select(p => new ModulePermissionDto
                {
                    Id = p.Id,
                    ModuleId = p.ModuleId,
                    ModuleName = p.Module?.Name ?? "",
                    ModuleIcon = p.Module?.Icon,
                    UserId = p.UserId,
                    UserName = p.User?.UserName ?? "",
                    RoleId = p.RoleId,
                    RoleName = p.Role?.Name,
                    IsActive = p.IsActive
                });

                return new APIResponse<IEnumerable<ModulePermissionDto>>
                {
                    Data = dtos,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<ModulePermissionDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        private ModuleDto MapToDto(Module entity)
        {
            var dto = _mapper.Map<ModuleDto>(entity);

            dto.CategoryModuleName = entity.CategoryModule?.Name;

            return dto;
        }
    }
}
