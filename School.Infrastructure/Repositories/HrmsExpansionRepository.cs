using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Hr;
using School.Domain.Hr.Recruitment;
using School.Domain.Hr.Performance;
using School.Domain.Hr.Training;
using School.Domain.Hr.Assets;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories
{
    public class HrmsExpansionRepository : Repository<Employee>, IHrmsExpansionRepository
    {
        private SchoolDbContext Context => (SchoolDbContext)_dbFactory.DbContext;

        public HrmsExpansionRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        // ==========================================
        // RECRUITMENT
        // ==========================================

        public async Task<IEnumerable<JobPosting>> GetJobPostingsAsync(int schoolId)
        {
            return await Context.JobPostings
                .Include(j => j.Department)
                .Where(j => j.SchoolRegistrationId == schoolId)
                .ToListAsync();
        }

        public async Task<JobPosting?> GetJobPostingByIdAsync(int id, int schoolId)
        {
            return await Context.JobPostings
                .Include(j => j.Department)
                .FirstOrDefaultAsync(j => j.Id == id && j.SchoolRegistrationId == schoolId);
        }

        public async Task<IEnumerable<Candidate>> GetCandidatesAsync(int schoolId)
        {
            return await Context.Candidates
                .Where(c => c.SchoolRegistrationId == schoolId)
                .ToListAsync();
        }

        public async Task<Candidate?> GetCandidateByIdAsync(int id, int schoolId)
        {
            return await Context.Candidates
                .FirstOrDefaultAsync(c => c.Id == id && c.SchoolRegistrationId == schoolId);
        }

        public async Task<IEnumerable<JobApplication>> GetJobApplicationsAsync(int schoolId)
        {
            return await Context.JobApplications
                .Include(ja => ja.JobPosting)
                .Include(ja => ja.Candidate)
                .Where(ja => ja.SchoolRegistrationId == schoolId)
                .ToListAsync();
        }

        public async Task<JobApplication?> GetJobApplicationByIdAsync(int id, int schoolId)
        {
            return await Context.JobApplications
                .Include(ja => ja.JobPosting)
                .Include(ja => ja.Candidate)
                .FirstOrDefaultAsync(ja => ja.Id == id && ja.SchoolRegistrationId == schoolId);
        }

        // ==========================================
        // PERFORMANCE
        // ==========================================

        public async Task<IEnumerable<PerformanceReview>> GetPerformanceReviewsAsync(int schoolId)
        {
            return await Context.PerformanceReviews
                .Include(pr => pr.Employee)
                .Include(pr => pr.Reviewer)
                .Where(pr => pr.SchoolRegistrationId == schoolId)
                .ToListAsync();
        }

        public async Task<PerformanceReview?> GetPerformanceReviewByIdAsync(int id, int schoolId)
        {
            return await Context.PerformanceReviews
                .Include(pr => pr.Employee)
                .Include(pr => pr.Reviewer)
                .FirstOrDefaultAsync(pr => pr.Id == id && pr.SchoolRegistrationId == schoolId);
        }

        public async Task<IEnumerable<KpiMetric>> GetKpiMetricsAsync(int schoolId)
        {
            return await Context.KpiMetrics
                .Where(k => k.SchoolRegistrationId == schoolId)
                .ToListAsync();
        }

        public async Task<KpiMetric?> GetKpiMetricByIdAsync(int id, int schoolId)
        {
            return await Context.KpiMetrics
                .FirstOrDefaultAsync(k => k.Id == id && k.SchoolRegistrationId == schoolId);
        }

        // ==========================================
        // TRAINING
        // ==========================================

        public async Task<IEnumerable<TrainingProgram>> GetTrainingProgramsAsync(int schoolId)
        {
            return await Context.TrainingPrograms
                .Where(tp => tp.SchoolRegistrationId == schoolId)
                .ToListAsync();
        }

        public async Task<TrainingProgram?> GetTrainingProgramByIdAsync(int id, int schoolId)
        {
            return await Context.TrainingPrograms
                .FirstOrDefaultAsync(tp => tp.Id == id && tp.SchoolRegistrationId == schoolId);
        }

        public async Task<IEnumerable<TrainingEnrollment>> GetTrainingEnrollmentsAsync(int schoolId)
        {
            return await Context.TrainingEnrollments
                .Include(te => te.TrainingProgram)
                .Include(te => te.Employee)
                .Where(te => te.SchoolRegistrationId == schoolId)
                .ToListAsync();
        }

        public async Task<TrainingEnrollment?> GetTrainingEnrollmentByIdAsync(int id, int schoolId)
        {
            return await Context.TrainingEnrollments
                .Include(te => te.TrainingProgram)
                .Include(te => te.Employee)
                .FirstOrDefaultAsync(te => te.Id == id && te.SchoolRegistrationId == schoolId);
        }

        // ==========================================
        // ASSETS
        // ==========================================

        public async Task<IEnumerable<SchoolAsset>> GetSchoolAssetsAsync(int schoolId)
        {
            return await Context.SchoolAssets
                .Where(sa => sa.SchoolRegistrationId == schoolId)
                .ToListAsync();
        }

        public async Task<SchoolAsset?> GetSchoolAssetByIdAsync(int id, int schoolId)
        {
            return await Context.SchoolAssets
                .FirstOrDefaultAsync(sa => sa.Id == id && sa.SchoolRegistrationId == schoolId);
        }

        public async Task<IEnumerable<AssetAssignment>> GetAssetAssignmentsAsync(int schoolId)
        {
            return await Context.AssetAssignments
                .Include(aa => aa.SchoolAsset)
                .Include(aa => aa.Employee)
                .Where(aa => aa.SchoolRegistrationId == schoolId)
                .ToListAsync();
        }

        public async Task<AssetAssignment?> GetAssetAssignmentByIdAsync(int id, int schoolId)
        {
            return await Context.AssetAssignments
                .Include(aa => aa.SchoolAsset)
                .Include(aa => aa.Employee)
                .FirstOrDefaultAsync(aa => aa.Id == id && aa.SchoolRegistrationId == schoolId);
        }

        // ==========================================
        // HELPERS
        // ==========================================

        public async Task AddEntityAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public void UpdateEntity<TEntity>(TEntity entity) where TEntity : class
        {
            Context.Set<TEntity>().Entry(entity).State = EntityState.Modified;
        }

        public void DeleteEntity<TEntity>(TEntity entity) where TEntity : class
        {
            Context.Set<TEntity>().Remove(entity);
        }
    }
}
