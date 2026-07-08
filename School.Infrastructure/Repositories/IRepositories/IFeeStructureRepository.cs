using System.Collections.Generic;
using System.Threading.Tasks;
using School.Domain.FeeManagnment;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IFeeStructureRepository
    {
        Task<FeeStructure> AddAsync(FeeStructure entity);
        Task<FeeStructure> GetByIdAsync(int id);
        Task<FeeStructure?> GetFeeStructureAsync(int campusId, int programId, int batchId);
        Task<IEnumerable<FeeStructure>> GetAllAsync();
        Task<int> UpdateAsync(FeeStructure entity);
        Task<int> DeleteAsync(int id);
        Task<int> AddFeeStructureItemAsync(FeeStructureItem item);
        Task<int> RemoveFeeStructureItemsAsync(int feeStructureId);
    }
}
