using System.Net;
using AutoMapper;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Course;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Course;

namespace School.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CourseService(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<CourseDto>> AddCourseAsync(CourseModel model)
        {
            var entity = _mapper.Map<Course>(model);
            entity = await _courseRepository.AddCourseAsync(entity);
            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<CourseDto>
                {
                    Success = false,
                    Data = _mapper.Map<CourseDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, typeof(Course).Name, model.Name),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            else if (entity != null && entity.Id > 0)
            {
                return new APIResponse<CourseDto>
                {
                    Success = true,
                    Data = _mapper.Map<CourseDto>(entity),
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<CourseDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<CourseDto>> GetCourseByIdAsync(int id)
        {
            var result = await _courseRepository.GetCourseByIdAsync(id);

            if (result != null && result.Id > 0)
            {
                return new APIResponse<CourseDto>
                {
                    Data = _mapper.Map<CourseDto>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<CourseDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<APIResponse<IEnumerable<CourseDto>>> GetAllCoursesAsync(int? courseType = null)
        {
            var result = await _courseRepository.GetAllCoursesAsync(courseType);
            if (result != null && result.Any())
            {

                return new APIResponse<IEnumerable<CourseDto>>
                {
                    Data = _mapper.Map<IEnumerable<CourseDto>>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<CourseDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> UpdateCourseAsync(CourseModel model)
        {
            var entity = _mapper.Map<Course>(model);
            var result = await _courseRepository.UpdateCourseAsync(entity);
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

        public async Task<APIResponse> DeleteCourseAsync(int id)
        {
            int changes = await _courseRepository.DeleteCourseAsync(id);
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

        public async Task<APIResponse> ToggleCourseStatusAsync(int id)
        {
            int changes = await _courseRepository.ToggleCourseStatusAsync(id);
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
