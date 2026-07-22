using System.ComponentModel.DataAnnotations;

namespace School.Models.Academic
{
    public class BatchModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Batch Name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Code is required")]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Academic Year is required")]
        public int AcademicYearId { get; set; }

        [Required(ErrorMessage = "Program is required")]
        public int ProgramId { get; set; }

        public string Status { get; set; } = "active";
    }
}
