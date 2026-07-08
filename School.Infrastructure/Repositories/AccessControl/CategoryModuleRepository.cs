using Microsoft.EntityFrameworkCore;
using School.Domain.AccessControl;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories.AccessControl
{
    public class CategoryModuleRepository : Repository<CategoryModule>, ICategoryModuleRepository
    {
        private readonly SchoolDbContext _context;

        public CategoryModuleRepository(DbFactory dbFactory, SchoolDbContext context) : base(dbFactory)
        {
            _context = context;
        }

        public async Task<CategoryModule> AddCategoryModuleAsync(CategoryModule entity)
        {
            var alreadyExists = await DbSet.AnyAsync(m => m.Name == entity.Name && !m.IsDeleted);
            if (alreadyExists) return new CategoryModule { Id = 0 };
            return await base.AddAsync(entity);
        }

        public async Task<CategoryModule> GetCategoryModuleByIdAsync(int id)
        {
            return await FindAsync(x => x.Id == id && !x.IsDeleted) ?? new CategoryModule();
        }

        public async Task<IEnumerable<CategoryModule>> GetAllAsync()
        {
            return await List(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<int> UpdateCategoryModuleAsync(CategoryModule entity)
        {
            Attach(entity, x => x.Name, x => x.Description, x => x.Order, x => x.IsActive, x => x.UpdatedBy, x => x.UpdatedDate);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteCategoryModuleAsync(int id)
        {
            var entity = await FindAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null) return 0;
            Delete(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> ToggleCategoryModuleStatusAsync(int id)
        {
            var entity = await FindAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null) return 0;
            entity.IsActive = !entity.IsActive;
            Attach(entity, x => x.IsActive, x => x.UpdatedBy, x => x.UpdatedDate);
            return await _context.SaveChangesAsync();
        }
    }
}
