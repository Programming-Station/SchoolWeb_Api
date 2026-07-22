namespace School_DTOs.Student
{
    public class AdmissionFormConfigDto : BaseDto
    {
        public int Id { get; set; }
        public int CampusId { get; set; }
        public string CampusName { get; set; } = string.Empty;
        public int EducationLevelId { get; set; }
        public string EducationLevelName { get; set; } = string.Empty;
        public int? ProgramId { get; set; }
        public string? ProgramName { get; set; }
        public string FormStepsJson { get; set; } = string.Empty;
        public string DocumentChecklistJson { get; set; } = string.Empty;
        public string? CustomFieldsJson { get; set; }
        public string AutoGenRulesJson { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
