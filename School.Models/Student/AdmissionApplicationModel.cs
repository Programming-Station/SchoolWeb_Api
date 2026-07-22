using System.ComponentModel.DataAnnotations;

namespace School.Models.Student
{
    public class AdmissionApplicationModel
    {
        public int? Id { get; set; }
        public string? ApplicationNo { get; set; }
        public string? RegistrationNo { get; set; }
        public string? AdmissionNo { get; set; }
        public string? EnrollmentNo { get; set; }
        public string? RollNo { get; set; }
        public string? StudentCode { get; set; }

        // ─── Academic Mapping ────────────────────────────────────────────────────
        [Required(ErrorMessage = "Academic Session is required")]
        public int AcademicYearId { get; set; }

        [Required(ErrorMessage = "Campus selection is required")]
        public int CampusId { get; set; }

        [Required(ErrorMessage = "Education Level selection is required")]
        public int EducationLevelId { get; set; }

        public int? FacultyId { get; set; }
        public int? DepartmentId { get; set; }
        public int? ProgramId { get; set; }
        public int? CourseId { get; set; }
        public int? BranchId { get; set; }
        public int? YearSemesterId { get; set; }
        public int? BatchId { get; set; }
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }

        // ─── Core Applicant Info ─────────────────────────────────────────────────
        [Required(ErrorMessage = "Full Name is required")]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [MaxLength(20)]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile Number is required")]
        [MaxLength(50)]
        public string Mobile { get; set; } = string.Empty;

        [MaxLength(200)]
        [EmailAddress(ErrorMessage = "A valid email is required")]
        public string? Email { get; set; }

        // ─── Personal Info (Normalized) ──────────────────────────────────────────
        [MaxLength(200)]
        public string? FathersName { get; set; }

        [MaxLength(200)]
        public string? MothersName { get; set; }

        [MaxLength(200)]
        public string? GuardianName { get; set; }

        [MaxLength(20)]
        public string? GuardianMobile { get; set; }

        [MaxLength(20)]
        public string? AadhaarNo { get; set; }

        [MaxLength(10)]
        public string? BloodGroup { get; set; }

        /// <summary>General, OBC, SC, ST, EWS, etc.</summary>
        [MaxLength(50)]
        public string? Category { get; set; }

        [MaxLength(50)]
        public string? Religion { get; set; }

        [MaxLength(50)]
        public string? Nationality { get; set; }

        [MaxLength(20)]
        public string? MaritalStatus { get; set; }

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        // ─── Address (Normalized) ─────────────────────────────────────────────────
        [MaxLength(500)]
        public string? PermanentAddress { get; set; }

        [MaxLength(100)]
        public string? PermanentCity { get; set; }

        [MaxLength(100)]
        public string? PermanentState { get; set; }

        [MaxLength(10)]
        public string? PermanentPinCode { get; set; }

        [MaxLength(100)]
        public string? PermanentCountry { get; set; }

        public bool SameAsPermAddress { get; set; } = false;

        [MaxLength(500)]
        public string? CorrespondenceAddress { get; set; }

        [MaxLength(100)]
        public string? CorrespondenceCity { get; set; }

        [MaxLength(100)]
        public string? CorrespondenceState { get; set; }

        [MaxLength(10)]
        public string? CorrespondencePinCode { get; set; }

        // ─── Last Qualification (Normalized) ─────────────────────────────────────
        [MaxLength(100)]
        public string? LastQualification { get; set; }

        [MaxLength(200)]
        public string? LastInstituteName { get; set; }

        [MaxLength(200)]
        public string? LastBoardUniversity { get; set; }

        [MaxLength(10)]
        public string? LastPassingYear { get; set; }

        [MaxLength(20)]
        public string? LastObtainedMarks { get; set; }

        [MaxLength(20)]
        public string? LastTotalMarks { get; set; }

        [MaxLength(10)]
        public string? LastPercentage { get; set; }

        [MaxLength(20)]
        public string? LastGrade { get; set; }

        // ─── JSON (Truly Dynamic / Course-Specific Data Only) ────────────────────
        /// <summary>Full history array of all previous qualifications.</summary>
        public string? PrevEducationJson { get; set; }

        /// <summary>Document checklist with URLs and verification states.</summary>
        public string? DocumentsJson { get; set; }

        /// <summary>Course-specific dynamic form fields (e.g. Trade for ITI, CAT Score for MBA).</summary>
        public string? CustomFieldsDataJson { get; set; }

        /// <summary>Fee breakdown, installments, scholarships.</summary>
        public string? AssignedFeesJson { get; set; }

        // ─── Workflow ─────────────────────────────────────────────────────────────
        public string Status { get; set; } = "Draft";
        public string? Remarks { get; set; }
        public string? VerificationNotes { get; set; }
        public string? StudentUserId { get; set; }
        public string? ParentUserId { get; set; }
    }
}
