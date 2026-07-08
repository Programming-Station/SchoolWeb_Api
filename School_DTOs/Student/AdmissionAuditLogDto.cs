using System;

namespace School_DTOs.Student
{
    public class AdmissionAuditLogDto : BaseDto
    {
        public int Id { get; set; }
        public int AdmissionApplicationId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string StatusFrom { get; set; } = string.Empty;
        public string StatusTo { get; set; } = string.Empty;
        public string PerformedBy { get; set; } = string.Empty;
        public DateTime PerformedDate { get; set; }
        public string? DetailsJson { get; set; }
    }
}
