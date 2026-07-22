using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class BranchRepository : Repository<Branch>, IBranchRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public BranchRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Branch> AddAsync(Branch entity)
        {
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<Branch> GetByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.Program)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new Branch();
        }

        public async Task<IEnumerable<Branch>> GetAllAsync(int? programId = null, bool? isActive = null)
        {
            var query = DbSet
                .Include(x => x.Program)
                .Where(x => !x.IsDeleted);

            if (programId.HasValue)
            {
                query = query.Where(x => x.ProgramId == programId.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(x => x.Status == (isActive.Value ? "active" : "inactive"));
            }

            return await query.ToListAsync();
        }

        public async Task<int> UpdateAsync(Branch entity)
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
    }
}
