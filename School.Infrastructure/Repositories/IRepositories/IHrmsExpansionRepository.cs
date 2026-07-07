using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using School.Domain.Hr.Recruitment;
using School.Domain.Hr.Performance;
using School.Domain.Hr.Training;
using School.Domain.Hr.Assets;
using School.Domain.Hr;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IHrmsExpansionRepository : IRepository<Employee>
    {
        // Recruitment
        Task<IEnumerable<JobPosting>> GetJobPostingsAsync(int schoolId);
        Task<JobPosting?> GetJobPostingByIdAsync(int id, int schoolId);
        Task<IEnumerable<Candidate>> GetCandidatesAsync(int schoolId);
        Task<Candidate?> GetCandidateByIdAsync(int id, int schoolId);
        Task<IEnumerable<JobApplication>> GetJobApplicationsAsync(int schoolId);
        Task<JobApplication?> GetJobApplicationByIdAsync(int id, int schoolId);

        // Performance
        Task<IEnumerable<PerformanceReview>> GetPerformanceReviewsAsync(int schoolId);
        Task<PerformanceReview?> GetPerformanceReviewByIdAsync(int id, int schoolId);
        Task<IEnumerable<KpiMetric>> GetKpiMetricsAsync(int schoolId);
        Task<KpiMetric?> GetKpiMetricByIdAsync(int id, int schoolId);

        // Training
        Task<IEnumerable<TrainingProgram>> GetTrainingProgramsAsync(int schoolId);
        Task<TrainingProgram?> GetTrainingProgramByIdAsync(int id, int schoolId);
        Task<IEnumerable<TrainingEnrollment>> GetTrainingEnrollmentsAsync(int schoolId);
        Task<TrainingEnrollment?> GetTrainingEnrollmentByIdAsync(int id, int schoolId);

        // Assets
        Task<IEnumerable<SchoolAsset>> GetSchoolAssetsAsync(int schoolId);
        Task<SchoolAsset?> GetSchoolAssetByIdAsync(int id, int schoolId);
        Task<IEnumerable<AssetAssignment>> GetAssetAssignmentsAsync(int schoolId);
        Task<AssetAssignment?> GetAssetAssignmentByIdAsync(int id, int schoolId);

        // CRUD Helpers for custom types
        Task AddEntityAsync<TEntity>(TEntity entity) where TEntity : class;
        void UpdateEntity<TEntity>(TEntity entity) where TEntity : class;
        void DeleteEntity<TEntity>(TEntity entity) where TEntity : class;
    }
}
