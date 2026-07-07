using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Website
{
    public class Enquiry : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string EnquiryFromNo { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        public string Mobile { get; set; } = null!;

        [MaxLength(200)]
        public string? Subject { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Message { get; set; } = null!;

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(10)]
        public string? PinCode { get; set; }

        public int? CourseId { get; set; }

        [MaxLength(200)]
        public string? CourseName { get; set; }

        [ForeignKey(nameof(CourseId))]
        public virtual School.Domain.Course? Course { get; set; }

        [Required]
        public int StatusId { get; set; } // Will be set programmatically, default to "New" status if not provided

        [ForeignKey(nameof(StatusId))]
        public virtual School.Domain.Status? Status { get; set; }

        [MaxLength(2000)]
        public string? AdminReply { get; set; }

        public DateTime? RepliedDate { get; set; }

        [MaxLength(200)]
        public string? RepliedBy { get; set; }

        [MaxLength(50)]
        public string? IpAddress { get; set; }

        [MaxLength(500)]
        public string? UserAgent { get; set; }
    }
}

