using School.Models.Course;
using School_DTOs;
using School_DTOs.Course;

namespace School.Services.Interfaces
{
    public interface ICourseService
    {
        Task<APIResponse<CourseDto>> AddCourseAsync(CourseModel model);
        Task<APIResponse<CourseDto>> GetCourseByIdAsync(int id);
        Task<APIResponse<IEnumerable<CourseDto>>> GetAllCoursesAsync(int? courseType = null);
        Task<APIResponse> UpdateCourseAsync(CourseModel model);
        Task<APIResponse> DeleteCourseAsync(int id);
        Task<APIResponse> ToggleCourseStatusAsync(int id);
    }
}

