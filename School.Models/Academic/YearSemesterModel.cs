using System.ComponentModel.DataAnnotations;

namespace School.Models.Academic
{
    public class YearSemesterModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Code is required")]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        public int Sequence { get; set; } = 1;

        public string Status { get; set; } = "active";
    }
}
