using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace School.Infrastructure.Repositories
{
    public class FacultyRepository : Repository<Faculty>, IFacultyRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public FacultyRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Faculty> AddFacultyAsync(Faculty entity)
        {
            var existingFaculty = await DbSet.FirstOrDefaultAsync(x =>
                               (x.Name.ToLower() == entity.Name.ToLower() || 
                                (!string.IsNullOrEmpty(entity.Code) && x.Code != null && x.Code.ToLower() == entity.Code.ToLower())) &&
                               !x.IsDeleted);

            if (existingFaculty != null)
            {
                existingFaculty.Id = 0;
                return existingFaculty;
            }

            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<int> DeleteFacultyAsync(int id)
        {
            var result = await FindAsync(expression: x => x.Id == id && !x.IsDeleted);

            if (result != null)
            {
                result.UpdatedDate = DateTime.Now;
                Delete(result);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }

        public async Task<IEnumerable<Faculty>> GetAllFacultiesAsync()
        {
            return await List(expression: x => !x.IsDeleted && x.Status == "active")
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Faculty> GetFacultyByIdAsync(int id)
        {
            return await FindAsync(expression: x => x.Id == id && !x.IsDeleted) ?? new Faculty();
        }

        public async Task<int> ToggleFacultyStatusAsync(int id)
        {
            var entity = await _context.Faculties
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);

            if (entity != null)
            {
                entity.Status = entity.Status == "active" ? "inactive" : "active";
                Update(entity);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }

        public async Task<int> UpdateFacultyAsync(Faculty entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<Faculty, object>>[]
            {
                f => f.Name,
                f => f.Code!,
                f => f.Description!,
                f => f.DisplayOrder,
                f => f.Status,
                f => f.UpdatedBy!,
            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}

