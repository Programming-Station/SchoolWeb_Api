namespace School_DTOs.Student
{
    public class StudentRegistrationDto : BaseDto
    {
        public string AcademicYear { get; set; } = string.Empty;
        public string? CouncilEnrollmentNo { get; set; }
        public string InstituteName { get; set; } = string.Empty;

        public string CourseType { get; set; } = string.Empty;
        public string? CourseName { get; set; } 
        public string PassYear { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
        public string FathersName { get; set; } = string.Empty;
        public string MothersName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? BloodGroup { get; set; }

        public string PermanentAddress { get; set; } = string.Empty;
        public string PinCode { get; set; } = string.Empty;

        public string Mobile { get; set; } = string.Empty;
        public string? GuardianMobile { get; set; }
        public string? Email { get; set; }
        public string AadhaarNumber { get; set; } = string.Empty;
        public string? PassportPhoto { get; set; }

        public string PaymentStatus { get; set; } = "pending";
        public string? PaymentTransactionId { get; set; }
        public decimal? PaymentAmount { get; set; }

        public string RegistrationStatus { get; set; } = "pending";
        public string? Remarks { get; set; }
        public List<EducationalDetailDto> EducationalDetails { get; set; } = new List<EducationalDetailDto>();

        public List<StudentExperienceCertificateDto> ExperienceCertificates { get; set; } = new List<StudentExperienceCertificateDto>();
    }
}

