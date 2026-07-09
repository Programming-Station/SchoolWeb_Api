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
    // ══════════════════════════════════════════════════════════════════════════
    // 4.3 TIMETABLE
    // ══════════════════════════════════════════════════════════════════════════
    public class TimetablePeriodRepository : Repository<TimetablePeriod>, ITimetablePeriodRepository
    {
        private readonly SchoolDbContext _ctx;
        private readonly IUnitOfWork _uow;
        public TimetablePeriodRepository(DbFactory f, SchoolDbContext ctx, IUnitOfWork uow) : base(f) { _ctx = ctx; _uow = uow; }

        public async Task<TimetablePeriod> AddAsync(TimetablePeriod e) { await base.AddAsync(e); await _uow.CommitAsync(); return e; }
        public async Task AddRangeAsync(IEnumerable<TimetablePeriod> list)
        {
            await _ctx.Set<TimetablePeriod>().AddRangeAsync(list);
            await _uow.CommitAsync();
        }
        public async Task<TimetablePeriod?> GetByIdAsync(int id) =>
            await DbSet.Include(x => x.Subject).Include(x => x.Class).Include(x => x.Teacher)
                       .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        public async Task<IEnumerable<TimetablePeriod>> GetByClassAsync(int classId, int academicYearId, int schoolId) =>
            await DbSet.Include(x => x.Subject).Include(x => x.Teacher)
                       .Where(x => x.ClassId == classId && x.AcademicYearId == academicYearId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                       .OrderBy(x => x.DayOfWeek).ThenBy(x => x.PeriodNo).ToListAsync();
        public async Task<IEnumerable<TimetablePeriod>> GetByDayAsync(int classId, int day, int academicYearId, int schoolId) =>
            await DbSet.Include(x => x.Subject).Include(x => x.Teacher)
                       .Where(x => x.ClassId == classId && x.DayOfWeek == day && x.AcademicYearId == academicYearId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                       .OrderBy(x => x.PeriodNo).ToListAsync();
        public async Task<int> UpdateAsync(TimetablePeriod e) { _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync(); }
        public async Task<int> DeleteAsync(int id)
        {
            var e = await DbSet.FindAsync(id); if (e == null) return 0;
            e.IsDeleted = true; _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync();
        }
        public async Task<int> DeleteByClassAsync(int classId, int academicYearId, int schoolId)
        {
            var items = await DbSet.Where(x => x.ClassId == classId && x.AcademicYearId == academicYearId && x.SchoolRegistrationId == schoolId && !x.IsDeleted).ToListAsync();
            items.ForEach(x => x.IsDeleted = true);
            return await _uow.CommitAsync();
        }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 4.4 HOMEWORK
    // ══════════════════════════════════════════════════════════════════════════
    public class HomeworkRepository : Repository<Homework>, IHomeworkRepository
    {
        private readonly SchoolDbContext _ctx;
        private readonly IUnitOfWork _uow;
        public HomeworkRepository(DbFactory f, SchoolDbContext ctx, IUnitOfWork uow) : base(f) { _ctx = ctx; _uow = uow; }

        public async Task<Homework> AddAsync(Homework e) { await base.AddAsync(e); await _uow.CommitAsync(); return e; }
        public async Task<Homework?> GetByIdAsync(int id) =>
            await DbSet.Include(x => x.Subject).Include(x => x.Class)
                       .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        public async Task<IEnumerable<Homework>> GetByClassAsync(int classId, int schoolId) =>
            await DbSet.Include(x => x.Subject)
                       .Where(x => x.ClassId == classId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                       .OrderByDescending(x => x.AssignedDate).ToListAsync();
        public async Task<IEnumerable<Homework>> GetBySubjectAsync(int subjectId, int classId, int schoolId) =>
            await DbSet.Where(x => x.SubjectId == subjectId && x.ClassId == classId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                       .OrderByDescending(x => x.DueDate).ToListAsync();
        public async Task<IEnumerable<Homework>> GetPendingByStudentAsync(int studentId, int classId, int schoolId) =>
            await DbSet.Include(x => x.Subject)
                       .Where(x => x.ClassId == classId && x.SchoolRegistrationId == schoolId && x.Status == "Active"
                           && x.DueDate >= DateTime.Today && !x.IsDeleted)
                       .ToListAsync();
        public async Task<int> UpdateAsync(Homework e) { _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync(); }
        public async Task<int> DeleteAsync(int id)
        {
            var e = await DbSet.FindAsync(id); if (e == null) return 0;
            e.IsDeleted = true; _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync();
        }
        public async Task<HomeworkSubmission> AddSubmissionAsync(HomeworkSubmission s)
        {
            await _ctx.Set<HomeworkSubmission>().AddAsync(s); await _uow.CommitAsync(); return s;
        }
        public async Task<HomeworkSubmission?> GetSubmissionAsync(int hwId, int studentId) =>
            await _ctx.Set<HomeworkSubmission>().FirstOrDefaultAsync(x => x.HomeworkId == hwId && x.StudentId == studentId && !x.IsDeleted);
        public async Task<IEnumerable<HomeworkSubmission>> GetSubmissionsByHomeworkAsync(int hwId) =>
            await _ctx.Set<HomeworkSubmission>().Include(x => x.Student)
                       .Where(x => x.HomeworkId == hwId && !x.IsDeleted).ToListAsync();
        public async Task<int> UpdateSubmissionAsync(HomeworkSubmission s) { _ctx.Entry(s).State = EntityState.Modified; return await _uow.CommitAsync(); }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 4.5 ASSIGNMENT
    // ══════════════════════════════════════════════════════════════════════════
    public class AssignmentRepository : Repository<Assignment>, IAssignmentRepository
    {
        private readonly SchoolDbContext _ctx;
        private readonly IUnitOfWork _uow;
        public AssignmentRepository(DbFactory f, SchoolDbContext ctx, IUnitOfWork uow) : base(f) { _ctx = ctx; _uow = uow; }

        public async Task<Assignment> AddAsync(Assignment e) { await base.AddAsync(e); await _uow.CommitAsync(); return e; }
        public async Task<Assignment?> GetByIdAsync(int id) =>
            await DbSet.Include(x => x.Subject).Include(x => x.Class)
                       .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        public async Task<IEnumerable<Assignment>> GetByClassAsync(int classId, int schoolId) =>
            await DbSet.Include(x => x.Subject)
                       .Where(x => x.ClassId == classId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                       .OrderByDescending(x => x.EndDate).ToListAsync();
        public async Task<IEnumerable<Assignment>> GetBySubjectAsync(int subjectId, int classId, int schoolId) =>
            await DbSet.Where(x => x.SubjectId == subjectId && x.ClassId == classId && x.SchoolRegistrationId == schoolId && !x.IsDeleted).ToListAsync();
        public async Task<int> UpdateAsync(Assignment e) { _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync(); }
        public async Task<int> DeleteAsync(int id)
        {
            var e = await DbSet.FindAsync(id); if (e == null) return 0;
            e.IsDeleted = true; _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync();
        }
        public async Task<AssignmentSubmission> AddSubmissionAsync(AssignmentSubmission s)
        {
            await _ctx.Set<AssignmentSubmission>().AddAsync(s); await _uow.CommitAsync(); return s;
        }
        public async Task<AssignmentSubmission?> GetSubmissionAsync(int asgId, int studentId) =>
            await _ctx.Set<AssignmentSubmission>().FirstOrDefaultAsync(x => x.AssignmentId == asgId && x.StudentId == studentId && !x.IsDeleted);
        public async Task<IEnumerable<AssignmentSubmission>> GetSubmissionsByAssignmentAsync(int asgId) =>
            await _ctx.Set<AssignmentSubmission>().Include(x => x.Student)
                       .Where(x => x.AssignmentId == asgId && !x.IsDeleted).ToListAsync();
        public async Task<int> GradeSubmissionAsync(int subId, decimal marks, string feedback)
        {
            var s = await _ctx.Set<AssignmentSubmission>().FindAsync(subId); if (s == null) return 0;
            s.MarksObtained = marks; s.TeacherFeedback = feedback; s.Status = "Graded";
            _ctx.Entry(s).State = EntityState.Modified; return await _uow.CommitAsync();
        }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 4.6 ONLINE CLASS
    // ══════════════════════════════════════════════════════════════════════════
    public class OnlineClassRepository : Repository<OnlineClass>, IOnlineClassRepository
    {
        private readonly SchoolDbContext _ctx;
        private readonly IUnitOfWork _uow;
        public OnlineClassRepository(DbFactory f, SchoolDbContext ctx, IUnitOfWork uow) : base(f) { _ctx = ctx; _uow = uow; }

        public async Task<OnlineClass> AddAsync(OnlineClass e) { await base.AddAsync(e); await _uow.CommitAsync(); return e; }
        public async Task<OnlineClass?> GetByIdAsync(int id) =>
            await DbSet.Include(x => x.Subject).Include(x => x.Class).Include(x => x.Teacher)
                       .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        public async Task<IEnumerable<OnlineClass>> GetByClassAsync(int classId, int schoolId) =>
            await DbSet.Include(x => x.Subject).Include(x => x.Teacher)
                       .Where(x => x.ClassId == classId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                       .OrderByDescending(x => x.ScheduledAt).ToListAsync();
        public async Task<IEnumerable<OnlineClass>> GetUpcomingAsync(int schoolId, int days = 7) =>
            await DbSet.Include(x => x.Subject).Include(x => x.Class).Include(x => x.Teacher)
                       .Where(x => x.SchoolRegistrationId == schoolId && x.ScheduledAt >= DateTime.Now
                           && x.ScheduledAt <= DateTime.Now.AddDays(days) && x.Status == "Scheduled" && !x.IsDeleted)
                       .OrderBy(x => x.ScheduledAt).ToListAsync();
        public async Task<int> UpdateAsync(OnlineClass e) { _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync(); }
        public async Task<int> UpdateStatusAsync(int id, string status)
        {
            var e = await DbSet.FindAsync(id); if (e == null) return 0;
            e.Status = status; _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync();
        }
        public async Task<int> DeleteAsync(int id)
        {
            var e = await DbSet.FindAsync(id); if (e == null) return 0;
            e.IsDeleted = true; _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync();
        }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 4.7 SYLLABUS
    // ══════════════════════════════════════════════════════════════════════════
    public class SyllabusRepository : ISyllabusRepository
    {
        private readonly SchoolDbContext _ctx;
        private readonly IUnitOfWork _uow;
        public SyllabusRepository(SchoolDbContext ctx, IUnitOfWork uow) { _ctx = ctx; _uow = uow; }

        public async Task<SyllabusChapter> AddChapterAsync(SyllabusChapter c) { await _ctx.Set<SyllabusChapter>().AddAsync(c); await _uow.CommitAsync(); return c; }
        public async Task<SyllabusChapter?> GetChapterByIdAsync(int id) =>
            await _ctx.Set<SyllabusChapter>().Include(x => x.Subject).Include(x => x.Class)
                       .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        public async Task<IEnumerable<SyllabusChapter>> GetChaptersBySubjectAsync(int subjectId, int classId, int schoolId) =>
            await _ctx.Set<SyllabusChapter>().Include(x => x.Subject).Include(x => x.Class)
                       .Where(x => x.SubjectId == subjectId && x.ClassId == classId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                       .OrderBy(x => x.ChapterNo).ToListAsync();
        public async Task<IEnumerable<SyllabusChapter>> GetChaptersByClassAsync(int classId, int schoolId) =>
            await _ctx.Set<SyllabusChapter>().Include(x => x.Subject).Include(x => x.Class)
                       .Where(x => x.ClassId == classId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                       .OrderBy(x => x.ChapterNo).ToListAsync();
        public async Task<int> UpdateChapterProgressAsync(int id, int completedPeriods, string status)
        {
            var c = await _ctx.Set<SyllabusChapter>().FindAsync(id); if (c == null) return 0;
            c.CompletedPeriods = completedPeriods; c.Status = status;
            if (status == "Completed") c.CompletedDate = DateTime.Now;
            if (status == "InProgress" && !c.StartedDate.HasValue) c.StartedDate = DateTime.Now;
            _ctx.Entry(c).State = EntityState.Modified; return await _uow.CommitAsync();
        }
        public async Task<int> UpdateChapterAsync(SyllabusChapter c) { _ctx.Entry(c).State = EntityState.Modified; return await _uow.CommitAsync(); }
        public async Task<int> DeleteChapterAsync(int id)
        {
            var c = await _ctx.Set<SyllabusChapter>().FindAsync(id); if (c == null) return 0;
            c.IsDeleted = true; _ctx.Entry(c).State = EntityState.Modified; return await _uow.CommitAsync();
        }
        public async Task<LessonPlan> AddLessonPlanAsync(LessonPlan p) { await _ctx.Set<LessonPlan>().AddAsync(p); await _uow.CommitAsync(); return p; }
        public async Task<LessonPlan?> GetLessonPlanByIdAsync(int id) =>
            await _ctx.Set<LessonPlan>().Include(x => x.SyllabusChapter).Include(x => x.Subject)
                       .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        public async Task<IEnumerable<LessonPlan>> GetLessonPlansByChapterAsync(int chapterId) =>
            await _ctx.Set<LessonPlan>().Where(x => x.SyllabusChapterId == chapterId && !x.IsDeleted)
                       .OrderBy(x => x.PlannedDate).ToListAsync();
        public async Task<IEnumerable<LessonPlan>> GetLessonPlansByDateAsync(int classId, DateTime date, int schoolId) =>
            await _ctx.Set<LessonPlan>().Include(x => x.Subject).Include(x => x.SyllabusChapter)
                       .Where(x => x.ClassId == classId && x.PlannedDate.Date == date.Date && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                       .ToListAsync();
        public async Task<int> UpdateLessonPlanAsync(LessonPlan p) { _ctx.Entry(p).State = EntityState.Modified; return await _uow.CommitAsync(); }
        public async Task<int> DeleteLessonPlanAsync(int id)
        {
            var p = await _ctx.Set<LessonPlan>().FindAsync(id); if (p == null) return 0;
            p.IsDeleted = true; _ctx.Entry(p).State = EntityState.Modified; return await _uow.CommitAsync();
        }
    }
}
