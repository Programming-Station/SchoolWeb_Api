using System.Net;
using AutoMapper;
using Microsoft.Extensions.Logging;
using School.Domain.Hr.Assets;
using School.Domain.Hr.Performance;
using School.Domain.Hr.Recruitment;
using School.Domain.Hr.Training;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Hr;

namespace School.Services.Hr
{
    public class HrmsExpansionService : IHrmsExpansionService
    {
        private readonly IHrmsExpansionRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<HrmsExpansionService> _logger;

        public HrmsExpansionService(
            IHrmsExpansionRepository repo,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<HrmsExpansionService> logger)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        // ==========================================
        // RECRUITMENT - JOB POSTINGS
        // ==========================================

        public async Task<APIResponse<IEnumerable<JobPostingDto>>> GetJobPostingsAsync(int schoolId)
        {
            try
            {
                var list = await _repo.GetJobPostingsAsync(schoolId);
                var dtos = _mapper.Map<IEnumerable<JobPostingDto>>(list);
                return new APIResponse<IEnumerable<JobPostingDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = dtos };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting job postings");
                return new APIResponse<IEnumerable<JobPostingDto>> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<JobPostingDto>> GetJobPostingByIdAsync(int id, int schoolId)
        {
            try
            {
                var item = await _repo.GetJobPostingByIdAsync(id, schoolId);
                if (item == null) return new APIResponse<JobPostingDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Job posting not found." };
                return new APIResponse<JobPostingDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<JobPostingDto>(item) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting job posting by id");
                return new APIResponse<JobPostingDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<JobPostingDto>> CreateJobPostingAsync(CreateJobPostingDto dto, int schoolId, string username)
        {
            try
            {
                var entity = _mapper.Map<JobPosting>(dto);
                entity.SchoolRegistrationId = schoolId;
                entity.CreatedBy = username;
                entity.CreatedDate = DateTime.Now;

                await _repo.AddEntityAsync(entity);
                await _unitOfWork.CommitAsync();

                // Fetch again to load navigation properties (like Department)
                var loaded = await _repo.GetJobPostingByIdAsync(entity.Id, schoolId);
                return new APIResponse<JobPostingDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<JobPostingDto>(loaded) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating job posting");
                return new APIResponse<JobPostingDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<object>> UpdateJobPostingAsync(UpdateJobPostingDto dto, int schoolId, string username)
        {
            try
            {
                var existing = await _repo.GetJobPostingByIdAsync(dto.Id, schoolId);
                if (existing == null) return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Job posting not found." };

                _mapper.Map(dto, existing);
                existing.UpdatedBy = username;
                existing.UpdatedDate = DateTime.Now;

                _repo.UpdateEntity(existing);
                await _unitOfWork.CommitAsync();

                return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Job posting updated successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating job posting");
                return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<object>> DeleteJobPostingAsync(int id, int schoolId, string username)
        {
            try
            {
                var existing = await _repo.GetJobPostingByIdAsync(id, schoolId);
                if (existing == null) return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Job posting not found." };

                _repo.DeleteEntity(existing);
                await _unitOfWork.CommitAsync();

                return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Job posting deleted successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting job posting");
                return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        // ==========================================
        // RECRUITMENT - CANDIDATES
        // ==========================================

        public async Task<APIResponse<IEnumerable<CandidateDto>>> GetCandidatesAsync(int schoolId)
        {
            try
            {
                var list = await _repo.GetCandidatesAsync(schoolId);
                var dtos = _mapper.Map<IEnumerable<CandidateDto>>(list);
                return new APIResponse<IEnumerable<CandidateDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = dtos };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting candidates");
                return new APIResponse<IEnumerable<CandidateDto>> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<CandidateDto>> GetCandidateByIdAsync(int id, int schoolId)
        {
            try
            {
                var item = await _repo.GetCandidateByIdAsync(id, schoolId);
                if (item == null) return new APIResponse<CandidateDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Candidate not found." };
                return new APIResponse<CandidateDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<CandidateDto>(item) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting candidate by id");
                return new APIResponse<CandidateDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<CandidateDto>> CreateCandidateAsync(CreateCandidateDto dto, int schoolId, string username)
        {
            try
            {
                var entity = _mapper.Map<Candidate>(dto);
                entity.SchoolRegistrationId = schoolId;
                entity.CreatedBy = username;
                entity.CreatedDate = DateTime.Now;

                await _repo.AddEntityAsync(entity);
                await _unitOfWork.CommitAsync();

                return new APIResponse<CandidateDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<CandidateDto>(entity) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating candidate");
                return new APIResponse<CandidateDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<object>> UpdateCandidateAsync(UpdateCandidateDto dto, int schoolId, string username)
        {
            try
            {
                var existing = await _repo.GetCandidateByIdAsync(dto.Id, schoolId);
                if (existing == null) return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Candidate not found." };

                _mapper.Map(dto, existing);
                existing.UpdatedBy = username;
                existing.UpdatedDate = DateTime.Now;

                _repo.UpdateEntity(existing);
                await _unitOfWork.CommitAsync();

                return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Candidate updated successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating candidate");
                return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<object>> DeleteCandidateAsync(int id, int schoolId, string username)
        {
            try
            {
                var existing = await _repo.GetCandidateByIdAsync(id, schoolId);
                if (existing == null) return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Candidate not found." };

                _repo.DeleteEntity(existing);
                await _unitOfWork.CommitAsync();

                return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Candidate deleted successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting candidate");
                return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        // ==========================================
        // RECRUITMENT - JOB APPLICATIONS
        // ==========================================

        public async Task<APIResponse<IEnumerable<JobApplicationDto>>> GetJobApplicationsAsync(int schoolId)
        {
            try
            {
                var list = await _repo.GetJobApplicationsAsync(schoolId);
                var dtos = _mapper.Map<IEnumerable<JobApplicationDto>>(list);
                return new APIResponse<IEnumerable<JobApplicationDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = dtos };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting job applications");
                return new APIResponse<IEnumerable<JobApplicationDto>> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<JobApplicationDto>> GetJobApplicationByIdAsync(int id, int schoolId)
        {
            try
            {
                var item = await _repo.GetJobApplicationByIdAsync(id, schoolId);
                if (item == null) return new APIResponse<JobApplicationDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Job application not found." };
                return new APIResponse<JobApplicationDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<JobApplicationDto>(item) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting job application by id");
                return new APIResponse<JobApplicationDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<JobApplicationDto>> CreateJobApplicationAsync(CreateJobApplicationDto dto, int schoolId, string username)
        {
            try
            {
                var entity = _mapper.Map<JobApplication>(dto);
                entity.SchoolRegistrationId = schoolId;
                entity.AppliedDate = DateTime.UtcNow;
                entity.Status = "Submitted";
                entity.CreatedBy = username;
                entity.CreatedDate = DateTime.Now;

                await _repo.AddEntityAsync(entity);
                await _unitOfWork.CommitAsync();

                var loaded = await _repo.GetJobApplicationByIdAsync(entity.Id, schoolId);
                return new APIResponse<JobApplicationDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<JobApplicationDto>(loaded) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating job application");
                return new APIResponse<JobApplicationDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<object>> UpdateJobApplicationStatusAsync(UpdateJobApplicationStatusDto dto, int schoolId, string username)
        {
            try
            {
                var existing = await _repo.GetJobApplicationByIdAsync(dto.Id, schoolId);
                if (existing == null) return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Job application not found." };

                existing.Status = dto.Status;
                existing.Feedback = dto.Feedback;
                existing.UpdatedBy = username;
                existing.UpdatedDate = DateTime.Now;

                _repo.UpdateEntity(existing);
                await _unitOfWork.CommitAsync();

                return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Application status updated successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating job application status");
                return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        // ==========================================
        // PERFORMANCE - KPI METRICS
        // ==========================================

        public async Task<APIResponse<IEnumerable<KpiMetricDto>>> GetKpiMetricsAsync(int schoolId)
        {
            try
            {
                var list = await _repo.GetKpiMetricsAsync(schoolId);
                var dtos = _mapper.Map<IEnumerable<KpiMetricDto>>(list);
                return new APIResponse<IEnumerable<KpiMetricDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = dtos };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting kpi metrics");
                return new APIResponse<IEnumerable<KpiMetricDto>> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<KpiMetricDto>> CreateKpiMetricAsync(CreateKpiMetricDto dto, int schoolId, string username)
        {
            try
            {
                var entity = _mapper.Map<KpiMetric>(dto);
                entity.SchoolRegistrationId = schoolId;
                entity.CreatedBy = username;
                entity.CreatedDate = DateTime.Now;

                await _repo.AddEntityAsync(entity);
                await _unitOfWork.CommitAsync();

                return new APIResponse<KpiMetricDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<KpiMetricDto>(entity) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating kpi metric");
                return new APIResponse<KpiMetricDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<object>> DeleteKpiMetricAsync(int id, int schoolId, string username)
        {
            try
            {
                var existing = await _repo.GetKpiMetricByIdAsync(id, schoolId);
                if (existing == null) return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "KPI Metric not found." };

                _repo.DeleteEntity(existing);
                await _unitOfWork.CommitAsync();

                return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "KPI Metric deleted successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting kpi metric");
                return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        // ==========================================
        // PERFORMANCE - PERFORMANCE REVIEWS
        // ==========================================

        public async Task<APIResponse<IEnumerable<PerformanceReviewDto>>> GetPerformanceReviewsAsync(int schoolId)
        {
            try
            {
                var list = await _repo.GetPerformanceReviewsAsync(schoolId);
                var dtos = _mapper.Map<IEnumerable<PerformanceReviewDto>>(list);
                return new APIResponse<IEnumerable<PerformanceReviewDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = dtos };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting performance reviews");
                return new APIResponse<IEnumerable<PerformanceReviewDto>> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<PerformanceReviewDto>> GetPerformanceReviewByIdAsync(int id, int schoolId)
        {
            try
            {
                var item = await _repo.GetPerformanceReviewByIdAsync(id, schoolId);
                if (item == null) return new APIResponse<PerformanceReviewDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Performance review not found." };
                return new APIResponse<PerformanceReviewDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<PerformanceReviewDto>(item) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting performance review by id");
                return new APIResponse<PerformanceReviewDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<PerformanceReviewDto>> CreatePerformanceReviewAsync(CreatePerformanceReviewDto dto, int schoolId, string username)
        {
            try
            {
                var entity = _mapper.Map<PerformanceReview>(dto);
                entity.SchoolRegistrationId = schoolId;
                entity.Status = "Draft";
                entity.CreatedBy = username;
                entity.CreatedDate = DateTime.Now;

                await _repo.AddEntityAsync(entity);
                await _unitOfWork.CommitAsync();

                var loaded = await _repo.GetPerformanceReviewByIdAsync(entity.Id, schoolId);
                return new APIResponse<PerformanceReviewDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<PerformanceReviewDto>(loaded) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating performance review");
                return new APIResponse<PerformanceReviewDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<object>> UpdatePerformanceReviewAsync(UpdatePerformanceReviewDto dto, int schoolId, string username)
        {
            try
            {
                var existing = await _repo.GetPerformanceReviewByIdAsync(dto.Id, schoolId);
                if (existing == null) return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Performance review not found." };

                _mapper.Map(dto, existing);
                existing.UpdatedBy = username;
                existing.UpdatedDate = DateTime.Now;

                _repo.UpdateEntity(existing);
                await _unitOfWork.CommitAsync();

                return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Performance review updated successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating performance review");
                return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<object>> DeletePerformanceReviewAsync(int id, int schoolId, string username)
        {
            try
            {
                var existing = await _repo.GetPerformanceReviewByIdAsync(id, schoolId);
                if (existing == null) return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Performance review not found." };

                _repo.DeleteEntity(existing);
                await _unitOfWork.CommitAsync();

                return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Performance review deleted successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting performance review");
                return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        // ==========================================
        // TRAINING - PROGRAMS
        // ==========================================

        public async Task<APIResponse<IEnumerable<TrainingProgramDto>>> GetTrainingProgramsAsync(int schoolId)
        {
            try
            {
                var list = await _repo.GetTrainingProgramsAsync(schoolId);
                var dtos = _mapper.Map<IEnumerable<TrainingProgramDto>>(list);
                return new APIResponse<IEnumerable<TrainingProgramDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = dtos };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting training programs");
                return new APIResponse<IEnumerable<TrainingProgramDto>> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<TrainingProgramDto>> GetTrainingProgramByIdAsync(int id, int schoolId)
        {
            try
            {
                var item = await _repo.GetTrainingProgramByIdAsync(id, schoolId);
                if (item == null) return new APIResponse<TrainingProgramDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Training program not found." };
                return new APIResponse<TrainingProgramDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<TrainingProgramDto>(item) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting training program by id");
                return new APIResponse<TrainingProgramDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<TrainingProgramDto>> CreateTrainingProgramAsync(CreateTrainingProgramDto dto, int schoolId, string username)
        {
            try
            {
                var entity = _mapper.Map<TrainingProgram>(dto);
                entity.SchoolRegistrationId = schoolId;
                entity.Status = "Scheduled";
                entity.CreatedBy = username;
                entity.CreatedDate = DateTime.Now;

                await _repo.AddEntityAsync(entity);
                await _unitOfWork.CommitAsync();

                return new APIResponse<TrainingProgramDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<TrainingProgramDto>(entity) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating training program");
                return new APIResponse<TrainingProgramDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<object>> UpdateTrainingProgramAsync(UpdateTrainingProgramDto dto, int schoolId, string username)
        {
            try
            {
                var existing = await _repo.GetTrainingProgramByIdAsync(dto.Id, schoolId);
                if (existing == null) return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Training program not found." };

                _mapper.Map(dto, existing);
                existing.UpdatedBy = username;
                existing.UpdatedDate = DateTime.Now;

                _repo.UpdateEntity(existing);
                await _unitOfWork.CommitAsync();

                return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Training program updated successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating training program");
                return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<object>> DeleteTrainingProgramAsync(int id, int schoolId, string username)
        {
            try
            {
                var existing = await _repo.GetTrainingProgramByIdAsync(id, schoolId);
                if (existing == null) return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Training program not found." };

                _repo.DeleteEntity(existing);
                await _unitOfWork.CommitAsync();

                return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Training program deleted successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting training program");
                return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        // ==========================================
        // TRAINING - ENROLLMENTS
        // ==========================================

        public async Task<APIResponse<IEnumerable<TrainingEnrollmentDto>>> GetTrainingEnrollmentsAsync(int schoolId)
        {
            try
            {
                var list = await _repo.GetTrainingEnrollmentsAsync(schoolId);
                var dtos = _mapper.Map<IEnumerable<TrainingEnrollmentDto>>(list);
                return new APIResponse<IEnumerable<TrainingEnrollmentDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = dtos };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting training enrollments");
                return new APIResponse<IEnumerable<TrainingEnrollmentDto>> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<TrainingEnrollmentDto>> CreateTrainingEnrollmentAsync(CreateTrainingEnrollmentDto dto, int schoolId, string username)
        {
            try
            {
                var entity = _mapper.Map<TrainingEnrollment>(dto);
                entity.SchoolRegistrationId = schoolId;
                entity.EnrollmentDate = DateTime.UtcNow;
                entity.Status = "Enrolled";
                entity.CreatedBy = username;
                entity.CreatedDate = DateTime.Now;

                await _repo.AddEntityAsync(entity);
                await _unitOfWork.CommitAsync();

                var loaded = await _repo.GetTrainingEnrollmentByIdAsync(entity.Id, schoolId);
                return new APIResponse<TrainingEnrollmentDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<TrainingEnrollmentDto>(loaded) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating training enrollment");
                return new APIResponse<TrainingEnrollmentDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<object>> UpdateTrainingEnrollmentAsync(UpdateTrainingEnrollmentDto dto, int schoolId, string username)
        {
            try
            {
                var existing = await _repo.GetTrainingEnrollmentByIdAsync(dto.Id, schoolId);
                if (existing == null) return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Training enrollment not found." };

                existing.Status = dto.Status;
                existing.Feedback = dto.Feedback;
                existing.UpdatedBy = username;
                existing.UpdatedDate = DateTime.Now;

                _repo.UpdateEntity(existing);
                await _unitOfWork.CommitAsync();

                return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Training enrollment updated successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating training enrollment");
                return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        // ==========================================
        // ASSETS - INVENTORY
        // ==========================================

        public async Task<APIResponse<IEnumerable<SchoolAssetDto>>> GetSchoolAssetsAsync(int schoolId)
        {
            try
            {
                var list = await _repo.GetSchoolAssetsAsync(schoolId);
                var dtos = _mapper.Map<IEnumerable<SchoolAssetDto>>(list);
                return new APIResponse<IEnumerable<SchoolAssetDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = dtos };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting school assets");
                return new APIResponse<IEnumerable<SchoolAssetDto>> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<SchoolAssetDto>> GetSchoolAssetByIdAsync(int id, int schoolId)
        {
            try
            {
                var item = await _repo.GetSchoolAssetByIdAsync(id, schoolId);
                if (item == null) return new APIResponse<SchoolAssetDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "School asset not found." };
                return new APIResponse<SchoolAssetDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<SchoolAssetDto>(item) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting school asset by id");
                return new APIResponse<SchoolAssetDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<SchoolAssetDto>> CreateSchoolAssetAsync(CreateSchoolAssetDto dto, int schoolId, string username)
        {
            try
            {
                var entity = _mapper.Map<SchoolAsset>(dto);
                entity.SchoolRegistrationId = schoolId;
                entity.Status = "Available";
                entity.CreatedBy = username;
                entity.CreatedDate = DateTime.Now;

                await _repo.AddEntityAsync(entity);
                await _unitOfWork.CommitAsync();

                return new APIResponse<SchoolAssetDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<SchoolAssetDto>(entity) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating school asset");
                return new APIResponse<SchoolAssetDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<object>> UpdateSchoolAssetAsync(UpdateSchoolAssetDto dto, int schoolId, string username)
        {
            try
            {
                var existing = await _repo.GetSchoolAssetByIdAsync(dto.Id, schoolId);
                if (existing == null) return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "School asset not found." };

                _mapper.Map(dto, existing);
                existing.UpdatedBy = username;
                existing.UpdatedDate = DateTime.Now;

                _repo.UpdateEntity(existing);
                await _unitOfWork.CommitAsync();

                return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "School asset updated successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating school asset");
                return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<object>> DeleteSchoolAssetAsync(int id, int schoolId, string username)
        {
            try
            {
                var existing = await _repo.GetSchoolAssetByIdAsync(id, schoolId);
                if (existing == null) return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "School asset not found." };

                _repo.DeleteEntity(existing);
                await _unitOfWork.CommitAsync();

                return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "School asset deleted successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting school asset");
                return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        // ==========================================
        // ASSETS - ASSIGNMENTS
        // ==========================================

        public async Task<APIResponse<IEnumerable<AssetAssignmentDto>>> GetAssetAssignmentsAsync(int schoolId)
        {
            try
            {
                var list = await _repo.GetAssetAssignmentsAsync(schoolId);
                var dtos = _mapper.Map<IEnumerable<AssetAssignmentDto>>(list);
                return new APIResponse<IEnumerable<AssetAssignmentDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = dtos };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting asset assignments");
                return new APIResponse<IEnumerable<AssetAssignmentDto>> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<AssetAssignmentDto>> CreateAssetAssignmentAsync(CreateAssetAssignmentDto dto, int schoolId, string username)
        {
            try
            {
                // Check if the asset is already assigned or not available
                var asset = await _repo.GetSchoolAssetByIdAsync(dto.SchoolAssetId, schoolId);
                if (asset == null) return new APIResponse<AssetAssignmentDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Asset not found." };
                if (asset.Status != "Available") return new APIResponse<AssetAssignmentDto> { Success = false, StatusCode = HttpStatusCode.BadRequest, Message = $"Asset is currently {asset.Status} and cannot be assigned." };

                var entity = _mapper.Map<AssetAssignment>(dto);
                entity.SchoolRegistrationId = schoolId;
                entity.AssignedDate = DateTime.UtcNow;
                entity.Status = "Assigned";
                entity.CreatedBy = username;
                entity.CreatedDate = DateTime.Now;

                // Update Asset status to Assigned
                asset.Status = "Assigned";
                _repo.UpdateEntity(asset);

                await _repo.AddEntityAsync(entity);
                await _unitOfWork.CommitAsync();

                var loaded = await _repo.GetAssetAssignmentByIdAsync(entity.Id, schoolId);
                return new APIResponse<AssetAssignmentDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<AssetAssignmentDto>(loaded) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating asset assignment");
                return new APIResponse<AssetAssignmentDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }

        public async Task<APIResponse<object>> ReturnAssetAsync(ReturnAssetDto dto, int schoolId, string username)
        {
            try
            {
                var existing = await _repo.GetAssetAssignmentByIdAsync(dto.Id, schoolId);
                if (existing == null) return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Asset assignment not found." };
                if (existing.Status == "Returned") return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.BadRequest, Message = "Asset has already been returned." };

                existing.Status = "Returned";
                existing.ReturnDate = DateTime.UtcNow;
                existing.Condition = dto.Condition;
                existing.UpdatedBy = username;
                existing.UpdatedDate = DateTime.Now;

                // Free up the asset status back to Available
                var asset = await _repo.GetSchoolAssetByIdAsync(existing.SchoolAssetId, schoolId);
                if (asset != null)
                {
                    asset.Status = "Available";
                    _repo.UpdateEntity(asset);
                }

                _repo.UpdateEntity(existing);
                await _unitOfWork.CommitAsync();

                return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Asset returned successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error returning asset");
                return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred." };
            }
        }
    }
}
