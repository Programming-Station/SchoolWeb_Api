using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hostel
{
    [Table("HostelMedicalLogs", Schema = "Hostel")]
    public class HostelMedicalLog : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }

        [Required, MaxLength(1000)]
        public string IncidentDescription { get; set; } = null!;

        [MaxLength(50)]
        public string? Temperature { get; set; }

        [MaxLength(50)]
        public string? Bp { get; set; }

        public bool DoctorVisited { get; set; }

        [MaxLength(500)]
        public string? MedicinesGiven { get; set; }

        public bool IsolationRequired { get; set; }

        public int? IsolationRoomId { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = "UnderTreatment"; // UnderTreatment, Recovered, ReferredToHospital

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual global::School.Domain.Student.Student Student { get; set; } = null!;

        [ForeignKey(nameof(IsolationRoomId))]
        public virtual Room? IsolationRoom { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
