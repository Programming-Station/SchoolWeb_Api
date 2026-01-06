using Microsoft.EntityFrameworkCore;

namespace School.Infrastructure
{
    public class DbFactory(Func<SchoolDbContext> dbContextFactory) : IDisposable
    {
        private bool _disposed;
        private Func<SchoolDbContext> _instanceFunc = dbContextFactory;
        private DbContext? _dbContext;
        public DbContext DbContext => _dbContext ?? (_dbContext = _instanceFunc.Invoke());

        public void Dispose()
        {
            if (!_disposed && _dbContext != null)
            {
                _disposed = true;
                _dbContext.Dispose();
            }
        }
    }
}
