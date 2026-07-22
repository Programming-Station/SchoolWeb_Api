using School_DTOs;
using School_DTOs.Administration;

namespace School.Services.Interfaces
{
    public interface IComplaintService
    {
        Task<APIResponse<List<ComplaintDto>>> GetComplaintsAsync(ComplaintFilterDto filter, int schoolId);
        Task<APIResponse<ComplaintDto>> GetComplaintByIdAsync(int id, int schoolId);
        Task<APIResponse<ComplaintDto>> CreateComplaintAsync(CreateComplaintDto dto, string userId, int schoolId);
        Task<APIResponse<ComplaintDto>> UpdateComplaintAsync(int id, ComplaintDto dto, int schoolId);
        Task<APIResponse<bool>> AssignComplaintAsync(int id, string assignedToUserId, string assignedToName, int schoolId);
        Task<APIResponse<bool>> ResolveComplaintAsync(int id, string resolutionDetails, string resolvedBy, int schoolId);
        Task<APIResponse<bool>> EscalateComplaintAsync(int id, string notes, int schoolId);
        Task<APIResponse<bool>> DeleteComplaintAsync(int id, int schoolId);
    }
}
