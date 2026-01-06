using School.Utilities.Constants;
using School.Utilities.Enums;
using Microsoft.AspNetCore.Identity;
namespace School.Infrastructure.Seeds
{

    public static class DefaultRoles
    {
        public static List<IdentityRole> IdentityRoleList()
        {
            return new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Id = Constants.SuperAdmin,
                    Name = Roles.SuperAdmin.ToString(),
                    NormalizedName = Roles.SuperAdmin.ToString().ToUpper()
                },
                new IdentityRole
                {
                    Id = Constants.Admin,
                    Name = Roles.Admin.ToString(),
                    NormalizedName = Roles.Admin.ToString().ToUpper()
                },
                new IdentityRole
                {
                    Id = Constants.Teacher,
                    Name = Roles.Teacher.ToString(),
                    NormalizedName = Roles.Teacher.ToString().ToUpper()
                },
                new IdentityRole
                {
                    Id = Constants.Student,
                    Name = Roles.Student.ToString(),
                    NormalizedName = Roles.Student.ToString().ToUpper()
                },
            };
        }
    }
}
