using Microsoft.EntityFrameworkCore.Storage;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.UnitOfWork
{
    public class UnitOfWork1 : IUnitOfWork
    {
        private DbFactory _dbFactory;
        private IDbContextTransaction? _transaction;
        public UnitOfWork1(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbFactory.DbContext.Database.BeginTransactionAsync();
        }
        public Task<int> CommitAsync()
        {
            return _dbFactory.DbContext.SaveChangesAsync();
        }
        public async Task CommitWithTnxAsync()
        {
            await _dbFactory.DbContext.SaveChangesAsync();

            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        public async Task CommitWithTnxAsync(IDbContextTransaction transaction)
        {
            await _dbFactory.DbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }


    }
}
