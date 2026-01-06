using System.ComponentModel.DataAnnotations;

namespace School.Models.Account
{
    public class LoginModel : DeviceInfo
    {
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = null!;
    }

    public class LoginWithOTPModel : DeviceInfo
    {
        [Required(ErrorMessage = "Email or Mobile is required")]
        public string EmailOrMobile { get; set; } = null!;
    }

    public class VerifyOTPModel : DeviceInfo
    {
        [Required(ErrorMessage = "Email or Mobile is required")]
        public string EmailOrMobile { get; set; } = null!;

        [Required(ErrorMessage = "OTP is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be 6 digits")]
        public string OTP { get; set; } = null!;
    }
    public class DeviceInfo
    {
        public string? DeviceType { get; set; }  // e.g., "Windows", "Android", "iOS", "Mac"
        public string? Browser { get; set; }     // e.g., "Chrome", "Firefox", "Safari"
        public string? BrowserVersion { get; set; }
        public string? OperatingSystem { get; set; }
        public string? OperatingSystemVersion { get; set; }
        public string? DeviceModel { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}

