using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class YearSemesterRepository : Repository<YearSemester>, IYearSemesterRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public YearSemesterRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<YearSemester> AddAsync(YearSemester entity)
        {
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<YearSemester> GetByIdAsync(int id)
        {
            return await FindAsync(x => x.Id == id && !x.IsDeleted) ?? new YearSemester();
        }

        public async Task<IEnumerable<YearSemester>> GetAllAsync(bool? isActive = null)
        {
            var query = DbSet.Where(x => !x.IsDeleted);
            if (isActive.HasValue)
            {
                query = query.Where(x => x.Status == (isActive.Value ? "active" : "inactive"));
            }
            return await query.OrderBy(x => x.Sequence).ToListAsync();
        }

        public async Task<int> UpdateAsync(YearSemester entity)
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
