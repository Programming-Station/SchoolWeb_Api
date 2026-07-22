using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hostel
{
    [Table("Hostels", Schema = "Hostel")]
    public class Hostel : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(50)]
        public string HostelType { get; set; } = null!; // Boys, Girls, Staff

        public int Capacity { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        public int BuildingCount { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "active"; // active, inactive

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        [MaxLength(100)]
        public string? GeoLocation { get; set; }

        [MaxLength(200)]
        public string? EmergencyContact { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
