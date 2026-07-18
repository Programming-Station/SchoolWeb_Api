using System;
using System.Collections.Generic;

namespace School_DTOs.Communication
{
    // ════════════════════════════════════════════════════════════════════════
    // NOTIFICATION LOG DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class NotificationLogDto : BaseDto
    {
        public string RecipientUserId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string NotificationType { get; set; } = "Info";
        public string Category { get; set; } = "General";
        public string? ReferenceType { get; set; }
        public int? ReferenceId { get; set; }
        public string? ActionUrl { get; set; }
        public string? IconClass { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public bool IsPushed { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? SenderUserId { get; set; }
        public string? SenderName { get; set; }
    }

    public class CreateNotificationDto
    {
        public string RecipientUserId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string NotificationType { get; set; } = "Info";
        public string Category { get; set; } = "General";
        public string? ReferenceType { get; set; }
        public int? ReferenceId { get; set; }
        public string? ActionUrl { get; set; }
        public string? IconClass { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class NotificationLogFilterDto
    {
        public bool? IsRead { get; set; }
        public string? Category { get; set; }
        public string? NotificationType { get; set; }
    }

    public class NotificationCountDto
    {
        public int TotalUnread { get; set; }
        public int FeeNotifications { get; set; }
        public int AttendanceNotifications { get; set; }
        public int GeneralNotifications { get; set; }
    }
}
