using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Transport
{
    public class RouteStopMapping : AuditEntity<int>, ITenantEntity
    {
        [Key] 
        public int Id { get; set; }

        [Required] 
        public int RouteId { get; set; }
        [ForeignKey(nameof(RouteId))] 
        public virtual TransportRoute Route { get; set; } = null!;

        [Required] 
        public int StopId { get; set; }
        [ForeignKey(nameof(StopId))] 
        public virtual TransportStop Stop { get; set; } = null!;

        public int SequenceOrder { get; set; } // Order of stops in the route

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] 
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
