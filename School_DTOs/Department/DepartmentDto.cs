using School_DTOs;

namespace School_DTOs.Department
{
    public class DepartmentDto : BaseDto
    {
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public int FacultyId { get; set; }
        public string? FacultyName { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public string Status { get; set; } = "active";
    }
}

