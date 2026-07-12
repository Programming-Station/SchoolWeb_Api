using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Inventory
{
    [Table("RequestForQuotations", Schema = "Inventory")]
    public class RequestForQuotation : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string RfqNo { get; set; } = null!; // RFQ-YYYY-XXXXX

        public int RequisitionId { get; set; }
        [ForeignKey(nameof(RequisitionId))]
        public virtual PurchaseRequisition Requisition { get; set; } = null!;

        [Required]
        public DateTime RfqDate { get; set; } = DateTime.UtcNow;

        [Required, MaxLength(30)]
        public string Status { get; set; } = "Draft"; // Draft, Sent, Received, Closed

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
