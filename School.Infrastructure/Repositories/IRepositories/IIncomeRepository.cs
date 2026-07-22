using System.Collections.Generic;
using System.Threading.Tasks;
using School.Domain.Finance;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IIncomeRepository : IRepository<Income>
    {
        Task<IEnumerable<Income>> GetAllIncomesAsync();
        Task<Income?> GetIncomeByIdAsync(int id);
    }
}
