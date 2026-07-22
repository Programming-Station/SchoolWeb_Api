using System.Net;
using AutoMapper;
using School.Domain.AccessControl;
using School.Infrastructure.Repositories.AccessControl;
using School.Models.Menu;
using School.Services.AccessControl.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Menu;

namespace School.Services.AccessControl
{
    public class MenuPermessionService : IMenuPermessionService
    {
        private readonly IMenuPermessionRepository _menuPermessionRepository;
        private readonly IMapper _mapper;

        public MenuPermessionService(IMenuPermessionRepository menuPermessionRepository, IMapper mapper)
        {
            _menuPermessionRepository = menuPermessionRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> GiveMenuPermissionAsync(MenuPermissionModel model)
        {
            var changes = await _menuPermessionRepository.GiveMenuPermissionAsync(model);

            if (changes > 0)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = "Permissions updated successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Failed to update permissions",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<APIResponse<MenuPermissionsDto>> GetMenuPermissionAsync(string? roleId)
        {
            var res = await _menuPermessionRepository.GetMenuPermissionAsync(roleId);

            return new APIResponse<MenuPermissionsDto>
            {
                Success = true,
                Data = res,
                Message = CommonResource.FetchSuccess,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse> AddMenuPermissionAsync(MenuPermissionModel model)
        {
            var entity = _mapper.Map<MenuPermession>(model);
            entity.CreatedBy = model.CreateedBy;
            entity.CreatedDate = DateTime.UtcNow;

            entity = await _menuPermessionRepository.AddMenuPermissionAsync(entity);

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

        public async Task<APIResponse> DeleteMenuPermissionAsync(int id)
        {
            var result = await _menuPermessionRepository.DeleteMenuPermissionAsync(id);

            if (result > 0)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.DeleteSuccess,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse
                {
                    Success = false,
                    Message = CommonResource.DeleteFailed,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<APIResponse> ToggleMenuPermissionStatusAsync(int id)
        {
            var result = await _menuPermessionRepository.ToggleMenuPermissionStatusAsync(id);

            if (result > 0)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.UpdateSuccess,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse
                {
                    Success = false,
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
    }
}
