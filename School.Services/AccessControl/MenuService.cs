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
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public MenuService(IMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<MenuDto>> AddMenuAsync(MenuModel model)
        {
            var entity = _mapper.Map<Menu>(model);
            entity.CreatedBy = model.CreatedBy;
            entity.CreatedDate = DateTime.UtcNow;

            entity = await _menuRepository.AddMenuAsync(entity);

            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<MenuDto>
                {
                    Success = false,
                    Data = _mapper.Map<MenuDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, typeof(Menu).Name, model.MenuName),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            else if (entity != null && entity.Id > 0)
            {
                return new APIResponse<MenuDto>
                {
                    Success = true,
                    Data = _mapper.Map<MenuDto>(entity),
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<MenuDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<MenuDto>> GetMenuByIdAsync(int menuId)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(menuId);

            if (menu != null && menu.Id > 0)
            {
                return new APIResponse<MenuDto>
                {
                    Success = true,
                    Data = _mapper.Map<MenuDto>(menu),
                    Message = CommonResource.FetchSuccess,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<MenuDto>
                {
                    Success = false,
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
        }

        public async Task<APIResponse<IEnumerable<MenuDto>>> GetAllMenusAsync()
        {
            var list = await _menuRepository.GetAllMenusAsync();

            if (list != null && list.Any())
            {
                var dtos = list.Select(_mapper.Map<MenuDto>);
                return new APIResponse<IEnumerable<MenuDto>>
                {
                    Success = true,
                    Data = dtos,
                    Message = CommonResource.FetchSuccess,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<IEnumerable<MenuDto>>
                {
                    Success = true,
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<APIResponse> UpdateMenuAsync(MenuModel model)
        {
            var entity = _mapper.Map<Menu>(model);
            entity.UpdatedBy = model.UpdatedBy;
            entity.UpdatedDate = DateTime.UtcNow;

            var changes = await _menuRepository.UpdateMenuAsync(entity);

            if (changes > 0)
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

        public async Task<APIResponse> DeleteMenuAsync(int menuId)
        {
            var changes = await _menuRepository.DeleteMenuAsync(menuId);

            if (changes > 0)
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

        public async Task<APIResponse> ToggleMenuStatusAsync(int menuId)
        {
            var changes = await _menuRepository.ToggleMenuStatusAsync(menuId);

            if (changes > 0)
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
