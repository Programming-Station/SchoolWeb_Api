using Microsoft.EntityFrameworkCore;
using School.Domain;

namespace School.Infrastructure.Seeds
{
    public static class DefaultClassData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var school = await context.SchoolRegistrations.FirstOrDefaultAsync();
            if (school == null) return;

            var existingClasses = await context.Classes
                .Where(c => c.SchoolRegistrationId == school.Id)
                .Select(c => c.Name)
                .ToHashSetAsync();

            var primaryCourse = await context.Courses.FirstOrDefaultAsync(c => c.CourseCode == "PRI-SCH" && c.SchoolRegistrationId == school.Id);
            var secondaryCourse = await context.Courses.FirstOrDefaultAsync(c => c.CourseCode == "SEC-SCH" && c.SchoolRegistrationId == school.Id);
            var bcaCourse = await context.Courses.FirstOrDefaultAsync(c => c.CourseCode == "BCA" && c.SchoolRegistrationId == school.Id);

            if (primaryCourse == null || secondaryCourse == null || bcaCourse == null) return;

            var defaultClasses = new List<Class>
            {
                new Class
                {
                    Name = "Grade 1-A",
                    Section = "A",
                    CourseId = primaryCourse.Id,
                    AcademicYear = "2026-2027",
                    Capacity = 40,
                    Status = "active",
                    SchoolRegistrationId = school.Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                },
                new Class
                {
                    Name = "Grade 5-A",
                    Section = "A",
                    CourseId = primaryCourse.Id,
                    AcademicYear = "2026-2027",
                    Capacity = 40,
                    Status = "active",
                    SchoolRegistrationId = school.Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                },
                new Class
                {
                    Name = "Grade 9-A",
                    Section = "A",
                    CourseId = secondaryCourse.Id,
                    AcademicYear = "2026-2027",
                    Capacity = 40,
                    Status = "active",
                    SchoolRegistrationId = school.Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                },
                new Class
                {
                    Name = "Grade 10-B",
                    Section = "B",
                    CourseId = secondaryCourse.Id,
                    AcademicYear = "2026-2027",
                    Capacity = 40,
                    Status = "active",
                    SchoolRegistrationId = school.Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                },
                new Class
                {
                    Name = "FY BCA",
                    Section = "A",
                    CourseId = bcaCourse.Id,
                    AcademicYear = "2026-2027",
                    Capacity = 60,
                    Status = "active",
                    SchoolRegistrationId = school.Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                },
                new Class
                {
                    Name = "SY BCA",
                    Section = "A",
                    CourseId = bcaCourse.Id,
                    AcademicYear = "2026-2027",
                    Capacity = 60,
                    Status = "active",
                    SchoolRegistrationId = school.Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                }
            };

            var classesToAdd = defaultClasses.Where(c => !existingClasses.Contains(c.Name)).ToList();
            if (classesToAdd.Any())
            {
                context.Classes.AddRange(classesToAdd);
                await context.SaveChangesAsync();
            }
        }
    }
}
