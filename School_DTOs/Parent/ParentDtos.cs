namespace School_DTOs.Parent
{
    // ── Request Models ────────────────────────────────────────────────────────────

    public class ParentLoginModel
    {
        public string Mobile { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? IpAddress { get; set; }
        public string? DeviceType { get; set; }
        public string? Browser { get; set; }
        public string? UserAgent { get; set; }
    }

    // ── Response DTOs ─────────────────────────────────────────────────────────────

    public class ParentLoginResponseDto
    {
        public string ParentUserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpireTime { get; set; }
        public List<ChildSummaryDto> Children { get; set; } = new();
    }

    public class ChildSummaryDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int SchoolRegistrationId { get; set; }
        public string SchoolName { get; set; } = string.Empty;
        public string? ClassName { get; set; }
        public string? Photo { get; set; }
        public string? EnrollmentNumber { get; set; }
        public string? Relationship { get; set; }
    }

    public class ChildDashboardDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string SchoolName { get; set; } = string.Empty;
        public string? ClassName { get; set; }
        public string? Photo { get; set; }
        public string? EnrollmentNumber { get; set; }
        public string? FathersName { get; set; }
        public string? DateOfBirth { get; set; }
        public string? MobileNo { get; set; }

        // Attendance snapshot
        public int TotalAttendanceDays { get; set; }
        public int PresentDays { get; set; }
        public double AttendancePercentage { get; set; }

        // Fee snapshot
        public decimal TotalFee { get; set; }
        public decimal PaidFee { get; set; }
        public decimal PendingFee { get; set; }
    }
}
