using School.Models.Department;
using School_DTOs;
using School_DTOs.Department;

namespace School.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<APIResponse<DepartmentDto>> AddDepartmentAsync(DepartmentModel model);
        Task<APIResponse<DepartmentDto>> GetDepartmentByIdAsync(int id);
        Task<APIResponse<IEnumerable<DepartmentDto>>> GetAllDepartmentsAsync();
        Task<APIResponse<IEnumerable<DepartmentDto>>> GetDepartmentsByFacultyIdAsync(int facultyId);
        Task<APIResponse> UpdateDepartmentAsync(DepartmentModel model);
        Task<APIResponse> DeleteDepartmentAsync(int id);
        Task<APIResponse> ToggleDepartmentStatusAsync(int id);
    }
}

