using School.Domain.Hr;
using School.Infrastructure.UnitOfWork.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> GetEmployeeWithDetailsAsync(int id);
        Task<IEnumerable<Employee>> GetAllEmployeesWithDetailsAsync();
        Task<bool> IsDuplicateEmployeeAsync(string email, string mobile, string? aadhaar, string? pan, int? excludeId = null);
        Task<Employee?> GetByIdAsync(int id);
        Task<int> BulkDeleteAsync(IEnumerable<int> ids, string username);
        Task<int> BulkRestoreAsync(IEnumerable<int> ids, string username);
        Task<int> BulkStatusUpdateAsync(IEnumerable<int> ids, string status, string username);
    }
}
