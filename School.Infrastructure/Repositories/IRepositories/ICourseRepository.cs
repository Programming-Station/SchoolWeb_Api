using School.Domain;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ICourseRepository
    {
        Task<Course> AddCourseAsync(Course entity);
        Task<Course> GetCourseByIdAsync(int id);
        Task<IEnumerable<Course>> GetAllCoursesAsync(int? courseType = null);
        Task<int> UpdateCourseAsync(Course entity);
        Task<int> DeleteCourseAsync(int id);
        Task<int> ToggleCourseStatusAsync(int id);
    }
}

