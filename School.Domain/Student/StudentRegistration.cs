using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Student
{
    public class StudentRegistration : AuditEntity<int>, ITenantEntity
    {
        public StudentRegistration()
        {
            ExperienceCertificates = new HashSet<StudentExperienceCertificate>();
            EducationalDetails = new HashSet<EducationalDetail>();
        }
        
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string AcademicYear { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string? CouncilEnrollmentNo { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string InstituteName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string CourseType { get; set; } = string.Empty; // School or UNIVERSITY
        
        [Required]
        public int CourseId { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string PassYear { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string FathersName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string MothersName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(10)]
        public string Gender { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string? DateOfBirth { get; set; }
        
        [MaxLength(10)]
        public string? BloodGroup { get; set; }

        [Required]
        [MaxLength(500)]
        public string PermanentAddress { get; set; } = string.Empty;
        
        [Required, MaxLength(10)] 
        public string PinCode { get; set; } = string.Empty;

        [Required, MaxLength(15)] 
        public string Mobile { get; set; } = string.Empty;
        
        [MaxLength(15)] 
        public string? GuardianMobile { get; set; }
        
        [MaxLength(200)]
        [EmailAddress]
        public string? Email { get; set; }
        
        [Required, MaxLength(12)] 
        public string AadhaarNumber { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? PassportPhoto { get; set; } // Base64 or file path

        [Required]
        [MaxLength(20)]
        public string PaymentStatus { get; set; } = "pending"; // pending, completed, failed
        
        [MaxLength(100)]
        public string? PaymentTransactionId { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PaymentAmount { get; set; }

        [Required]
        [MaxLength(20)]
        public string RegistrationStatus { get; set; } = "pending"; // pending, approved, rejected, under_review
        
        [MaxLength(1000)]
        public string? Remarks { get; set; }

        public virtual ICollection<StudentExperienceCertificate> ExperienceCertificates { get; set; } = new List<StudentExperienceCertificate>();
        public virtual ICollection<EducationalDetail> EducationalDetails { get; set; } = new List<EducationalDetail>();
        
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; } = null!; 

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}


