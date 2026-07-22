using School_DTOs;
using School_DTOs.Common;

namespace School.Services.Hr
{
    public interface IHrMasterService<TEntity> where TEntity : class, Domain.BaseEntity.IAuditEntity<int>, Domain.BaseEntity.ITenantEntity
    {
        Task<APIResponse<PagedResponse<object>>> GetAllAsync(PaginationFilterDto filter);
        Task<APIResponse<object>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(object dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, object dto, string username);
        Task<APIResponse<bool>> DeleteAsync(int id, string username);
        Task<APIResponse<bool>> ToggleStatusAsync(int id, string username);
        Task<APIResponse<IEnumerable<DropdownDto>>> GetLookupAsync();
        Task<APIResponse<bool>> BulkDeleteAsync(IEnumerable<int> ids, string username);
        Task<APIResponse<bool>> BulkStatusChangeAsync(IEnumerable<int> ids, string status, string username);
    }
}
