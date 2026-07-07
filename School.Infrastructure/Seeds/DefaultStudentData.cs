using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Domain.Student;
using School.Domain.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Seeds
{
    public static class DefaultStudentData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var school = await context.SchoolRegistrations.FirstOrDefaultAsync();
            if (school == null) return;

            // Find Course and Class seeded by DefaultCourseData / DefaultClassData
            var course = await context.Courses.FirstOrDefaultAsync(c => c.CourseCode == "PRI-SCH" && c.SchoolRegistrationId == school.Id);
            var classEntity = await context.Classes.FirstOrDefaultAsync(c => c.Name == "Grade 5-A" && c.SchoolRegistrationId == school.Id);

            if (course == null || classEntity == null) return;

            // Check if student exists
            var existingStudents = await context.Students.AnyAsync(s => s.SchoolRegistrationId == school.Id);
            if (!existingStudents)
            {
                var defaultState = await context.States.FirstOrDefaultAsync();
                var defaultCity = await context.Cities.FirstOrDefaultAsync();
                var defaultStatus = await context.Statuses.FirstOrDefaultAsync(s => s.Name == "Active") 
                                    ?? await context.Statuses.FirstOrDefaultAsync();

                if (defaultStatus == null) return;

                var students = new List<Student>
                {
                    new Student
                    {
                        StudentId = "STU260001",
                        EnrollmentNumber = "ENR260001",
                        CourseType = "School",
                        CourseId = course.Id,
                        CourseOpted = "Primary School Course",
                        Name = "Aarav Sharma",
                        FathersName = "Rajesh Sharma",
                        Gender = "Male",
                        Nationality = "Indian",
                        Occupation = "Service",
                        DateOfBirth = "2015-05-15",
                        PostalAddress = "456 Park Avenue, Mumbai",
                        StateId = defaultState?.Id,
                        CityId = defaultCity?.Id,
                        PinCode = "400001",
                        MobileNo1 = "9876543210",
                        MobileNo2 = "9876543211",
                        ClassId = classEntity.Id,
                        StatusId = defaultStatus.Id,
                        Remarks = "Regular default seeded student.",
                        SchoolRegistrationId = school.Id,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    },
                    new Student
                    {
                        StudentId = "STU260002",
                        EnrollmentNumber = "ENR260002",
                        CourseType = "School",
                        CourseId = course.Id,
                        CourseOpted = "Primary School Course",
                        Name = "Ananya Patel",
                        FathersName = "Vijay Patel",
                        Gender = "Female",
                        Nationality = "Indian",
                        Occupation = "Business",
                        DateOfBirth = "2016-08-20",
                        PostalAddress = "789 Lake View Road, Pune",
                        StateId = defaultState?.Id,
                        CityId = defaultCity?.Id,
                        PinCode = "411001",
                        MobileNo1 = "8765432109",
                        MobileNo2 = "8765432108",
                        ClassId = classEntity.Id,
                        StatusId = defaultStatus.Id,
                        Remarks = "Active default seeded student.",
                        SchoolRegistrationId = school.Id,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    }
                };

                foreach (var std in students)
                {
                    var exists = await context.Students.AnyAsync(s => s.StudentId == std.StudentId && s.SchoolRegistrationId == school.Id);
                    if (!exists)
                    {
                        context.Students.Add(std);
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
