using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Inventory
{
    [Table("WarehouseBins", Schema = "Inventory")]
    public class WarehouseBin : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int WarehouseId { get; set; }
        [ForeignKey(nameof(WarehouseId))]
        public virtual Warehouse Warehouse { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Zone { get; set; } = "Default";

        [Required, MaxLength(50)]
        public string Rack { get; set; } = "Default";

        [Required, MaxLength(50)]
        public string Shelf { get; set; } = "Default";

        [Required, MaxLength(50)]
        public string BinCode { get; set; } = null!;

        public decimal Capacity { get; set; } = 0;

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
