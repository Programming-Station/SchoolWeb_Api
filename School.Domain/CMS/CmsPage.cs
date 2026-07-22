using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.CMS
{
    [Table("CmsPages", Schema = "CMS")]
    public class CmsPage : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int SchoolRegistrationId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Slug { get; set; } = string.Empty;

        public string ContentHtml { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? MetaTitle { get; set; }

        [MaxLength(500)]
        public string? MetaDescription { get; set; }

        public string? BannerImageUrl { get; set; }

        public bool IsPublished { get; set; } = true;

        public int DisplayOrder { get; set; } = 0;
    }
}
