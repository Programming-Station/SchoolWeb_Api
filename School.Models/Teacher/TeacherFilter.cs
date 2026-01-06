using School.Models;

namespace School.Models.Teacher
{
    public class TeacherFilter : BaseFilter
    {
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? CourseId { get; set; }
        public string? Faculty { get; set; }
        public string? Department { get; set; }
        public string? Status { get; set; }
    }
}

