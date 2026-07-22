using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Payroll
{
    [Table("PayGroups", Schema = "Payroll")]
    public class PayGroup : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!; // e.g. "Monthly Staff", "Weekly Contract", "Daily Wages"

        [Required, MaxLength(20)]
        public string Frequency { get; set; } = "Monthly"; // Monthly, Weekly, BiWeekly, Daily

        [Required, MaxLength(10)]
        public string Currency { get; set; } = "INR";

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
