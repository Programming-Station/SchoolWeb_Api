using AutoMapper;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Class;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Class;
using System.Net;

namespace School.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public ClassService(IClassRepository classRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<ClassDto>> AddClassAsync(ClassModel model)
        {
            var entity = _mapper.Map<Class>(model);
            entity = await _classRepository.AddClassAsync(entity);
            if (entity != null && entity.Id > 0)
            {
                return new APIResponse<ClassDto>
                {
                    Success = true,
                    Data = _mapper.Map<ClassDto>(entity),
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<ClassDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<ClassDto>> GetClassByIdAsync(int id)
        {
            var result = await _classRepository.GetClassByIdAsync(id);

            if (result != null && result.Id > 0)
            {
                return new APIResponse<ClassDto>
                {
                    Data = _mapper.Map<ClassDto>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<ClassDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<APIResponse<IEnumerable<ClassDto>>> GetAllClassesAsync()
        {
            var result = await _classRepository.GetAllClassesAsync();
            if (result != null && result.Any())
            {
                return new APIResponse<IEnumerable<ClassDto>>
                {
                    Data = _mapper.Map<IEnumerable<ClassDto>>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<ClassDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse<IEnumerable<ClassDto>>> GetClassesByCourseIdAsync(int courseId)
        {
            var result = await _classRepository.GetClassesByCourseIdAsync(courseId);
            if (result != null && result.Any())
            {
                return new APIResponse<IEnumerable<ClassDto>>
                {
                    Data = _mapper.Map<IEnumerable<ClassDto>>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<ClassDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> UpdateClassAsync(ClassModel model)
        {
            var entity = _mapper.Map<Class>(model);
            var result = await _classRepository.UpdateClassAsync(entity);
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
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> DeleteClassAsync(int id)
        {
            int changes = await _classRepository.DeleteClassAsync(id);
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

        public async Task<APIResponse> ToggleClassStatusAsync(int id)
        {
            int changes = await _classRepository.ToggleClassStatusAsync(id);
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

        public async Task<APIResponse> UpdateClassStrengthAsync(int id, int newStrength)
        {
            int changes = await _classRepository.UpdateClassStrengthAsync(id, newStrength);
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
