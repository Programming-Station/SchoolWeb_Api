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
    public class AdmissionApplicationRepository : Repository<AdmissionApplication>, IAdmissionApplicationRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public AdmissionApplicationRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<AdmissionApplication> AddAsync(AdmissionApplication entity)
        {
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<AdmissionApplication> GetByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.AcademicYear)
                .Include(x => x.Campus)
                .Include(x => x.EducationLevel)
                .Include(x => x.Faculty)
                .Include(x => x.Department)
                .Include(x => x.Program)
                .Include(x => x.Course)
                .Include(x => x.Branch)
                .Include(x => x.YearSemester)
                .Include(x => x.Batch)
                .Include(x => x.Class)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new AdmissionApplication();
        }

        public async Task<IEnumerable<AdmissionApplication>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? status = null, int? campusId = null, int? programId = null)
        {
            var query = DbSet
                .Include(x => x.AcademicYear)
                .Include(x => x.Campus)
                .Include(x => x.EducationLevel)
                .Include(x => x.Program)
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(x => x.Status == status);
            }

            if (campusId.HasValue)
            {
                query = query.Where(x => x.CampusId == campusId.Value);
            }

            if (programId.HasValue)
            {
                query = query.Where(x => x.ProgramId == programId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();
                query = query.Where(x => x.FullName.ToLower().Contains(term) || 
                                         x.ApplicationNo.ToLower().Contains(term) || 
                                         (x.AdmissionNo != null && x.AdmissionNo.ToLower().Contains(term)) ||
                                         (x.EnrollmentNo != null && x.EnrollmentNo.ToLower().Contains(term)) ||
                                         x.Mobile.Contains(term));
            }

            return await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync(string? searchTerm = null, string? status = null, int? campusId = null, int? programId = null)
        {
            var query = DbSet.Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(x => x.Status == status);
            }

            if (campusId.HasValue)
            {
                query = query.Where(x => x.CampusId == campusId.Value);
            }

            if (programId.HasValue)
            {
                query = query.Where(x => x.ProgramId == programId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();
                query = query.Where(x => x.FullName.ToLower().Contains(term) || 
                                         x.ApplicationNo.ToLower().Contains(term) || 
                                         (x.AdmissionNo != null && x.AdmissionNo.ToLower().Contains(term)) ||
                                         (x.EnrollmentNo != null && x.EnrollmentNo.ToLower().Contains(term)) ||
                                         x.Mobile.Contains(term));
            }

            return await query.CountAsync();
        }

        public async Task<int> UpdateAsync(AdmissionApplication entity)
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

        public async Task<bool> ExistsByMobileAsync(string mobile, int? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                return await DbSet.AnyAsync(x => x.Mobile == mobile && x.Id != excludeId.Value && !x.IsDeleted);
            }
            return await DbSet.AnyAsync(x => x.Mobile == mobile && !x.IsDeleted);
        }

        public async Task<bool> ExistsByAadhaarAsync(string aadhaar, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(aadhaar)) return false;

            if (excludeId.HasValue)
            {
                return await DbSet.AnyAsync(x => x.AadhaarNo == aadhaar && x.Id != excludeId.Value && !x.IsDeleted);
            }
            return await DbSet.AnyAsync(x => x.AadhaarNo == aadhaar && !x.IsDeleted);
        }

        public async Task<string> GetLastApplicationNoAsync(string prefix)
        {
            var match = await DbSet
                .Where(x => x.ApplicationNo.StartsWith(prefix))
                .OrderByDescending(x => x.ApplicationNo)
                .Select(x => x.ApplicationNo)
                .FirstOrDefaultAsync();
            return match ?? string.Empty;
        }

        public async Task<string> GetLastAdmissionNoAsync(string prefix)
        {
            var match = await DbSet
                .Where(x => x.AdmissionNo != null && x.AdmissionNo.StartsWith(prefix))
                .OrderByDescending(x => x.AdmissionNo)
                .Select(x => x.AdmissionNo)
                .FirstOrDefaultAsync();
            return match ?? string.Empty;
        }

        public async Task<string> GetLastEnrollmentNoAsync(string prefix)
        {
            var match = await DbSet
                .Where(x => x.EnrollmentNo != null && x.EnrollmentNo.StartsWith(prefix))
                .OrderByDescending(x => x.EnrollmentNo)
                .Select(x => x.EnrollmentNo)
                .FirstOrDefaultAsync();
            return match ?? string.Empty;
        }

        public async Task<string> GetLastStudentCodeAsync(string prefix)
        {
            var match = await DbSet
                .Where(x => x.StudentCode != null && x.StudentCode.StartsWith(prefix))
                .OrderByDescending(x => x.StudentCode)
                .Select(x => x.StudentCode)
                .FirstOrDefaultAsync();
            return match ?? string.Empty;
        }

        public async Task<int> AddAuditLogAsync(AdmissionAuditLog log)
        {
            await _context.AdmissionAuditLogs.AddAsync(log);
            return await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<AdmissionAuditLog>> GetAuditLogsAsync(int applicationId)
        {
            return await _context.AdmissionAuditLogs
                .Where(x => x.AdmissionApplicationId == applicationId)
                .OrderByDescending(x => x.PerformedDate)
                .ToListAsync();
        }
    }
}
