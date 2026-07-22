using Microsoft.EntityFrameworkCore;
using School.Domain.Hr;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<Employee?> GetEmployeeWithDetailsAsync(int id)
        {
            return await DbSet
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Include(e => e.Documents)
                .Include(e => e.BankDetails)
                .Include(e => e.Educations)
                .Include(e => e.Experiences)
                .Include(e => e.SalaryDetails)
                .Include(e => e.EmployeeDetail)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesWithDetailsAsync()
        {
            return await DbSet
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .ToListAsync();
        }

        public async Task<bool> IsDuplicateEmployeeAsync(string email, string mobile, string? aadhaar, string? pan, int? excludeId = null)
        {
            var query = DbSet.AsQueryable();
            if (excludeId.HasValue)
            {
                query = query.Where(e => e.Id != excludeId.Value);
            }

            return await query.AnyAsync(e =>
                e.Email == email ||
                e.MobileNumber == mobile ||
                (e.EmployeeDetail != null && !string.IsNullOrEmpty(aadhaar) && e.EmployeeDetail.AadhaarNumber == aadhaar) ||
                (e.EmployeeDetail != null && !string.IsNullOrEmpty(pan) && e.EmployeeDetail.PANNumber == pan)
            );
        }

        public async Task<int> BulkDeleteAsync(IEnumerable<int> ids, string username)
        {
            return await DbSet
                .Where(e => ids.Contains(e.Id))
                .ExecuteUpdateAsync(s => s
                    .SetProperty(e => e.IsDeleted, true)
                    .SetProperty(e => e.UpdatedBy, username)
                    .SetProperty(e => e.UpdatedDate, DateTime.Now));
        }

        public async Task<int> BulkRestoreAsync(IEnumerable<int> ids, string username)
        {
            return await DbSet
                .IgnoreQueryFilters()
                .Where(e => ids.Contains(e.Id) && e.IsDeleted)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(e => e.IsDeleted, false)
                    .SetProperty(e => e.UpdatedBy, username)
                    .SetProperty(e => e.UpdatedDate, DateTime.Now));
        }

        public async Task<int> BulkStatusUpdateAsync(IEnumerable<int> ids, string status, string username)
        {
            return await DbSet
                .Where(e => ids.Contains(e.Id))
                .ExecuteUpdateAsync(s => s
                    .SetProperty(e => e.Status, status)
                    .SetProperty(e => e.UpdatedBy, username)
                    .SetProperty(e => e.UpdatedDate, DateTime.Now));
        }
    }
}
