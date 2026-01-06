using School.Domain;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IDepartmentRepository
    {
        Task<Department> AddDepartmentAsync(Department entity);
        Task<Department> GetDepartmentByIdAsync(int id);
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<IEnumerable<Department>> GetDepartmentsByFacultyIdAsync(int facultyId);
        Task<int> UpdateDepartmentAsync(Department entity);
        Task<int> DeleteDepartmentAsync(int id);
        Task<int> ToggleDepartmentStatusAsync(int id);
    }
}

