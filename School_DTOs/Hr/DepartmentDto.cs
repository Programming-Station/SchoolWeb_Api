using System;

namespace School_DTOs.Hr
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public int FacultyId { get; set; }
        public string? FacultyName { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public string Status { get; set; } = "active";
    }

    public class CreateDepartmentDto
    {
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public int FacultyId { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public string Status { get; set; } = "active";
    }

    public class UpdateDepartmentDto : CreateDepartmentDto
    {
        public int Id { get; set; }
    }
}
