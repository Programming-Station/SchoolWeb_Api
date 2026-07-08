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
    public class AdmissionFormConfigRepository : Repository<AdmissionFormConfig>, IAdmissionFormConfigRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public AdmissionFormConfigRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<AdmissionFormConfig> AddAsync(AdmissionFormConfig entity)
        {
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<AdmissionFormConfig> GetByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.Campus)
                .Include(x => x.EducationLevel)
                .Include(x => x.Program)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new AdmissionFormConfig();
        }

        public async Task<AdmissionFormConfig?> GetConfigAsync(int campusId, int educationLevelId, int? programId)
        {
            // First search for program-specific config
            if (programId.HasValue)
            {
                var config = await DbSet
                    .Include(x => x.Campus)
                    .Include(x => x.EducationLevel)
                    .Include(x => x.Program)
                    .FirstOrDefaultAsync(x => x.CampusId == campusId && 
                                               x.EducationLevelId == educationLevelId && 
                                               x.ProgramId == programId.Value && 
                                               x.IsActive && !x.IsDeleted);
                if (config != null) return config;
            }

            // Fall back to level-level default config for that campus
            return await DbSet
                .Include(x => x.Campus)
                .Include(x => x.EducationLevel)
                .Include(x => x.Program)
                .FirstOrDefaultAsync(x => x.CampusId == campusId && 
                                           x.EducationLevelId == educationLevelId && 
                                           x.ProgramId == null && 
                                           x.IsActive && !x.IsDeleted);
        }

        public async Task<IEnumerable<AdmissionFormConfig>> GetAllAsync()
        {
            return await DbSet
                .Include(x => x.Campus)
                .Include(x => x.EducationLevel)
                .Include(x => x.Program)
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> UpdateAsync(AdmissionFormConfig entity)
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
