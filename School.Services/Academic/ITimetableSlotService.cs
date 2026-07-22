using School_DTOs;
using School_DTOs.Academic;
namespace School.Services.Interfaces.Academic
{
    public interface ITimetableSlotService
    {
        Task<APIResponse<List<TimetableSlotDto>>> GetAllAsync();
        Task<APIResponse<TimetableSlotDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateTimetableSlotDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateTimetableSlotDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
}

