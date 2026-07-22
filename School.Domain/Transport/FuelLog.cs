using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Transport
{
    [Table("FuelLogs", Schema = "Transport")]
    public class FuelLog : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int VehicleId { get; set; }
        [ForeignKey(nameof(VehicleId))]
        public virtual Vehicle Vehicle { get; set; } = null!;

        public DateTime PurchaseDate { get; set; }

        [Required, MaxLength(200)]
        public string VendorName { get; set; } = null!;

        public double FuelQuantity { get; set; } // in Liters/Gallons

        [Column(TypeName = "decimal(10,2)")]
        public decimal CostPerUnit { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalCost { get; set; }

        public double OdometerReading { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
