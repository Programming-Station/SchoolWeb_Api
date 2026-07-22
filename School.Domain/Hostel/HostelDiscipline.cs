using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hostel
{
    [Table("HostelDisciplines", Schema = "Hostel")]
    public class HostelDiscipline : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }

        [Required, MaxLength(500)]
        public string Offense { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Severity { get; set; } = "Minor"; // Minor, Major, Severe

        [MaxLength(500)]
        public string? ActionTaken { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal FineAmount { get; set; }

        public DateTime IncidentDate { get; set; }

        [MaxLength(1000)]
        public string? WardenRemarks { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual global::School.Domain.Student.Student Student { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
