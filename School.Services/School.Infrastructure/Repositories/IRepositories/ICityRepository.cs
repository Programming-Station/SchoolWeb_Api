using School.Domain; 

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ICityRepository
    {
        Task<City> AddCityAsync(City entity);

        Task<City> GetCityByIdAsync(int id); 

        Task<IEnumerable<City>> GetAllAsync(int? stateId = null);

        Task<int> UpdateCityAsync(City entity);

        Task<int> DeleteCityAsync(int id);

        Task<int> ToggleCityStatusAsync(int id);
    }
}
