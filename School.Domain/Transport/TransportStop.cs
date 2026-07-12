using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Transport
{
    [Table("TransportStops", Schema = "Transport")]
    public class TransportStop : AuditEntity<int>, ITenantEntity
    {
        [Key] 
        public int Id { get; set; }

        [Required, MaxLength(200)] 
        public string StopName { get; set; } = null!;

        [Column(TypeName = "decimal(9,6)")] 
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(9,6)")] 
        public decimal Longitude { get; set; }

        public TimeSpan ArrivalTime { get; set; }
        
        public TimeSpan DepartureTime { get; set; }

        public double DistanceFromSource { get; set; }

        [MaxLength(500)] 
        public string? GoogleMapsLink { get; set; }

        [MaxLength(200)] 
        public string? Landmark { get; set; }

        [Required, MaxLength(20)] 
        public string Status { get; set; } = "Active";

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] 
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
