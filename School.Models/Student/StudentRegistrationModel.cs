using System.ComponentModel.DataAnnotations;
using School.Models.CustomeVailidation;

namespace School.Models.Student
{
    public class StudentRegistrationModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Academic Year is required")]
        [MaxLength(50)]
        [NoScript]
        public string AcademicYear { get; set; } = string.Empty;

        [MaxLength(100)]
        [NoScript]
        public string? CouncilEnrollmentNo { get; set; }

        [Required(ErrorMessage = "Institute Name is required")]
        [MaxLength(200)]
        [NoScript]
        public string InstituteName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course Type is required")]
        [MaxLength(50)]
        public string CourseType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course ID is required")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Pass Year is required")]
        [MaxLength(10)]
        public string PassYear { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full Name is required")]
        [MaxLength(200)]
        [NoScript]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Father's Name is required")]
        [MaxLength(200)]
        [NoScript]
        public string FathersName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mother's Name is required")]
        [MaxLength(200)]
        [NoScript]
        public string MothersName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required")]
        [MaxLength(10)]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(10)]
        public string? BloodGroup { get; set; }

        [Required(ErrorMessage = "Permanent Address is required")]
        [MaxLength(500)]
        [NoScript]
        public string PermanentAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pin Code is required")]
        [MaxLength(10)]
        [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Pin Code must be 6 digits")]
        public string PinCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile Number is required")]
        [MaxLength(15)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile Number must be 10 digits")]
        public string Mobile { get; set; } = string.Empty;

        [MaxLength(15)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Guardian Mobile Number must be 10 digits")]
        public string? GuardianMobile { get; set; }

        [MaxLength(200)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Aadhaar Number is required")]
        [MaxLength(12)]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "Aadhaar Number must be 12 digits")]
        public string AadhaarNumber { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? PassportPhoto { get; set; }

        [MaxLength(20)]
        public string PaymentStatus { get; set; } = "pending";
        [MaxLength(100)]
        public string? PaymentTransactionId { get; set; }
        public decimal? PaymentAmount { get; set; }

        [MaxLength(20)]
        public string RegistrationStatus { get; set; } = "pending";
        [MaxLength(1000)]
        [NoScript]
        public string? Remarks { get; set; }
        public List<EducationalDetailModel> EducationalDetails { get; set; } = new List<EducationalDetailModel>();
        public List<StudentExperienceCertificateModel> ExperienceCertificates { get; set; } = new List<StudentExperienceCertificateModel>();

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

