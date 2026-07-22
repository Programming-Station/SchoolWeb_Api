using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Administration
{
    [Table("AutoNumberSettings", Schema = "Administration")]
    public class AutoNumberSetting : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string EntityType { get; set; } = null!; // e.g. "Admission", "Employee", "Voucher", "Invoice", "Receipt", "Expense", "Purchase", "LibraryIssue", "TransportPass", "HostelAdmission"

        [MaxLength(20)]
        public string? Prefix { get; set; }

        [MaxLength(20)]
        public string? Suffix { get; set; }

        public int NextValue { get; set; } = 1;

        public int PaddingLength { get; set; } = 4;

        public bool IncludeYear { get; set; } = true;

        public int SchoolRegistrationId { get; set; }
    }
}
