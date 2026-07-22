using School.Domain;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ISectionRepository
    {
        Task<Section> AddSectionAsync(Section entity);
        Task<Section> GetSectionByIdAsync(int id);
        Task<IEnumerable<Section>> GetAllSectionsAsync();
        Task<IEnumerable<Section>> GetSectionsByClassIdAsync(int classId);
        Task<int> UpdateSectionAsync(Section entity);
        Task<int> DeleteSectionAsync(int id);
        Task<int> ToggleSectionStatusAsync(int id);
    }
}
