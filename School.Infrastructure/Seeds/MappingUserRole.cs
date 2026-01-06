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
                // Super Admin - SuperAdmin Role (Single Role)
                new IdentityUserRole<string>
                {
                    RoleId = Constants.SuperAdmin,
                    UserId = Constants.SuperAdminUser
                },
                // Admin - Admin Role (Single Role)
                new IdentityUserRole<string>
                {
                    RoleId = Constants.Admin,
                    UserId = Constants.AdminUser
                },
                // Teacher - Teacher Role (Single Role)
                new IdentityUserRole<string>
                {
                    RoleId = Constants.Teacher,
                    UserId = Constants.TeacherUser
                },
                // Student - Student Role (Single Role)
                new IdentityUserRole<string>
                {
                    RoleId = Constants.Student,
                    UserId = Constants.StudentUser
                },
            };
        }
    }
}
