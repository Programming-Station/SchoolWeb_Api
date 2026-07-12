using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Transport
{
    [Table("VehicleMaintenances", Schema = "Transport")]
    public class VehicleMaintenance : AuditEntity<int>, ITenantEntity
    {
        [Key] 
        public int Id { get; set; }

        [Required] 
        public int VehicleId { get; set; }
        [ForeignKey(nameof(VehicleId))] 
        public virtual Vehicle Vehicle { get; set; } = null!;

        [Required, MaxLength(50)] 
        public string MaintenanceType { get; set; } = "OilChange"; // OilChange, TyreChange, Battery, Brake, Engine, Repair

        public DateTime ServiceDate { get; set; }
        public double Odometer { get; set; }

        [Column(TypeName = "decimal(10,2)")] 
        public decimal Cost { get; set; }

        [Required, MaxLength(200)] 
        public string VendorName { get; set; } = null!;

        [MaxLength(500)] 
        public string? Details { get; set; }

        public DateTime? NextServiceDue { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] 
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
