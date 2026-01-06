using System.ComponentModel.DataAnnotations;

namespace School.Models.Class
{
    public class ClassModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Class name is required")]
        [MaxLength(100, ErrorMessage = "Class name cannot exceed 100 characters")]
        public string Name { get; set; } = null!;

        [MaxLength(50, ErrorMessage = "Section cannot exceed 50 characters")]
        public string? Section { get; set; }

        [Required(ErrorMessage = "Course is required")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Academic year is required")]
        [MaxLength(20, ErrorMessage = "Academic year cannot exceed 20 characters")]
        public string AcademicYear { get; set; } = null!;

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
        public int Capacity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Current strength must be greater than or equal to 0")]
        public int CurrentStrength { get; set; } = 0;

        [MaxLength(200, ErrorMessage = "Class teacher cannot exceed 200 characters")]
        public string? ClassTeacher { get; set; }

        [MaxLength(50, ErrorMessage = "Room number cannot exceed 50 characters")]
        public string? RoomNumber { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [MaxLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string Status { get; set; } = "active"; // active or inactive

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

