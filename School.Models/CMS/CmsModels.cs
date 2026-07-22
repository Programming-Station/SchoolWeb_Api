using System;
using System.ComponentModel.DataAnnotations;

namespace School.Models.CMS
{
    public class CmsPageModel
    {
        public int Id { get; set; }

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

    public class CmsBannerModel
    {
        public int Id { get; set; }

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

    public class CmsNoticeModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Category { get; set; } = "General";

        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ExpiryDate { get; set; }

        public string? AttachmentUrl { get; set; }

        public bool IsImportant { get; set; } = false;

        public bool IsActive { get; set; } = true;
    }
}
