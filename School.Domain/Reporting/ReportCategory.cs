using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Reporting
{
    /// <summary>
    /// Report category for grouping reports in the enterprise reporting engine.
    /// E.g. Admission, Student, Fee, Finance, HR, Payroll, Certificates, etc.
    /// </summary>
    [Table("ReportCategories", Schema = "Reporting")]
    public class ReportCategory : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        /// <summary>Unique category code e.g. ADMISSION, FEE, HR.</summary>
        [Required, MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        /// <summary>Display name for the category.</summary>
        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        /// <summary>PrimeNG/FontAwesome icon class e.g. pi pi-file-pdf.</summary>
        [MaxLength(100)]
        public string? IconClass { get; set; }

        /// <summary>Hex color for the category tile in the UI e.g. #1e3a8a.</summary>
        [MaxLength(20)]
        public string? ColorHex { get; set; }

        /// <summary>Short description shown in the report dashboard.</summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>Display sort order.</summary>
        public int SortOrder { get; set; } = 0;

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        // Navigation
        public virtual ICollection<ReportTemplate> ReportTemplates { get; set; } = new List<ReportTemplate>();
    }
}


