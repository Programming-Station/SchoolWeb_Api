using School.Domain;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IClassRepository
    {
        Task<Class> AddClassAsync(Class entity);
        Task<Class> GetClassByIdAsync(int id);
        Task<IEnumerable<Class>> GetAllClassesAsync();
        Task<IEnumerable<Class>> GetClassesByCourseIdAsync(int courseId);
        Task<int> UpdateClassAsync(Class entity);
        Task<int> DeleteClassAsync(int id);
        Task<int> ToggleClassStatusAsync(int id);
        Task<int> UpdateClassStrengthAsync(int id, int newStrength);
    }
}

