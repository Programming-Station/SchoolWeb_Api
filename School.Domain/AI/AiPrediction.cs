using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.AI
{
    [Table("AiPredictions", Schema = "AI")]
    public class AiPrediction : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string PredictionType { get; set; } = null!; // FeeDefaulter, StudentPerformance, AttendanceAnomaly

        [Required]
        public int TargetEntityId { get; set; } // Student ID or Employee ID

        public decimal ConfidenceScore { get; set; } // Probability from 0.00 to 1.00

        [MaxLength(4000)]
        public string? FactorsJson { get; set; } // JSON of contributing factors

        public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;

        public int SchoolRegistrationId { get; set; }
    }
}
