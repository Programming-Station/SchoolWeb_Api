namespace School_DTOs.Class
{
    public class ClassDto: BaseDto
    {
        public string Name { get; set; } = null!;
        public string? Section { get; set; }
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? CourseCode { get; set; }
        public string AcademicYear { get; set; } = null!;
        public int Capacity { get; set; }
        public int CurrentStrength { get; set; }
        public int? ClassTeacherId { get; set; }
        public string? ClassTeacherName { get; set; }
        public string? RoomNumber { get; set; }
        public string Status { get; set; } = null!; 
    }
}

