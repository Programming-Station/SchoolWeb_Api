using System.ComponentModel.DataAnnotations;

namespace School.Models.Student
{
    public class AdmissionRuleModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Campus selection is required")]
        public int CampusId { get; set; }

        [Required(ErrorMessage = "Education Level selection is required")]
        public int EducationLevelId { get; set; }

        public int? ProgramId { get; set; }

        [Required(ErrorMessage = "Rule Name is required")]
        [MaxLength(200)]
        public string RuleName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rule Type is required")]
        [MaxLength(100)]
        public string RuleType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rule Target Value is required")]
        [MaxLength(200)]
        public string RuleValue { get; set; } = string.Empty;

        [Required(ErrorMessage = "Error Message is required")]
        [MaxLength(500)]
        public string ErrorMessage { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
