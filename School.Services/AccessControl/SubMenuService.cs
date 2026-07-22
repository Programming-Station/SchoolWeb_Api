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
    public class SubMenuService : ISubMenuService
    {
        private readonly ISubMenuRepository _subMenuRepository;
        private readonly IMapper _mapper;

        public SubMenuService(ISubMenuRepository subMenuRepository, IMapper mapper)
        {
            _subMenuRepository = subMenuRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<SubMenuDto>> AddSubMenuAsync(SubMenuModel model)
        {
            var entity = _mapper.Map<SubMenu>(model);
            entity.CreatedBy = model.CreatedBy;
            entity.CreatedDate = DateTime.UtcNow;

            entity = await _subMenuRepository.AddSubMenuAsync(entity);

            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<SubMenuDto>
                {
                    Success = false,
                    Data = _mapper.Map<SubMenuDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, typeof(SubMenu).Name, model.SubMenuName),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            else if (entity != null && entity.Id > 0)
            {
                return new APIResponse<SubMenuDto>
                {
                    Success = true,
                    Data = _mapper.Map<SubMenuDto>(entity),
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
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<SubMenuDto>> GetSubMenuByIdAsync(int subMenuId)
        {
            var result = await _subMenuRepository.GetSubMenuByIdAsync(subMenuId);

            if (result != null && result.Id > 0)
            {
                return new APIResponse<SubMenuDto>
                {
                    Success = true,
                    Data = _mapper.Map<SubMenuDto>(result),
                    Message = CommonResource.FetchSuccess,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<SubMenuDto>
                {
                    Success = false,
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
        }

        public async Task<APIResponse<IEnumerable<SubMenuDto>>> GetSubMenusByMenuIdAsync(int menuId)
        {
            var result = await _subMenuRepository.GetSubMenusByMenuIdAsync(menuId);

            if (result != null && result.Any())
            {
                var dtos = result.Select(_mapper.Map<SubMenuDto>);
                return new APIResponse<IEnumerable<SubMenuDto>>
                {
                    Success = true,
                    Data = dtos,
                    Message = CommonResource.FetchSuccess,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<IEnumerable<SubMenuDto>>
                {
                    Success = true,
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<APIResponse<IEnumerable<SubMenuDto>>> GetAllSubMenusAsync()
        {
            var result = await _subMenuRepository.GetAllSubMenusAsync();

            if (result != null && result.Any())
            {
                var dtos = result.Select(_mapper.Map<SubMenuDto>);
                return new APIResponse<IEnumerable<SubMenuDto>>
                {
                    Success = true,
                    Data = dtos,
                    Message = CommonResource.FetchSuccess,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<IEnumerable<SubMenuDto>>
                {
                    Success = true,
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<APIResponse> UpdateSubMenuAsync(SubMenuModel model)
        {
            var entity = _mapper.Map<SubMenu>(model);
            entity.UpdatedBy = model.UpdatedBy;
            entity.UpdatedDate = DateTime.UtcNow;

            var result = await _subMenuRepository.UpdateSubMenuAsync(entity);

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

        public async Task<APIResponse> DeleteSubMenuAsync(int subMenuId)
        {
            var result = await _subMenuRepository.DeleteSubMenuAsync(subMenuId);

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

        public async Task<APIResponse> ToggleSubMenuStatusAsync(int subMenuId)
        {
            var result = await _subMenuRepository.ToggleSubMenuStatusAsync(subMenuId);

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
