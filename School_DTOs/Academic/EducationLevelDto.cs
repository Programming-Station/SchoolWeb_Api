using System;

namespace School_DTOs.Academic
{
    public class EducationLevelDto : BaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = "active";
    }
}
