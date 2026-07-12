using School.Domain;
using School.Domain.Location;
using School.Domain.School;
using School.Domain.Auth;
using School.Infrastructure.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.Domain.Email;
using School.Utilities.Security;

namespace School.Infrastructure
{
    public class DbInitializer
    {
        public static void Seed(SchoolDbContext context, IEncryptionService encryptionService)
        {
            // ─── 1. Core Lookups & Configuration ─────────────────────────────────────
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
                context.SaveChanges();
            }

            var existingStatusNames = context.Statuses.Select(s => s.Name).ToHashSet();
            var defaultStatuses = DefaultStatusList.StatusList();
            var newStatuses = defaultStatuses.Where(s => !existingStatusNames.Contains(s.Name)).ToList();
            if (newStatuses.Any())
            {
                context.Statuses.AddRange(newStatuses);
                context.SaveChanges();
            }

            var existingMediumNames = context.SchoolMediums.Select(m => m.Name).ToHashSet();
            var defaultMediums = DefaultMasterData.SchoolMediumList();
            var newMediums = defaultMediums.Where(m => !existingMediumNames.Contains(m.Name)).ToList();
            if (newMediums.Any())
            {
                context.SchoolMediums.AddRange(newMediums);
                context.SaveChanges();
            }

            var existingTypeNames = context.SchoolTypes.Select(t => t.Name).ToHashSet();
            var defaultTypes = DefaultMasterData.SchoolTypeList();
            var newTypes = defaultTypes.Where(t => !existingTypeNames.Contains(t.Name)).ToList();
            if (newTypes.Any())
            {
                context.SchoolTypes.AddRange(newTypes);
                context.SaveChanges();
            }

            var existingBoardNames = context.AffiliationBoards.Select(b => b.Name).ToHashSet();
            var defaultBoards = DefaultMasterData.AffiliationBoardList();
            var newBoards = defaultBoards.Where(b => !existingBoardNames.Contains(b.Name)).ToList();
            if (newBoards.Any())
            {
                context.AffiliationBoards.AddRange(newBoards);
                context.SaveChanges();
            }

            // ─── 2. Location & School Seeding (Run early to resolve foreign keys) ──────
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

            // Resolve Default School Registration ID for subsequent seeding steps
            var defaultSchool = context.SchoolRegistrations.FirstOrDefault(s => s.SchoolCode == "DPSVAR001") 
                                ?? context.SchoolRegistrations.FirstOrDefault(s => s.SchoolCode == "DEF001") 
                                ?? context.SchoolRegistrations.FirstOrDefault();
            int defaultSchoolId = defaultSchool?.Id ?? 1;

            // ─── 3. Category Modules Seeding (Assign correct SchoolRegistrationId) ───
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
                foreach (var cat in newCategories)
                {
                    cat.SchoolRegistrationId = defaultSchoolId;
                }
                context.CategoryModules.AddRange(newCategories);
                context.SaveChanges();
            }

            // ─── 4. Users & Roles Seeding ────────────────────────────────────────────
            var existingUserEmails = context.Users
                .Select(u => u.NormalizedEmail != null ? u.NormalizedEmail.ToUpper() : string.Empty)
                .ToHashSet();
            var existingUserNames = context.Users
                .Select(u => u.NormalizedUserName != null ? u.NormalizedUserName.ToUpper() : string.Empty)
                .ToHashSet();
            var defaultUsers = DefaultUser.IdentityBasicUserList();
            var newUsers = defaultUsers
                .Where(u => u.NormalizedEmail != null && !existingUserEmails.Contains(u.NormalizedEmail.ToUpper())
                         && u.NormalizedUserName != null && !existingUserNames.Contains(u.NormalizedUserName.ToUpper()))
                .ToList();
            if (newUsers.Any())
            {
                context.Users.AddRange(newUsers);
                context.SaveChanges();
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

            var employeeUser = allUsers.FirstOrDefault(u =>
               u.NormalizedUserName != null && u.NormalizedUserName.ToUpper() == "EMPLOYEE");
            var employeeRole = allRoles.FirstOrDefault(r =>
                r.NormalizedName != null && r.NormalizedName.ToUpper() == "EMPLOYEE");
            if (employeeUser != null && employeeRole != null)
            {
                if (!existingUserRoles.Any(ur => ur.UserId == employeeUser.Id && ur.RoleId == employeeRole.Id))
                {
                    userRoleMappings.Add(new IdentityUserRole<string>
                    {
                        UserId = employeeUser.Id,
                        RoleId = employeeRole.Id
                    });
                }
            }

            if (userRoleMappings.Any())
            {
                context.UserRoles.AddRange(userRoleMappings);
                context.SaveChanges();
            }

            // Correct SchoolRegistrationId for existing users (except superadmin)
            var usersToCorrect = context.Users
                .Where(u => u.NormalizedUserName != "SUPERADMIN" && (u.SchoolRegistrationId == null || u.SchoolRegistrationId == 0))
                .ToList();

            if (usersToCorrect.Any())
            {
                var schoolOwnersMap = context.SchoolOwners
                    .ToDictionary(so => so.ApplicationUserId, so => so.SchoolRegistrationId);

                foreach (var user in usersToCorrect)
                {
                    if (schoolOwnersMap.TryGetValue(user.Id, out var mappedSchoolId))
                    {
                        user.SchoolRegistrationId = mappedSchoolId;
                    }
                    else
                    {
                        user.SchoolRegistrationId = defaultSchoolId;
                    }
                }
                context.SaveChanges();
            }

            // ─── 5. Dependent Domain Seeding ─────────────────────────────────────────
            DefaultAccessControlData.SeedAsync(context).Wait();
            DefaultHrData.SeedAsync(context).Wait();
            DefaultAcademicYearData.SeedAsync(context).Wait();
            DefaultAdmissionData.SeedAsync(context).Wait();
            DefaultCourseData.SeedAsync(context).Wait();
            DefaultClassData.SeedAsync(context).Wait();
            DefaultStudentData.SeedAsync(context).Wait();

            // Seed all Email data (SMTP settings, templates, branding) via consolidated seed
            DefaultEmailData.SeedAsync(context, encryptionService).Wait();

            DefaultAcademicExtendedData.SeedAsync(context).Wait();
            DefaultAdditionalData.SeedAsync(context).Wait();
            DefaultHostelData.SeedAsync(context).Wait();
            TransportSeedData.SeedAsync(context).Wait();
            DefaultPayrollData.SeedAsync(context).Wait();
            DefaultFinanceData.SeedAsync(context).Wait();
            DefaultInventoryData.SeedAsync(context).Wait();
        }
    }
}
