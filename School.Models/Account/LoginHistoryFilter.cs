namespace School.Models.Account
{
    /// <summary>
    /// Filter model for login history queries
    /// </summary>
    public class LoginHistoryFilter : BaseFilter
    {
        /// <summary>
        /// Filter by specific user ID. Leave null for all users
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Filter by login method (e.g., "Credentials", "OTP")
        /// </summary>
        public string? LoginMethod { get; set; }

        /// <summary>
        /// Filter by login status (successful/failed)
        /// </summary>
        public bool? LoginSuccessful { get; set; }

        /// <summary>
        /// Filter by IP address
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// Filter by device type (e.g., "Desktop", "Mobile")
        /// </summary>
        public string? DeviceType { get; set; }
    }
}

