using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;
using School.Domain.Hr;

namespace School.Domain.Transport
{
    public class TransportTrip : AuditEntity<int>, ITenantEntity
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
        public int DriverId { get; set; } // Employee Driver
        [ForeignKey(nameof(DriverId))] 
        public virtual Employee Driver { get; set; } = null!;

        public int? ConductorId { get; set; }
        [ForeignKey(nameof(ConductorId))] 
        public virtual Conductor? Conductor { get; set; }

        public DateTime TripDate { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualEnd { get; set; }

        public int DelayMinutes { get; set; }

        [Required, MaxLength(30)] 
        public string Status { get; set; } = "Scheduled"; // Scheduled, Running, Completed, Cancelled

        [MaxLength(300)] 
        public string? CancellationReason { get; set; }

        [MaxLength(500)] 
        public string? DriverNotes { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] 
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
