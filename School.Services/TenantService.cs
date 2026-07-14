using Microsoft.AspNetCore.Http;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using System.Linq;

namespace School.Services
{
    public class TenantService : ITenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int? _overriddenTenantId;

        public TenantService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetTenantId(int tenantId)
        {
            _overriddenTenantId = tenantId;
        }

        public int? GetTenantId()
        {
            if (_overriddenTenantId.HasValue)
            {
                return _overriddenTenantId.Value;
            }

            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null) return null;

            var tenantClaim = user.Claims.FirstOrDefault(c => c.Type == "TenantId" || c.Type == "SchoolRegistrationId");
            
            if (tenantClaim != null && int.TryParse(tenantClaim.Value, out int tenantId))
            {
                return tenantId;
            }

            // If the user belongs to the SuperAdmin role, return null (meaning they see all tenants)
            if (user.IsInRole("SuperAdmin") || user.IsInRole("SUPERADMIN"))
            {
                return null;
            }

            // Fallback: Standard user without a TenantId claim is restricted
            return -999;
        }
    }
}


