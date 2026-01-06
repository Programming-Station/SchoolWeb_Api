using School.Domain;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IFacultyRepository
    {
        Task<Faculty> AddFacultyAsync(Faculty entity);
        Task<Faculty> GetFacultyByIdAsync(int id);
        Task<IEnumerable<Faculty>> GetAllFacultiesAsync();
        Task<int> UpdateFacultyAsync(Faculty entity);
        Task<int> DeleteFacultyAsync(int id);
        Task<int> ToggleFacultyStatusAsync(int id);
    }
}

