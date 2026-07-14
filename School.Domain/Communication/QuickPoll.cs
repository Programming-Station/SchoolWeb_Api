using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("QuickPolls", Schema = "Communication")]
    public class QuickPoll : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(500)]
        public string Question { get; set; } = null!;

        [Required, MaxLength(2000)]
        public string OptionsJson { get; set; } = "[]"; // Array of choice strings, e.g. ["Yes", "No", "Maybe"]

        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(7);

        public bool IsActive { get; set; } = true;

        [Required, MaxLength(50)]
        public string TargetAudience { get; set; } = "All"; // All, Students, Parents, Employees

        public int SchoolRegistrationId { get; set; }
    }
}
