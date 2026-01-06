namespace School_DTOs.Account
{
    public class LoginHistoryDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? FullName { get; set; }

        // Login Information
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public DateTime? SessionExpireTime { get; set; }
        public bool IsActive { get; set; }
        public string LoginMethod { get; set; } = null!;

        // Session Information
        public string? SessionId { get; set; }
        public int SessionDurationMinutes { get; set; }
        public string? LogoutReason { get; set; }

        // Device Information
        public string? IpAddress { get; set; }
        public string? DeviceType { get; set; }
        public string? Browser { get; set; }
        public string? BrowserVersion { get; set; }
        public string? OperatingSystem { get; set; }
        public string? OperatingSystemVersion { get; set; }
        public string? DeviceModel { get; set; }

        // Location Information
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }

        // Status and Security
        public bool LoginSuccessful { get; set; }
        public string? FailureReason { get; set; }
        public bool IsSuspicious { get; set; }
        public string? SecurityNotes { get; set; }
    }
}

