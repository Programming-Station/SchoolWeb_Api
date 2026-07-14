using School.Utilities.Constants;
using School_API.Common.Interface;
using System.Security.Claims;

namespace School_API.Common
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;
            SessionId = user?.FindFirstValue(ClaimConstants.SessionId) ?? string.Empty;
            UserId = user?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            UserName = user?.FindFirstValue(ClaimConstants.UserName) ?? string.Empty;
            RoleId = user?.FindFirstValue(ClaimConstants.RoleId) ?? string.Empty;
            RoleName = user?.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            Name = user?.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
            Latitude = user?.FindFirstValue(ClaimConstants.Latitude) ?? string.Empty;
            Longitude = user?.FindFirstValue(ClaimConstants.Longitude) ?? string.Empty;
            IsAuthenticated = !string.IsNullOrEmpty(UserId);

            var tenantClaim = user?.FindFirstValue(ClaimConstants.TenantId) ?? user?.FindFirstValue("SchoolRegistrationId");
            if (int.TryParse(tenantClaim, out int tenantIdValue))
            {
                TenantId = tenantIdValue;
            }
        }

        public string SessionId { get; }
        public string UserId { get; }
        public string UserName { get; }
        public string Name { get; }
        public string RoleId { get; }
        public string RoleName { get; }
        public bool IsAuthenticated { get; }
        public string Latitude { get; }
        public string Longitude { get; }
        public int? TenantId { get; }
    }
}

