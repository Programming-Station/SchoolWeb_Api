using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Communication.Recipients
{
    [Table("RecipientGroups", Schema = "Communication")]
    public class RecipientGroup : BaseEntity.AuditEntity<int>, BaseEntity.ITenantEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int SchoolRegistrationId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsDynamic { get; set; } = false;
        [StringLength(2000)]
        public string? DynamicFilterCriteria { get; set; } // JSON defining the filter

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual RecipientCategory? Category { get; set; }

        public virtual ICollection<RecipientGroupMember> Members { get; set; } = new List<RecipientGroupMember>();
    }
}
