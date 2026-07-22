using School.Models.Class;
using School_DTOs;
using School_DTOs.Class;

namespace School.Services.Interfaces
{
    public interface ISectionService
    {
        Task<APIResponse<SectionDto>> AddSectionAsync(SectionModel model);
        Task<APIResponse<SectionDto>> GetSectionByIdAsync(int id);
        Task<APIResponse<IEnumerable<SectionDto>>> GetAllSectionsAsync();
        Task<APIResponse<IEnumerable<SectionDto>>> GetSectionsByClassIdAsync(int classId);
        Task<APIResponse> UpdateSectionAsync(SectionModel model);
        Task<APIResponse> DeleteSectionAsync(int id);
        Task<APIResponse> ToggleSectionStatusAsync(int id);
    }
}
