using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Utilities.Enums;

namespace School.Infrastructure.Seeds
{
    public static class DefaultCourseData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var school = await context.SchoolRegistrations.FirstOrDefaultAsync();
            if (school == null) return;

            var existingCourses = await context.Courses
                .Where(c => c.SchoolRegistrationId == school.Id)
                .Select(c => c.CourseCode)
                .ToHashSetAsync();

            var defaultCourses = new List<Course>
            {
                new Course
                {
                    Name = "Primary School Course",
                    CourseCode = "PRI-SCH",
                    CourseType = CourseType.School,
                    Duration = "1 Year",
                    Fees = 45000,
                    Description = "Primary School Education (Grades 1-5)",
                    IsActive = true,
                    SchoolRegistrationId = school.Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                },
                new Course
                {
                    Name = "Secondary School Course",
                    CourseCode = "SEC-SCH",
                    CourseType = CourseType.School,
                    Duration = "1 Year",
                    Fees = 55000,
                    Description = "Secondary School Education (Grades 6-10)",
                    IsActive = true,
                    SchoolRegistrationId = school.Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                },
                new Course
                {
                    Name = "Higher Secondary Science",
                    CourseCode = "HSC-SCI",
                    CourseType = CourseType.School,
                    Duration = "2 Years",
                    Fees = 75000,
                    Description = "Higher Secondary Education (Science Stream)",
                    IsActive = true,
                    SchoolRegistrationId = school.Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                },
                new Course
                {
                    Name = "Higher Secondary Commerce",
                    CourseCode = "HSC-COM",
                    CourseType = CourseType.School,
                    Duration = "2 Years",
                    Fees = 65000,
                    Description = "Higher Secondary Education (Commerce Stream)",
                    IsActive = true,
                    SchoolRegistrationId = school.Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                },
                new Course
                {
                    Name = "Bachelor of Computer Applications",
                    CourseCode = "BCA",
                    CourseType = CourseType.University,
                    Duration = "3 Years",
                    Fees = 95000,
                    Description = "Undergraduate BCA Degree Course",
                    IsActive = true,
                    SchoolRegistrationId = school.Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                },
                new Course
                {
                    Name = "Master of Computer Applications",
                    CourseCode = "MCA",
                    CourseType = CourseType.University,
                    Duration = "2 Years",
                    Fees = 120000,
                    Description = "Postgraduate MCA Degree Course",
                    IsActive = true,
                    SchoolRegistrationId = school.Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                }
            };

            var coursesToAdd = defaultCourses.Where(c => !existingCourses.Contains(c.CourseCode)).ToList();
            if (coursesToAdd.Any())
            {
                context.Courses.AddRange(coursesToAdd);
                await context.SaveChangesAsync();
            }
        }
    }
}
