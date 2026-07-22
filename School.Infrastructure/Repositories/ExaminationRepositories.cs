using Microsoft.EntityFrameworkCore;
using School.Domain.Academic;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    // ══════════════════════════════════════════════════════════════════════════
    // 5.1 EXAM SCHEDULE
    // ══════════════════════════════════════════════════════════════════════════
    public class ExamScheduleRepository : Repository<ExamSchedule>, IExamScheduleRepository
    {
        private readonly SchoolDbContext _ctx; private readonly IUnitOfWork _uow;
        public ExamScheduleRepository(DbFactory f, SchoolDbContext ctx, IUnitOfWork uow) : base(f) { _ctx = ctx; _uow = uow; }
        public async Task<ExamSchedule> AddAsync(ExamSchedule e) { await base.AddAsync(e); await _uow.CommitAsync(); return e; }
        public async Task AddRangeAsync(IEnumerable<ExamSchedule> list) { await _ctx.Set<ExamSchedule>().AddRangeAsync(list); await _uow.CommitAsync(); }
        public async Task<ExamSchedule?> GetByIdAsync(int id) =>
            await DbSet.Include(x => x.Exam).Include(x => x.Subject).Include(x => x.Class)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        public async Task<IEnumerable<ExamSchedule>> GetByExamAsync(int examId, int schoolId) =>
            await DbSet.Include(x => x.Subject).Include(x => x.Class)
                .Where(x => x.ExamId == examId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .OrderBy(x => x.ExamDate).ThenBy(x => x.StartTime).ToListAsync();
        public async Task<IEnumerable<ExamSchedule>> GetByClassAsync(int classId, int schoolId) =>
            await DbSet.Include(x => x.Exam).Include(x => x.Subject)
                .Where(x => x.ClassId == classId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .OrderBy(x => x.ExamDate).ToListAsync();
        public async Task<int> UpdateAsync(ExamSchedule e) { _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync(); }
        public async Task<int> DeleteAsync(int id) { var e = await DbSet.FindAsync(id); if (e == null) return 0; e.IsDeleted = true; _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync(); }
        public async Task<int> DeleteByExamAsync(int examId) { var items = await DbSet.Where(x => x.ExamId == examId && !x.IsDeleted).ToListAsync(); items.ForEach(x => x.IsDeleted = true); return await _uow.CommitAsync(); }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 5.3 GRADE CONFIG
    // ══════════════════════════════════════════════════════════════════════════
    public class GradeConfigRepository : Repository<GradeConfig>, IGradeConfigRepository
    {
        private readonly SchoolDbContext _ctx; private readonly IUnitOfWork _uow;
        public GradeConfigRepository(DbFactory f, SchoolDbContext ctx, IUnitOfWork uow) : base(f) { _ctx = ctx; _uow = uow; }
        public async Task<GradeConfig> AddAsync(GradeConfig e) { await base.AddAsync(e); await _uow.CommitAsync(); return e; }
        public async Task AddRangeAsync(IEnumerable<GradeConfig> list) { await _ctx.Set<GradeConfig>().AddRangeAsync(list); await _uow.CommitAsync(); }
        public async Task<IEnumerable<GradeConfig>> GetBySchoolAsync(int schoolId) =>
            await DbSet.Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .OrderBy(x => x.DisplayOrder).ToListAsync();
        public async Task<GradeConfig?> GetByPercentAsync(decimal percent, int schoolId) =>
            await DbSet.FirstOrDefaultAsync(x => x.SchoolRegistrationId == schoolId
                && percent >= x.MinPercent && percent <= x.MaxPercent && !x.IsDeleted);
        public async Task<int> DeleteAllAsync(int schoolId)
        {
            var all = await DbSet.Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted).ToListAsync();
            all.ForEach(x => x.IsDeleted = true);
            return await _uow.CommitAsync();
        }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 5.4 REPORT CARD
    // ══════════════════════════════════════════════════════════════════════════
    public class ReportCardRepository : Repository<ReportCard>, IReportCardRepository
    {
        private readonly SchoolDbContext _ctx; private readonly IUnitOfWork _uow;
        public ReportCardRepository(DbFactory f, SchoolDbContext ctx, IUnitOfWork uow) : base(f) { _ctx = ctx; _uow = uow; }
        public async Task<ReportCard> AddAsync(ReportCard e) { await base.AddAsync(e); await _uow.CommitAsync(); return e; }
        public async Task<ReportCard?> GetByIdAsync(int id) =>
            await DbSet.Include(x => x.Student).Include(x => x.Exam).Include(x => x.Class)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        public async Task<ReportCard?> GetByStudentExamAsync(int studentId, int examId) =>
            await DbSet.Include(x => x.Student).Include(x => x.Exam)
                .FirstOrDefaultAsync(x => x.StudentId == studentId && x.ExamId == examId && !x.IsDeleted);
        public async Task<IEnumerable<ReportCard>> GetByExamAsync(int examId, int schoolId) =>
            await DbSet.Include(x => x.Student)
                .Where(x => x.ExamId == examId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .OrderBy(x => x.Rank).ToListAsync();
        public async Task<IEnumerable<ReportCard>> GetByStudentAsync(int studentId, int schoolId) =>
            await DbSet.Include(x => x.Exam)
                .Where(x => x.StudentId == studentId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate).ToListAsync();
        public async Task<int> UpdateAsync(ReportCard e) { _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync(); }
        public async Task<int> PublishAsync(int examId, int schoolId)
        {
            var cards = await DbSet.Where(x => x.ExamId == examId && x.SchoolRegistrationId == schoolId && x.PublishStatus == "Draft" && !x.IsDeleted).ToListAsync();
            cards.ForEach(x => { x.PublishStatus = "Published"; x.PublishedDate = DateTime.Now; });
            return await _uow.CommitAsync();
        }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 5.6 STUDENT PROMOTION
    // ══════════════════════════════════════════════════════════════════════════
    public class StudentPromotionRepository : Repository<StudentPromotion>, IStudentPromotionRepository
    {
        private readonly SchoolDbContext _ctx; private readonly IUnitOfWork _uow;
        public StudentPromotionRepository(DbFactory f, SchoolDbContext ctx, IUnitOfWork uow) : base(f) { _ctx = ctx; _uow = uow; }
        public async Task<StudentPromotion> AddAsync(StudentPromotion e) { await base.AddAsync(e); await _uow.CommitAsync(); return e; }
        public async Task AddRangeAsync(IEnumerable<StudentPromotion> list) { await _ctx.Set<StudentPromotion>().AddRangeAsync(list); await _uow.CommitAsync(); }
        public async Task<StudentPromotion?> GetByIdAsync(int id) =>
            await DbSet.Include(x => x.Student).Include(x => x.FromClass).Include(x => x.ToClass)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        public async Task<IEnumerable<StudentPromotion>> GetByClassAsync(int fromClassId, int schoolId) =>
            await DbSet.Include(x => x.Student).Include(x => x.ToClass)
                .Where(x => x.FromClassId == fromClassId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .OrderBy(x => x.Student.Name).ToListAsync();
        public async Task<IEnumerable<StudentPromotion>> GetByStudentAsync(int studentId, int schoolId) =>
            await DbSet.Include(x => x.FromClass).Include(x => x.ToClass).Include(x => x.FromAcademicYear).Include(x => x.ToAcademicYear)
                .Where(x => x.StudentId == studentId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .OrderByDescending(x => x.PromotionDate).ToListAsync();
        public async Task<int> UpdateAsync(StudentPromotion e) { _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync(); }
    }
}
