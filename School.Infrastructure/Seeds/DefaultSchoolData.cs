using Microsoft.EntityFrameworkCore;
using School.Domain.School;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Seeds
{
    public static class DefaultSchoolData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            // Seed Affiliation Boards if missing
            var existingBoards = await context.AffiliationBoards.Select(b => b.Name).ToHashSetAsync();
            var defaultBoards = new[] { "CBSE", "ICSE", "State Board", "IB" };
            var boardsToAdd = defaultBoards.Where(b => !existingBoards.Contains(b)).Select(b => new AffiliationBoard { Name = b }).ToList();
            if (boardsToAdd.Any())
            {
                context.AffiliationBoards.AddRange(boardsToAdd);
                await context.SaveChangesAsync();
            }

            // Seed School Mediums if missing
            var existingMediums = await context.SchoolMediums.Select(m => m.Name).ToHashSetAsync();
            var defaultMediums = new[] { "English", "Hindi", "Gujarati", "Marathi", "Urdu" };
            var mediumsToAdd = defaultMediums.Where(m => !existingMediums.Contains(m)).Select(m => new SchoolMedium { Name = m }).ToList();
            if (mediumsToAdd.Any())
            {
                context.SchoolMediums.AddRange(mediumsToAdd);
                await context.SaveChangesAsync();
            }

            // Seed School Types if missing
            var existingTypes = await context.SchoolTypes.Select(t => t.Name).ToHashSetAsync();
            var defaultTypes = new[] { "Co-Education", "Boys Only", "Girls Only" };
            var typesToAdd = defaultTypes.Where(t => !existingTypes.Contains(t)).Select(t => new SchoolType { Name = t }).ToList();
            if (typesToAdd.Any())
            {
                context.SchoolTypes.AddRange(typesToAdd);
                await context.SaveChangesAsync();
            }

            var school = await context.SchoolRegistrations.FirstOrDefaultAsync(s => s.SchoolCode == "DEF001");
            if (school == null)
            {
                var defaultState = await context.States.FirstOrDefaultAsync();
                var defaultCity = await context.Cities.FirstOrDefaultAsync();
                var defaultMedium = (await context.SchoolMediums.FirstOrDefaultAsync(m => m.Name == "English"))?.Id ?? (await context.SchoolMediums.FirstOrDefaultAsync())?.Id ?? 1;
                var defaultType = (await context.SchoolTypes.FirstOrDefaultAsync(t => t.Name == "Co-Education"))?.Id ?? (await context.SchoolTypes.FirstOrDefaultAsync())?.Id ?? 1;
                var defaultBoard = (await context.AffiliationBoards.FirstOrDefaultAsync(b => b.Name == "CBSE"))?.Id ?? (await context.AffiliationBoards.FirstOrDefaultAsync())?.Id ?? 1;

                if (defaultState == null || defaultCity == null) return;

                school = new SchoolRegistration
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
                await context.SaveChangesAsync();
            }

            var schoolOwnerUser = await context.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == "OWNER");
            if (schoolOwnerUser != null)
            {
                var ownerExists = await context.SchoolOwners.AnyAsync(o => o.SchoolRegistrationId == school.Id && o.ApplicationUserId == schoolOwnerUser.Id);
                if (!ownerExists)
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
                }

                var subExists = await context.SchoolSubscriptions.AnyAsync(s => s.SchoolRegistrationId == school.Id);
                if (!subExists)
                {
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
                }
                
                var profileExists = await context.SchoolProfileSettings.AnyAsync(s => s.SchoolRegistrationId == school.Id);
                if (!profileExists)
                {
                    var defaultMedium = (await context.SchoolMediums.FirstOrDefaultAsync())?.Id ?? 1;
                    var profileSetting = new SchoolProfileSetting
                    {
                        SchoolRegistrationId = school.Id,
                        Tagline = "A Great Place to Learn",
                        PrimaryMediumId = defaultMedium
                    };
                    context.SchoolProfileSettings.Add(profileSetting);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
