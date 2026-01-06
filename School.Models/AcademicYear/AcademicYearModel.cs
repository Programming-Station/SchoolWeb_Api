using System.ComponentModel.DataAnnotations;

namespace School.Models.AcademicYear
{
    public class AcademicYearModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Year name is required")]
        [MaxLength(50, ErrorMessage = "Year name cannot exceed 50 characters")]
        public string YearName { get; set; } = null!;

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsCurrent { get; set; } = false;

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

