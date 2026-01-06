using School.Models.AcademicYear;
using School_DTOs;
using School_DTOs.AcademicYear;

namespace School.Services.Interfaces
{
    public interface IAcademicYearService
    {
        Task<APIResponse<AcademicYearDto>> AddAcademicYearAsync(AcademicYearModel model);
        Task<APIResponse<AcademicYearDto>> GetAcademicYearByIdAsync(int id);
        Task<APIResponse<IEnumerable<AcademicYearDto>>> GetAllAcademicYearsAsync(bool? isActive = null);
        Task<APIResponse<AcademicYearDto>> GetCurrentAcademicYearAsync();
        Task<APIResponse> UpdateAcademicYearAsync(AcademicYearModel model);
        Task<APIResponse> DeleteAcademicYearAsync(int id);
        Task<APIResponse> ToggleAcademicYearStatusAsync(int id);
        Task<APIResponse> SetCurrentAcademicYearAsync(int id);
    }
}

