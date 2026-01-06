using Microsoft.EntityFrameworkCore.Storage;

namespace School.Infrastructure.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> CommitAsync();
        Task CommitWithTnxAsync();
        Task CommitWithTnxAsync(IDbContextTransaction transaction);
        Task RollbackAsync();
    }

}
