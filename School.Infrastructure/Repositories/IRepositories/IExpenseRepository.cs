using System.Collections.Generic;
using System.Threading.Tasks;
using School.Domain.Finance;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetAllExpensesAsync();
        Task<Expense?> GetExpenseByIdAsync(int id);
    }
}
