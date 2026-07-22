using System.Security.Claims;
using School.Models.Account;
using School_DTOs;
using School_DTOs.Account;

namespace School.Infrastructure.JWTAuthenticationManager.Interfaces
{
    public interface IJWTAuthenticationManager
    {
        Task<AuthenticationDto> Authenticate(string userId, Claim[] claims, string accessChannel = "", string accessPlatform = "");
        public Task<APIResponse<AuthenticationDto>> RefreshTokenAsync(RefreshTokenModel refreshToken);
    }
}
