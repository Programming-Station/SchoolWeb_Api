using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Communication.Recipients
{
    [Table("RecipientTags", Schema = "Communication")]
    public class RecipientTag : BaseEntity.AuditEntity<int>, BaseEntity.ITenantEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int SchoolRegistrationId { get; set; }

        public int RecipientId { get; set; }
        [ForeignKey("RecipientId")]
        public virtual Recipient Recipient { get; set; } = null!;

        [Required, StringLength(50)]
        public string TagName { get; set; } = null!; // VIP, Defaulter etc
    }
}

