using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using School.Domain.Academic;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ITimetablePeriodRepository
    {
        Task<TimetablePeriod> AddAsync(TimetablePeriod entity);
        Task AddRangeAsync(IEnumerable<TimetablePeriod> entities);
        Task<TimetablePeriod?> GetByIdAsync(int id);
        Task<IEnumerable<TimetablePeriod>> GetByClassAsync(int classId, int academicYearId, int schoolRegistrationId);
        Task<IEnumerable<TimetablePeriod>> GetByDayAsync(int classId, int dayOfWeek, int academicYearId, int schoolRegistrationId);
        Task<int> UpdateAsync(TimetablePeriod entity);
        Task<int> DeleteAsync(int id);
        Task<int> DeleteByClassAsync(int classId, int academicYearId, int schoolRegistrationId);
    }

    public interface IHomeworkRepository
    {
        Task<Homework> AddAsync(Homework entity);
        Task<Homework?> GetByIdAsync(int id);
        Task<IEnumerable<Homework>> GetByClassAsync(int classId, int schoolRegistrationId);
        Task<IEnumerable<Homework>> GetBySubjectAsync(int subjectId, int classId, int schoolRegistrationId);
        Task<IEnumerable<Homework>> GetPendingByStudentAsync(int studentId, int classId, int schoolRegistrationId);
        Task<int> UpdateAsync(Homework entity);
        Task<int> DeleteAsync(int id);
        // Submissions
        Task<HomeworkSubmission> AddSubmissionAsync(HomeworkSubmission sub);
        Task<HomeworkSubmission?> GetSubmissionAsync(int homeworkId, int studentId);
        Task<IEnumerable<HomeworkSubmission>> GetSubmissionsByHomeworkAsync(int homeworkId);
        Task<int> UpdateSubmissionAsync(HomeworkSubmission sub);
    }

    public interface IAssignmentRepository
    {
        Task<Assignment> AddAsync(Assignment entity);
        Task<Assignment?> GetByIdAsync(int id);
        Task<IEnumerable<Assignment>> GetByClassAsync(int classId, int schoolRegistrationId);
        Task<IEnumerable<Assignment>> GetBySubjectAsync(int subjectId, int classId, int schoolRegistrationId);
        Task<int> UpdateAsync(Assignment entity);
        Task<int> DeleteAsync(int id);
        // Submissions
        Task<AssignmentSubmission> AddSubmissionAsync(AssignmentSubmission sub);
        Task<AssignmentSubmission?> GetSubmissionAsync(int assignmentId, int studentId);
        Task<IEnumerable<AssignmentSubmission>> GetSubmissionsByAssignmentAsync(int assignmentId);
        Task<int> GradeSubmissionAsync(int submissionId, decimal marks, string feedback);
    }

    public interface IOnlineClassRepository
    {
        Task<OnlineClass> AddAsync(OnlineClass entity);
        Task<OnlineClass?> GetByIdAsync(int id);
        Task<IEnumerable<OnlineClass>> GetByClassAsync(int classId, int schoolRegistrationId);
        Task<IEnumerable<OnlineClass>> GetUpcomingAsync(int schoolRegistrationId, int days = 7);
        Task<int> UpdateAsync(OnlineClass entity);
        Task<int> UpdateStatusAsync(int id, string status);
        Task<int> DeleteAsync(int id);
    }

    public interface ISyllabusRepository
    {
        // Chapters
        Task<SyllabusChapter> AddChapterAsync(SyllabusChapter chapter);
        Task<SyllabusChapter?> GetChapterByIdAsync(int id);
        Task<IEnumerable<SyllabusChapter>> GetChaptersBySubjectAsync(int subjectId, int classId, int schoolRegistrationId);
        Task<IEnumerable<SyllabusChapter>> GetChaptersByClassAsync(int classId, int schoolRegistrationId);
        Task<int> UpdateChapterProgressAsync(int id, int completedPeriods, string status);
        Task<int> UpdateChapterAsync(SyllabusChapter chapter);
        Task<int> DeleteChapterAsync(int id);
        // Lesson Plans
        Task<LessonPlan> AddLessonPlanAsync(LessonPlan plan);
        Task<LessonPlan?> GetLessonPlanByIdAsync(int id);
        Task<IEnumerable<LessonPlan>> GetLessonPlansByChapterAsync(int chapterId);
        Task<IEnumerable<LessonPlan>> GetLessonPlansByDateAsync(int classId, DateTime date, int schoolRegistrationId);
        Task<int> UpdateLessonPlanAsync(LessonPlan plan);
        Task<int> DeleteLessonPlanAsync(int id);
    }
}
