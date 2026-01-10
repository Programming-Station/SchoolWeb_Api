using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity; 
namespace School.Domain.Website
{
    public class SchoolRegistration : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        // Basic Information
        [Required]
        [MaxLength(200)]
        public string SchoolName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string SchoolType { get; set; } = null!;

        [Required]
        public int EstablishmentYear { get; set; }

        [Required]
        [MaxLength(200)]
        public string PrincipalName { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string PrincipalEmail { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string PasswordHash { get; set; } = null!;

        // Contact Information
        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string State { get; set; } = null!;

        [Required]
        [MaxLength(10)]
        [RegularExpression(@"^[0-9]{6}$")]
        public string Pincode { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        [RegularExpression(@"^[0-9]{10}$")]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(15)]
        [RegularExpression(@"^[0-9]{10}$")]
        public string? AlternatePhone { get; set; }

        // School Details
        [Required]
        [MaxLength(100)]
        public string BoardAffiliation { get; set; } = null!;

        [MaxLength(100)]
        public string? GovernmentRegistrationNumber { get; set; }

        [MaxLength(500)]
        [Url]
        public string? SchoolWebsite { get; set; }

        // Registration Number (auto-generated at approval)
        [MaxLength(50)]
        public string? RegistrationNumber { get; set; }

        // Facilities (stored as JSON string)
        [MaxLength(2000)]
        public string FacilitiesJson { get; set; } = "{}";

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public bool TermsAccepted { get; set; }

        // Approval fields
        public DateTime? ApprovedAt { get; set; }

        [MaxLength(200)]
        public string? ApprovedBy { get; set; }

        [MaxLength(500)]
        public string? RejectionReason { get; set; }

        // IP Address for tracking
        [MaxLength(50)]
        public string? IpAddress { get; set; }

        // User Agent
        [MaxLength(500)]
        public string? UserAgent { get; set; }
         
        [Required] 
        public int StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
    }
}
