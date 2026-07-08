using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Student;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class AdmissionRuleRepository : Repository<AdmissionRule>, IAdmissionRuleRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public AdmissionRuleRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<AdmissionRule> AddAsync(AdmissionRule entity)
        {
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<AdmissionRule> GetByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.Campus)
                .Include(x => x.EducationLevel)
                .Include(x => x.Program)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new AdmissionRule();
        }

        public async Task<IEnumerable<AdmissionRule>> GetRulesAsync(int campusId, int educationLevelId, int? programId = null)
        {
            var query = DbSet
                .Where(x => x.CampusId == campusId && 
                            x.EducationLevelId == educationLevelId && 
                            x.IsActive && !x.IsDeleted);

            if (programId.HasValue)
            {
                query = query.Where(x => x.ProgramId == null || x.ProgramId == programId.Value);
            }
            else
            {
                query = query.Where(x => x.ProgramId == null);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<AdmissionRule>> GetAllAsync()
        {
            return await DbSet
                .Include(x => x.Campus)
                .Include(x => x.EducationLevel)
                .Include(x => x.Program)
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> UpdateAsync(AdmissionRule entity)
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
