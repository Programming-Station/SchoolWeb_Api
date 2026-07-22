using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using School.Infrastructure.JWTAuthenticationManager.Interfaces;
using School.Models.Account;
using School.Services.Interfaces;
using School_DTOs;

namespace School_API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer,Basic")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [EnableRateLimiting("api")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IJWTAuthenticationManager _jwtService;
        public AccountController(IAccountService accountService, IJWTAuthenticationManager jWTAuthentication)
        {
            _accountService = accountService;
            _jwtService = jWTAuthentication;
        }

        /// <summary>
        /// Login with User ID and Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(model.IpAddress))
            {
                model.IpAddress = ipAddress;
            }

            var result = await _accountService.LoginAsync(model);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Send OTP to Email or Mobile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SendOTP([FromBody] LoginWithOTPModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(model.IpAddress))
            {
                model.IpAddress = ipAddress;
            }

            var result = await _accountService.SendOTPAsync(model);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Verify OTP and Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(model.IpAddress))
            {
                model.IpAddress = ipAddress;
            }

            var result = await _accountService.VerifyOTPAsync(model);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Logout user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Logout([FromBody] LogoutModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new APIResponse
                {
                    Message = "User not authenticated",
                    StatusCode = System.Net.HttpStatusCode.Unauthorized,
                    Success = false
                });
            }

            var result = await _accountService.LogoutAsync(userId, model);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// This method user for refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel refreshToken)
        {
            return Ok(await _jwtService.RefreshTokenAsync(refreshToken));
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _accountService.ForgotPasswordAsync(model));
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (DateTime.Now > model.ExpireTime)
            {
                return Ok(new APIResponse
                {
                    Message = "The token has expired.",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                });
            }
            return Ok(await _accountService.ResetPasswordAsync(model));
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await _accountService.ChangePasswordAsync(resetPasswordDto);
            return Ok(res);
        }

        /// <summary>
        /// Get login audit/history logs with filtering, pagination, and sorting
        /// </summary>
        /// <param name="filter">Filter parameters including pagination, search, date range, etc.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLoginHistory([FromQuery] LoginHistoryFilter filter)
        {
            var result = await _accountService.GetLoginHistoryAsync(filter);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all users by role name (e.g., "Admin")
        /// </summary>
        /// <param name="roleName">Role name to filter users</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUsersByRole(string roleName = "Admin")
        {
            var result = await _accountService.GetUsersByRoleAsync(roleName);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

    }
}
