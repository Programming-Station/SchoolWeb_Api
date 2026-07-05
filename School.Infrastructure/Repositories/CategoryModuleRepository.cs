using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace School.Infrastructure.Repositories
{
    public class CategoryModuleRepository : Repository<CategoryModule>, ICategoryModuleRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryModuleRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryModule> AddCategoryModuleAsync(CategoryModule entity)
        {
            var existingByName = await DbSet.FirstOrDefaultAsync(x =>
                               x.Name.ToLower() == entity.Name.ToLower() &&
                               !x.IsDeleted);

            if (existingByName != null)
            {
                existingByName.Id = 0;
                return existingByName;
            }

            await AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<CategoryModule> GetCategoryModuleByIdAsync(int id)
        {
            return await FindAsync(expression: x => x.Id == id && !x.IsDeleted) ?? new CategoryModule();
        }

        public async Task<IEnumerable<CategoryModule>> GetAllAsync()
        {
            return await List(expression: x => !x.IsDeleted)
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<int> UpdateCategoryModuleAsync(CategoryModule entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<CategoryModule, object>>[]
            {
                u => u.Name,
                u => u.Description!,
                u => u.Order,
                u => u.IsActive,
                u => u.UpdatedBy!,
                u => u.UpdatedDate!
            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<int> DeleteCategoryModuleAsync(int id)
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

        public async Task<int> ToggleCategoryModuleStatusAsync(int id)
        {
            var entity = await _context.CategoryModules
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

        public async Task<bool> IsCategoryInUseAsync(int categoryId)
        {
            return await _context.Modules
                .AnyAsync(m => m.CategoryModuleId == categoryId && !m.IsDeleted);
        }
    }
}
