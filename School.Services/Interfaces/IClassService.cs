using School.Models.Class;
using School_DTOs;
using School_DTOs.Class;

namespace School.Services.Interfaces
{
    public interface IClassService
    {
        Task<APIResponse<ClassDto>> AddClassAsync(ClassModel model);
        Task<APIResponse<ClassDto>> GetClassByIdAsync(int id);
        Task<APIResponse<IEnumerable<ClassDto>>> GetAllClassesAsync();
        Task<APIResponse<IEnumerable<ClassDto>>> GetClassesByCourseIdAsync(int courseId);
        Task<APIResponse> UpdateClassAsync(ClassModel model);
        Task<APIResponse> DeleteClassAsync(int id);
        Task<APIResponse> ToggleClassStatusAsync(int id);
        Task<APIResponse> UpdateClassStrengthAsync(int id, int newStrength);
    }
}

