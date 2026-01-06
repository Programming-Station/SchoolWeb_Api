using School.Models.Event;
using School_DTOs;
using School_DTOs.Event;

namespace School.Services.Interfaces
{
    public interface IEventService
    {
        Task<APIResponse<EventDto>> AddEventAsync(EventModel model);
        Task<APIResponse<EventDto>> GetEventByIdAsync(int id);
        Task<APIResponse<IEnumerable<EventDto>>> GetAllEventsAsync(bool? upcomingOnly = null);
        Task<APIResponse> UpdateEventAsync(EventModel model);
        Task<APIResponse> DeleteEventAsync(int id);
        Task<APIResponse> ToggleEventStatusAsync(int id);
    }
}

