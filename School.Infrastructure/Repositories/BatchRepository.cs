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
    public class BatchRepository : Repository<Batch>, IBatchRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public BatchRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Batch> AddAsync(Batch entity)
        {
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<Batch> GetByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.Program)
                .Include(x => x.AcademicYear)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new Batch();
        }

        public async Task<IEnumerable<Batch>> GetAllAsync(int? programId = null, bool? isActive = null)
        {
            var query = DbSet
                .Include(x => x.Program)
                .Include(x => x.AcademicYear)
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

        public async Task<int> UpdateAsync(Batch entity)
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
