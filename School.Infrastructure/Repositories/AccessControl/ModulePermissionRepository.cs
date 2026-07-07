using Microsoft.EntityFrameworkCore;
using School.Domain.AccessControl;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories.AccessControl
{
    public class ModulePermissionRepository : Repository<ModulePermission>, IModulePermissionRepository
    {
        private readonly SchoolDbContext _context;

        public ModulePermissionRepository(DbFactory dbFactory, SchoolDbContext context) : base(dbFactory)
        {
            _context = context;
        }

        public async Task<ModulePermission> AddModulePermissionAsync(ModulePermission entity)
        {
            var alreadyExists = await DbSet.AnyAsync(x => x.ModuleId == entity.ModuleId && x.UserId == entity.UserId && !x.IsDeleted);
            if (alreadyExists) return new ModulePermission { Id = 0 };

            entity.IsDeleted = false;
            await AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<ModulePermission> GetModulePermissionByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.Module)
                .Include(x => x.User)
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new ModulePermission();
        }

        public async Task<IEnumerable<ModulePermission>> GetAllModulePermissionsAsync()
        {
            return await DbSet
                .Include(x => x.Module)
                .Include(x => x.User)
                .Include(x => x.Role)
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> UpdateModulePermissionAsync(ModulePermission entity)
        {
            var existing = await DbSet.FirstOrDefaultAsync(x => x.Id == entity.Id && !x.IsDeleted);
            if (existing == null) return 0;

            existing.ModuleId = entity.ModuleId;
            existing.UserId = entity.UserId;
            existing.RoleId = entity.RoleId;
            existing.IsActive = entity.IsActive;
            existing.UpdatedBy = entity.UpdatedBy;
            existing.UpdatedDate = DateTime.UtcNow;

            Update(existing);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteModulePermissionAsync(int id)
        {
            var entity = await FindAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null) return 0;
            Delete(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> ToggleModulePermissionStatusAsync(int id)
        {
            var entity = await DbSet.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
            if (entity != null)
            {
                entity.IsActive = !entity.IsActive;
                entity.UpdatedDate = DateTime.UtcNow;
                Update(entity);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<Module>> GetModulesByUserIdAsync(string userId)
        {
            return await _context.ModulePermissions
                .Where(mp => mp.UserId == userId && mp.IsActive && !mp.IsDeleted)
                .Include(mp => mp.Module)
                    .ThenInclude(m => m!.CategoryModule)
                .Where(mp => mp.Module != null && !mp.Module!.IsDeleted && mp.Module!.IsActive)
                .Select(mp => mp.Module!)
                .OrderBy(m => m.Order)
                .ThenBy(m => m.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<int> AssignModulesToUserAsync(string userId, List<int> moduleIds, string? createdBy = null)
        {
            var existingPermissions = await _context.ModulePermissions
                .Where(mp => mp.UserId == userId && !mp.IsDeleted)
                .ToListAsync();

            foreach (var perm in existingPermissions)
            {
                perm.IsDeleted = true;
                perm.UpdatedDate = DateTime.UtcNow;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var defaultSchool = await _context.SchoolRegistrations.FirstOrDefaultAsync(s => s.SchoolCode == "DPSVAR001")
                                ?? await _context.SchoolRegistrations.FirstOrDefaultAsync(s => s.SchoolCode == "DEF001")
                                ?? await _context.SchoolRegistrations.FirstOrDefaultAsync();
            int schoolRegistrationId = user?.SchoolRegistrationId ?? defaultSchool?.Id ?? 1;

            var newPermissions = moduleIds.Select(moduleId => new ModulePermission
            {
                ModuleId = moduleId,
                UserId = userId,
                SchoolRegistrationId = schoolRegistrationId,
                IsActive = true,
                CreatedBy = createdBy,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            }).ToList();

            _context.ModulePermissions.AddRange(newPermissions);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveModulePermissionAsync(int moduleId, string userId)
        {
            var permission = await _context.ModulePermissions
                .FirstOrDefaultAsync(mp => mp.ModuleId == moduleId && mp.UserId == userId && !mp.IsDeleted);

            if (permission != null)
            {
                permission.IsDeleted = true;
                permission.UpdatedDate = DateTime.UtcNow;
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<ModulePermission>> GetModulePermissionsByUserIdAsync(string userId)
        {
            return await _context.ModulePermissions
                .Where(mp => mp.UserId == userId && !mp.IsDeleted)
                .Include(mp => mp.Module)
                .Include(mp => mp.User)
                .Include(mp => mp.Role)
                .ToListAsync();
        }
    }
}
