using School_DTOs;
using School_DTOs.Account;
using School.Models.Account;

namespace School.Services.Interfaces
{
    public interface IAccountService
    {
        Task<APIResponse<LoginResponseDto>> LoginAsync(LoginModel model);
        Task<APIResponse<OTPResponseDto>> SendOTPAsync(LoginWithOTPModel model);
        Task<APIResponse<LoginResponseDto>> VerifyOTPAsync(VerifyOTPModel model);
        Task<APIResponse> LogoutAsync(string userId, LogoutModel model);
        Task<APIResponse> ChangePasswordAsync(ChangePasswordModel model);
        Task<APIResponse<ForgotPasswordDto>> ForgotPasswordAsync(ForgotPasswordModel model);
        Task<APIResponse> ResetPasswordAsync(ResetPasswordModel model);
        Task<PagedResponse<LoginHistoryDto>> GetLoginHistoryAsync(LoginHistoryFilter filter);
        Task<APIResponse<IEnumerable<UserDto>>> GetUsersByRoleAsync(string roleName);
    }
}
