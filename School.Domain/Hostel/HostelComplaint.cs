using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hostel
{
    [Table("HostelComplaints", Schema = "Hostel")]
    public class HostelComplaint : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }

        [Required, MaxLength(100)]
        public string Category { get; set; } = null!; // Electrical, Water, Furniture, Internet, Cleaning, Food, Security, Medical

        [Required, MaxLength(1000)]
        public string Description { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Priority { get; set; } = "Medium"; // Low, Medium, High, Emergency

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Open"; // Open, Assigned, Resolved, Closed

        public int? AssignedStaffId { get; set; }

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        [MaxLength(1000)]
        public string? ResolutionDetails { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual global::School.Domain.Student.Student Student { get; set; } = null!;

        [ForeignKey(nameof(AssignedStaffId))]
        public virtual global::School.Domain.Hr.Employee? AssignedStaff { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
