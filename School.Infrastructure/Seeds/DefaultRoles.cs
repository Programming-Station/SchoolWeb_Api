using System.Reflection;
using Microsoft.AspNetCore.Identity;
using School.Utilities.Constants;
using School.Utilities.Enums;

namespace School.Infrastructure.Seeds
{
    public static class DefaultRoles
    {
        public static List<IdentityRole> IdentityRoleList()
        {
            var roles = new List<IdentityRole>();
            foreach (Roles roleEnum in Enum.GetValues(typeof(Roles)))
            {
                string roleName = roleEnum.ToString();
                // Retrieve the GUID constant from Constants class matching the role name
                var field = typeof(Constants).GetField(roleName, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
                string roleId = field != null ? field.GetValue(null) as string : Guid.NewGuid().ToString();
                roles.Add(new IdentityRole
                {
                    Id = roleId,
                    Name = roleName,
                    NormalizedName = roleName.ToUpperInvariant()
                });
            }
            return roles;
        }
    }
}
