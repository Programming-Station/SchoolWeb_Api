using Microsoft.AspNetCore.Http;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using System.Linq;

namespace School.Services
{
    public class TenantService : ITenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetTenantId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null) return null;

            // In a real application, the SchoolRegistrationId (TenantId) would be stored as a Claim in the JWT
            var tenantClaim = user.Claims.FirstOrDefault(c => c.Type == "TenantId" || c.Type == "SchoolRegistrationId");
            
            if (tenantClaim != null && int.TryParse(tenantClaim.Value, out int tenantId))
            {
                return tenantId;
            }

            return null;
        }
    }
}


