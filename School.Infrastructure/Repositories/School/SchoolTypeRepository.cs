using Microsoft.EntityFrameworkCore;
using School.Domain.School;
using School.Infrastructure.Repositories.School;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories
{
    public class SchoolTypeRepository : Repository<SchoolType>, ISchoolTypeRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public SchoolTypeRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public IQueryable<SchoolType> GetAllQueryable()
        {
            return List(x => true);
        }

        public async Task<SchoolType?> GetByIdAsync(int id)
        {
            return await FindAsync(x => x.Id == id);
        }

        public new async Task<int> AddAsync(SchoolType entity)
        {
            await _context.Set<SchoolType>().AddAsync(entity);
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> UpdateAsync(SchoolType entity)
        {
            var existing = await FindAsync(x => x.Id == entity.Id);
            if (existing != null)
            {
                existing.Name = entity.Name;
                existing.Description = entity.Description;
                existing.IsActive = entity.IsActive;
                existing.UpdatedDate = DateTime.UtcNow;
                _context.Set<SchoolType>().Update(existing);
                return await _unitOfWork.CommitAsync();
            }
            return 0;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await FindAsync(x => x.Id == id);
            if (entity != null)
            {
                _context.Set<SchoolType>().Remove(entity);
                return await _unitOfWork.CommitAsync();
            }
            return 0;
        }
    }
}


