using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Transport
{
    #nullable disable

    public class TransportAllocation : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required] public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))] public virtual Student.Student Student { get; set; }

        [Required] public int TransportRouteId { get; set; }
        [ForeignKey(nameof(TransportRouteId))] public virtual TransportRoute TransportRoute { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal MonthlyCharge { get; set; }

        [Required] public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [MaxLength(20)] public string Status { get; set; } = "Active"; // Active / Cancelled

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }
}
