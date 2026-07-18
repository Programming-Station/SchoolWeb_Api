using System.Collections.Generic;
using System.Threading.Tasks;
using School_DTOs;
using School_DTOs.Administration;

namespace School.Services.Interfaces
{
    public interface IVisitorService
    {
        Task<APIResponse<List<VisitorDto>>> GetVisitorsAsync(VisitorFilterDto filter, int schoolId);
        Task<APIResponse<VisitorDto>> GetVisitorByIdAsync(int id, int schoolId);
        Task<APIResponse<VisitorDto>> CheckInVisitorAsync(CreateVisitorDto dto, string userId, int schoolId);
        Task<APIResponse<bool>> CheckOutVisitorAsync(int id, int schoolId);
        Task<APIResponse<bool>> DeleteVisitorAsync(int id, int schoolId);
    }
}
