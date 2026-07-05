using AutoMapper;
using School.Domain.AccessControl;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Menu;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Menu;
using System.Net;

namespace School.Services
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
            
            if (model.SubMenus != null && model.SubMenus.Any())
            {
                entity.SubMenus = model.SubMenus
                    .Where(sm => !string.IsNullOrWhiteSpace(sm.SubMenuName))
                    .Select(sm => new SubMenu
                    {
                        SubMenuName = sm.SubMenuName,
                        URL = sm.URL ?? string.Empty,
                        Priority = sm.Priority,
                        Icon = sm.Icon,
                        Controller = sm.Controller ?? string.Empty,
                        Action = sm.Action ?? string.Empty,
                        IsVisible = sm.IsVisible,
                        AccesibleFor = sm.AccesibleFor,
                        CreatedBy = model.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false
                    })
                    .ToList();
            }

            entity = await _menuRepository.AddMenuAsync(entity);

            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<MenuDto>
                {
                    Success = false,
                    Data = _mapper.Map<MenuDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, typeof(Menu).Name, model.MenuName),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            else if (entity != null && entity.Id > 0)
            {
                var savedMenu = await _menuRepository.GetMenuByIdAsync(entity.Id);
                return new APIResponse<MenuDto>
                {
                    Success = true,
                    Data = _mapper.Map<MenuDto>(savedMenu),
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
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<APIResponse<MenuDto>> GetMenuByIdAsync(int menuId)
        {
            var result = await _menuRepository.GetMenuByIdAsync(menuId);

            if (result != null && result.Id > 0)
            {
                return new APIResponse<MenuDto>
                {
                    Data = _mapper.Map<MenuDto>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<MenuDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
        }

        public async Task<APIResponse<IEnumerable<MenuDto>>> GetAllMenusAsync()
        {
            var result = await _menuRepository.GetAllMenusAsync();

            if (result != null && result.Any())
            {
                return new APIResponse<IEnumerable<MenuDto>>
                {
                    Data = _mapper.Map<IEnumerable<MenuDto>>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<MenuDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> UpdateMenuAsync(MenuModel model)
        {
            var existingMenu = await _menuRepository.GetMenuByIdAsync(model.MenuId);
            
            if (existingMenu == null || existingMenu.Id == 0)
            {
                return new APIResponse
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var entity = _mapper.Map<Menu>(model);
            entity.CreatedBy = existingMenu.CreatedBy;
            entity.CreatedDate = existingMenu.CreatedDate;
            entity.UpdatedDate = DateTime.Now;

            if (model.SubMenus != null && model.SubMenus.Any())
            {
                entity.SubMenus = model.SubMenus
                    .Where(sm => !string.IsNullOrWhiteSpace(sm.SubMenuName))
                    .Select(sm => new SubMenu
                    {
                        Id = sm.SubMenuId > 0 ? sm.SubMenuId : 0,
                        SubMenuName = sm.SubMenuName,
                        URL = sm.URL ?? string.Empty,
                        Priority = sm.Priority,
                        Icon = sm.Icon,
                        Controller = sm.Controller ?? string.Empty,
                        Action = sm.Action ?? string.Empty,
                        IsVisible = sm.IsVisible,
                        AccesibleFor = sm.AccesibleFor,
                        MenuId = model.MenuId,
                        CreatedBy = sm.SubMenuId > 0 ? existingMenu.SubMenus?.FirstOrDefault(s => s.Id == sm.SubMenuId)?.CreatedBy : model.CreatedBy,
                        CreatedDate = sm.SubMenuId > 0 ? existingMenu.SubMenus?.FirstOrDefault(s => s.Id == sm.SubMenuId)?.CreatedDate ?? DateTime.Now : DateTime.Now,
                        UpdatedBy = model.UpdatedBy,
                        UpdatedDate = DateTime.Now,
                        IsDeleted = false
                    })
                    .ToList();
            }

            var result = await _menuRepository.UpdateMenuAsync(entity);

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

        public async Task<APIResponse> DeleteMenuAsync(int menuId)
        {
            int changes = await _menuRepository.DeleteMenuAsync(menuId);

            if (changes > 0)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.DeleteSuccess,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse
                {
                    Message = CommonResource.DeleteFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
        }

        public async Task<APIResponse> ToggleMenuStatusAsync(int menuId)
        {
            int changes = await _menuRepository.ToggleMenuStatusAsync(menuId);

            if (changes > 0)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.UpdateSuccess,
                    StatusCode = HttpStatusCode.OK,
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


        public async Task<APIResponse<SubMenuDto>> AddSubMenuAsync(SubMenuModel model)
        {
            var entity = _mapper.Map<SubMenu>(model);
            entity.CreatedDate = DateTime.Now;
            entity.CreatedBy = model.CreatedBy;
            entity.IsDeleted = false;

            entity = await _menuRepository.AddSubMenuAsync(entity);

            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<SubMenuDto>
                {
                    Success = false,
                    Data = _mapper.Map<SubMenuDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, typeof(SubMenu).Name, model.SubMenuName),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            else if (entity != null && entity.Id > 0)
            {
                var savedSubMenu = await _menuRepository.GetSubMenuByIdAsync(entity.Id);
                return new APIResponse<SubMenuDto>
                {
                    Success = true,
                    Data = _mapper.Map<SubMenuDto>(savedSubMenu),
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<SubMenuDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<APIResponse<SubMenuDto>> GetSubMenuByIdAsync(int subMenuId)
        {
            var result = await _menuRepository.GetSubMenuByIdAsync(subMenuId);

            if (result != null && result.Id > 0)
            {
                return new APIResponse<SubMenuDto>
                {
                    Data = _mapper.Map<SubMenuDto>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<SubMenuDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
        }

        public async Task<APIResponse<IEnumerable<SubMenuDto>>> GetSubMenusByMenuIdAsync(int menuId)
        {
            var result = await _menuRepository.GetSubMenusByMenuIdAsync(menuId);

            if (result != null && result.Any())
            {
                return new APIResponse<IEnumerable<SubMenuDto>>
                {
                    Data = _mapper.Map<IEnumerable<SubMenuDto>>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<SubMenuDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> UpdateSubMenuAsync(SubMenuModel model)
        {
            var existingSubMenu = await _menuRepository.GetSubMenuByIdAsync(model.SubMenuId);

            if (existingSubMenu == null || existingSubMenu.Id == 0)
            {
                return new APIResponse
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var entity = _mapper.Map<SubMenu>(model);
            entity.CreatedBy = existingSubMenu.CreatedBy;
            entity.CreatedDate = existingSubMenu.CreatedDate;
            entity.UpdatedDate = DateTime.Now;

            var result = await _menuRepository.UpdateSubMenuAsync(entity);

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

        public async Task<APIResponse> DeleteSubMenuAsync(int subMenuId)
        {
            int changes = await _menuRepository.DeleteSubMenuAsync(subMenuId);

            if (changes > 0)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.DeleteSuccess,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse
                {
                    Message = CommonResource.DeleteFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
        }


        public async Task<APIResponse> GiveMenuPermissionAsync(MenuPermissionModel model)
        {
            int result = await _menuRepository.GiveMenuPermissionAsync(model);

            if (result > 0)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.UpdateSuccess,
                    StatusCode = HttpStatusCode.OK,
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

        public async Task<APIResponse<MenuPermissionsDto>> GetMenuPermissionAsync(string? roleId)
        {
            var result = await _menuRepository.GetMenuPermissionAsync(roleId);

            if (result != null)
            {
                return new APIResponse<MenuPermissionsDto>
                {
                    Success = true,
                    Data = result,
                    Message = CommonResource.FetchSuccess,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<MenuPermissionsDto>
                {
                    Success = false,
                    Message = CommonResource.FetchFailed,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
