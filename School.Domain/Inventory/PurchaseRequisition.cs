using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Inventory
{
    [Table("PurchaseRequisitions", Schema = "Inventory")]
    public class PurchaseRequisition : AuditEntity<int>, ITenantEntity
    {
        public PurchaseRequisition()
        {
            Items = new HashSet<PurchaseRequisitionItem>();
        }

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string RequisitionNo { get; set; } = null!; // REQ-YYYY-MM-XXXX

        [Required, MaxLength(150)]
        public string RequestedBy { get; set; } = null!;

        [Required]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        public int DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; } = null!;

        [MaxLength(1000)]
        public string? Remarks { get; set; }

        [Required, MaxLength(30)]
        public string Status { get; set; } = "Draft"; // Draft, PendingApproval, Approved, Rejected

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        public virtual ICollection<PurchaseRequisitionItem> Items { get; set; }
    }
}
