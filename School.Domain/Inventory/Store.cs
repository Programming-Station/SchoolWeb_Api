using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Inventory
{
    [Table("Stores", Schema = "Inventory")]
    public class Store : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(50)]
        public string StoreType { get; set; } = "General"; // School, Hostel, Transport, Library, Laboratory, Sports, Medical, Maintenance, Department

        [MaxLength(100)]
        public string? ContactPerson { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
