using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("SurveyQuestions", Schema = "Communication")]
    public class SurveyQuestion : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int SurveyId { get; set; }
        [ForeignKey(nameof(SurveyId))]
        public virtual FeedbackSurvey Survey { get; set; } = null!;

        [Required, MaxLength(500)]
        public string QuestionText { get; set; } = null!;

        [Required, MaxLength(30)]
        public string QuestionType { get; set; } = "Text"; // Text, Rating, SingleChoice

        [MaxLength(2000)]
        public string? OptionsJson { get; set; } // JSON array: ["Poor", "Average", "Good", "Excellent"]

        public int SchoolRegistrationId { get; set; }
    }
}
