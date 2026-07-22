using System.ComponentModel.DataAnnotations;

namespace School_DTOs.Hr
{
    // ==========================================
    // RECRUITMENT DTOs
    // ==========================================

    public class JobPostingDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string Status { get; set; } = "Open";
        public DateTime ExpiryDate { get; set; }
        public int SchoolRegistrationId { get; set; }
    }

    public class CreateJobPostingDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Requirements { get; set; } = string.Empty;

        public int DepartmentId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class UpdateJobPostingDto : CreateJobPostingDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Open";
    }

    public class CandidateDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? ResumePath { get; set; }
        public string Status { get; set; } = "Applied";
        public int SchoolRegistrationId { get; set; }
    }

    public class CreateCandidateDto
    {
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? ResumePath { get; set; }
    }

    public class UpdateCandidateDto : CreateCandidateDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Applied";
    }

    public class JobApplicationDto
    {
        public int Id { get; set; }
        public int JobPostingId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public int CandidateId { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string CandidateEmail { get; set; } = string.Empty;
        public string CandidatePhone { get; set; } = string.Empty;
        public string? CandidateResumePath { get; set; }
        public DateTime AppliedDate { get; set; }
        public string Status { get; set; } = "Submitted";
        public string? Feedback { get; set; }
    }

    public class CreateJobApplicationDto
    {
        public int JobPostingId { get; set; }
        public int CandidateId { get; set; }
    }

    public class UpdateJobApplicationStatusDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "Submitted";
        [MaxLength(1000)]
        public string? Feedback { get; set; }
    }


    // ==========================================
    // PERFORMANCE APPRAISAL DTOs
    // ==========================================

    public class KpiMetricDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Weightage { get; set; }
        public string TargetValue { get; set; } = string.Empty;
    }

    public class CreateKpiMetricDto
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public int Weightage { get; set; }

        [Required]
        [MaxLength(200)]
        public string TargetValue { get; set; } = string.Empty;
    }

    public class PerformanceReviewDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public int ReviewerId { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public DateTime ReviewDate { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal KpiScore { get; set; }
        public string? Comments { get; set; }
        public string Status { get; set; } = "Draft";
    }

    public class CreatePerformanceReviewDto
    {
        public int EmployeeId { get; set; }
        public int ReviewerId { get; set; }
        public DateTime ReviewDate { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal KpiScore { get; set; }
        [MaxLength(1000)]
        public string? Comments { get; set; }
    }

    public class UpdatePerformanceReviewDto : CreatePerformanceReviewDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Draft";
    }


    // ==========================================
    // TRAINING DTOs
    // ==========================================

    public class TrainingProgramDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TrainerName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; }
        public string Status { get; set; } = "Scheduled";
    }

    public class CreateTrainingProgramDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string TrainerName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [MaxLength(150)]
        public string? Location { get; set; }
    }

    public class UpdateTrainingProgramDto : CreateTrainingProgramDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Scheduled";
    }

    public class TrainingEnrollmentDto
    {
        public int Id { get; set; }
        public int TrainingProgramId { get; set; }
        public string TrainingTitle { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; } = "Enrolled";
        public string? Feedback { get; set; }
    }

    public class CreateTrainingEnrollmentDto
    {
        public int TrainingProgramId { get; set; }
        public int EmployeeId { get; set; }
    }

    public class UpdateTrainingEnrollmentDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Enrolled";
        [MaxLength(500)]
        public string? Feedback { get; set; }
    }


    // ==========================================
    // ASSET ASSIGNMENT DTOs
    // ==========================================

    public class SchoolAssetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AssetCode { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? SerialNo { get; set; }
        public string Status { get; set; } = "Available";
    }

    public class CreateSchoolAssetDto
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string AssetCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? SerialNo { get; set; }
    }

    public class UpdateSchoolAssetDto : CreateSchoolAssetDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Available";
    }

    public class AssetAssignmentDto
    {
        public int Id { get; set; }
        public int SchoolAssetId { get; set; }
        public string AssetName { get; set; } = string.Empty;
        public string AssetCode { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = "Assigned";
        public string? Condition { get; set; }
    }

    public class CreateAssetAssignmentDto
    {
        public int SchoolAssetId { get; set; }
        public int EmployeeId { get; set; }
        [MaxLength(200)]
        public string? Condition { get; set; }
    }

    public class ReturnAssetDto
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string? Condition { get; set; }
    }
}
