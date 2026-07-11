using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;
using School.Domain.Hr;

namespace School.Domain.Transport
{
    public class TransportAllocation : AuditEntity<int>, ITenantEntity
    {
        [Key] 
        public int Id { get; set; }

        public int? StudentId { get; set; }
        [ForeignKey(nameof(StudentId))] 
        public virtual Student.Student? Student { get; set; }

        public int? EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))] 
        public virtual Employee? Employee { get; set; }

        [Required] 
        public int TransportRouteId { get; set; }
        [ForeignKey(nameof(TransportRouteId))] 
        public virtual TransportRoute TransportRoute { get; set; } = null!;

        public int? PickupStopId { get; set; }
        [ForeignKey(nameof(PickupStopId))] 
        public virtual TransportStop? PickupStop { get; set; }

        public int? DropStopId { get; set; }
        [ForeignKey(nameof(DropStopId))] 
        public virtual TransportStop? DropStop { get; set; }

        public TimeSpan? PickupTime { get; set; }
        public TimeSpan? DropTime { get; set; }

        [MaxLength(20)] 
        public string? SeatNumber { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")] 
        public decimal MonthlyCharge { get; set; }

        [Required] 
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required] 
        public int AcademicYearId { get; set; }
        [ForeignKey(nameof(AcademicYearId))] 
        public virtual AcademicYear AcademicYear { get; set; } = null!;

        [MaxLength(200)] 
        public string? QrCode { get; set; }

        [MaxLength(200)] 
        public string? RfidTag { get; set; }

        public bool IsSuspended { get; set; }

        [Required, MaxLength(20)] 
        public string Status { get; set; } = "Active"; // Active, Cancelled

        [Required] 
        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] 
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
