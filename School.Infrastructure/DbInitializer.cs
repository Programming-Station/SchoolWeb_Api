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

            if (!context.Statuses.Any())
            {
                List<Status> statuses = DefaultStatusList.StatusList();
                context.Statuses.AddRange(statuses);
                context.SaveChanges();
            }

            if (!context.SchoolMediums.Any())
            {
                var mediums = DefaultMasterData.SchoolMediumList();
                context.SchoolMediums.AddRange(mediums);
                context.SaveChanges();
            }

            if (!context.SchoolTypes.Any())
            {
                var types = DefaultMasterData.SchoolTypeList();
                context.SchoolTypes.AddRange(types);
                context.SaveChanges();
            }

            if (!context.AffiliationBoards.Any())
            {
                var boards = DefaultMasterData.AffiliationBoardList();
                context.AffiliationBoards.AddRange(boards);
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

            if (!context.SchoolRegistrations.Any())
            {
                var defaultState = context.States.First();
                var defaultCity = context.Cities.First();
                var defaultMedium = context.SchoolMediums.FirstOrDefault()?.Id ?? 1;
                var defaultType = context.SchoolTypes.FirstOrDefault()?.Id ?? 1;
                var defaultBoard = context.AffiliationBoards.FirstOrDefault()?.Id ?? 1;

                var school = new SchoolRegistration
                {
                    SchoolName = "Default School",
                    SchoolCode = "DEF001",
                    Address = "123 Default Street",
                    Email = "info@defaultschool.com",
                    PhoneNumber = "1234567890",
                    CityId = defaultCity.Id,
                    StateId = defaultState.Id,
                    IsActive = true,
                    RegistrationDate = DateTime.UtcNow,
                    ContactPersonName = "School Owner",
                    ApprovalStatus = "Approved",
                    SubDomain = "default",
                    AffiliationBoardId = defaultBoard,
                    SchoolTypeId = defaultType
                };
                context.SchoolRegistrations.Add(school);
                context.SaveChanges();

                var schoolOwnerUser = context.Users.FirstOrDefault(u => u.NormalizedUserName == "OWNER");
                if (schoolOwnerUser != null)
                {
                    var owner = new SchoolOwner
                    {
                        SchoolRegistrationId = school.Id,
                        ApplicationUserId = schoolOwnerUser.Id,
                        StatusId = 1,
                        EmailVerified = true,
                        MobileVerified = true,
                        IsLocked = false
                    };
                    context.SchoolOwners.Add(owner);

                    var subscription = new SchoolSubscription
                    {
                        SchoolRegistrationId = school.Id,
                        SubscriptionPlanId = 1,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        AmountPaid = 0,
                        PaymentStatus = "Free",
                        IsActive = true
                    };
                    context.SchoolSubscriptions.Add(subscription);
                    
                    var profileSetting = new SchoolProfileSetting
                    {
                        SchoolRegistrationId = school.Id,
                        Tagline = "A Great Place to Learn",
                        PrimaryMediumId = defaultMedium
                    };
                    context.SchoolProfileSettings.Add(profileSetting);

                    context.SaveChanges();
                }
            }
        }
    }
}




