using System.ComponentModel.DataAnnotations;

namespace School.Models.Student
{
    public class AdmissionFormConfigModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Campus selection is required")]
        public int CampusId { get; set; }

        [Required(ErrorMessage = "Education Level selection is required")]
        public int EducationLevelId { get; set; }

        public int? ProgramId { get; set; }

        [Required(ErrorMessage = "Form steps configuration is required")]
        public string FormStepsJson { get; set; } = string.Empty;

        [Required(ErrorMessage = "Document checklist configuration is required")]
        public string DocumentChecklistJson { get; set; } = string.Empty;

        public string? CustomFieldsJson { get; set; }

        [Required(ErrorMessage = "Auto-generation rules configuration is required")]
        public string AutoGenRulesJson { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
