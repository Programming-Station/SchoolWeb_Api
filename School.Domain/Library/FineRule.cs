using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Library
{
    [Table("LibraryFineRules", Schema = "Library")]
    public class LibraryFineRule : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string RuleName { get; set; } = "Default Fine Rule";

        [Column(TypeName = "decimal(18,2)")]
        public decimal PerDayFine { get; set; } = 10;

        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxFine { get; set; } = 500;

        public int GraceDays { get; set; } = 0;

        public bool HolidayExemption { get; set; } = true;

        public bool CategoryWise { get; set; } = false;

        public int? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual BookCategory? Category { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "Active";

        public bool IsDefault { get; set; } = false;

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration? SchoolRegistration { get; set; }
    }
}
