using School.Infrastructure;
using School.Infrastructure.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static School.Domain.BaseEntity;

namespace School.Infrastructure.UnitOfWork
{
    public class Repository<T>(DbFactory dbFactory) : IRepository<T> where T : class
    {
        public readonly DbFactory _dbFactory = dbFactory;
        public DbSet<T>? _dbSet;

        protected DbSet<T> DbSet
        {
            get => _dbSet ?? (_dbSet = _dbFactory.DbContext.Set<T>());
        }

        public void Add(T entity)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            {
                ((IAuditEntity)entity).CreatedDate = DateTime.Now;
            }
            DbSet.Add(entity);
        }
        public async Task<T> AddAsync(T entity)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            {
                ((IAuditEntity)entity).CreatedDate = DateTime.Now;
            }
            await DbSet.AddAsync(entity);
            return entity;
        }
        public void AddRange(IEnumerable<T> entities)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            {
                foreach (var entity in entities)
                {
                    ((IAuditEntity)entity).CreatedDate = DateTime.Now;
                }
            }
            DbSet.AddRange(entities);
        }
        public void Update(T entity)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            {
                ((IAuditEntity)entity).UpdatedDate = DateTime.Now;
            }
            DbSet.Entry(entity).State = EntityState.Modified;
        }
        public void UpdateRange(IEnumerable<T> entities)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            {
                foreach (var entity in entities)
                {
                    ((IAuditEntity)entity).UpdatedDate = DateTime.Now;
                }
            }
            DbSet.UpdateRange(entities);
        }
        public void Attach(T entity, params Expression<Func<T, object>>[] updatedProperties)
        {
            foreach (var property in updatedProperties)
            {
                DbSet.Entry(entity).Property(property).IsModified = true;
            }

            if (entity is IAuditEntity audit)
                audit.UpdatedDate = DateTime.Now;
        }
        public void AttachRange(IEnumerable<T> entities)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            {
                foreach (var entity in entities)
                {
                    ((IAuditEntity)entity).UpdatedDate = DateTime.Now;
                }
            }
            DbSet.AttachRange(entities);
        }
        public void Delete(T entity)
        {
            if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
            {
                ((IDeleteEntity)entity).IsDeleted = true;
                DbSet.Update(entity);
            }
            else
                DbSet.Remove(entity);
        }

        public IQueryable<T> List(Expression<Func<T, bool>> expression)
        {
            return DbSet.Where(expression);
        }
        public IQueryable<T> List(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = DbSet;
            // Include related entities
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.Where(expression);
        }
        public IQueryable<T> List()
        {
            return DbSet;
        }

        public T Find(Expression<Func<T, bool>> expression)
        {
            return DbSet.FirstOrDefault(expression)!;
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await DbSet.FirstOrDefaultAsync(expression) ?? null;
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = DbSet;
            // Include related entities
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(expression);
        }
    }
}
