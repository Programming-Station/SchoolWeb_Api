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
            if (!context.SchoolRegistrations.Any())
            {
                var defaultState = context.States.FirstOrDefault();
                var defaultCity = context.Cities.FirstOrDefault();
                var defaultMedium = context.SchoolMediums.FirstOrDefault()?.Id ?? 1;
                var defaultType = context.SchoolTypes.FirstOrDefault()?.Id ?? 1;
                var defaultBoard = context.AffiliationBoards.FirstOrDefault()?.Id ?? 1;

                if (defaultState == null || defaultCity == null) return;

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
                await context.SaveChangesAsync();

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

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
