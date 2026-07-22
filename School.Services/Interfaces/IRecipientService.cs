using School_DTOs;
using School_DTOs.Communication.Recipients;

namespace School.Services.Interfaces
{
    public interface IRecipientService
    {
        Task<APIResponse<IEnumerable<RecipientDto>>> GetAllRecipientsAsync();
        Task<APIResponse<RecipientDto>> GetRecipientByIdAsync(int id);
        Task<APIResponse<RecipientDto>> CreateRecipientAsync(RecipientCreateDto dto);
        Task<APIResponse<bool>> DeleteRecipientAsync(int id);
        Task<APIResponse<IEnumerable<RecipientGroupDto>>> GetAllGroupsAsync();
    }
}
