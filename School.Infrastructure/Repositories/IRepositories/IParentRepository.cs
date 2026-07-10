using School_DTOs;
using School_DTOs.Parent;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IParentRepository
    {
        /// <summary>Login by mobile + password. Returns JWT + all mapped children across tenants.</summary>
        Task<APIResponse<ParentLoginResponseDto>> ParentLoginAsync(ParentLoginModel model);

        /// <summary>Returns all children mapped to this parent (cross-tenant).</summary>
        Task<APIResponse<List<ChildSummaryDto>>> GetChildrenAsync(string parentUserId);

        /// <summary>Returns dashboard snapshot for one child, scoped to their school tenant.</summary>
        Task<APIResponse<ChildDashboardDto>> GetChildDashboardAsync(string parentUserId, int studentId, int schoolRegistrationId);
    }
}
