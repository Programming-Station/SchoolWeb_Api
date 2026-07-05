using School.Domain.Auth;
using School.Utilities.Constants;
using School.Utilities.Enums;

namespace School.Infrastructure.Seeds
{
    public static class DefaultUser
    {
        public static List<ApplicationUser> IdentityBasicUserList()
        {
            var passwordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==";

            return new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = Constants.SuperAdminUser,
                    UserName = "superadmin",
                    Email = "superadmin@gmail.com",
                    FirstName = "Parvez",
                    LastName = "Ansari",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PasswordHash = passwordHash,
                    NormalizedEmail = "SUPERADMIN@GMAIL.COM",
                    NormalizedUserName = "SUPERADMIN",
                    StatusId = (int)DefaultStatus.Verified,
                    IsDefaultPassword = false,
                    IsActive = true,
                },
                new ApplicationUser
                {
                    Id = Constants.AdminUser,
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PasswordHash = passwordHash,
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    NormalizedUserName = "ADMIN",
                    StatusId = (int)DefaultStatus.Verified,
                    IsDefaultPassword = false,
                    IsActive = true,
                },

                new ApplicationUser
                {
                    Id = Constants.StudentUser,
                    UserName = "student",
                    Email = "student@gmail.com",
                    FirstName = "Student",
                    LastName = "User",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PasswordHash = passwordHash,
                    NormalizedEmail = "STUDENT@GMAIL.COM",
                    NormalizedUserName = "STUDENT",
                    StatusId = (int)DefaultStatus.Verified,
                    IsDefaultPassword = false,
                    IsActive = true,
                },
                new ApplicationUser
                {
                    Id = Constants.OwnerUser,
                    UserName = "owner",
                    Email = "owner@gmail.com",
                    FirstName = "School",
                    LastName = "Owner",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PasswordHash = passwordHash,
                    NormalizedEmail = "OWNER@GMAIL.COM",
                    NormalizedUserName = "OWNER",
                    StatusId = (int)DefaultStatus.Verified,
                    IsDefaultPassword = false,
                    IsActive = true,
                }
            };
        }
    }
}
