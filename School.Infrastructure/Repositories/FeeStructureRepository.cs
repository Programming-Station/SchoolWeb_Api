using Microsoft.EntityFrameworkCore;
using School.Domain.FeeManagnment;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class FeeStructureRepository : Repository<FeeStructure>, IFeeStructureRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public FeeStructureRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<FeeStructure> AddAsync(FeeStructure entity)
        {
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<FeeStructure> GetByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.Campus)
                .Include(x => x.Program)
                .Include(x => x.Batch)
                .Include(x => x.FeeStructureItems)
                    .ThenInclude(i => i.FeeType)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new FeeStructure();
        }

        public async Task<FeeStructure?> GetFeeStructureAsync(int campusId, int programId, int batchId)
        {
            return await DbSet
                .Include(x => x.Campus)
                .Include(x => x.Program)
                .Include(x => x.Batch)
                .Include(x => x.FeeStructureItems)
                    .ThenInclude(i => i.FeeType)
                .FirstOrDefaultAsync(x => x.CampusId == campusId &&
                                           x.ProgramId == programId &&
                                           x.BatchId == batchId &&
                                           x.IsActive && !x.IsDeleted);
        }

        public async Task<IEnumerable<FeeStructure>> GetAllAsync()
        {
            return await DbSet
                .Include(x => x.Campus)
                .Include(x => x.Program)
                .Include(x => x.Batch)
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> UpdateAsync(FeeStructure entity)
        {
            entity.UpdatedDate = DateTime.Now;
            Update(entity);
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await FindAsync(x => x.Id == id && !x.IsDeleted);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.UpdatedDate = DateTime.Now;
                Update(entity);
                return await _unitOfWork.CommitAsync();
            }
            return 0;
        }

        public async Task<int> AddFeeStructureItemAsync(FeeStructureItem item)
        {
            await _context.FeeStructureItems.AddAsync(item);
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> RemoveFeeStructureItemsAsync(int feeStructureId)
        {
            var items = await _context.FeeStructureItems
                .Where(x => x.FeeStructureId == feeStructureId)
                .ToListAsync();

            if (items.Any())
            {
                _context.FeeStructureItems.RemoveRange(items);
                return await _unitOfWork.CommitAsync();
            }
            return 0;
        }
    }
}
