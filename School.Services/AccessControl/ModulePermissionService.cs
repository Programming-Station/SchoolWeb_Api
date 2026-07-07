using AutoMapper;
using School.Domain.AccessControl;
using School.Infrastructure.Repositories.AccessControl;
using School.Models.Module;
using School.Services.AccessControl.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Module;
using System.Net;

namespace School.Services.AccessControl
{
    public class ModulePermissionService : IModulePermissionService
    {
        private readonly IModulePermissionRepository _modulePermissionRepository;
        private readonly IMapper _mapper;

        public ModulePermissionService(IModulePermissionRepository modulePermissionRepository, IMapper mapper)
        {
            _modulePermissionRepository = modulePermissionRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<IEnumerable<ModuleDto>>> GetModulesByUserIdAsync(string userId)
        {
            var result = await _modulePermissionRepository.GetModulesByUserIdAsync(userId);

            if (result != null && result.Any())
            {
                var dtos = result.Select(x => _mapper.Map<ModuleDto>(x));
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
            int result = await _modulePermissionRepository.AssignModulesToUserAsync(model.UserId, model.ModuleIds, model.CreatedBy);
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
            int changes = await _modulePermissionRepository.RemoveModulePermissionAsync(moduleId, userId);
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
            var permissions = await _modulePermissionRepository.GetModulePermissionsByUserIdAsync(userId);

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

        public async Task<APIResponse> AddModulePermissionAsync(ModulePermissionModel model)
        {
            var entity = _mapper.Map<ModulePermission>(model);
            entity.CreatedBy = model.CreatedBy;
            entity.CreatedDate = DateTime.UtcNow;

            entity = await _modulePermissionRepository.AddModulePermissionAsync(entity);

            if (entity != null && entity.Id > 0)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<APIResponse> DeleteModulePermissionAsync(int id)
        {
            int changes = await _modulePermissionRepository.DeleteModulePermissionAsync(id);
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

        public async Task<APIResponse> ToggleModulePermissionStatusAsync(int id)
        {
            int changes = await _modulePermissionRepository.ToggleModulePermissionStatusAsync(id);
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
    }
}
