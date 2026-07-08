using System.ComponentModel.DataAnnotations;

namespace School.Models.Academic
{
    public class BranchModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Branch name is required")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Code is required")]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Program is required")]
        public int ProgramId { get; set; }

        public string Status { get; set; } = "active";
    }
}
