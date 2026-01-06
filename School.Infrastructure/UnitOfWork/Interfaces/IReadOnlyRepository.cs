using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace School.Infrastructure.UnitOfWork.Interfaces
{
    public interface IReadOnlyRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetFirstAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> Query();
    }
}
