using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.CMS
{
    [Table("CmsBanners", Schema = "CMS")]
    public class CmsBanner : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int SchoolRegistrationId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Subtitle { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ButtonText { get; set; }

        [MaxLength(500)]
        public string? ButtonUrl { get; set; }

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}
