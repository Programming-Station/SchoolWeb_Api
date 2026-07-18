using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    /// <summary>
    /// In-app notification bell notifications. Stores individual notifications
    /// delivered to users via SignalR or polling.
    /// </summary>
    [Table("NotificationLogs", Schema = "Communication")]
    public class NotificationLog : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(450)]
        public string RecipientUserId { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required, MaxLength(1000)]
        public string Message { get; set; } = null!;

        [Required, MaxLength(50)]
        public string NotificationType { get; set; } = "Info"; // Info, Warning, Success, Error, Reminder

        [Required, MaxLength(50)]
        public string Category { get; set; } = "General"; // Fee, Attendance, Exam, Transport, Hostel, Library, HR, General

        [MaxLength(50)]
        public string? ReferenceType { get; set; } // Entity type: Student, Employee, FeePayment, etc.

        public int? ReferenceId { get; set; } // FK to referenced entity

        [MaxLength(500)]
        public string? ActionUrl { get; set; } // Deep link to relevant page

        [MaxLength(200)]
        public string? IconClass { get; set; } // PrimeNG icon class

        public bool IsRead { get; set; } = false;

        public DateTime? ReadDate { get; set; }

        public bool IsPushed { get; set; } = false; // Sent via push notification?

        public DateTime? PushedDate { get; set; }

        public DateTime? ExpiryDate { get; set; } // Auto-dismiss after this date

        [MaxLength(450)]
        public string? SenderUserId { get; set; } // Null for system-generated

        public int SchoolRegistrationId { get; set; }
    }
}
