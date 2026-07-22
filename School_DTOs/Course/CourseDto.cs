namespace School_DTOs.Course
{
    public class CourseDto : BaseDto
    {
        public string Name { get; set; } = null!;
        public string CourseCode { get; set; } = null!;
        public string? CourseType { get; set; }
        public string? Duration { get; set; }
        public decimal Fees { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string? ImagePath { get; set; } = null!;
    }
}

