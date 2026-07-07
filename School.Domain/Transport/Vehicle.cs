using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;
namespace School.Domain.Transport
{
    public class Vehicle : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id{get;set;}
        [Required,MaxLength(100)] public string Name{get;set;}=null!;
        [MaxLength(30)] public string? RegistrationNumber{get;set;}
        [MaxLength(50)] public string? DriverName{get;set;}
        [MaxLength(20)] public string? DriverPhone{get;set;}
        public int Capacity{get;set;}
        [MaxLength(20)] public string Status{get;set;}="active";
        public int SchoolRegistrationId{get;set;}
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration{get;set;}=null!;
    }
}
