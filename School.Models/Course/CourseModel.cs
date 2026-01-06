using School.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace School.Models.Course
{
    public class CourseModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        [MaxLength(200, ErrorMessage = "Course name cannot exceed 200 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Course code is required")]
        [MaxLength(50, ErrorMessage = "Course code cannot exceed 50 characters")]
        public string CourseCode { get; set; } = null!;

        [Required(ErrorMessage = "Course type is required")]

        public CourseType CourseType { get; set; } // 1 = School, 2 = University

        [MaxLength(100, ErrorMessage = "Duration cannot exceed 100 characters")]
        public string? Duration { get; set; }

        [Required(ErrorMessage = "Fees is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Fees must be greater than or equal to 0")]
        public decimal Fees { get; set; }

        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

