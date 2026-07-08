using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Academic;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class SubjectEnrollmentRepository : Repository<SubjectEnrollment>, ISubjectEnrollmentRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public SubjectEnrollmentRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork)
            : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<SubjectEnrollment> AddAsync(SubjectEnrollment entity)
        {
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<SubjectEnrollment?> GetByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.Student)
                .Include(x => x.Subject)
                .Include(x => x.Batch)
                .Include(x => x.YearSemester)
                .Include(x => x.Class)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<IEnumerable<SubjectEnrollment>> GetByStudentAsync(int studentId, int schoolRegistrationId)
        {
            return await DbSet
                .Include(x => x.Subject)
                .Include(x => x.Batch)
                .Include(x => x.YearSemester)
                .Include(x => x.Class)
                .Where(x => x.StudentId == studentId
                    && x.SchoolRegistrationId == schoolRegistrationId
                    && !x.IsDeleted)
                .OrderBy(x => x.Subject.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<SubjectEnrollment>> GetByClassAsync(int classId, int schoolRegistrationId)
        {
            return await DbSet
                .Include(x => x.Student)
                .Include(x => x.Subject)
                .Where(x => x.ClassId == classId
                    && x.SchoolRegistrationId == schoolRegistrationId
                    && !x.IsDeleted)
                .OrderBy(x => x.Student.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<SubjectEnrollment>> GetByBatchAsync(int batchId, int schoolRegistrationId)
        {
            return await DbSet
                .Include(x => x.Student)
                .Include(x => x.Subject)
                .Where(x => x.BatchId == batchId
                    && x.SchoolRegistrationId == schoolRegistrationId
                    && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<SubjectEnrollment>> GetBySubjectAsync(int subjectId, int schoolRegistrationId)
        {
            return await DbSet
                .Include(x => x.Student)
                .Where(x => x.SubjectId == subjectId
                    && x.SchoolRegistrationId == schoolRegistrationId
                    && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> IsEnrolledAsync(int studentId, int subjectId)
        {
            return await DbSet.AnyAsync(x => x.StudentId == studentId
                && x.SubjectId == subjectId
                && x.Status == "Enrolled"
                && !x.IsDeleted);
        }

        public async Task<int> UpdateStatusAsync(int id, string status, string? remarks)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity == null) return 0;
            entity.Status = status;
            if (!string.IsNullOrEmpty(remarks)) entity.Remarks = remarks;
            if (status == "Dropped") entity.DroppedDate = DateTime.Now;
            _context.Entry(entity).State = EntityState.Modified;
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity == null) return 0;
            entity.IsDeleted = true;
            _context.Entry(entity).State = EntityState.Modified;
            return await _unitOfWork.CommitAsync();
        }
    }
}
