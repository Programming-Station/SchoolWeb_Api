using System;

namespace School_DTOs.CMS
{
    public class CmsPageDto : BaseDto
    {
        public int Id { get; set; }
        public int SchoolRegistrationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ContentHtml { get; set; } = string.Empty;
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? BannerImageUrl { get; set; }
        public bool IsPublished { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CmsBannerDto : BaseDto
    {
        public int Id { get; set; }
        public int SchoolRegistrationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class CmsNoticeDto : BaseDto
    {
        public int Id { get; set; }
        public int SchoolRegistrationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? AttachmentUrl { get; set; }
        public bool IsImportant { get; set; }
        public bool IsActive { get; set; }
    }
}
