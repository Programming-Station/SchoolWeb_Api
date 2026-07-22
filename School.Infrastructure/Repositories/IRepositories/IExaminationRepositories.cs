using School.Domain.Academic;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IExamScheduleRepository
    {
        Task<ExamSchedule> AddAsync(ExamSchedule entity);
        Task AddRangeAsync(IEnumerable<ExamSchedule> entities);
        Task<ExamSchedule?> GetByIdAsync(int id);
        Task<IEnumerable<ExamSchedule>> GetByExamAsync(int examId, int schoolId);
        Task<IEnumerable<ExamSchedule>> GetByClassAsync(int classId, int schoolId);
        Task<int> UpdateAsync(ExamSchedule entity);
        Task<int> DeleteAsync(int id);
        Task<int> DeleteByExamAsync(int examId);
    }

    public interface IGradeConfigRepository
    {
        Task<GradeConfig> AddAsync(GradeConfig entity);
        Task AddRangeAsync(IEnumerable<GradeConfig> entities);
        Task<IEnumerable<GradeConfig>> GetBySchoolAsync(int schoolId);
        Task<GradeConfig?> GetByPercentAsync(decimal percent, int schoolId);
        Task<int> DeleteAllAsync(int schoolId);
    }

    public interface IReportCardRepository
    {
        Task<ReportCard> AddAsync(ReportCard entity);
        Task<ReportCard?> GetByIdAsync(int id);
        Task<ReportCard?> GetByStudentExamAsync(int studentId, int examId);
        Task<IEnumerable<ReportCard>> GetByExamAsync(int examId, int schoolId);
        Task<IEnumerable<ReportCard>> GetByStudentAsync(int studentId, int schoolId);
        Task<int> UpdateAsync(ReportCard entity);
        Task<int> PublishAsync(int examId, int schoolId);
    }

    public interface IStudentPromotionRepository
    {
        Task<StudentPromotion> AddAsync(StudentPromotion entity);
        Task AddRangeAsync(IEnumerable<StudentPromotion> entities);
        Task<StudentPromotion?> GetByIdAsync(int id);
        Task<IEnumerable<StudentPromotion>> GetByClassAsync(int fromClassId, int schoolId);
        Task<IEnumerable<StudentPromotion>> GetByStudentAsync(int studentId, int schoolId);
        Task<int> UpdateAsync(StudentPromotion entity);
    }
}
