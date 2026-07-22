using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class ProgramRepository : Repository<Program>, IProgramRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ProgramRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Program> AddAsync(Program entity)
        {
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<Program> GetByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.EducationLevel)
                .Include(x => x.Faculty)
                .Include(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new Program();
        }

        public async Task<IEnumerable<Program>> GetAllAsync(int? educationLevelId = null, bool? isActive = null)
        {
            var query = DbSet
                .Include(x => x.EducationLevel)
                .Include(x => x.Faculty)
                .Include(x => x.Department)
                .Where(x => !x.IsDeleted);

            if (educationLevelId.HasValue)
            {
                query = query.Where(x => x.EducationLevelId == educationLevelId.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(x => x.Status == (isActive.Value ? "active" : "inactive"));
            }

            return await query.ToListAsync();
        }

        public async Task<int> UpdateAsync(Program entity)
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
