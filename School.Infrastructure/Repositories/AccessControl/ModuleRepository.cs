using Microsoft.EntityFrameworkCore;
using School.Domain.AccessControl;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories.AccessControl
{
    public class ModuleRepository : Repository<Module>, IModuleRepository
    {
        private readonly SchoolDbContext _context;

        public ModuleRepository(DbFactory dbFactory, SchoolDbContext context) : base(dbFactory)
        {
            _context = context;
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

            entity.IsDeleted = false;
            await base.AddAsync(entity);
            await _context.SaveChangesAsync();
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
            var existing = await DbSet.FirstOrDefaultAsync(x => x.Id == entity.Id && !x.IsDeleted);
            if (existing == null) return 0;

            existing.Name = entity.Name;
            existing.Description = entity.Description;
            existing.Route = entity.Route;
            existing.Icon = entity.Icon;
            existing.CategoryModuleId = entity.CategoryModuleId;
            existing.Order = entity.Order;
            existing.IsActive = entity.IsActive;
            existing.UpdatedBy = entity.UpdatedBy;
            existing.UpdatedDate = DateTime.UtcNow;

            Update(existing);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteModuleAsync(int id)
        {
            var result = await FindAsync(x => x.Id == id && !x.IsDeleted);
            if (result != null)
            {
                result.UpdatedDate = DateTime.UtcNow;
                Delete(result);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> ToggleModuleStatusAsync(int id)
        {
            var entity = await _context.Modules
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

            if (entity != null)
            {
                entity.IsActive = !entity.IsActive;
                entity.UpdatedDate = DateTime.UtcNow;
                Update(entity);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}
