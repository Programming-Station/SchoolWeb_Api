using School.Domain;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IAcademicYearRepository
    {
        Task<AcademicYear> AddAcademicYearAsync(AcademicYear entity);
        Task<AcademicYear> GetAcademicYearByIdAsync(int id);
        Task<IEnumerable<AcademicYear>> GetAllAcademicYearsAsync(bool? isActive = null);
        Task<AcademicYear> GetCurrentAcademicYearAsync();
        Task<int> UpdateAcademicYearAsync(AcademicYear entity);
        Task<int> DeleteAcademicYearAsync(int id);
        Task<int> ToggleAcademicYearStatusAsync(int id);
        Task<int> SetCurrentAcademicYearAsync(int id);
    }
}

