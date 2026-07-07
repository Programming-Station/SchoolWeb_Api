using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using School_DTOs;
using School_DTOs.Common;
using School_DTOs.Hr;

namespace School.Services.Interfaces
{
    public interface IHrmsExpansionService
    {
        // Recruitment - Job Posting
        Task<APIResponse<IEnumerable<JobPostingDto>>> GetJobPostingsAsync(int schoolId);
        Task<APIResponse<JobPostingDto>> GetJobPostingByIdAsync(int id, int schoolId);
        Task<APIResponse<JobPostingDto>> CreateJobPostingAsync(CreateJobPostingDto dto, int schoolId, string username);
        Task<APIResponse<object>> UpdateJobPostingAsync(UpdateJobPostingDto dto, int schoolId, string username);
        Task<APIResponse<object>> DeleteJobPostingAsync(int id, int schoolId, string username);

        // Recruitment - Candidate
        Task<APIResponse<IEnumerable<CandidateDto>>> GetCandidatesAsync(int schoolId);
        Task<APIResponse<CandidateDto>> GetCandidateByIdAsync(int id, int schoolId);
        Task<APIResponse<CandidateDto>> CreateCandidateAsync(CreateCandidateDto dto, int schoolId, string username);
        Task<APIResponse<object>> UpdateCandidateAsync(UpdateCandidateDto dto, int schoolId, string username);
        Task<APIResponse<object>> DeleteCandidateAsync(int id, int schoolId, string username);

        // Recruitment - Job Application
        Task<APIResponse<IEnumerable<JobApplicationDto>>> GetJobApplicationsAsync(int schoolId);
        Task<APIResponse<JobApplicationDto>> GetJobApplicationByIdAsync(int id, int schoolId);
        Task<APIResponse<JobApplicationDto>> CreateJobApplicationAsync(CreateJobApplicationDto dto, int schoolId, string username);
        Task<APIResponse<object>> UpdateJobApplicationStatusAsync(UpdateJobApplicationStatusDto dto, int schoolId, string username);

        // Performance - KPI Metrics
        Task<APIResponse<IEnumerable<KpiMetricDto>>> GetKpiMetricsAsync(int schoolId);
        Task<APIResponse<KpiMetricDto>> CreateKpiMetricAsync(CreateKpiMetricDto dto, int schoolId, string username);
        Task<APIResponse<object>> DeleteKpiMetricAsync(int id, int schoolId, string username);

        // Performance - Performance Review
        Task<APIResponse<IEnumerable<PerformanceReviewDto>>> GetPerformanceReviewsAsync(int schoolId);
        Task<APIResponse<PerformanceReviewDto>> GetPerformanceReviewByIdAsync(int id, int schoolId);
        Task<APIResponse<PerformanceReviewDto>> CreatePerformanceReviewAsync(CreatePerformanceReviewDto dto, int schoolId, string username);
        Task<APIResponse<object>> UpdatePerformanceReviewAsync(UpdatePerformanceReviewDto dto, int schoolId, string username);
        Task<APIResponse<object>> DeletePerformanceReviewAsync(int id, int schoolId, string username);

        // Training - Program
        Task<APIResponse<IEnumerable<TrainingProgramDto>>> GetTrainingProgramsAsync(int schoolId);
        Task<APIResponse<TrainingProgramDto>> GetTrainingProgramByIdAsync(int id, int schoolId);
        Task<APIResponse<TrainingProgramDto>> CreateTrainingProgramAsync(CreateTrainingProgramDto dto, int schoolId, string username);
        Task<APIResponse<object>> UpdateTrainingProgramAsync(UpdateTrainingProgramDto dto, int schoolId, string username);
        Task<APIResponse<object>> DeleteTrainingProgramAsync(int id, int schoolId, string username);

        // Training - Enrollment
        Task<APIResponse<IEnumerable<TrainingEnrollmentDto>>> GetTrainingEnrollmentsAsync(int schoolId);
        Task<APIResponse<TrainingEnrollmentDto>> CreateTrainingEnrollmentAsync(CreateTrainingEnrollmentDto dto, int schoolId, string username);
        Task<APIResponse<object>> UpdateTrainingEnrollmentAsync(UpdateTrainingEnrollmentDto dto, int schoolId, string username);

        // Assets - Asset
        Task<APIResponse<IEnumerable<SchoolAssetDto>>> GetSchoolAssetsAsync(int schoolId);
        Task<APIResponse<SchoolAssetDto>> GetSchoolAssetByIdAsync(int id, int schoolId);
        Task<APIResponse<SchoolAssetDto>> CreateSchoolAssetAsync(CreateSchoolAssetDto dto, int schoolId, string username);
        Task<APIResponse<object>> UpdateSchoolAssetAsync(UpdateSchoolAssetDto dto, int schoolId, string username);
        Task<APIResponse<object>> DeleteSchoolAssetAsync(int id, int schoolId, string username);

        // Assets - Assignment
        Task<APIResponse<IEnumerable<AssetAssignmentDto>>> GetAssetAssignmentsAsync(int schoolId);
        Task<APIResponse<AssetAssignmentDto>> CreateAssetAssignmentAsync(CreateAssetAssignmentDto dto, int schoolId, string username);
        Task<APIResponse<object>> ReturnAssetAsync(ReturnAssetDto dto, int schoolId, string username);
    }
}
