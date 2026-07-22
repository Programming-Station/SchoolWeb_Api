using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hostel
{
    [Table("Buildings", Schema = "Hostel")]
    public class Building : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int HostelId { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;

        public int NumberOfFloors { get; set; }

        public int ConstructionYear { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "active"; // active, inactive

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(HostelId))]
        public virtual Hostel Hostel { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
