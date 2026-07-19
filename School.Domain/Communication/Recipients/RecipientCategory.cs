using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Communication.Recipients
{
    [Table("RecipientCategories", Schema = "Communication")]
    public class RecipientCategory : BaseEntity.AuditEntity<int>, BaseEntity.ITenantEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int SchoolRegistrationId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;
        [StringLength(255)]
        public string? Description { get; set; }
        [StringLength(50)]
        public string? ColorHex { get; set; }
    }
}
