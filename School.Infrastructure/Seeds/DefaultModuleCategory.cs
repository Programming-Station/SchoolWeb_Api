using School.Domain;
using School.Utilities.Enums;

namespace School.Infrastructure.Seeds
{
    public static class DefaultModuleCategory
    {
        public static List<CategoryModule> CategoryModuleList()
        {
            return new List<CategoryModule>()
            {
                new CategoryModule
                {
                    Name = DefaultCategoryModule.Management.ToString(),
                    Description = "Management related modules for student, faculty, and resource management",
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    Order = 1
                },
                new CategoryModule
                {
                    Name =DefaultCategoryModule.Academic.ToString(),
                    Description = "Academic and education related modules for courses, classes, exams, and library",
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    Order = 2
                },
                new CategoryModule
                {
                    Name = DefaultCategoryModule.Administrative.ToString(),
                    Description = "Administrative and office management modules for attendance, leave, and fees",
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    Order = 3
                },
                new CategoryModule
                {
                    Name = DefaultCategoryModule.Communication.ToString(),
                    Description = "Communication and messaging modules for website management and enquiries",
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    Order = 4
                },
                new CategoryModule
                {
                    Name = DefaultCategoryModule.Reports.ToString(),
                    Description = "Reporting and analytics modules for generating various reports",
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    Order = 5
                }
            };
        }
    }
}
