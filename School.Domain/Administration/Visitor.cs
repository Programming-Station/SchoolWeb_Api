using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Administration
{
    /// <summary>
    /// Tracks visitors to the school campus (gate management / reception desk).
    /// </summary>
    [Table("Visitors", Schema = "Administration")]
    public class Visitor : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string VisitorNumber { get; set; } = null!; // e.g. VIS-2026-0001

        [Required, MaxLength(200)]
        public string VisitorName { get; set; } = null!;

        [MaxLength(100)]
        public string? Organization { get; set; }

        [Required, MaxLength(20)]
        public string ContactNumber { get; set; } = null!;

        [MaxLength(100)]
        public string? Email { get; set; }

        [Required, MaxLength(50)]
        public string IdProofType { get; set; } = "Aadhaar"; // Aadhaar, PAN, DrivingLicense, VoterID, Passport

        [MaxLength(50)]
        public string? IdProofNumber { get; set; }

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        [MaxLength(500)]
        public string? IdProofUrl { get; set; }

        [Required, MaxLength(100)]
        public string Purpose { get; set; } = null!; // Meeting, Delivery, Interview, ParentVisit, Official, Other

        [MaxLength(200)]
        public string? PersonToMeet { get; set; }

        [MaxLength(100)]
        public string? Department { get; set; }

        public int? StudentId { get; set; } // If visiting a specific student

        public int NumberOfVisitors { get; set; } = 1;

        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        [MaxLength(50)]
        public string? VisitorBadgeNumber { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }

        [MaxLength(50)]
        public string? QrCode { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "CheckedIn"; // CheckedIn, CheckedOut, Denied, Waiting

        [MaxLength(450)]
        public string? ApprovedByUserId { get; set; }

        public int SchoolRegistrationId { get; set; }
    }
}
