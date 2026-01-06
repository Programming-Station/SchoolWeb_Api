using School.Models.Faculty;
using School_DTOs;
using School_DTOs.Faculty;

namespace School.Services.Interfaces
{
    public interface IFacultyService
    {
        Task<APIResponse<FacultyDto>> AddFacultyAsync(FacultyModel model);
        Task<APIResponse<FacultyDto>> GetFacultyByIdAsync(int id);
        Task<APIResponse<IEnumerable<FacultyDto>>> GetAllFacultiesAsync();
        Task<APIResponse> UpdateFacultyAsync(FacultyModel model);
        Task<APIResponse> DeleteFacultyAsync(int id);
        Task<APIResponse> ToggleFacultyStatusAsync(int id);
    }
}

