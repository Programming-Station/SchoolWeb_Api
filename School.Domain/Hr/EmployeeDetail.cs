using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Hr
{
    public class EmployeeDetail : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        public int? BloodGroupMasterId { get; set; }
        [ForeignKey(nameof(BloodGroupMasterId))]
        public virtual BloodGroupMaster? BloodGroup { get; set; }

        public int? ReligionMasterId { get; set; }
        [ForeignKey(nameof(ReligionMasterId))]
        public virtual ReligionMaster? Religion { get; set; }

        public int? QualificationMasterId { get; set; }
        [ForeignKey(nameof(QualificationMasterId))]
        public virtual QualificationMaster? Qualification { get; set; }

        [MaxLength(200)]
        public string? FatherName { get; set; }

        [MaxLength(200)]
        public string? MotherName { get; set; }

        [MaxLength(50)]
        public string? MaritalStatus { get; set; }

        [MaxLength(50)]
        public string? Nationality { get; set; }

        [MaxLength(50)]
        public string? Category { get; set; }

        [MaxLength(20)]
        public string? AadhaarNumber { get; set; }

        [MaxLength(20)]
        public string? PANNumber { get; set; }

        [MaxLength(50)]
        public string? PassportNumber { get; set; }

        [MaxLength(50)]
        public string? DrivingLicense { get; set; }

        [MaxLength(20)]
        public string? AlternateMobile { get; set; }

        [MaxLength(20)]
        public string? EmergencyContact { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(100)]
        public string? District { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(20)]
        public string? PinCode { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(200)]
        public string? PreviousOrganization { get; set; }

        [MaxLength(100)]
        public string? BiometricID { get; set; }

        [MaxLength(2000)]
        public string? EmployeeNotes { get; set; }
    }
}
