using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hostel
{
    [Table("HostelAdmissions", Schema = "Hostel")]
    public class HostelAdmission : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }

        public int HostelId { get; set; }

        public int RoomId { get; set; }

        public int BedId { get; set; }

        public int AcademicYearId { get; set; }

        [Required, MaxLength(50)]
        public string AdmissionNumber { get; set; } = null!;

        public DateTime AdmissionDate { get; set; }

        public DateTime? CheckInDate { get; set; }

        public DateTime? ExpectedCheckOutDate { get; set; }

        public DateTime? ActualCheckOutDate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "admitted"; // admitted, checkedin, checkedout, cancelled

        [Column(TypeName = "decimal(18,2)")]
        public decimal SecurityDeposit { get; set; }

        public bool SecurityDepositRefunded { get; set; }

        [MaxLength(500)]
        public string? DocumentsUrl { get; set; }

        [MaxLength(1000)]
        public string? MedicalDetails { get; set; }

        [MaxLength(1000)]
        public string? SpecialNotes { get; set; }

        [MaxLength(100)]
        public string? BiometricId { get; set; }

        [MaxLength(100)]
        public string? RfidTag { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual global::School.Domain.Student.Student Student { get; set; } = null!;

        [ForeignKey(nameof(HostelId))]
        public virtual Hostel Hostel { get; set; } = null!;

        [ForeignKey(nameof(RoomId))]
        public virtual Room Room { get; set; } = null!;

        [ForeignKey(nameof(BedId))]
        public virtual Bed Bed { get; set; } = null!;

        [ForeignKey(nameof(AcademicYearId))]
        public virtual AcademicYear AcademicYear { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
