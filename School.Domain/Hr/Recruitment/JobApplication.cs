using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hr.Recruitment
{
    public class JobApplication : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int JobPostingId { get; set; }
        [ForeignKey(nameof(JobPostingId))]
        public virtual JobPosting JobPosting { get; set; } = null!;

        public int CandidateId { get; set; }
        [ForeignKey(nameof(CandidateId))]
        public virtual Candidate Candidate { get; set; } = null!;

        public DateTime AppliedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "Submitted"; // Submitted, UnderReview, Selected, Rejected

        [MaxLength(1000)]
        public string? Feedback { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
