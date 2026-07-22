using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Student
{
    public class AdmissionApplication : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        // ─── Identity & Numbers ──────────────────────────────────────────────────
        [Required]
        [MaxLength(100)]
        public string ApplicationNo { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? RegistrationNo { get; set; }

        [MaxLength(100)]
        public string? AdmissionNo { get; set; }

        [MaxLength(100)]
        public string? EnrollmentNo { get; set; }

        [MaxLength(50)]
        public string? RollNo { get; set; }

        [MaxLength(100)]
        public string? StudentCode { get; set; }

        // ─── Academic Mapping FKs ────────────────────────────────────────────────
        [Required]
        public int AcademicYearId { get; set; }

        [Required]
        public int CampusId { get; set; }

        [Required]
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

        // ─── Core Applicant Info (Normal Columns — Searchable / Filterable) ─────
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(20)]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Mobile { get; set; } = string.Empty;

        [MaxLength(200)]
        [EmailAddress]
        public string? Email { get; set; }

        // ─── Personal Info (Normalized from PersonalInfoJson) ────────────────────
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

        // ─── Address (Normalized from AddressInfoJson) ───────────────────────────
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

        // ─── Last / Latest Qualification (Normalized from PrevEducationJson) ─────
        // Full history array stays in PrevEducationJson; last qualification summary in columns
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
        /// <summary>Full history of all previous qualifications — array of objects.</summary>
        public string? PrevEducationJson { get; set; }

        /// <summary>Document checklist with URLs and verification state (Pending/Verified/Rejected).</summary>
        public string? DocumentsJson { get; set; }

        /// <summary>Dynamic fields specific to the course/program (e.g., Trade for ITI, CAT Score for MBA, Supervisor for PhD).</summary>
        public string? CustomFieldsDataJson { get; set; }

        /// <summary>Fee breakdown, installments, scholarships, hostel/transport assignments.</summary>
        public string? AssignedFeesJson { get; set; }

        // ─── Workflow & Auth ──────────────────────────────────────────────────────
        /// <summary>Draft, Submitted, Under Verification, Approved, Rejected, Waiting List, Cancelled, Enrolled</summary>
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Draft";

        [MaxLength(1000)]
        public string? Remarks { get; set; }

        [MaxLength(1000)]
        public string? VerificationNotes { get; set; }

        [MaxLength(450)]
        public string? StudentUserId { get; set; }

        [MaxLength(450)]
        public string? ParentUserId { get; set; }

        // ─── Navigation Properties ────────────────────────────────────────────────
        [ForeignKey(nameof(AcademicYearId))]
        public virtual AcademicYear AcademicYear { get; set; } = null!;

        [ForeignKey(nameof(CampusId))]
        public virtual Campus Campus { get; set; } = null!;

        [ForeignKey(nameof(EducationLevelId))]
        public virtual EducationLevel EducationLevel { get; set; } = null!;

        [ForeignKey(nameof(FacultyId))]
        public virtual Faculty? Faculty { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department? Department { get; set; }

        [ForeignKey(nameof(ProgramId))]
        public virtual Program? Program { get; set; }

        [ForeignKey(nameof(CourseId))]
        public virtual Course? Course { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch? Branch { get; set; }

        [ForeignKey(nameof(YearSemesterId))]
        public virtual YearSemester? YearSemester { get; set; }

        [ForeignKey(nameof(BatchId))]
        public virtual Batch? Batch { get; set; }

        [ForeignKey(nameof(ClassId))]
        public virtual Class? Class { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
