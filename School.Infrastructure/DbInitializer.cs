using School.Domain;
using School.Domain.Location;
using School.Domain.School;
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
            var existingRoleNames = context.Roles
                .Select(r => r.NormalizedName != null ? r.NormalizedName.ToUpper() : string.Empty)
                .ToList();
            var defaultRoles = DefaultRoles.IdentityRoleList();
            var newRoles = defaultRoles
                .Where(r => r.NormalizedName != null && !existingRoleNames.Contains(r.NormalizedName.ToUpper()))
                .ToList();
            if (newRoles.Any())
            {
                context.Roles.AddRange(newRoles);
                context.SaveChanges(); // Save to get role IDs
            }

            // Seed Statuses
            var existingStatusNames = context.Statuses.Select(s => s.Name).ToHashSet();
            var defaultStatuses = DefaultStatusList.StatusList();
            var newStatuses = defaultStatuses.Where(s => !existingStatusNames.Contains(s.Name)).ToList();
            if (newStatuses.Any())
            {
                context.Statuses.AddRange(newStatuses);
                context.SaveChanges();
            }

            // Seed SchoolMediums
            var existingMediumNames = context.SchoolMediums.Select(m => m.Name).ToHashSet();
            var defaultMediums = DefaultMasterData.SchoolMediumList();
            var newMediums = defaultMediums.Where(m => !existingMediumNames.Contains(m.Name)).ToList();
            if (newMediums.Any())
            {
                context.SchoolMediums.AddRange(newMediums);
                context.SaveChanges();
            }

            // Seed SchoolTypes
            var existingTypeNames = context.SchoolTypes.Select(t => t.Name).ToHashSet();
            var defaultTypes = DefaultMasterData.SchoolTypeList();
            var newTypes = defaultTypes.Where(t => !existingTypeNames.Contains(t.Name)).ToList();
            if (newTypes.Any())
            {
                context.SchoolTypes.AddRange(newTypes);
                context.SaveChanges();
            }

            // Seed AffiliationBoards
            var existingBoardNames = context.AffiliationBoards.Select(b => b.Name).ToHashSet();
            var defaultBoards = DefaultMasterData.AffiliationBoardList();
            var newBoards = defaultBoards.Where(b => !existingBoardNames.Contains(b.Name)).ToList();
            if (newBoards.Any())
            {
                context.AffiliationBoards.AddRange(newBoards);
                context.SaveChanges();
            }

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

            var existingUserEmails = context.Users
                .Select(u => u.NormalizedEmail != null ? u.NormalizedEmail.ToUpper() : string.Empty)
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

            var allUsers = context.Users.ToList();
            var allRoles = context.Roles.ToList();
            var existingUserRoles = context.UserRoles
                .Select(ur => new { ur.UserId, ur.RoleId })
                .ToList();

            var userRoleMappings = new List<IdentityUserRole<string>>();

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
            var ownerUser = allUsers.FirstOrDefault(u =>
               u.NormalizedUserName != null && u.NormalizedUserName.ToUpper() == "OWNER");
            var ownerRole = allRoles.FirstOrDefault(r =>
                r.NormalizedName != null && r.NormalizedName.ToUpper() == "OWNER");
            if (ownerUser != null && ownerRole != null)
            {
                if (!existingUserRoles.Any(ur => ur.UserId == ownerUser.Id && ur.RoleId == ownerRole.Id))
                {
                    userRoleMappings.Add(new IdentityUserRole<string>
                    {
                        UserId = ownerUser.Id,
                        RoleId = ownerRole.Id
                    });
                }
            }

            if (userRoleMappings.Any())
            {
                context.UserRoles.AddRange(userRoleMappings);
                context.SaveChanges();
            }

            DefaultLocationData.SeedAsync(context).Wait();

            if (!context.States.Any())
            {
                var countryId = context.Countries.First().Id;
                var state = new State { Name = "Maharashtra", StateCode = "MH", CountryId = countryId, IsActive = true };
                context.States.Add(state);
                context.SaveChanges();
            }

            if (!context.Cities.Any())
            {
                var stateId = context.States.First().Id;
                var city = new City { Name = "Mumbai", CityCode = "MUM", StateId = stateId, IsActive = true };
                context.Cities.Add(city);
                context.SaveChanges();
            }

            DefaultSchoolData.SeedAsync(context).Wait();
            DefaultHrData.SeedAsync(context).Wait();
            DefaultCourseData.SeedAsync(context).Wait();
            DefaultClassData.SeedAsync(context).Wait();
            DefaultStudentData.SeedAsync(context).Wait();
        }
    }
}




