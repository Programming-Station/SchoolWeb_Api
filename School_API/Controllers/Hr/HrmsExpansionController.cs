using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School_DTOs.Hr;

namespace School_API.Controllers.Hr
{
    [Route("api/[controller]")]
    [ApiController]
    public class HrmsExpansionController : BaseController
    {
        private readonly IHrmsExpansionService _service;

        public HrmsExpansionController(
            IHrmsExpansionService service,
            ICurrentUserService currentUserService) : base(currentUserService)
        {
            _service = service;
        }

        // ==========================================
        // RECRUITMENT - JOB POSTINGS
        // ==========================================

        [HttpGet("job-postings")]
        public async Task<IActionResult> GetJobPostings([FromQuery] int schoolId)
        {
            var result = await _service.GetJobPostingsAsync(schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("job-postings/{id}")]
        public async Task<IActionResult> GetJobPostingById(int id, [FromQuery] int schoolId)
        {
            var result = await _service.GetJobPostingByIdAsync(id, schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("job-postings")]
        public async Task<IActionResult> CreateJobPosting([FromBody] CreateJobPostingDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.CreateJobPostingAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("job-postings")]
        public async Task<IActionResult> UpdateJobPosting([FromBody] UpdateJobPostingDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.UpdateJobPostingAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("job-postings/{id}")]
        public async Task<IActionResult> DeleteJobPosting(int id, [FromQuery] int schoolId)
        {
            var result = await _service.DeleteJobPostingAsync(id, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        // ==========================================
        // RECRUITMENT - CANDIDATES
        // ==========================================

        [HttpGet("candidates")]
        public async Task<IActionResult> GetCandidates([FromQuery] int schoolId)
        {
            var result = await _service.GetCandidatesAsync(schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("candidates/{id}")]
        public async Task<IActionResult> GetCandidateById(int id, [FromQuery] int schoolId)
        {
            var result = await _service.GetCandidateByIdAsync(id, schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("candidates")]
        public async Task<IActionResult> CreateCandidate([FromBody] CreateCandidateDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.CreateCandidateAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("candidates")]
        public async Task<IActionResult> UpdateCandidate([FromBody] UpdateCandidateDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.UpdateCandidateAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("candidates/{id}")]
        public async Task<IActionResult> DeleteCandidate(int id, [FromQuery] int schoolId)
        {
            var result = await _service.DeleteCandidateAsync(id, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        // ==========================================
        // RECRUITMENT - JOB APPLICATIONS
        // ==========================================

        [HttpGet("job-applications")]
        public async Task<IActionResult> GetJobApplications([FromQuery] int schoolId)
        {
            var result = await _service.GetJobApplicationsAsync(schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("job-applications/{id}")]
        public async Task<IActionResult> GetJobApplicationById(int id, [FromQuery] int schoolId)
        {
            var result = await _service.GetJobApplicationByIdAsync(id, schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("job-applications")]
        public async Task<IActionResult> CreateJobApplication([FromBody] CreateJobApplicationDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.CreateJobApplicationAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("job-applications/status")]
        public async Task<IActionResult> UpdateJobApplicationStatus([FromBody] UpdateJobApplicationStatusDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.UpdateJobApplicationStatusAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        // ==========================================
        // PERFORMANCE - KPI METRICS
        // ==========================================

        [HttpGet("kpi-metrics")]
        public async Task<IActionResult> GetKpiMetrics([FromQuery] int schoolId)
        {
            var result = await _service.GetKpiMetricsAsync(schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("kpi-metrics")]
        public async Task<IActionResult> CreateKpiMetric([FromBody] CreateKpiMetricDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.CreateKpiMetricAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("kpi-metrics/{id}")]
        public async Task<IActionResult> DeleteKpiMetric(int id, [FromQuery] int schoolId)
        {
            var result = await _service.DeleteKpiMetricAsync(id, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        // ==========================================
        // PERFORMANCE - PERFORMANCE REVIEWS
        // ==========================================

        [HttpGet("performance-reviews")]
        public async Task<IActionResult> GetPerformanceReviews([FromQuery] int schoolId)
        {
            var result = await _service.GetPerformanceReviewsAsync(schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("performance-reviews/{id}")]
        public async Task<IActionResult> GetPerformanceReviewById(int id, [FromQuery] int schoolId)
        {
            var result = await _service.GetPerformanceReviewByIdAsync(id, schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("performance-reviews")]
        public async Task<IActionResult> CreatePerformanceReview([FromBody] CreatePerformanceReviewDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.CreatePerformanceReviewAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("performance-reviews")]
        public async Task<IActionResult> UpdatePerformanceReview([FromBody] UpdatePerformanceReviewDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.UpdatePerformanceReviewAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("performance-reviews/{id}")]
        public async Task<IActionResult> DeletePerformanceReview(int id, [FromQuery] int schoolId)
        {
            var result = await _service.DeletePerformanceReviewAsync(id, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        // ==========================================
        // TRAINING - PROGRAMS
        // ==========================================

        [HttpGet("training-programs")]
        public async Task<IActionResult> GetTrainingPrograms([FromQuery] int schoolId)
        {
            var result = await _service.GetTrainingProgramsAsync(schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("training-programs/{id}")]
        public async Task<IActionResult> GetTrainingProgramById(int id, [FromQuery] int schoolId)
        {
            var result = await _service.GetTrainingProgramByIdAsync(id, schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("training-programs")]
        public async Task<IActionResult> CreateTrainingProgram([FromBody] CreateTrainingProgramDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.CreateTrainingProgramAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("training-programs")]
        public async Task<IActionResult> UpdateTrainingProgram([FromBody] UpdateTrainingProgramDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.UpdateTrainingProgramAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("training-programs/{id}")]
        public async Task<IActionResult> DeleteTrainingProgram(int id, [FromQuery] int schoolId)
        {
            var result = await _service.DeleteTrainingProgramAsync(id, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        // ==========================================
        // TRAINING - ENROLLMENTS
        // ==========================================

        [HttpGet("training-enrollments")]
        public async Task<IActionResult> GetTrainingEnrollments([FromQuery] int schoolId)
        {
            var result = await _service.GetTrainingEnrollmentsAsync(schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("training-enrollments")]
        public async Task<IActionResult> CreateTrainingEnrollment([FromBody] CreateTrainingEnrollmentDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.CreateTrainingEnrollmentAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("training-enrollments")]
        public async Task<IActionResult> UpdateTrainingEnrollment([FromBody] UpdateTrainingEnrollmentDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.UpdateTrainingEnrollmentAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        // ==========================================
        // ASSETS - INVENTORY
        // ==========================================

        [HttpGet("assets")]
        public async Task<IActionResult> GetSchoolAssets([FromQuery] int schoolId)
        {
            var result = await _service.GetSchoolAssetsAsync(schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("assets/{id}")]
        public async Task<IActionResult> GetSchoolAssetById(int id, [FromQuery] int schoolId)
        {
            var result = await _service.GetSchoolAssetByIdAsync(id, schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("assets")]
        public async Task<IActionResult> CreateSchoolAsset([FromBody] CreateSchoolAssetDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.CreateSchoolAssetAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("assets")]
        public async Task<IActionResult> UpdateSchoolAsset([FromBody] UpdateSchoolAssetDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.UpdateSchoolAssetAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("assets/{id}")]
        public async Task<IActionResult> DeleteSchoolAsset(int id, [FromQuery] int schoolId)
        {
            var result = await _service.DeleteSchoolAssetAsync(id, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        // ==========================================
        // ASSETS - ASSIGNMENTS
        // ==========================================

        [HttpGet("asset-assignments")]
        public async Task<IActionResult> GetAssetAssignments([FromQuery] int schoolId)
        {
            var result = await _service.GetAssetAssignmentsAsync(schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("asset-assignments")]
        public async Task<IActionResult> CreateAssetAssignment([FromBody] CreateAssetAssignmentDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.CreateAssetAssignmentAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("asset-assignments/return")]
        public async Task<IActionResult> ReturnAsset([FromBody] ReturnAssetDto dto, [FromQuery] int schoolId)
        {
            var result = await _service.ReturnAssetAsync(dto, schoolId, UserName);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
