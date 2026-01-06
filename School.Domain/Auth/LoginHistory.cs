namespace School.Domain.Auth
{
    public class LoginHistory : BaseEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        // Login Information
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public DateTime? SessionExpireTime { get; set; }
        public bool IsActive { get; set; } = true;
        public string LoginMethod { get; set; } = null!; // "Credentials" or "OTP"

        // Session Information
        public string? SessionId { get; set; }
        public int SessionDurationMinutes { get; set; }
        public string? LogoutReason { get; set; } // "Manual", "Timeout", "ForceLogout", etc.

        // Device Information
        public string? IpAddress { get; set; }
        public string? DeviceType { get; set; } // "Windows", "Android", "iOS", "Mac", etc.
        public string? Browser { get; set; } // "Chrome", "Firefox", "Safari", etc.
        public string? BrowserVersion { get; set; }
        public string? OperatingSystem { get; set; }
        public string? OperatingSystemVersion { get; set; }
        public string? DeviceModel { get; set; }
        public string? UserAgent { get; set; }

        // Location Information (optional for future)
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }

        // Status and Security
        public bool LoginSuccessful { get; set; } = true;
        public string? FailureReason { get; set; }
        public bool IsSuspicious { get; set; } = false;
        public string? SecurityNotes { get; set; }
    }
}

