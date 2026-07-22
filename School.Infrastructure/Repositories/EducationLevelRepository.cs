using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class EducationLevelRepository : Repository<EducationLevel>, IEducationLevelRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public EducationLevelRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<EducationLevel> AddAsync(EducationLevel entity)
        {
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<EducationLevel> GetByIdAsync(int id)
        {
            return await FindAsync(x => x.Id == id && !x.IsDeleted) ?? new EducationLevel();
        }

        public async Task<IEnumerable<EducationLevel>> GetAllAsync(bool? isActive = null)
        {
            if (isActive.HasValue)
            {
                return await List(x => !x.IsDeleted && x.Status == (isActive.Value ? "active" : "inactive")).ToListAsync();
            }
            return await List(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<int> UpdateAsync(EducationLevel entity)
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
