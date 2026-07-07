using School.Utilities.Constants;
using Microsoft.AspNetCore.Identity;

namespace School.Infrastructure.Seeds
{
    public static class MappingUserRole
    {
        public static List<IdentityUserRole<string>> IdentityUserRoleList()
        {
            return new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>
                {
                    RoleId = Constants.SuperAdmin,
                    UserId = Constants.SuperAdminUser
                },
                new IdentityUserRole<string>
                {
                    RoleId = Constants.Admin,
                    UserId = Constants.AdminUser
                },

                new IdentityUserRole<string>
                {
                    RoleId = Constants.Student,
                    UserId = Constants.StudentUser
                },
            };
        }
    }
}
