using System;

namespace School_DTOs.Student
{
    public class AdmissionApplicationDto : BaseDto
    {
        // ─── Identity & Numbers ──────────────────────────────────────────────────
        public int Id { get; set; }
        public string ApplicationNo { get; set; } = string.Empty;
        public string? RegistrationNo { get; set; }
        public string? AdmissionNo { get; set; }
        public string? EnrollmentNo { get; set; }
        public string? RollNo { get; set; }
        public string? StudentCode { get; set; }

        // ─── Academic Mapping ────────────────────────────────────────────────────
        public int AcademicYearId { get; set; }
        public string AcademicYearName { get; set; } = string.Empty;
        public int CampusId { get; set; }
        public string CampusName { get; set; } = string.Empty;
        public int EducationLevelId { get; set; }
        public string EducationLevelName { get; set; } = string.Empty;

        public int? FacultyId { get; set; }
        public string? FacultyName { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? ProgramId { get; set; }
        public string? ProgramName { get; set; }
        public int? CourseId { get; set; }
        public string? CourseName { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public int? YearSemesterId { get; set; }
        public string? YearSemesterName { get; set; }
        public int? BatchId { get; set; }
        public string? BatchName { get; set; }
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
        public int? SectionId { get; set; }
        public string? SectionName { get; set; }

        // ─── Core Applicant Info ─────────────────────────────────────────────────
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; }

        // ─── Personal Info (Normalized) ──────────────────────────────────────────
        public string? FathersName { get; set; }
        public string? MothersName { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianMobile { get; set; }
        public string? AadhaarNo { get; set; }
        public string? BloodGroup { get; set; }
        public string? Category { get; set; }
        public string? Religion { get; set; }
        public string? Nationality { get; set; }
        public string? MaritalStatus { get; set; }
        public string? PhotoUrl { get; set; }

        // ─── Address (Normalized) ─────────────────────────────────────────────────
        public string? PermanentAddress { get; set; }
        public string? PermanentCity { get; set; }
        public string? PermanentState { get; set; }
        public string? PermanentPinCode { get; set; }
        public string? PermanentCountry { get; set; }
        public bool SameAsPermAddress { get; set; }
        public string? CorrespondenceAddress { get; set; }
        public string? CorrespondenceCity { get; set; }
        public string? CorrespondenceState { get; set; }
        public string? CorrespondencePinCode { get; set; }

        // ─── Last Qualification (Normalized) ─────────────────────────────────────
        public string? LastQualification { get; set; }
        public string? LastInstituteName { get; set; }
        public string? LastBoardUniversity { get; set; }
        public string? LastPassingYear { get; set; }
        public string? LastObtainedMarks { get; set; }
        public string? LastTotalMarks { get; set; }
        public string? LastPercentage { get; set; }
        public string? LastGrade { get; set; }

        // ─── JSON (Dynamic / Course-Specific) ────────────────────────────────────
        public string? PrevEducationJson { get; set; }
        public string? DocumentsJson { get; set; }
        public string? CustomFieldsDataJson { get; set; }
        public string? AssignedFeesJson { get; set; }

        // ─── Workflow & Auth ──────────────────────────────────────────────────────
        public string Status { get; set; } = "Draft";
        public string? Remarks { get; set; }
        public string? VerificationNotes { get; set; }
        public string? StudentUserId { get; set; }
        public string? ParentUserId { get; set; }
    }
}
