namespace School_DTOs.Account
{
    public class ForgotPasswordDto
    {
        public string Token { get; set; } = null!;
        public DateTime ExpireTime { get; set; }
    }
}
