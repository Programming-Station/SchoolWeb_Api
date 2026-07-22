using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using School.Infrastructure;
using School.Infrastructure.Interfaces;

namespace School.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly SchoolDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionService(SchoolDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> HasModulePermissionAsync(string moduleName)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated) return false;

            // Super admin or specific roles might bypass
            var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (roleClaim == "SuperAdmin") return true;

            var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return false;

            var hasPermission = await _context.ModulePermissions
                .Include(mp => mp.Module)
                .AnyAsync(mp => mp.IsActive &&
                                mp.Module.Name.ToLower() == moduleName.ToLower() &&
                                (mp.UserId == userId || (roleClaim != null && mp.Role.Name == roleClaim)));

            return hasPermission;
        }
    }
}
