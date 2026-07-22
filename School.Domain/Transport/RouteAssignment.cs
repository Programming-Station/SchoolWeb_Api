using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.Hr;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Transport
{
    [Table("RouteAssignments", Schema = "Transport")]
    public class RouteAssignment : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RouteId { get; set; }
        [ForeignKey(nameof(RouteId))]
        public virtual TransportRoute Route { get; set; } = null!;

        [Required]
        public int VehicleId { get; set; }
        [ForeignKey(nameof(VehicleId))]
        public virtual Vehicle Vehicle { get; set; } = null!;

        [Required]
        public int DriverId { get; set; } // Driver mapped to Hr Employee
        [ForeignKey(nameof(DriverId))]
        public virtual Employee Driver { get; set; } = null!;

        public int? ConductorId { get; set; }
        [ForeignKey(nameof(ConductorId))]
        public virtual Conductor? Conductor { get; set; }

        [Required]
        public int AcademicYearId { get; set; }
        [ForeignKey(nameof(AcademicYearId))]
        public virtual AcademicYear AcademicYear { get; set; } = null!;

        public int? BackupVehicleId { get; set; }
        [ForeignKey(nameof(BackupVehicleId))]
        public virtual Vehicle? BackupVehicle { get; set; }

        public int? BackupDriverId { get; set; }
        [ForeignKey(nameof(BackupDriverId))]
        public virtual Employee? BackupDriver { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Active";

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
