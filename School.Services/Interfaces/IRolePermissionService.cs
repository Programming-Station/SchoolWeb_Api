using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using School.Domain.Entities;

namespace School.Services.Interfaces
{
    public interface IRolePermissionService
    {
        Task<IReadOnlyList<RolePermission>> GetAllAsync(CancellationToken ct = default);
        Task<RolePermission?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<RolePermission> CreateAsync(RolePermission rolePermission, CancellationToken ct = default);
        Task<RolePermission?> UpdateAsync(Guid id, RolePermission rolePermission, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
