using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.UnitOfWork
{
    public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
    {
        private readonly SchoolDbContext _context;
        private readonly DbSet<T> _dbSet;
        public ReadOnlyRepository(SchoolDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet!.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(expression) ?? null;
        }
        public IQueryable<T> Query() => _context.Set<T>().AsNoTracking();
    }

}
