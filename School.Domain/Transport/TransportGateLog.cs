using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Transport
{
    public class TransportGateLog : AuditEntity<int>, ITenantEntity
    {
        [Key] 
        public int Id { get; set; }

        [Required, MaxLength(30)] 
        public string VehicleNumber { get; set; } = null!;

        [Required, MaxLength(150)] 
        public string DriverName { get; set; } = null!;

        [Required, MaxLength(300)] 
        public string Purpose { get; set; } = null!;

        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }

        [Required, MaxLength(20)] 
        public string Status { get; set; } = "In"; // In, Out

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] 
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
