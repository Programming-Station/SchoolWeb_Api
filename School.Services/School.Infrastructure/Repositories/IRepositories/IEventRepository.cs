using School.Domain;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IEventRepository
    {
        Task<Event> AddEventAsync(Event entity);
        Task<Event> GetEventByIdAsync(int id);
        Task<IEnumerable<Event>> GetAllEventsAsync(bool? upcomingOnly = null);
        Task<int> UpdateEventAsync(Event entity);
        Task<int> DeleteEventAsync(int id);
        Task<int> ToggleEventStatusAsync(int id);
    }
}

