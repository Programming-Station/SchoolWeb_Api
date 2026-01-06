namespace School_DTOs.Account
{
    public class LoginResponseDto
    {
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime ExpireTime { get; set; }
        public bool IsDefaultPassword { get; set; }
    }

    public class OTPResponseDto
    {
        public string Message { get; set; } = null!;
        public DateTime ExpiryTime { get; set; }
    }
}

