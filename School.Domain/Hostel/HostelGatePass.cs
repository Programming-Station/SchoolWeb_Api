using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hostel
{
    [Table("HostelGatePasses", Schema = "Hostel")]
    public class HostelGatePass : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int AdmissionId { get; set; }

        public int StudentId { get; set; }

        public DateTime OutTime { get; set; }

        public DateTime? InTime { get; set; }

        public DateTime ExpectedReturn { get; set; }

        [Required, MaxLength(1000)]
        public string Reason { get; set; } = null!;

        [MaxLength(200)]
        public string? WardenApproval { get; set; } // Approved, Pending, Rejected

        [MaxLength(200)]
        public string? ParentApproval { get; set; } // Approved, Pending, Rejected

        [MaxLength(500)]
        public string? QrCode { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "pending"; // pending, approved, rejected, expired, completed

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(AdmissionId))]
        public virtual HostelAdmission Admission { get; set; } = null!;

        [ForeignKey(nameof(StudentId))]
        public virtual global::School.Domain.Student.Student Student { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
