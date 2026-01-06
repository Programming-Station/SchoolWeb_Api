using School.Domain;
using School.Models.Teacher;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ITeacherRepository
    {
        Task<Teacher> AddTeacherAsync(Teacher entity);
        Task<Teacher> GetTeacherByIdAsync(int id);
        Task<Teacher?> GetTeacherByTeacherIdAsync(string teacherId);
        Task<Teacher?> GetTeacherByEmailAsync(string email);
        Task<Teacher?> GetTeacherByUserIdAsync(string userId);
        Task<(IEnumerable<Teacher> Teachers, int TotalCount)> GetAllTeachersAsync(TeacherFilter filter);
        Task<int> UpdateTeacherAsync(Teacher entity);
        Task<int> DeleteTeacherAsync(int id);
        Task<int> ToggleTeacherStatusAsync(int id);
    }
}

