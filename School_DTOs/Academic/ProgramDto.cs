using System;

namespace School_DTOs.Academic
{
    public class ProgramDto : BaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int EducationLevelId { get; set; }
        public string EducationLevelName { get; set; } = string.Empty;
        public int? FacultyId { get; set; }
        public string? FacultyName { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int DurationYears { get; set; }
        public string Status { get; set; } = "active";
    }
}
