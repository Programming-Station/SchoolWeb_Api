using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Communication.Recipients
{
    [Table("RecipientGroupMembers", Schema = "Communication")]
    public class RecipientGroupMember : BaseEntity.AuditEntity<int>, BaseEntity.ITenantEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int SchoolRegistrationId { get; set; }

        public int RecipientId { get; set; }
        [ForeignKey("RecipientId")]
        public virtual Recipient Recipient { get; set; } = null!;

        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual RecipientGroup Group { get; set; } = null!;
    }
}

