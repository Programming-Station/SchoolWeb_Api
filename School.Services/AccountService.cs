using School.Infrastructure.Repositories.IRepositories;
using School.Models.Account;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Account;

namespace School.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<APIResponse<LoginResponseDto>> LoginAsync(LoginModel model)
        {
            return _accountRepository.LoginAsync(model);
        }

        public Task<APIResponse<OTPResponseDto>> SendOTPAsync(LoginWithOTPModel model)
        {
            return _accountRepository.SendOTPAsync(model);
        }

        public Task<APIResponse<LoginResponseDto>> VerifyOTPAsync(VerifyOTPModel model)
        {
            return _accountRepository.VerifyOTPAsync(model);
        }

        public Task<APIResponse> LogoutAsync(string userId, LogoutModel model)
        {
            return _accountRepository.LogoutAsync(userId, model);
        }

        public Task<APIResponse> ChangePasswordAsync(ChangePasswordModel model)
        {
            return _accountRepository.ChangePasswordAsync(model);
        }

        public async Task<APIResponse<ForgotPasswordDto>> ForgotPasswordAsync(ForgotPasswordModel model)
        {
            return await _accountRepository.ForgotPasswordAsync(model);
        }

        public Task<APIResponse> ResetPasswordAsync(ResetPasswordModel model)
        {
            return _accountRepository.ResetPasswordAsync(model);
        }

        public Task<PagedResponse<LoginHistoryDto>> GetLoginHistoryAsync(LoginHistoryFilter filter)
        {
            return _accountRepository.GetLoginHistoryAsync(filter);
        }

        public Task<APIResponse<IEnumerable<UserDto>>> GetUsersByRoleAsync(string roleName)
        {
            return _accountRepository.GetUsersByRoleAsync(roleName);
        }

    }
}
