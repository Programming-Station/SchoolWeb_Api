using System.Net;
using System.Data;
using System.Security.Claims;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.JWTAuthenticationManager.Interfaces;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School_DTOs.Account;
using School.Models.Account;
using School.Domain.Auth;
using Microsoft.AspNetCore.Identity;
using School.Models.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using School_DTOs;
using School.Utilities.Resources;
using Microsoft.EntityFrameworkCore;

namespace School.Infrastructure.Repositories
{
    public class AccountRepository : Repository<ApplicationUser>, IAccountRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppSettings _appSettings;
        private readonly IJWTAuthenticationManager _jWTAuthenticationManager;
        private readonly IHttpContextAccessor _accessor;
        private readonly SchoolDbContext _context;

        private static readonly Dictionary<string, (string OTP, DateTime Expiry)> _otpStorage = new();

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
         IOptions<AppSettings> appSetting, IJWTAuthenticationManager jWTAuthenticationManager,
        IHttpContextAccessor accessor, DbFactory dbFactory, IUnitOfWork unitOfWork,
        RoleManager<IdentityRole> roleManager, SchoolDbContext context) : base(dbFactory)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSetting.Value;
            _jWTAuthenticationManager = jWTAuthenticationManager;
            _accessor = accessor;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<APIResponse<LoginResponseDto>> LoginAsync(LoginModel model)
        {
            try
            {
                var email = model.UserId.Contains('@') ? model.UserId : $"{model.UserId}";

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = await _userManager.FindByNameAsync(model.UserId);
                }

                if (user == null)
                {
                    return new APIResponse<LoginResponseDto>
                    {
                        Message = "Invalid User ID or Password",
                        StatusCode = HttpStatusCode.Unauthorized,
                        Success = false
                    };
                }

                if (user.LockoutEndDate.HasValue && user.LockoutEndDate.Value > DateTime.Now)
                {
                    return new APIResponse<LoginResponseDto>
                    {
                        Message = $"Account is locked. Please try again after {user.LockoutEndDate.Value:yyyy-MM-dd HH:mm:ss}",
                        StatusCode = HttpStatusCode.Forbidden,
                        Success = false
                    };
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (!result.Succeeded)
                {
                    user.FailedLoginAttempts++;

                    if (user.FailedLoginAttempts >= 5)
                    {
                        user.LockoutEndDate = DateTime.Now.AddMinutes(30);
                        user.FailedLoginAttempts = 0;
                        await _userManager.UpdateAsync(user);

                        return new APIResponse<LoginResponseDto>
                        {
                            Message = "Too many failed attempts. Account locked for 30 minutes.",
                            StatusCode = HttpStatusCode.Forbidden,
                            Success = false
                        };
                    }

                    await _userManager.UpdateAsync(user);

                    return new APIResponse<LoginResponseDto>
                    {
                        Message = "Invalid User ID or Password",
                        StatusCode = HttpStatusCode.Unauthorized,
                        Success = false
                    };
                }

                user.FailedLoginAttempts = 0;
                user.LastLoginDate = DateTime.Now;
                user.LastLoginIpAddress = model.IpAddress;
                user.LockoutEndDate = null;

                await _userManager.UpdateAsync(user);

                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "User";

                if (role == "SchoolOwner")
                {
                    var owner = await _context.SchoolOwners.FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id);
                    if (owner == null || owner.SchoolSubscriptionId == null)
                    {
                        return new APIResponse<LoginResponseDto>
                        {
                            Message = "No active subscription found. Please renew your subscription to continue.",
                            StatusCode = HttpStatusCode.Forbidden,
                            Success = false
                        };
                    }
                }

                var claimsList = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName! ?? user.Email!),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.Role, role)
                };

                if (user.SchoolRegistrationId.HasValue)
                {
                    claimsList.Add(new Claim("TenantId", user.SchoolRegistrationId.Value.ToString()));
                }

                var claims = claimsList.ToArray();

                var authResponse = await _jWTAuthenticationManager.Authenticate(user.Id ?? user.Email!, claims);

                var loginHistory = new LoginHistory
                {
                    UserId = user.Id!,
                    LoginTime = DateTime.Now,
                    SessionExpireTime = authResponse.ExpireTime,
                    IsActive = true,
                    LoginMethod = "Credentials",
                    SessionId = authResponse.AccessToken.Substring(0, Math.Min(50, authResponse.AccessToken.Length)),
                    SessionDurationMinutes = _appSettings.ExpireTime,
                    IpAddress = model.IpAddress,
                    DeviceType = model.DeviceType,
                    Browser = model.Browser,
                    BrowserVersion = model.BrowserVersion,
                    OperatingSystem = model.OperatingSystem,
                    OperatingSystemVersion = model.OperatingSystemVersion,
                    DeviceModel = model.DeviceModel,
                    UserAgent = model.UserAgent,
                    LoginSuccessful = true
                };

                _context.LoginHistories.Add(loginHistory);
                await _context.SaveChangesAsync();

                return new APIResponse<LoginResponseDto>
                {
                    Message = "Login successful",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = new LoginResponseDto
                    {
                        UserId = user.Id!,
                        UserName = user.UserName ?? "",
                        Email = user.Email ?? "",
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Role = role,
                        AccessToken = authResponse.AccessToken,
                        RefreshToken = authResponse.RefreshToken,
                        ExpireTime = authResponse.ExpireTime,
                        IsDefaultPassword = user.IsDefaultPassword
                    }
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<LoginResponseDto>
                {
                    Message = $"Login failed: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false
                };
            }
        }

        public async Task<APIResponse<OTPResponseDto>> SendOTPAsync(LoginWithOTPModel model)
        {
            try
            {
                bool isEmail = model.EmailOrMobile.Contains('@');
                ApplicationUser? user = null;

                if (isEmail)
                {
                    user = await _userManager.FindByEmailAsync(model.EmailOrMobile);
                }
                else
                {
                    user = await FindAsync(expression: x => x.PhoneNumber == model.EmailOrMobile);
                }

                if (user == null)
                {
                    return new APIResponse<OTPResponseDto>
                    {
                        Message = "User not found with this email or mobile number",
                        StatusCode = HttpStatusCode.NotFound,
                        Success = false
                    };
                }

                var random = new Random();
                var otp = random.Next(100000, 999999).ToString();

                var expiry = DateTime.Now.AddMinutes(5);
                _otpStorage[model.EmailOrMobile] = (otp, expiry);

                Console.WriteLine($"OTP for {model.EmailOrMobile}: {otp}");

                return new APIResponse<OTPResponseDto>
                {
                    Message = $"OTP sent successfully to {model.EmailOrMobile}. OTP: {otp} (Dev Mode)",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = new OTPResponseDto
                    {
                        Message = "OTP sent successfully",
                        ExpiryTime = expiry
                    }
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<OTPResponseDto>
                {
                    Message = $"Failed to send OTP: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false
                };
            }
        }

        public async Task<APIResponse<LoginResponseDto>> VerifyOTPAsync(VerifyOTPModel model)
        {
            try
            {
                if (!_otpStorage.ContainsKey(model.EmailOrMobile))
                {
                    return new APIResponse<LoginResponseDto>
                    {
                        Message = "No OTP found. Please request a new OTP.",
                        StatusCode = HttpStatusCode.BadRequest,
                        Success = false
                    };
                }

                var (storedOtp, expiry) = _otpStorage[model.EmailOrMobile];

                if (DateTime.Now > expiry)
                {
                    _otpStorage.Remove(model.EmailOrMobile);
                    return new APIResponse<LoginResponseDto>
                    {
                        Message = "OTP has expired. Please request a new OTP.",
                        StatusCode = HttpStatusCode.BadRequest,
                        Success = false
                    };
                }

                if (storedOtp != model.OTP)
                {
                    return new APIResponse<LoginResponseDto>
                    {
                        Message = "Invalid OTP. Please try again.",
                        StatusCode = HttpStatusCode.Unauthorized,
                        Success = false
                    };
                }

                _otpStorage.Remove(model.EmailOrMobile);

                bool isEmail = model.EmailOrMobile.Contains('@');
                ApplicationUser? user = null;

                if (isEmail)
                {
                    user = await _userManager.FindByEmailAsync(model.EmailOrMobile);
                }
                else
                {
                    user = await FindAsync(expression: x => x.PhoneNumber == model.EmailOrMobile);
                }

                if (user == null)
                {
                    return new APIResponse<LoginResponseDto>
                    {
                        Message = "User not found",
                        StatusCode = HttpStatusCode.NotFound,
                        Success = false
                    };
                }

                user.LastLoginDate = DateTime.Now;
                user.LastLoginIpAddress = model.IpAddress;

                await _userManager.UpdateAsync(user);

                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "User";

                var claimsList = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName ?? user.Email!),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.Role, role)
                };

                if (user.SchoolRegistrationId.HasValue)
                {
                    claimsList.Add(new Claim("TenantId", user.SchoolRegistrationId.Value.ToString()));
                }

                var claims = claimsList.ToArray();

                var authResponse = await _jWTAuthenticationManager.Authenticate(user.Id ?? user.Email!, claims);

                var loginHistory = new LoginHistory
                {
                    UserId = user.Id!,
                    LoginTime = DateTime.Now,
                    SessionExpireTime = authResponse.ExpireTime,
                    IsActive = true,
                    LoginMethod = "OTP",
                    SessionId = authResponse.AccessToken.Substring(0, Math.Min(50, authResponse.AccessToken.Length)),
                    SessionDurationMinutes = _appSettings.ExpireTime,
                    IpAddress = model.IpAddress,
                    DeviceType = model.DeviceType,
                    Browser = model.Browser,
                    BrowserVersion = model.BrowserVersion,
                    OperatingSystem = model.OperatingSystem,
                    OperatingSystemVersion = model.OperatingSystemVersion,
                    DeviceModel = model.DeviceModel,
                    UserAgent = model.UserAgent,
                    LoginSuccessful = true
                };

                _context.LoginHistories.Add(loginHistory);
                await _context.SaveChangesAsync();

                return new APIResponse<LoginResponseDto>
                {
                    Message = "Login successful",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = new LoginResponseDto
                    {
                        UserId = user.Id!,
                        UserName = user.UserName ?? "",
                        Email = user.Email ?? "",
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Role = role,
                        AccessToken = authResponse.AccessToken,
                        RefreshToken = authResponse.RefreshToken,
                        ExpireTime = authResponse.ExpireTime,
                        IsDefaultPassword = user.IsDefaultPassword
                    }
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<LoginResponseDto>
                {
                    Message = $"OTP verification failed: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false
                };
            }
        }

        public async Task<APIResponse> LogoutAsync(string userId, LogoutModel model)
        {
            try
            {
                var activeSessions = _context.LoginHistories
                    .Where(lh => lh.UserId == userId && lh.IsActive)
                    .ToList();

                if (!activeSessions.Any())
                {
                    return new APIResponse
                    {
                        Message = "No active session found",
                        StatusCode = HttpStatusCode.NotFound,
                        Success = false
                    };
                }

                var logoutTime = DateTime.Now;
                var logoutReason = string.IsNullOrEmpty(model.LogoutReason) ? "Manual" : model.LogoutReason;

                if (!string.IsNullOrEmpty(model.SessionId))
                {
                    var session = activeSessions.FirstOrDefault(s => s.SessionId == model.SessionId);
                    if (session != null)
                    {
                        session.LogoutTime = logoutTime;
                        session.IsActive = false;
                        session.LogoutReason = logoutReason;
                        _context.LoginHistories.Update(session);
                    }
                }
                else
                {
                    foreach (var session in activeSessions)
                    {
                        session.LogoutTime = logoutTime;
                        session.IsActive = false;
                        session.LogoutReason = logoutReason;
                        _context.LoginHistories.Update(session);
                    }
                }

                await _context.SaveChangesAsync();

                return new APIResponse
                {
                    Message = "Logout successful",
                    StatusCode = HttpStatusCode.OK,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Message = $"Logout failed: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false
                };
            }
        }

        public async Task<APIResponse> ChangePasswordAsync(ChangePasswordModel model)
        {
            var user = await FindAsync(expression: x => Equals(x.UserName, model.UserName));
            if (user == null)
            {
                return new APIResponse
                {
                    Message = CommonResource.PleaseTryAgain,
                    StatusCode = HttpStatusCode.NotFound,

                };
            }
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);


            if (!result.Succeeded)
            {
                return new APIResponse
                {
                    Message = "Old Password is wrong. Please enter correct password",
                    StatusCode = HttpStatusCode.Unauthorized,
                };
            }
            return new APIResponse
            {
                Message = CommonResource.UpdateSuccess,
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<APIResponse<ForgotPasswordDto>> ForgotPasswordAsync(ForgotPasswordModel model)
        {
            var user = await FindAsync(expression: x => Equals(x.UserName, model.UserName));
            if (user == null)
            {
                return new APIResponse<ForgotPasswordDto>
                {
                    Message = CommonResource.PleaseTryAgain,
                    StatusCode = HttpStatusCode.NotFound,

                };
            }
            if (user.PhoneNumber != model.MobileNumber)
            {
                return new APIResponse<ForgotPasswordDto>
                {
                    Message = "Mobile number mismatch. Please enter your correct mobile number",
                    StatusCode = HttpStatusCode.NotFound,

                };

            }
            if (user.Email != model.Email)
            {
                return new APIResponse<ForgotPasswordDto>
                {
                    Message = "Email id mismatch. Please enter your correct email id",
                    StatusCode = HttpStatusCode.NotFound,

                };

            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return new APIResponse<ForgotPasswordDto>
            {
                Message = "Token generated successfuly. Forget password vailid for 5 minutes",
                Data = new ForgotPasswordDto
                {
                    ExpireTime = DateTime.Now.AddMinutes(5),
                    Token = token,
                },
                StatusCode = HttpStatusCode.Unauthorized,
            };
        }

        public async Task<APIResponse> ResetPasswordAsync(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.EmailId);
            if (user == null)
            {
                return new APIResponse
                {
                    Message = $"This user <b> {model.EmailId} </b>  does not exist.",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                return new APIResponse
                {
                    Message = result?.Errors.Select(e => e.Description).ToString()!,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            return new APIResponse
            {
                Message = "Password reset successfully.",
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };
        }

        public async Task<PagedResponse<LoginHistoryDto>> GetLoginHistoryAsync(LoginHistoryFilter filter)
        {
            filter.Validate();

            var query = _context.LoginHistories
                .Include(lh => lh.User)
                .AsQueryable();

            if (filter.IsActive.HasValue)
            {
                query = query.Where(lh => lh.IsActive == filter.IsActive.Value);
            }

            if (filter.FromDate.HasValue)
            {
                query = query.Where(lh => lh.LoginTime >= filter.FromDate.Value);
            }

            if (filter.ToDate.HasValue)
            {
                var toDate = filter.ToDate.Value.Date.AddDays(1); // Include the entire end date
                query = query.Where(lh => lh.LoginTime < toDate);
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchTerm = filter.Search.ToLower();
                query = query.Where(lh =>
                    (lh.User.UserName != null && lh.User.UserName.ToLower().Contains(searchTerm)) ||
                    (lh.User.Email != null && lh.User.Email.ToLower().Contains(searchTerm)) ||
                    (lh.User.FirstName != null && lh.User.FirstName.ToLower().Contains(searchTerm)) ||
                    (lh.User.LastName != null && lh.User.LastName.ToLower().Contains(searchTerm)) ||
                    (lh.IpAddress != null && lh.IpAddress.ToLower().Contains(searchTerm))
                );
            }

            if (!string.IsNullOrEmpty(filter.UserId))
            {
                query = query.Where(lh => lh.UserId == filter.UserId);
            }

            if (!string.IsNullOrEmpty(filter.LoginMethod))
            {
                query = query.Where(lh => lh.LoginMethod == filter.LoginMethod);
            }

            if (filter.LoginSuccessful.HasValue)
            {
                query = query.Where(lh => lh.LoginSuccessful == filter.LoginSuccessful.Value);
            }

            if (!string.IsNullOrEmpty(filter.IpAddress))
            {
                query = query.Where(lh => lh.IpAddress != null && lh.IpAddress.Contains(filter.IpAddress));
            }

            if (!string.IsNullOrEmpty(filter.DeviceType))
            {
                query = query.Where(lh => lh.DeviceType != null && lh.DeviceType.Contains(filter.DeviceType));
            }

            if (!string.IsNullOrWhiteSpace(filter.OrderBy))
            {
                var sortDirection = filter.SortDirection?.ToLower() == "desc";
                query = filter.OrderBy.ToLower() switch
                {
                    "logintime" or "login_time" => sortDirection
                        ? query.OrderByDescending(lh => lh.LoginTime)
                        : query.OrderBy(lh => lh.LoginTime),
                    "logouttime" or "logout_time" => sortDirection
                        ? query.OrderByDescending(lh => lh.LogoutTime)
                        : query.OrderBy(lh => lh.LogoutTime),
                    "username" or "user_name" => sortDirection
                        ? query.OrderByDescending(lh => lh.User.UserName)
                        : query.OrderBy(lh => lh.User.UserName),
                    "email" => sortDirection
                        ? query.OrderByDescending(lh => lh.User.Email)
                        : query.OrderBy(lh => lh.User.Email),
                    _ => query.OrderByDescending(lh => lh.LoginTime) // Default sort
                };
            }
            else
            {
                query = query.OrderByDescending(lh => lh.LoginTime);
            }

            var totalCount = await query.CountAsync();

            var loginHistories = await query
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(lh => new LoginHistoryDto
                {
                    Id = lh.Id,
                    UserId = lh.UserId,
                    UserName = lh.User.UserName ?? "",
                    Email = lh.User.Email ?? "",
                    FullName = $"{lh.User.FirstName} {lh.User.LastName}".Trim(),
                    LoginTime = lh.LoginTime,
                    LogoutTime = lh.LogoutTime,
                    SessionExpireTime = lh.SessionExpireTime,
                    IsActive = lh.IsActive,
                    LoginMethod = lh.LoginMethod,
                    SessionId = lh.SessionId,
                    SessionDurationMinutes = lh.SessionDurationMinutes,
                    LogoutReason = lh.LogoutReason,
                    IpAddress = lh.IpAddress,
                    DeviceType = lh.DeviceType,
                    Browser = lh.Browser,
                    BrowserVersion = lh.BrowserVersion,
                    OperatingSystem = lh.OperatingSystem,
                    OperatingSystemVersion = lh.OperatingSystemVersion,
                    DeviceModel = lh.DeviceModel,
                    Country = lh.Country,
                    City = lh.City,
                    Region = lh.Region,
                    LoginSuccessful = lh.LoginSuccessful,
                    FailureReason = lh.FailureReason,
                    IsSuspicious = lh.IsSuspicious,
                    SecurityNotes = lh.SecurityNotes
                })
                .ToListAsync();

            var apiResponse = new PagedResponse<LoginHistoryDto>
            {
                Success = true,
                Message = "Login history retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Data = loginHistories,
                CurrentPage = filter.PageIndex,
                PageSize = filter.PageSize,
                TotalRecords = totalCount
            };

            return apiResponse;

        }

        public async Task<APIResponse<IEnumerable<UserDto>>> GetUsersByRoleAsync(string roleName)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    return new APIResponse<IEnumerable<UserDto>>
                    {
                        Success = false,
                        Message = $"Role '{roleName}' not found",
                        StatusCode = HttpStatusCode.NotFound,
                        Data = new List<UserDto>()
                    };
                }

                var userIds = _context.UserRoles
                    .Where(ur => ur.RoleId == role.Id)
                    .Select(ur => ur.UserId)
                    .ToList();

                var users = await _context.Users
                    .Where(u => userIds.Contains(u.Id) && u.IsActive)
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        UserName = u.UserName ?? "",
                        Email = u.Email ?? "",
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        PhoneNumber = u.PhoneNumber,
                        IsActive = u.IsActive,
                        Role = roleName,
                        RoleId = role.Id
                    })
                    .ToListAsync();

                return new APIResponse<IEnumerable<UserDto>>
                {
                    Success = true,
                    Message = $"Users with role '{roleName}' retrieved successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = users
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<IEnumerable<UserDto>>
                {
                    Success = false,
                    Message = $"Failed to get users: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = new List<UserDto>()
                };
            }
        }


    }
}
