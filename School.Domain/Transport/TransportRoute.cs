using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Transport
{
    [Table("TransportRoutes", Schema = "Transport")]
    public class TransportRoute : AuditEntity<int>, ITenantEntity
    {
        [Key] 
        public int Id { get; set; }

        [Required, MaxLength(150)] 
        public string RouteName { get; set; } = null!;

        [Required, MaxLength(50)] 
        public string RouteCode { get; set; } = null!;

        [MaxLength(200)] 
        public string? Description { get; set; }

        [MaxLength(200)] 
        public string? Source { get; set; }

        [MaxLength(200)] 
        public string? Destination { get; set; }

        public double DistanceKm { get; set; }
        
        public int EstimatedTimeMinutes { get; set; }

        [MaxLength(500)] 
        public string? RouteMapPath { get; set; } // GeoJson or route visual path trace

        [MaxLength(20)] 
        public string RouteColor { get; set; } = "#4f46e5"; // CSS Hex Color for map overlays

        public int MaximumCapacity { get; set; }

        // Backwards compatibility VehicleId mapping
        public int VehicleId { get; set; }
        [ForeignKey(nameof(VehicleId))] 
        public virtual Vehicle Vehicle { get; set; } = null!;

        [Required, MaxLength(20)] 
        public string Status { get; set; } = "active";

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] 
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
