using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Transport
{
    [Table("VehicleIncidents", Schema = "Transport")]
    public class VehicleIncident : AuditEntity<int>, ITenantEntity
    {
        [Key] 
        public int Id { get; set; }

        [Required] 
        public int VehicleId { get; set; }
        [ForeignKey(nameof(VehicleId))] 
        public virtual Vehicle Vehicle { get; set; } = null!;

        public DateTime IncidentDate { get; set; }

        [Required, MaxLength(1000)] 
        public string Description { get; set; } = null!;

        [MaxLength(100)] 
        public string? ClaimNumber { get; set; }

        [Column(TypeName = "decimal(10,2)")] 
        public decimal ClaimAmount { get; set; }

        [Column(TypeName = "decimal(10,2)")] 
        public decimal RepairCost { get; set; }

        [MaxLength(500)] 
        public string? PoliceReportFileUrl { get; set; }

        [MaxLength(500)] 
        public string? PhotoUrl { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] 
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
