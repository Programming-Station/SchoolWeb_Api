using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;
namespace School.Domain.Transport
{
    public class TransportRoute : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id{get;set;}
        [Required,MaxLength(150)] public string RouteName{get;set;}=null!;
        [MaxLength(200)] public string? Description{get;set;}
        public int VehicleId{get;set;}
        [ForeignKey(nameof(VehicleId))] public virtual Vehicle Vehicle{get;set;}=null!;
        [MaxLength(20)] public string Status{get;set;}=""active"";
        public int SchoolRegistrationId{get;set;}
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration{get;set;}=null!;
    }
}
