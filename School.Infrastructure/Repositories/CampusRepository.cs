using Microsoft.EntityFrameworkCore;
using School.Domain.School;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class CampusRepository : Repository<Campus>, ICampusRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CampusRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Campus> AddAsync(Campus entity)
        {
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<Campus> GetByIdAsync(int id)
        {
            return await FindAsync(x => x.Id == id && !x.IsDeleted) ?? new Campus();
        }

        public async Task<IEnumerable<Campus>> GetAllAsync(bool? isActive = null)
        {
            if (isActive.HasValue)
            {
                return await List(x => !x.IsDeleted && x.Status == (isActive.Value ? "active" : "inactive")).ToListAsync();
            }
            return await List(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<int> UpdateAsync(Campus entity)
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
