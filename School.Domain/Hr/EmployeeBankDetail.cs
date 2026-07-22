using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hr
{
    public class EmployeeBankDetail : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Required, MaxLength(200)]
        public string BankName { get; set; } = null!;

        [MaxLength(200)]
        public string? Branch { get; set; }

        [Required, MaxLength(50)]
        public string AccountNumber { get; set; } = null!;

        [Required, MaxLength(20)]
        public string IFSC { get; set; } = null!;

        [MaxLength(50)]
        public string? UPI { get; set; }

        public bool IsSalaryAccount { get; set; } = true;

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
