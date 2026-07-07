using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class EmployeeDocument : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Required, MaxLength(200)]
        public string DocumentName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string DocumentType { get; set; } = null!; // ID, Address, Certificate etc.

        [Required, MaxLength(500)]
        public string FilePath { get; set; } = null!;

        [MaxLength(200)]
        public string? OriginalFileName { get; set; }

        [MaxLength(50)]
        public string? FileExtension { get; set; }

        public decimal FileSize { get; set; } // in MB

        public int Version { get; set; } = 1;

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
