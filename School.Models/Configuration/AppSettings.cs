namespace School.Models.Configuration
{
    public class AppSettings
    {
        public string Mode { get; set; } = "D";
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public int ExpireTime { get; set; } = 1440; // minutes
        public int RefreshTokenExpireTime { get; set; } = 10080; // 7 days in minutes
        public string Audience { get; set; } = string.Empty;
        public bool EnablePushNotification { get; set; } = false;
        public string InvalidAllowedLoginAttempts { get; set; } = "5";
        public string AccountBlockedPeriodinMinutes { get; set; } = "30";
        public bool RequireHttps { get; set; } = true;
        public int RateLimitPerMinute { get; set; } = 60;
        public bool EnableCors { get; set; } = true;
        public List<string> AllowedOrigins { get; set; } = new List<string>();
        public string ImageStoragePath { get; set; } = string.Empty; // Drive path for storing images (e.g., "D:\\Images" or "E:\\Uploads")
    }
}

