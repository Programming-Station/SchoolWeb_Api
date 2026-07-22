using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.Domain.Auth;
using School.Domain.School;
using School.Utilities.Enums;

namespace School.Infrastructure.Seeds
{
    public static class DefaultSchoolData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            // ─── 1. Seed Affiliation Boards ───────────────────────────────────────────
            var existingBoards = await context.AffiliationBoards.Select(b => b.Name).ToHashSetAsync();
            var defaultBoards = new[] { "CBSE", "ICSE", "State Board", "IB" };
            var boardsToAdd = defaultBoards.Where(b => !existingBoards.Contains(b)).Select(b => new AffiliationBoard { Name = b }).ToList();
            if (boardsToAdd.Any())
            {
                context.AffiliationBoards.AddRange(boardsToAdd);
                await context.SaveChangesAsync();
            }

            // ─── 2. Seed School Mediums ───────────────────────────────────────────────
            var existingMediums = await context.SchoolMediums.Select(m => m.Name).ToHashSetAsync();
            var defaultMediums = new[] { "English", "Hindi", "Gujarati", "Marathi", "Urdu" };
            var mediumsToAdd = defaultMediums.Where(m => !existingMediums.Contains(m)).Select(m => new SchoolMedium { Name = m }).ToList();
            if (mediumsToAdd.Any())
            {
                context.SchoolMediums.AddRange(mediumsToAdd);
                await context.SaveChangesAsync();
            }

            // ─── 3. Seed School Types ─────────────────────────────────────────────────
            var existingTypes = await context.SchoolTypes.Select(t => t.Name).ToHashSetAsync();
            var defaultTypes = new[] { "Co-Education", "Boys Only", "Girls Only" };
            var typesToAdd = defaultTypes.Where(t => !existingTypes.Contains(t)).Select(t => new SchoolType { Name = t }).ToList();
            if (typesToAdd.Any())
            {
                context.SchoolTypes.AddRange(typesToAdd);
                await context.SaveChangesAsync();
            }

            // Cache Lookups
            var dbBoards = await context.AffiliationBoards.ToDictionaryAsync(b => b.Name, b => b.Id);
            var dbMediums = await context.SchoolMediums.ToDictionaryAsync(m => m.Name, m => m.Id);
            var dbTypes = await context.SchoolTypes.ToDictionaryAsync(t => t.Name, t => t.Id);

            var defaultState = await context.States.FirstOrDefaultAsync(s => s.Name == "Uttar Pradesh") ?? await context.States.FirstOrDefaultAsync();
            var defaultCity = await context.Cities.FirstOrDefaultAsync(c => c.Name == "Varanasi") ?? await context.Cities.FirstOrDefaultAsync();

            if (defaultState == null || defaultCity == null) return;

            // Password hash for "Admin@123"
            const string defaultPasswordHash = "AQAAAAIAAYagAAAAEMMwXeCGG7j8o/vXy+ixddWrSkk2h+DqCkkhYk1gbp70CzYdJ6+dNhyrgMZus4pYNA==";

            // ─── 4. Define 5 Realistic Schools ─────────────────────────────────────────
            var schoolsToSeed = new List<(SchoolRegistration Registration, string OwnerUsername, string OwnerEmail, string OwnerFirstName, string OwnerLastName, string Tagline, string Motto, string PrimaryMedium)>
            {
                (
                    new SchoolRegistration
                    {
                        SchoolName = "Delhi Public School Varanasi",
                        SchoolCode = "DPSVAR001", // Kept DEF001 so existing seeds referencing DEF001 do not break
                        EstablishedYear = 2003,
                        Email = "mariyoo365@gmail.com",
                        PhoneNumber = "05422440022",
                        AlternatePhoneNumber = "9839012345",
                        WebsiteUrl = "https://www.dpsvaranasi.edu.in",
                        RegistrationDate = new DateTime(2003, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                        ApprovalStatus = "Approved",
                        SubDomain = "dpsvaranasi",
                        MaxStudentsAllowed = 3000,
                        AffiliationBoardId = dbBoards["CBSE"],
                        AffiliationNumber = "2130825",
                        SchoolTypeId = dbTypes["Co-Education"],
                        GSTNumber = "09AAATD1234F1Z1",
                        PANNumber = "AAATD1234F",
                        ContactPersonName = "Dr. Subhash Chandra",
                        ContactPersonRole = "Principal",
                        Address = "Vishwapuram, Khajuri, Bypass, NH-2, Varanasi",
                        Pincode = "221005",
                        CountryId = defaultState.CountryId,
                        StateId = defaultState.Id,
                        CityId = defaultCity.Id,
                        Logo = "dps_logo.png",
                        IsActive = true
                    },
                    "owner", // Links to existing default owner user
                    "mariyoo365@gmail.com",
                    "School",
                    "Owner",
                    "Service Before Self",
                    "To light the path of learning and development.",
                    "English"
                ),
                (
                    new SchoolRegistration
                    {
                        SchoolName = "Kendriya Vidyalaya Varanasi",
                        SchoolCode = "KVVAR002",
                        EstablishedYear = 1978,
                        Email = "kunvarprabhat16@gmail.com",
                        PhoneNumber = "05422223456",
                        AlternatePhoneNumber = "9415678901",
                        WebsiteUrl = "https://varanasi.kvs.ac.in",
                        RegistrationDate = new DateTime(1978, 8, 15, 0, 0, 0, DateTimeKind.Utc),
                        ApprovalStatus = "Approved",
                        SubDomain = "kvvaranasi",
                        MaxStudentsAllowed = 2500,
                        AffiliationBoardId = dbBoards["CBSE"],
                        AffiliationNumber = "2100015",
                        SchoolTypeId = dbTypes["Co-Education"],
                        GSTNumber = "09AAATK5678D1Z2",
                        PANNumber = "AAATK5678D",
                        ContactPersonName = "Shri Ramesh Prasad",
                        ContactPersonRole = "Director",
                        Address = "39, Cantt Road, Varanasi",
                        Pincode = "221002",
                        CountryId = defaultState.CountryId,
                        StateId = defaultState.Id,
                        CityId = defaultCity.Id,
                        Logo = "kv_logo.png",
                        IsActive = true
                    },
                    "kv_owner",
                    "kunvarprabhat16@gmail.com",
                    "Ramesh",
                    "Prasad",
                    "Tat Tvam Pooshan Apavrinu",
                    "Unity, Knowledge, and Discipline.",
                    "Hindi"
                ),
                (
                    new SchoolRegistration
                    {
                        SchoolName = "St. Xavier's School Varanasi",
                        SchoolCode = "SXSVAR003",
                        EstablishedYear = 1960,
                        Email = "sakshaminstitutepramedical@gmail.com",
                        PhoneNumber = "05422501234",
                        AlternatePhoneNumber = "9935123456",
                        WebsiteUrl = "https://www.stxaviersvaranasi.edu.in",
                        RegistrationDate = new DateTime(1960, 7, 10, 0, 0, 0, DateTimeKind.Utc),
                        ApprovalStatus = "Approved",
                        SubDomain = "stxaviers",
                        MaxStudentsAllowed = 4000,
                        AffiliationBoardId = dbBoards["ICSE"],
                        AffiliationNumber = "UP032",
                        SchoolTypeId = dbTypes["Co-Education"],
                        GSTNumber = "09AAATS9012R1Z3",
                        PANNumber = "AAATS9012R",
                        ContactPersonName = "Fr. John D'Souza",
                        ContactPersonRole = "Principal",
                        Address = "Christian Nagar, Cantonment, Varanasi",
                        Pincode = "221002",
                        CountryId = defaultState.CountryId,
                        StateId = defaultState.Id,
                        CityId = defaultCity.Id,
                        Logo = "stxaviers_logo.png",
                        IsActive = true
                    },
                    "stxav_owner",
                    "sakshaminstitutepramedical@gmail.com",
                    "Fr. John",
                    "DSouza",
                    "For God and Country",
                    "Inspiring excellence, character, and compassion.",
                    "English"
                ),
                (
                    new SchoolRegistration
                    {
                        SchoolName = "Oxford Public School Varanasi",
                        SchoolCode = "OPSVAR004",
                        EstablishedYear = 2010,
                        Email = "info@oxfordpublicschool.edu.in",
                        PhoneNumber = "05422312345",
                        AlternatePhoneNumber = "8808123456",
                        WebsiteUrl = "https://www.oxfordpublicschool.edu.in",
                        RegistrationDate = new DateTime(2010, 6, 20, 0, 0, 0, DateTimeKind.Utc),
                        ApprovalStatus = "Approved",
                        SubDomain = "oxford",
                        MaxStudentsAllowed = 1800,
                        AffiliationBoardId = dbBoards["State Board"],
                        AffiliationNumber = "2134567",
                        SchoolTypeId = dbTypes["Co-Education"],
                        GSTNumber = "09AAATO3456B1Z4",
                        PANNumber = "AAATO3456B",
                        ContactPersonName = "Mrs. Neeta Sen",
                        ContactPersonRole = "Director",
                        Address = "Shivpur Bypass Road, Varanasi",
                        Pincode = "221003",
                        CountryId = defaultState.CountryId,
                        StateId = defaultState.Id,
                        CityId = defaultCity.Id,
                        Logo = "oxford_logo.png",
                        IsActive = true
                    },
                    "oxford_owner",
                    "oxford_owner@gmail.com",
                    "Neeta",
                    "Sen",
                    "Knowledge is Power",
                    "Building leaders of tomorrow.",
                    "English"
                ),
                (
                    new SchoolRegistration
                    {
                        SchoolName = "Sunrise International School",
                        SchoolCode = "SISVAR005",
                        EstablishedYear = 2015,
                        Email = "admissions@sunriseinternational.edu.in",
                        PhoneNumber = "05422589012",
                        AlternatePhoneNumber = "7707123456",
                        WebsiteUrl = "https://www.sunriseinternational.edu.in",
                        RegistrationDate = new DateTime(2015, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                        ApprovalStatus = "Approved",
                        SubDomain = "sunrise",
                        MaxStudentsAllowed = 1500,
                        AffiliationBoardId = dbBoards["IB"],
                        AffiliationNumber = "IB9012",
                        SchoolTypeId = dbTypes["Co-Education"],
                        GSTNumber = "09AAATR7890N1Z5",
                        PANNumber = "AAATR7890N",
                        ContactPersonName = "Mr. Anil Kapoor",
                        ContactPersonRole = "Chairman",
                        Address = "Rathyatra Crossing, Varanasi",
                        Pincode = "221010",
                        CountryId = defaultState.CountryId,
                        StateId = defaultState.Id,
                        CityId = defaultCity.Id,
                        Logo = "sunrise_logo.png",
                        IsActive = true
                    },
                    "sunrise_owner",
                    "sunrise_owner@gmail.com",
                    "Anil",
                    "Kapoor",
                    "Shine Brightly",
                    "Nurturing global citizens.",
                    "English"
                )
            };

            // ─── 5. Seed Schools, Owners, Subscriptions, and Profile Settings ─────────
            foreach (var item in schoolsToSeed)
            {
                var school = await context.SchoolRegistrations.FirstOrDefaultAsync(s => s.SchoolCode == item.Registration.SchoolCode);
                if (school == null)
                {
                    school = item.Registration;
                    context.SchoolRegistrations.Add(school);
                    await context.SaveChangesAsync(); // Save to generate SchoolId
                }
                else
                {
                    // Update existing school with realistic data if it was dummy before
                    school.SchoolName = item.Registration.SchoolName;
                    school.EstablishedYear = item.Registration.EstablishedYear;
                    school.Email = item.Registration.Email;
                    school.PhoneNumber = item.Registration.PhoneNumber;
                    school.AlternatePhoneNumber = item.Registration.AlternatePhoneNumber;
                    school.WebsiteUrl = item.Registration.WebsiteUrl;
                    school.MaxStudentsAllowed = item.Registration.MaxStudentsAllowed;
                    school.AffiliationBoardId = item.Registration.AffiliationBoardId;
                    school.AffiliationNumber = item.Registration.AffiliationNumber;
                    school.SchoolTypeId = item.Registration.SchoolTypeId;
                    school.GSTNumber = item.Registration.GSTNumber;
                    school.PANNumber = item.Registration.PANNumber;
                    school.ContactPersonName = item.Registration.ContactPersonName;
                    school.ContactPersonRole = item.Registration.ContactPersonRole;
                    school.Address = item.Registration.Address;
                    school.Pincode = item.Registration.Pincode;
                    school.Logo = item.Registration.Logo;
                    await context.SaveChangesAsync();
                }

                // Check or Create Owner User
                var ownerUser = await context.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == item.OwnerUsername.ToUpper());
                if (ownerUser == null)
                {
                    ownerUser = new ApplicationUser
                    {
                        UserName = item.OwnerUsername,
                        Email = item.OwnerEmail,
                        FirstName = item.OwnerFirstName,
                        LastName = item.OwnerLastName,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        PasswordHash = defaultPasswordHash,
                        NormalizedEmail = item.OwnerEmail.ToUpper(),
                        NormalizedUserName = item.OwnerUsername.ToUpper(),
                        StatusId = (int)DefaultStatus.Verified,
                        IsDefaultPassword = false,
                        IsActive = true,
                        SchoolRegistrationId = school.Id
                    };
                    context.Users.Add(ownerUser);
                    await context.SaveChangesAsync();

                    // Assign Owner role
                    var ownerRole = await context.Set<IdentityRole>().FirstOrDefaultAsync(r => r.NormalizedName == "OWNER");
                    if (ownerRole != null)
                    {
                        context.Set<IdentityUserRole<string>>().Add(new IdentityUserRole<string>
                        {
                            UserId = ownerUser.Id,
                            RoleId = ownerRole.Id
                        });
                        await context.SaveChangesAsync();
                    }
                }
                else if (ownerUser.SchoolRegistrationId == null || ownerUser.SchoolRegistrationId == 0)
                {
                    ownerUser.SchoolRegistrationId = school.Id;
                    await context.SaveChangesAsync();
                }

                // Link School Owner
                var ownerExists = await context.SchoolOwners.AnyAsync(o => o.SchoolRegistrationId == school.Id && o.ApplicationUserId == ownerUser.Id);
                if (!ownerExists)
                {
                    var owner = new SchoolOwner
                    {
                        SchoolRegistrationId = school.Id,
                        ApplicationUserId = ownerUser.Id,
                        StatusId = 1,
                        EmailVerified = true,
                        MobileVerified = true,
                        IsLocked = false,
                        CreatedBy = "System",
                        CreatedDate = DateTime.UtcNow
                    };
                    context.SchoolOwners.Add(owner);
                }

                // Seed Free Subscription
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
                        IsActive = true,
                        CreatedBy = "System",
                        CreatedDate = DateTime.UtcNow
                    };
                    context.SchoolSubscriptions.Add(subscription);
                }

                // Seed Profile Settings
                var profileExists = await context.SchoolProfileSettings.AnyAsync(s => s.SchoolRegistrationId == school.Id);
                if (!profileExists)
                {
                    var primaryMediumId = dbMediums.ContainsKey(item.PrimaryMedium) ? dbMediums[item.PrimaryMedium] : dbMediums.Values.FirstOrDefault();
                    var profileSetting = new SchoolProfileSetting
                    {
                        SchoolRegistrationId = school.Id,
                        Tagline = item.Tagline,
                        PrimaryMediumId = primaryMediumId,
                        CreatedBy = "System",
                        CreatedDate = DateTime.UtcNow
                    };
                    context.SchoolProfileSettings.Add(profileSetting);
                }

                // Seed Organization Profile
                var orgProfileExists = await context.OrganizationProfiles.AnyAsync(s => s.SchoolRegistrationId == school.Id);
                if (!orgProfileExists)
                {
                    var orgProfile = new OrganizationProfile
                    {
                        SchoolRegistrationId = school.Id,
                        OrganizationName = school.SchoolName,
                        SchoolName = school.SchoolName,
                        ShortName = school.SchoolCode,
                        SchoolCode = school.SchoolCode,
                        Email = school.Email,
                        Phone = school.PhoneNumber,
                        AddressLine1 = school.Address,
                        Pincode = school.Pincode,
                        HeaderLogo = school.Logo,
                        PrimaryColor = "#1e3a8a",
                        SecondaryColor = "#0d9488",
                        Theme = "Light",
                        PrincipalName = school.ContactPersonName ?? "Dr. Rajesh Sharma",
                        Board = "CBSE",
                        Status = true,
                        CreatedBy = "System",
                        CreatedDate = DateTime.UtcNow
                    };
                    context.OrganizationProfiles.Add(orgProfile);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
