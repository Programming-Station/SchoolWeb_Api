using System.ComponentModel.DataAnnotations;

namespace School.Models.Academic
{
    public class ProgramModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Program name is required")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Code is required")]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Education Level is required")]
        public int EducationLevelId { get; set; }

        public int? FacultyId { get; set; }

        public int? DepartmentId { get; set; }

        public int DurationYears { get; set; } = 1;

        public string Status { get; set; } = "active";
    }
}
