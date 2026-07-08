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
    public class StudentAttendanceRepository : Repository<StudentAttendance>, IStudentAttendanceRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public StudentAttendanceRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork)
            : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<StudentAttendance> AddAsync(StudentAttendance entity)
        {
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<StudentAttendance> entities)
        {
            foreach (var e in entities) e.CreatedDate = DateTime.Now;
            await _context.Set<StudentAttendance>().AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
        }

        public async Task<StudentAttendance?> GetByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.Student)
                .Include(x => x.Subject)
                .Include(x => x.Class)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<IEnumerable<StudentAttendance>> GetByDateAndClassAsync(DateTime date, int classId, int schoolRegistrationId)
        {
            return await DbSet
                .Include(x => x.Student)
                .Include(x => x.Subject)
                .Where(x => x.AttendanceDate.Date == date.Date
                    && x.ClassId == classId
                    && x.SchoolRegistrationId == schoolRegistrationId
                    && !x.IsDeleted)
                .OrderBy(x => x.Student.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentAttendance>> GetByStudentAsync(int studentId, int schoolRegistrationId,
            DateTime? from = null, DateTime? to = null)
        {
            var query = DbSet
                .Include(x => x.Subject)
                .Include(x => x.Class)
                .Where(x => x.StudentId == studentId
                    && x.SchoolRegistrationId == schoolRegistrationId
                    && !x.IsDeleted);

            if (from.HasValue) query = query.Where(x => x.AttendanceDate >= from.Value);
            if (to.HasValue)   query = query.Where(x => x.AttendanceDate <= to.Value);

            return await query.OrderByDescending(x => x.AttendanceDate).ToListAsync();
        }

        public async Task<IEnumerable<StudentAttendance>> GetByClassAndMonthAsync(int classId, int month, int year, int schoolRegistrationId)
        {
            return await DbSet
                .Include(x => x.Student)
                .Include(x => x.Subject)
                .Where(x => x.ClassId == classId
                    && x.AttendanceDate.Month == month
                    && x.AttendanceDate.Year == year
                    && x.SchoolRegistrationId == schoolRegistrationId
                    && !x.IsDeleted)
                .OrderBy(x => x.AttendanceDate).ThenBy(x => x.Student.Name)
                .ToListAsync();
        }

        public async Task<(int Present, int Absent, int Late, int Leave, int Total)> GetStudentSummaryAsync(
            int studentId, int schoolRegistrationId, DateTime? from = null, DateTime? to = null)
        {
            var query = DbSet.Where(x => x.StudentId == studentId
                && x.SchoolRegistrationId == schoolRegistrationId
                && !x.IsDeleted);

            if (from.HasValue) query = query.Where(x => x.AttendanceDate >= from.Value);
            if (to.HasValue)   query = query.Where(x => x.AttendanceDate <= to.Value);

            var records = await query.ToListAsync();
            return (
                Present: records.Count(x => x.Status == "Present"),
                Absent:  records.Count(x => x.Status == "Absent"),
                Late:    records.Count(x => x.Status == "Late"),
                Leave:   records.Count(x => x.Status == "Leave"),
                Total:   records.Count
            );
        }

        public async Task<int> UpdateAsync(StudentAttendance entity)
        {
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

        public async Task<bool> ExistsAsync(int studentId, DateTime date, int? subjectId, int schoolRegistrationId)
        {
            var query = DbSet.Where(x => x.StudentId == studentId
                && x.AttendanceDate.Date == date.Date
                && x.SchoolRegistrationId == schoolRegistrationId
                && !x.IsDeleted);

            if (subjectId.HasValue)
                query = query.Where(x => x.SubjectId == subjectId.Value);

            return await query.AnyAsync();
        }
    }
}
