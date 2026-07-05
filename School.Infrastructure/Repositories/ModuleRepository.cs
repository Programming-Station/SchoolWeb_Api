using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using School.Domain.AccessControl;

namespace School.Infrastructure.Repositories
{
    public class ModuleRepository : Repository<Module>, IModuleRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ModuleRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Module> AddModuleAsync(Module entity)
        {
            var existingByRoute = await DbSet.FirstOrDefaultAsync(x =>
                               x.Route.ToLower() == entity.Route.ToLower() &&
                               !x.IsDeleted);

            if (existingByRoute != null)
            {
                existingByRoute.Id = 0;
                return existingByRoute;
            }

            await AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<Module> GetModuleByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.CategoryModule)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new Module();
        }

        public async Task<IEnumerable<Module>> GetAllAsync()
        {
            return await List(expression: x => !x.IsDeleted)
                .Include(x => x.CategoryModule)
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<int> UpdateModuleAsync(Module entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<Module, object>>[]
            {
                u => u.Name,
                u => u.Description!,
                u => u.Route,
                u => u.Icon!,
                u => u.CategoryModuleId,
                u => u.Order,
                u => u.IsActive,
                u => u.UpdatedBy!,
                u => u.UpdatedDate
            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<int> DeleteModuleAsync(int id)
        {
            var result = await FindAsync(expression: x => x.Id == id && !x.IsDeleted);

            if (result != null)
            {
                result.UpdatedDate = DateTime.UtcNow;
                Delete(result);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }

        public async Task<int> ToggleModuleStatusAsync(int id)
        {
            var entity = await _context.Modules
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

            if (entity != null)
            {
                entity.IsActive = !entity.IsActive;
                Update(entity);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
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

            var newPermissions = moduleIds.Select(moduleId => new ModulePermission
            {
                ModuleId = moduleId,
                UserId = userId,
                IsActive = true,
                CreatedBy = createdBy,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            }).ToList();

            _context.ModulePermissions.AddRange(newPermissions);
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<int> RemoveModulePermissionAsync(int moduleId, string userId)
        {
            var permission = await _context.ModulePermissions
                .FirstOrDefaultAsync(mp => mp.ModuleId == moduleId && mp.UserId == userId && !mp.IsDeleted);

            if (permission != null)
            {
                permission.IsDeleted = true;
                permission.UpdatedDate = DateTime.UtcNow;
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
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
