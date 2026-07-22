namespace School_DTOs.Faculty
{
    public class FacultyDto : BaseDto
    {
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public string Status { get; set; } = "active";
    }
}

