namespace School_DTOs.Student
{
    public class StudentRegistrationDto : BaseDto
    {
        // Academic Information
        public string AcademicYear { get; set; } = string.Empty;
        public string? CouncilEnrollmentNo { get; set; }
        public string InstituteName { get; set; } = string.Empty;

        // Course Information
        public string CourseType { get; set; } = string.Empty;
        public string? CourseName { get; set; } 
        public string PassYear { get; set; } = string.Empty;

        // Personal Information
        public string FullName { get; set; } = string.Empty;
        public string FathersName { get; set; } = string.Empty;
        public string MothersName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? BloodGroup { get; set; }

        // Address
        public string PermanentAddress { get; set; } = string.Empty;
        public string PinCode { get; set; } = string.Empty;

        // Contact Information
        public string Mobile { get; set; } = string.Empty;
        public string? GuardianMobile { get; set; }
        public string? Email { get; set; }
        public string AadhaarNumber { get; set; } = string.Empty;
        // Photo
        public string? PassportPhoto { get; set; }

        // Payment Information
        public string PaymentStatus { get; set; } = "pending";
        public string? PaymentTransactionId { get; set; }
        public decimal? PaymentAmount { get; set; }

        // Registration Status
        public string RegistrationStatus { get; set; } = "pending";
        public string? Remarks { get; set; }
        // Educational Details  
        public List<EducationalDetailDto> EducationalDetails { get; set; } = new List<EducationalDetailDto>();

        // Experience Certificates
        public List<StudentExperienceCertificateDto> ExperienceCertificates { get; set; } = new List<StudentExperienceCertificateDto>();
    }
}

