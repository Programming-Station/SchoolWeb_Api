using Microsoft.AspNetCore.Identity;
using School.Utilities.Constants;

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
                new IdentityUserRole<string>
                {
                    RoleId = Constants.Employee,
                    UserId = Constants.EmployeeUser
                },
            };
        }
    }
}
