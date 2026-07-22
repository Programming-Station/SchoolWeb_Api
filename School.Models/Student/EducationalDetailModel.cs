using System.ComponentModel.DataAnnotations;
using School.Models.CustomeVailidation;

namespace School.Models.Student
{
    public class EducationalDetailModel
    {
        public int Id { get; set; }
        [MaxLength(200)]
        [NoScript]
        public string ExamName { get; set; } = null!;
        [MaxLength(10)]
        public string? PassingYear { get; set; }
        [MaxLength(200)]
        [NoScript]
        public string? InstituteName { get; set; }
        [MaxLength(500)]
        [NoScript]
        public string? InstituteAddress { get; set; }
        public decimal? TotalMarks { get; set; }
        public decimal? ObtainedMarks { get; set; }
        [MaxLength(2000)]
        public string? Certificate { get; set; }

    }
}

