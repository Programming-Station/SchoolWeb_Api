using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Finance;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class IncomeRepository : Repository<Income>, IIncomeRepository
    {
        private readonly SchoolDbContext _context;

        public IncomeRepository(DbFactory dbFactory, SchoolDbContext context) : base(dbFactory)
        {
            _context = context;
        }

        public async Task<IEnumerable<Income>> GetAllIncomesAsync()
        {
            return await _context.Incomes
                .Include(i => i.CoaAccount)
                .ToListAsync();
        }

        public async Task<Income?> GetIncomeByIdAsync(int id)
        {
            return await _context.Incomes
                .Include(i => i.CoaAccount)
                .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
