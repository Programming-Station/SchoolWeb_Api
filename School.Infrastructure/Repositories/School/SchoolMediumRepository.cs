using Microsoft.EntityFrameworkCore;
using School.Domain.School;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories
{
    public class SchoolMediumRepository : Repository<SchoolMedium>, ISchoolMediumRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public SchoolMediumRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public IQueryable<SchoolMedium> GetAllQueryable()
        {
            return List(x => true);
        }

        public async Task<SchoolMedium?> GetByIdAsync(int id)
        {
            return await FindAsync(x => x.Id == id);
        }

        public new async Task<int> AddAsync(SchoolMedium entity)
        {
            await _context.Set<SchoolMedium>().AddAsync(entity);
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> UpdateAsync(SchoolMedium entity)
        {
            var existing = await FindAsync(x => x.Id == entity.Id);
            if (existing != null)
            {
                existing.Name = entity.Name;
                existing.Description = entity.Description;
                existing.IsActive = entity.IsActive;
                existing.UpdatedDate = DateTime.UtcNow;
                _context.Set<SchoolMedium>().Update(existing);
                return await _unitOfWork.CommitAsync();
            }
            return 0;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await FindAsync(x => x.Id == id);
            if (entity != null)
            {
                _context.Set<SchoolMedium>().Remove(entity);
                return await _unitOfWork.CommitAsync();
            }
            return 0;
        }
    }
}


