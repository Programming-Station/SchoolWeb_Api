using School.Domain.Location;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IStateRepository
    {
        Task<State> AddStateAsync(State entity);

        Task<State> GetStateByIdAsync(int id);

        Task<IEnumerable<State>> GetAllAsync();

        Task<int> UpdateStateAsync(State entity);

        Task<int> DeleteStateAsync(int id);

        Task<int> ToggleStateStatusAsync(int id);
    }
}
