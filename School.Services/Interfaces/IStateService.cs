using School.Models.State;
using School_DTOs;
using School_DTOs.State;

namespace School.Services.Interfaces
{
    public interface IStateService
    {
        Task<APIResponse<StateDto>> AddStateAsync(StateModel model);

        Task<APIResponse<StateDto>> GetStateByIdAsync(int id);

        Task<APIResponse<IEnumerable<StateDto>>> GetAllStatesAsync();

        Task<APIResponse> UpdateStateAsync(StateModel model);

        Task<APIResponse> DeleteStateAsync(int id);

        Task<APIResponse> ToggleStateStatusAsync(int id);
    }

}
