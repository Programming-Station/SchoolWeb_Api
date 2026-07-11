using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Transport
{
    public class TransportInventory : AuditEntity<int>, ITenantEntity
    {
        [Key] 
        public int Id { get; set; }

        [Required, MaxLength(200)] 
        public string ItemName { get; set; } = null!;

        [Required, MaxLength(50)] 
        public string Category { get; set; } = null!; // Tyres, Battery, FirstAid, GPSDevice, RFIDDevice, EmergencyTool

        [MaxLength(150)] 
        public string? SerialNumber { get; set; }

        public DateTime? InstallationDate { get; set; }

        [Required, MaxLength(30)] 
        public string Status { get; set; } = "Active"; // Active, Spare, Scrapped

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] 
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
