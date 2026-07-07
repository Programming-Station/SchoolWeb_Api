using Microsoft.EntityFrameworkCore;
using School.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Seeds
{
    public static class DefaultAcademicYearData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var schools = await context.SchoolRegistrations.ToListAsync();
            if (!schools.Any()) return;

            foreach (var school in schools)
            {
                var existingYears = await context.AcademicYears
                    .Where(ay => ay.SchoolRegistrationId == school.Id)
                    .Select(ay => ay.YearName)
                    .ToHashSetAsync();

                var defaultYears = new List<AcademicYear>
                {
                    new AcademicYear
                    {
                        YearName = "2024-2025",
                        StartDate = new DateTime(2024, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                        EndDate = new DateTime(2025, 3, 31, 23, 59, 59, DateTimeKind.Utc),
                        Description = "Academic Year 2024-2025",
                        IsActive = true,
                        IsCurrent = false,
                        SchoolRegistrationId = school.Id,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    },
                    new AcademicYear
                    {
                        YearName = "2025-2026",
                        StartDate = new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                        EndDate = new DateTime(2026, 3, 31, 23, 59, 59, DateTimeKind.Utc),
                        Description = "Academic Year 2025-2026",
                        IsActive = true,
                        IsCurrent = false,
                        SchoolRegistrationId = school.Id,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    },
                    new AcademicYear
                    {
                        YearName = "2026-2027",
                        StartDate = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                        EndDate = new DateTime(2027, 3, 31, 23, 59, 59, DateTimeKind.Utc),
                        Description = "Academic Year 2026-2027",
                        IsActive = true,
                        IsCurrent = true,
                        SchoolRegistrationId = school.Id,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    },
                    new AcademicYear
                    {
                        YearName = "2027-2028",
                        StartDate = new DateTime(2027, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                        EndDate = new DateTime(2028, 3, 31, 23, 59, 59, DateTimeKind.Utc),
                        Description = "Academic Year 2027-2028",
                        IsActive = true,
                        IsCurrent = false,
                        SchoolRegistrationId = school.Id,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    }
                };

                var yearsToAdd = defaultYears.Where(ay => !existingYears.Contains(ay.YearName)).ToList();
                if (yearsToAdd.Any())
                {
                    context.AcademicYears.AddRange(yearsToAdd);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
