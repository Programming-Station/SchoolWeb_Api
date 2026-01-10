using AutoMapper;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Module;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Module;
using System.Net;

namespace School.Services
{
    public class CategoryModuleService : ICategoryModuleService
    {
        private readonly ICategoryModuleRepository _categoryModuleRepository;
        private readonly IMapper _mapper;

        public CategoryModuleService(ICategoryModuleRepository categoryModuleRepository, IMapper mapper)
        {
            _categoryModuleRepository = categoryModuleRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<CategoryModuleDto>> AddCategoryModuleAsync(CategoryModuleModel model)
        {
            // Map Model to Entity
            var entity = _mapper.Map<CategoryModule>(model);
            entity.CreatedBy = model.CreatedBy;
            entity.CreatedDate = DateTime.UtcNow;

            // Trim string fields
            entity.Name = entity.Name?.Trim() ?? "";
            entity.Description = entity.Description?.Trim();

            entity = await _categoryModuleRepository.AddCategoryModuleAsync(entity);

            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<CategoryModuleDto>
                {
                    Success = false,
                    Data = _mapper.Map<CategoryModuleDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, typeof(CategoryModule).Name, model.Name),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            else if (entity != null && entity.Id > 0)
            {
                return new APIResponse<CategoryModuleDto>
                {
                    Success = true,
                    Data = _mapper.Map<CategoryModuleDto>(entity),
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<CategoryModuleDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<CategoryModuleDto>> GetCategoryModuleByIdAsync(int id)
        {
            var result = await _categoryModuleRepository.GetCategoryModuleByIdAsync(id);

            if (result != null && result.Id > 0)
            {
                return new APIResponse<CategoryModuleDto>
                {
                    Data = _mapper.Map<CategoryModuleDto>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<CategoryModuleDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
        }

        public async Task<APIResponse<IEnumerable<CategoryModuleDto>>> GetAllCategoryModulesAsync()
        {
            var result = await _categoryModuleRepository.GetAllAsync();

            if (result != null && result.Any())
            {
                var dtos = result.Select(_mapper.Map<CategoryModuleDto>);
                return new APIResponse<IEnumerable<CategoryModuleDto>>
                {
                    Data = dtos,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<CategoryModuleDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> UpdateCategoryModuleAsync(CategoryModuleModel model)
        {
            if (model.Id <= 0)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Invalid category ID",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            var existingEntity = await _categoryModuleRepository.GetCategoryModuleByIdAsync(model.Id);
            if (existingEntity == null || existingEntity.Id == 0)
            {
                return new APIResponse
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            // Update properties from model
            existingEntity.Name = model.Name.Trim();
            existingEntity.Description = model.Description?.Trim();
            existingEntity.Order = model.Order;
            existingEntity.IsActive = model.IsActive;
            existingEntity.UpdatedBy = model.UpdatedBy;
            existingEntity.UpdatedDate = DateTime.UtcNow;

            var result = await _categoryModuleRepository.UpdateCategoryModuleAsync(existingEntity);
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

        public async Task<APIResponse> DeleteCategoryModuleAsync(int id)
        {
            // Check if category is being used by any module
            var isInUse = await _categoryModuleRepository.IsCategoryInUseAsync(id);
            if (isInUse)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Cannot delete category. It is being used by one or more modules.",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            int changes = await _categoryModuleRepository.DeleteCategoryModuleAsync(id);
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

        public async Task<APIResponse> ToggleCategoryModuleStatusAsync(int id)
        {
            int changes = await _categoryModuleRepository.ToggleCategoryModuleStatusAsync(id);
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
