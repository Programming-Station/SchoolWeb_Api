using System.ComponentModel.DataAnnotations;

namespace School_DTOs.School
{
    public class SchoolOwnerDto : BaseDto
    {
        public int SchoolRegistrationId { get; set; }
        public int? SchoolSubscriptionId { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public string? ProfilePhoto { get; set; }
        public int StatusId { get; set; }
        public bool EmailVerified { get; set; }
        public bool MobileVerified { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string? LastLoginIp { get; set; }
        public int FailedLoginAttempt { get; set; }
        public bool IsLocked { get; set; }
        public string? SchoolName { get; set; }
        public string? UserName { get; set; }
    }

    public class SchoolOwnerModel
    {
        public int? Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        public int? SchoolSubscriptionId { get; set; }

        public string ApplicationUserId { get; set; } = string.Empty;

        public string? ProfilePhoto { get; set; }

        public int StatusId { get; set; }

        public bool EmailVerified { get; set; }

        public bool MobileVerified { get; set; }

        public bool IsLocked { get; set; }
    }
}
