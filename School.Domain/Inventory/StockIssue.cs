using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Inventory
{
    [Table("StockIssues", Schema = "Inventory")]
    public class StockIssue : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string IssueNo { get; set; } = null!; // ISS-YYYY-XXXXX

        public int ItemId { get; set; }
        [ForeignKey(nameof(ItemId))]
        public virtual InventoryItem Item { get; set; } = null!;

        [Required, Column(TypeName = "decimal(12,2)")]
        public decimal Quantity { get; set; }

        [Required, MaxLength(50)]
        public string IssuedToType { get; set; } = null!; // Student, Employee, Department, Hostel, Library, Transport, Lab, Sports, Maintenance

        [Required]
        public int IssuedToId { get; set; } // Reference to target entity ID (e.g. StudentId, EmployeeId)

        [Required]
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;

        public bool Returnable { get; set; } = false;

        public DateTime? DueDate { get; set; }

        [Required, MaxLength(30)]
        public string Status { get; set; } = "Issued"; // Issued, Returned, Overdue, Damaged, Lost

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
