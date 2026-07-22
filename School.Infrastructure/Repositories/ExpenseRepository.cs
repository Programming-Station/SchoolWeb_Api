using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Finance;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        private readonly SchoolDbContext _context;

        public ExpenseRepository(DbFactory dbFactory, SchoolDbContext context) : base(dbFactory)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
        {
            return await _context.Expenses
                .Include(e => e.CoaAccount)
                .Include(e => e.Vendor)
                .ToListAsync();
        }

        public async Task<Expense?> GetExpenseByIdAsync(int id)
        {
            return await _context.Expenses
                .Include(e => e.CoaAccount)
                .Include(e => e.Vendor)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
