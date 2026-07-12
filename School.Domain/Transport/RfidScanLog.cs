using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;
using School.Domain.Hr;

namespace School.Domain.Transport
{
    [Table("RfidScanLogs", Schema = "Transport")]
    public class RfidScanLog : AuditEntity<int>, ITenantEntity
    {
        [Key] 
        public int Id { get; set; }

        [Required] 
        public int TripId { get; set; }
        [ForeignKey(nameof(TripId))] 
        public virtual TransportTrip Trip { get; set; } = null!;

        public int? StudentId { get; set; }
        [ForeignKey(nameof(StudentId))] 
        public virtual Student.Student? Student { get; set; }

        public int? EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))] 
        public virtual Employee? Employee { get; set; }

        [Required, MaxLength(100)] 
        public string RfidTag { get; set; } = null!;

        public DateTime ScanTime { get; set; }

        [Required, MaxLength(20)] 
        public string ScanType { get; set; } = "Boarding"; // Boarding, Drop

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] 
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
