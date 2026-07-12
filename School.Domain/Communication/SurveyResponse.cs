using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("SurveyResponses", Schema = "Communication")]
    public class SurveyResponse : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int SurveyId { get; set; }
        [ForeignKey(nameof(SurveyId))]
        public virtual FeedbackSurvey Survey { get; set; } = null!;

        [Required, MaxLength(450)]
        public string RespondentUserId { get; set; } = null!;

        [Required, MaxLength(8000)]
        public string AnswersJson { get; set; } = null!; // Stored as {"1": "Good", "2": "5"}

        public DateTime SubmittedDate { get; set; } = DateTime.UtcNow;

        public int SchoolRegistrationId { get; set; }
    }
}
