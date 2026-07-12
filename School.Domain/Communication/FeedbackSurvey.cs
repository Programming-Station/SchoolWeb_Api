using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("FeedbackSurveys", Schema = "Communication")]
    public class FeedbackSurvey : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required, MaxLength(50)]
        public string TargetAudience { get; set; } = "All"; // All, Students, Parents, Employees

        public bool IsActive { get; set; } = true;

        public int SchoolRegistrationId { get; set; }

        public virtual ICollection<SurveyQuestion> Questions { get; set; } = new List<SurveyQuestion>();
        public virtual ICollection<SurveyResponse> Responses { get; set; } = new List<SurveyResponse>();
    }
}
