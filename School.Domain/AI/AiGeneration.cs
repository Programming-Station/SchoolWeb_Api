using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.AI
{
    [Table("AiGenerations", Schema = "AI")]
    public class AiGeneration : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string GenerationType { get; set; } = null!; // Timetable, LessonPlan, QuestionPaper

        [Required, MaxLength(2000)]
        public string Prompt { get; set; } = null!;

        [Required]
        public string OutputJson { get; set; } = null!;

        public int SchoolRegistrationId { get; set; }
    }
}
