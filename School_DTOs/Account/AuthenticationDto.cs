namespace School_DTOs.Account
{
    public class AuthenticationDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime ExpireTime { get; set; }
    }
}
