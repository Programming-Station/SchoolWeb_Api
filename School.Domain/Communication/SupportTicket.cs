using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("SupportTickets", Schema = "Communication")]
    public class SupportTicket : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string TicketNumber { get; set; } = null!; // e.g. TKT-2026-0001

        [Required, MaxLength(200)]
        public string Subject { get; set; } = null!;

        [Required, MaxLength(4000)]
        public string Description { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Category { get; set; } = "General"; // Academic, Fee, Transport, Hostel, IT, General

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Open"; // Open, InProgress, Resolved, Closed

        [Required, MaxLength(20)]
        public string Priority { get; set; } = "Medium"; // Low, Medium, High, Urgent

        [Required, MaxLength(450)]
        public string RaisedByUserId { get; set; } = null!;

        [MaxLength(450)]
        public string? AssignedStaffId { get; set; } // Employee ID or User ID assigned

        public DateTime? SLAExpiryDate { get; set; }

        [MaxLength(1000)]
        public string? ResolutionNotes { get; set; }

        public int? FeedbackRating { get; set; } // 1 to 5 stars

        [MaxLength(500)]
        public string? FeedbackComments { get; set; }

        public int SchoolRegistrationId { get; set; }
    }
}
