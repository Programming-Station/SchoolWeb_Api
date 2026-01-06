using System.ComponentModel.DataAnnotations;

namespace School.Models.Department
{
    public class DepartmentModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Department name is required")]
        [MaxLength(200, ErrorMessage = "Department name cannot exceed 200 characters")]
        public string Name { get; set; } = null!;

        [MaxLength(50, ErrorMessage = "Department code cannot exceed 50 characters")]
        public string? Code { get; set; }

        [Required(ErrorMessage = "Faculty is required")]
        public int FacultyId { get; set; }

        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        public int DisplayOrder { get; set; } = 0;

        [MaxLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string Status { get; set; } = "active";

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

