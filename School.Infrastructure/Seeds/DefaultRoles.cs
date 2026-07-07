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
                    Id = Constants.Student,
                    Name = Roles.Student.ToString(),
                    NormalizedName = Roles.Student.ToString().ToUpper()
                },
                 new IdentityRole
                {
                    Id = Constants.Owner,
                    Name = Roles.Owner.ToString(),
                    NormalizedName = Roles.Owner.ToString().ToUpper()
                },
                new IdentityRole
                {
                    Id = Constants.Employee,
                    Name =Roles.Employee.ToString(),
                    NormalizedName = Roles.Employee.ToString().ToUpper()
                },
            };
        }
    }
}
