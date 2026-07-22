using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Infrastructure;
using School.Services.Interfaces;

namespace School.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly SchoolDbContext _context;

        public RolePermissionService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<RolePermission>> GetAllAsync(CancellationToken ct = default) =>
            await _context.RolePermissions
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
                .ToListAsync(ct);

        public async Task<RolePermission?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            await _context.RolePermissions
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
                .FirstOrDefaultAsync(rp => rp.Id == id, ct);

        public async Task<RolePermission> CreateAsync(RolePermission rolePermission, CancellationToken ct = default)
        {
            _context.RolePermissions.Add(rolePermission);
            await _context.SaveChangesAsync(ct);
            return rolePermission;
        }

        public async Task<RolePermission?> UpdateAsync(Guid id, RolePermission updated, CancellationToken ct = default)
        {
            var existing = await _context.RolePermissions.FirstOrDefaultAsync(rp => rp.Id == id, ct);
            if (existing == null) return null;

            existing.RoleId = updated.RoleId;
            existing.PermissionId = updated.PermissionId;

            await _context.SaveChangesAsync(ct);
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _context.RolePermissions.FirstOrDefaultAsync(rp => rp.Id == id, ct);
            if (entity == null) return false;
            _context.RolePermissions.Remove(entity);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IReadOnlyList<Microsoft.AspNetCore.Identity.IdentityRole>> GetRolesAsync(CancellationToken ct = default) =>
            await _context.Roles.ToListAsync(ct);

        public async Task<IReadOnlyList<Permission>> GetPermissionsAsync(CancellationToken ct = default) =>
            await _context.Permissions.ToListAsync(ct);
    }
}
