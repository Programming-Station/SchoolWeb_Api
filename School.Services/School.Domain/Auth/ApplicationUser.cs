using Microsoft.AspNetCore.Identity;

namespace School.Domain.Auth
{
    public partial class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            RefreshTokens = new List<RefreshToken>();
        }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsDefaultPassword { get; set; } = false;
        public DateTime? PasswordUpdatedOn { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public bool OwnsToken(string token)
        {
            return RefreshTokens?.Find(x => x.Token == token) != null;
        }
        public int StatusId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? LastLoginDate { get; set; }
        public string? LastLoginIpAddress { get; set; }
        public int FailedLoginAttempts { get; set; } = 0;
        public DateTime? LockoutEndDate { get; set; }
    }
}

