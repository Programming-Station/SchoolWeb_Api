using System.Linq.Expressions;

namespace School.Infrastructure.UnitOfWork.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        Task<T> AddAsync(T entity);
        void AddRange(IEnumerable<T> entities);
        void Delete(T entity);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Attach(T entity, params Expression<Func<T, object>>[] updatedProperties);
        void AttachRange(IEnumerable<T> entities);
        T Find(Expression<Func<T, bool>> expression);
        Task<T?> FindAsync(Expression<Func<T, bool>> expression);
        Task<T?> FindAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        IQueryable<T> List(Expression<Func<T, bool>> expression);
        IQueryable<T> List(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        IQueryable<T> List();
    }
}
