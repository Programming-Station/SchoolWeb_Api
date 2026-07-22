namespace School_DTOs.Student
{
    public class AdmissionRuleDto : BaseDto
    {
        public int Id { get; set; }
        public int CampusId { get; set; }
        public string CampusName { get; set; } = string.Empty;
        public int EducationLevelId { get; set; }
        public string EducationLevelName { get; set; } = string.Empty;
        public int? ProgramId { get; set; }
        public string? ProgramName { get; set; }
        public string RuleName { get; set; } = string.Empty;
        public string RuleType { get; set; } = string.Empty;
        public string RuleValue { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
