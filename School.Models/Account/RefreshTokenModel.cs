namespace School.Models.Account
{
    public class RefreshTokenModel
    {
        public string UserId { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
