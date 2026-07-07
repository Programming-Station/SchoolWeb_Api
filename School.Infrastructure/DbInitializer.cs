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

            // Retrieve default school registration to set SchoolRegistrationId
            var defaultSchool = context.SchoolRegistrations.FirstOrDefault(s => s.SchoolCode == "DEF001");
            int defaultSchoolId = defaultSchool?.Id ?? 1;

            if (!context.EmailServerSettings.Any())
            {
                var settings = DefaultEmailServerSetting.GetAllEmailServerSetting();
                foreach (var setting in settings)
                {
                    setting.SchoolRegistrationId = defaultSchoolId;
                    // Encrypt password before seeding
                    setting.Password = encryptionService.Encrypt(setting.Password);
                }
                context.EmailServerSettings.AddRange(settings);
                context.SaveChanges();
            }

            // Incremental seeding: insert any template not yet present (keyed by TemplateName)
            var allTemplates = DefaultEmailTemplate.GetAllEmailTemplate();
            var existingNames = context.EmailTemplates
                .Select(t => t.TemplateName)
                .ToHashSet();
            var newTemplates = allTemplates
                .Where(t => !existingNames.Contains(t.TemplateName))
                .ToList();
            if (newTemplates.Count > 0)
            {
                foreach (var template in newTemplates)
                {
                    template.SchoolRegistrationId = defaultSchoolId;
                }
                context.EmailTemplates.AddRange(newTemplates);
                context.SaveChanges();
            }

            // Seed default EmailBranding details if missing
            if (!context.EmailBrandings.Any())
            {
                var branding = new EmailBranding
                {
                    SchoolRegistrationId = defaultSchoolId,
                    ThemeColor = "#1e3a8a", // Navy Blue
                    HeaderHtml = @"<div style=""background-color: #1e3a8a; padding: 20px; text-align: center; border-radius: 4px 4px 0 0;"">
                                    <h1 style=""color: #ffffff; margin: 0; font-family: Arial, sans-serif;"">{{SchoolName}}</h1>
                                   </div>",
                    FooterHtml = @"<div style=""background-color: #f3f4f6; padding: 20px; text-align: center; color: #6b7280; font-size: 12px; border-radius: 0 0 4px 4px; border-top: 1px solid #e5e7eb;"">
                                    <p style=""margin: 0 0 8px 0;"">This is an automated notification from {{SchoolName}}.</p>
                                    <p style=""margin: 0;"">{{SchoolAddress}} | Support: {{SupportEmail}} | Phone: {{SupportPhone}}</p>
                                    <p style=""margin: 8px 0 0 0;"">&copy; {{CurrentYear}} {{SchoolName}}. All rights reserved.</p>
                                   </div>",
                    SupportEmail = defaultSchool?.Email ?? "support@schoolsaas.com",
                    SupportPhone = defaultSchool?.PhoneNumber ?? "1234567890",
                    PrincipalName = defaultSchool?.ContactPersonName ?? "Principal",
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow
                };

                context.EmailBrandings.Add(branding);
                context.SaveChanges();
            }
        }
    }
}




