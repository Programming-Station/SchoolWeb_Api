using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Administration
{
    /// <summary>
    /// General school complaint raised by student, parent, or employee.
    /// Different from HostelComplaint which is hostel-specific.
    /// </summary>
    [Table("Complaints", Schema = "Administration")]
    public class Complaint : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string ComplaintNumber { get; set; } = null!; // e.g. CMP-2026-0001

        [Required, MaxLength(200)]
        public string Subject { get; set; } = null!;

        [Required, MaxLength(4000)]
        public string Description { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Category { get; set; } = "General"; // Academic, Discipline, Bullying, Infrastructure, Fee, Staff, Other

        [Required, MaxLength(20)]
        public string Priority { get; set; } = "Medium"; // Low, Medium, High, Critical

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Open"; // Open, InProgress, Resolved, Closed, Escalated

        [Required, MaxLength(450)]
        public string RaisedByUserId { get; set; } = null!;

        [Required, MaxLength(20)]
        public string RaisedByRole { get; set; } = "Student"; // Student, Parent, Employee

        public int? StudentId { get; set; }
        public int? EmployeeId { get; set; }

        [MaxLength(450)]
        public string? AssignedToUserId { get; set; }

        [MaxLength(200)]
        public string? AssignedToName { get; set; }

        [MaxLength(500)]
        public string? AttachmentUrl { get; set; }

        [MaxLength(2000)]
        public string? ResolutionDetails { get; set; }

        public DateTime? ResolvedDate { get; set; }

        [MaxLength(500)]
        public string? EscalationNotes { get; set; }

        public int? FeedbackRating { get; set; } // 1-5

        [MaxLength(500)]
        public string? FeedbackComments { get; set; }

        public bool IsAnonymous { get; set; } = false;

        public int SchoolRegistrationId { get; set; }
    }
}
