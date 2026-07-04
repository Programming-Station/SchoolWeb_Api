using School.Domain;
using School.Domain.Auth;
using School.Infrastructure.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace School.Infrastructure
{
    public class DbInitializer
    {
        public static void Seed(SchoolDbContext context)
        {
            // Seed Roles - Check by NormalizedName to avoid duplicates
            var existingRoleNames = context.Roles
                .Select(r => r.NormalizedName.ToUpper())
                .ToList();
            var defaultRoles = DefaultRoles.IdentityRoleList();
            var newRoles = defaultRoles
                .Where(r => !existingRoleNames.Contains(r.NormalizedName.ToUpper()))
                .ToList();
            if (newRoles.Any())
            {
                context.Roles.AddRange(newRoles);
                context.SaveChanges(); // Save to get role IDs
            }

            // Seed Statuses
            if (!context.Statuses.Any())
            {
                List<Status> statuses = DefaultStatusList.StatusList();
                context.Statuses.AddRange(statuses);
                context.SaveChanges();
            }

            // Seed CategoryModules - Check by Name to avoid duplicates
            var existingCategoryNames = context.CategoryModules
                .Where(c => !c.IsDeleted)
                .Select(c => c.Name)
                .ToList();
            var defaultCategories = DefaultModuleCategory.CategoryModuleList();
            var newCategories = defaultCategories
                .Where(c => !existingCategoryNames.Contains(c.Name))
                .ToList();
            if (newCategories.Any())
            {
                context.CategoryModules.AddRange(newCategories);
                context.SaveChanges();
            }

            // Seed Users - Check by NormalizedEmail to avoid duplicates
            var existingUserEmails = context.Users
                .Select(u => u.NormalizedEmail.ToUpper())
                .ToList();
            var defaultUsers = DefaultUser.IdentityBasicUserList();
            var newUsers = defaultUsers
                .Where(u => u.NormalizedEmail != null && !existingUserEmails.Contains(u.NormalizedEmail.ToUpper()))
                .ToList();
            if (newUsers.Any())
            {
                context.Users.AddRange(newUsers);
                context.SaveChanges(); // Save to get user IDs
            }

            // Seed User Roles - Map using actual database IDs
            var allUsers = context.Users.ToList();
            var allRoles = context.Roles.ToList();
            var existingUserRoles = context.UserRoles
                .Select(ur => new { ur.UserId, ur.RoleId })
                .ToList();

            var userRoleMappings = new List<IdentityUserRole<string>>();

            // SuperAdmin User -> SuperAdmin Role
            var superAdminUser = allUsers.FirstOrDefault(u => 
                u.NormalizedUserName != null && u.NormalizedUserName.ToUpper() == "SUPERADMIN");
            var superAdminRole = allRoles.FirstOrDefault(r => 
                r.NormalizedName != null && r.NormalizedName.ToUpper() == "SUPERADMIN");
            if (superAdminUser != null && superAdminRole != null)
            {
                if (!existingUserRoles.Any(ur => ur.UserId == superAdminUser.Id && ur.RoleId == superAdminRole.Id))
                {
                    userRoleMappings.Add(new IdentityUserRole<string>
                    {
                        UserId = superAdminUser.Id,
                        RoleId = superAdminRole.Id
                    });
                }
            }

            // Admin User -> Admin Role
            var adminUser = allUsers.FirstOrDefault(u => 
                u.NormalizedUserName != null && u.NormalizedUserName.ToUpper() == "ADMIN");
            var adminRole = allRoles.FirstOrDefault(r => 
                r.NormalizedName != null && r.NormalizedName.ToUpper() == "ADMIN");
            if (adminUser != null && adminRole != null)
            {
                if (!existingUserRoles.Any(ur => ur.UserId == adminUser.Id && ur.RoleId == adminRole.Id))
                {
                    userRoleMappings.Add(new IdentityUserRole<string>
                    {
                        UserId = adminUser.Id,
                        RoleId = adminRole.Id
                    });
                }
            }

            // Teacher User -> Teacher Role
            var teacherUser = allUsers.FirstOrDefault(u => 
                u.NormalizedUserName != null && u.NormalizedUserName.ToUpper() == "TEACHER");
            var teacherRole = allRoles.FirstOrDefault(r => 
                r.NormalizedName != null && r.NormalizedName.ToUpper() == "TEACHER");
            if (teacherUser != null && teacherRole != null)
            {
                if (!existingUserRoles.Any(ur => ur.UserId == teacherUser.Id && ur.RoleId == teacherRole.Id))
                {
                    userRoleMappings.Add(new IdentityUserRole<string>
                    {
                        UserId = teacherUser.Id,
                        RoleId = teacherRole.Id
                    });
                }
            }

            // Student User -> Student Role
            var studentUser = allUsers.FirstOrDefault(u => 
                u.NormalizedUserName != null && u.NormalizedUserName.ToUpper() == "STUDENT");
            var studentRole = allRoles.FirstOrDefault(r => 
                r.NormalizedName != null && r.NormalizedName.ToUpper() == "STUDENT");
            if (studentUser != null && studentRole != null)
            {
                if (!existingUserRoles.Any(ur => ur.UserId == studentUser.Id && ur.RoleId == studentRole.Id))
                {
                    userRoleMappings.Add(new IdentityUserRole<string>
                    {
                        UserId = studentUser.Id,
                        RoleId = studentRole.Id
                    });
                }
            }
            // Owner Users
            var onwerUser = allUsers.FirstOrDefault(u =>
               u.NormalizedUserName != null && u.NormalizedUserName.ToUpper() == "ONWER");
            var ownerRole = allRoles.FirstOrDefault(r =>
                r.NormalizedName != null && r.NormalizedName.ToUpper() == "ONWER");
            if (onwerUser != null && ownerRole != null)
            {
                if (!existingUserRoles.Any(ur => ur.UserId == onwerUser.Id && ur.RoleId == ownerRole.Id))
                {
                    userRoleMappings.Add(new IdentityUserRole<string>
                    {
                        UserId = onwerUser.Id,
                        RoleId = ownerRole.Id
                    });
                }
            }

            if (userRoleMappings.Any())
            {
                context.UserRoles.AddRange(userRoleMappings);
                context.SaveChanges();
            }
        }
    }
}
