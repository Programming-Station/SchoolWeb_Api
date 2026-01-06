namespace School.Models.Account
{
    public class LogoutModel
    {
        public string? SessionId { get; set; }
        public string? LogoutReason { get; set; } // "Manual", "Timeout", "ForceLogout"
    }
}

