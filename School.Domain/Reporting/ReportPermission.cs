using System.ComponentModel.DataAnnotations;
using School.Domain.School;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Reporting
{
    /// <summary>
    /// Role-based permission matrix for report access.
    /// Controls which roles can view, export, print, email, and schedule each report.
    /// </summary>
    [Table("ReportPermissions" , Schema = "Reporting")]
    public class ReportPermission : AuditEntity<int>, ITenantEntity{
        [Key]
        public int Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        public int ReportTemplateId { get; set; }

        [ForeignKey(nameof(ReportTemplateId))]
        public virtual ReportTemplate? ReportTemplate { get; set; }

        /// <summary>Role name as registered in ASP.NET Identity e.g. SchoolAdmin, Teacher.</summary>
        [Required, MaxLength(100)]
        public string RoleName { get; set; } = string.Empty;

        public bool CanView { get; set; } = true;

        public bool CanExportPdf { get; set; } = true;

        public bool CanExportExcel { get; set; } = false;

        public bool CanExportWord { get; set; } = false;

        public bool CanExportCsv { get; set; } = false;

        public bool CanPrint { get; set; } = true;

        public bool CanEmail { get; set; } = false;

        public bool CanSchedule { get; set; } = false;

        public bool CanManageTemplate { get; set; } = false;
    }
}


