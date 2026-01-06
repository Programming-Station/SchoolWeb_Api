using School.Models.Teacher;
using School_DTOs;
using School_DTOs.Teacher;

namespace School.Services.Interfaces
{
    public interface ITeacherService
    {
        Task<APIResponse<TeacherDto>> AddTeacherAsync(TeacherModel model, string? password = null);
        Task<APIResponse<TeacherDto>> GetTeacherByIdAsync(int id);
        Task<APIResponse<TeacherDto>> GetTeacherByTeacherIdAsync(string teacherId);
        Task<APIResponse<TeacherDto>> GetTeacherByEmailAsync(string email);
        Task<APIResponse<TeacherDto>> GetTeacherByUserIdAsync(string userId);
        Task<APIResponse<PagedResponse<TeacherDto>>> GetAllTeachersAsync(TeacherFilter filter);
        Task<APIResponse> UpdateTeacherAsync(TeacherModel model);
        Task<APIResponse> DeleteTeacherAsync(int id);
        Task<APIResponse> ToggleTeacherStatusAsync(int id);
        Task<APIResponse<string>> GenerateTeacherIdAsync();
    }
}

