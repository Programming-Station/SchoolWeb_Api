using System.Net;
using Microsoft.EntityFrameworkCore;
using School.Domain.CMS;
using School.Infrastructure;
using School.Models.CMS;
using School_DTOs;
using School_DTOs.CMS;

namespace School.Services.CMS
{
    public interface ICmsService
    {
        // Pages
        Task<PagedResponse<CmsPageDto>> GetPagesAsync(int schoolId, int pageNumber = 1, int pageSize = 10, string? search = null);
        Task<APIResponse<CmsPageDto>> GetPageByIdAsync(int id, int schoolId);
        Task<APIResponse<CmsPageDto>> GetPageBySlugAsync(string slug, int schoolId);
        Task<APIResponse<CmsPageDto>> CreatePageAsync(CmsPageModel model, int schoolId);
        Task<APIResponse<CmsPageDto>> UpdatePageAsync(CmsPageModel model, int schoolId);
        Task<APIResponse> DeletePageAsync(int id, int schoolId);

        // Banners
        Task<APIResponse<List<CmsBannerDto>>> GetBannersAsync(int schoolId, bool activeOnly = false);
        Task<APIResponse<CmsBannerDto>> CreateBannerAsync(CmsBannerModel model, int schoolId);
        Task<APIResponse<CmsBannerDto>> UpdateBannerAsync(CmsBannerModel model, int schoolId);
        Task<APIResponse> DeleteBannerAsync(int id, int schoolId);

        // Notices
        Task<PagedResponse<CmsNoticeDto>> GetNoticesAsync(int schoolId, int pageNumber = 1, int pageSize = 10, string? category = null);
        Task<APIResponse<CmsNoticeDto>> CreateNoticeAsync(CmsNoticeModel model, int schoolId);
        Task<APIResponse<CmsNoticeDto>> UpdateNoticeAsync(CmsNoticeModel model, int schoolId);
        Task<APIResponse> DeleteNoticeAsync(int id, int schoolId);
    }

    public class CmsService : ICmsService
    {
        private readonly SchoolDbContext _db;

        public CmsService(SchoolDbContext db)
        {
            _db = db;
        }

        #region Pages
        public async Task<PagedResponse<CmsPageDto>> GetPagesAsync(int schoolId, int pageNumber = 1, int pageSize = 10, string? search = null)
        {
            var query = _db.CmsPages.Where(p => p.SchoolRegistrationId == schoolId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(p => p.Title.ToLower().Contains(search) || p.Slug.ToLower().Contains(search));
            }

            var total = await query.CountAsync();
            var items = await query
                .OrderBy(p => p.DisplayOrder)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new CmsPageDto
                {
                    Id = p.Id,
                    SchoolRegistrationId = p.SchoolRegistrationId,
                    Title = p.Title,
                    Slug = p.Slug,
                    ContentHtml = p.ContentHtml,
                    MetaTitle = p.MetaTitle,
                    MetaDescription = p.MetaDescription,
                    BannerImageUrl = p.BannerImageUrl,
                    IsPublished = p.IsPublished,
                    DisplayOrder = p.DisplayOrder
                })
                .ToListAsync();

            return new PagedResponse<CmsPageDto>
            {
                Data = items,
                TotalRecords = total,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Success = true,
                Message = "Pages retrieved.",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<CmsPageDto>> GetPageByIdAsync(int id, int schoolId)
        {
            var p = await _db.CmsPages.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId);
            if (p == null) return new APIResponse<CmsPageDto> { Success = false, Message = "Page not found.", StatusCode = HttpStatusCode.NotFound };

            return new APIResponse<CmsPageDto>
            {
                Data = MapPageDto(p),
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<CmsPageDto>> GetPageBySlugAsync(string slug, int schoolId)
        {
            var p = await _db.CmsPages.FirstOrDefaultAsync(x => x.Slug.ToLower() == slug.ToLower() && x.SchoolRegistrationId == schoolId && x.IsPublished);
            if (p == null) return new APIResponse<CmsPageDto> { Success = false, Message = "Page not found.", StatusCode = HttpStatusCode.NotFound };

            return new APIResponse<CmsPageDto>
            {
                Data = MapPageDto(p),
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<CmsPageDto>> CreatePageAsync(CmsPageModel model, int schoolId)
        {
            var entity = new CmsPage
            {
                SchoolRegistrationId = schoolId,
                Title = model.Title,
                Slug = model.Slug.ToLower().Replace(" ", "-"),
                ContentHtml = model.ContentHtml,
                MetaTitle = model.MetaTitle,
                MetaDescription = model.MetaDescription,
                BannerImageUrl = model.BannerImageUrl,
                IsPublished = model.IsPublished,
                DisplayOrder = model.DisplayOrder
            };

            await _db.CmsPages.AddAsync(entity);
            await _db.SaveChangesAsync();

            return new APIResponse<CmsPageDto>
            {
                Data = MapPageDto(entity),
                Success = true,
                Message = "CMS Page created.",
                StatusCode = HttpStatusCode.Created
            };
        }

        public async Task<APIResponse<CmsPageDto>> UpdatePageAsync(CmsPageModel model, int schoolId)
        {
            var entity = await _db.CmsPages.FirstOrDefaultAsync(x => x.Id == model.Id && x.SchoolRegistrationId == schoolId);
            if (entity == null) return new APIResponse<CmsPageDto> { Success = false, Message = "Page not found.", StatusCode = HttpStatusCode.NotFound };

            entity.Title = model.Title;
            entity.Slug = model.Slug.ToLower().Replace(" ", "-");
            entity.ContentHtml = model.ContentHtml;
            entity.MetaTitle = model.MetaTitle;
            entity.MetaDescription = model.MetaDescription;
            entity.BannerImageUrl = model.BannerImageUrl;
            entity.IsPublished = model.IsPublished;
            entity.DisplayOrder = model.DisplayOrder;

            await _db.SaveChangesAsync();

            return new APIResponse<CmsPageDto>
            {
                Data = MapPageDto(entity),
                Success = true,
                Message = "CMS Page updated.",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse> DeletePageAsync(int id, int schoolId)
        {
            var entity = await _db.CmsPages.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId);
            if (entity == null) return new APIResponse { Success = false, Message = "Page not found.", StatusCode = HttpStatusCode.NotFound };

            _db.CmsPages.Remove(entity);
            await _db.SaveChangesAsync();
            return new APIResponse { Success = true, Message = "CMS Page deleted.", StatusCode = HttpStatusCode.OK };
        }
        #endregion

        #region Banners
        public async Task<APIResponse<List<CmsBannerDto>>> GetBannersAsync(int schoolId, bool activeOnly = false)
        {
            var query = _db.CmsBanners.Where(b => b.SchoolRegistrationId == schoolId).AsQueryable();
            if (activeOnly) query = query.Where(b => b.IsActive);

            var list = await query.OrderBy(b => b.DisplayOrder)
                .Select(b => new CmsBannerDto
                {
                    Id = b.Id,
                    SchoolRegistrationId = b.SchoolRegistrationId,
                    Title = b.Title,
                    Subtitle = b.Subtitle,
                    ImageUrl = b.ImageUrl,
                    ButtonText = b.ButtonText,
                    ButtonUrl = b.ButtonUrl,
                    DisplayOrder = b.DisplayOrder,
                    IsActive = b.IsActive
                }).ToListAsync();

            return new APIResponse<List<CmsBannerDto>>
            {
                Data = list,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<CmsBannerDto>> CreateBannerAsync(CmsBannerModel model, int schoolId)
        {
            var entity = new CmsBanner
            {
                SchoolRegistrationId = schoolId,
                Title = model.Title,
                Subtitle = model.Subtitle,
                ImageUrl = model.ImageUrl,
                ButtonText = model.ButtonText,
                ButtonUrl = model.ButtonUrl,
                DisplayOrder = model.DisplayOrder,
                IsActive = model.IsActive
            };

            await _db.CmsBanners.AddAsync(entity);
            await _db.SaveChangesAsync();

            return new APIResponse<CmsBannerDto>
            {
                Data = MapBannerDto(entity),
                Success = true,
                Message = "Banner created.",
                StatusCode = HttpStatusCode.Created
            };
        }

        public async Task<APIResponse<CmsBannerDto>> UpdateBannerAsync(CmsBannerModel model, int schoolId)
        {
            var entity = await _db.CmsBanners.FirstOrDefaultAsync(b => b.Id == model.Id && b.SchoolRegistrationId == schoolId);
            if (entity == null) return new APIResponse<CmsBannerDto> { Success = false, Message = "Banner not found.", StatusCode = HttpStatusCode.NotFound };

            entity.Title = model.Title;
            entity.Subtitle = model.Subtitle;
            entity.ImageUrl = model.ImageUrl;
            entity.ButtonText = model.ButtonText;
            entity.ButtonUrl = model.ButtonUrl;
            entity.DisplayOrder = model.DisplayOrder;
            entity.IsActive = model.IsActive;

            await _db.SaveChangesAsync();
            return new APIResponse<CmsBannerDto> { Data = MapBannerDto(entity), Success = true, Message = "Banner updated.", StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse> DeleteBannerAsync(int id, int schoolId)
        {
            var entity = await _db.CmsBanners.FirstOrDefaultAsync(b => b.Id == id && b.SchoolRegistrationId == schoolId);
            if (entity == null) return new APIResponse { Success = false, Message = "Banner not found.", StatusCode = HttpStatusCode.NotFound };

            _db.CmsBanners.Remove(entity);
            await _db.SaveChangesAsync();
            return new APIResponse { Success = true, Message = "Banner deleted.", StatusCode = HttpStatusCode.OK };
        }
        #endregion

        #region Notices
        public async Task<PagedResponse<CmsNoticeDto>> GetNoticesAsync(int schoolId, int pageNumber = 1, int pageSize = 10, string? category = null)
        {
            var query = _db.CmsNotices.Where(n => n.SchoolRegistrationId == schoolId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(n => n.Category == category);
            }

            var total = await query.CountAsync();
            var items = await query.OrderByDescending(n => n.IsImportant).ThenByDescending(n => n.PublishedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new CmsNoticeDto
                {
                    Id = n.Id,
                    SchoolRegistrationId = n.SchoolRegistrationId,
                    Title = n.Title,
                    Content = n.Content,
                    Category = n.Category,
                    PublishedDate = n.PublishedDate,
                    ExpiryDate = n.ExpiryDate,
                    AttachmentUrl = n.AttachmentUrl,
                    IsImportant = n.IsImportant,
                    IsActive = n.IsActive
                }).ToListAsync();

            return new PagedResponse<CmsNoticeDto>
            {
                Data = items,
                TotalRecords = total,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<CmsNoticeDto>> CreateNoticeAsync(CmsNoticeModel model, int schoolId)
        {
            var entity = new CmsNotice
            {
                SchoolRegistrationId = schoolId,
                Title = model.Title,
                Content = model.Content,
                Category = model.Category,
                PublishedDate = model.PublishedDate,
                ExpiryDate = model.ExpiryDate,
                AttachmentUrl = model.AttachmentUrl,
                IsImportant = model.IsImportant,
                IsActive = model.IsActive
            };

            await _db.CmsNotices.AddAsync(entity);
            await _db.SaveChangesAsync();

            return new APIResponse<CmsNoticeDto> { Data = MapNoticeDto(entity), Success = true, Message = "Notice created.", StatusCode = HttpStatusCode.Created };
        }

        public async Task<APIResponse<CmsNoticeDto>> UpdateNoticeAsync(CmsNoticeModel model, int schoolId)
        {
            var entity = await _db.CmsNotices.FirstOrDefaultAsync(n => n.Id == model.Id && n.SchoolRegistrationId == schoolId);
            if (entity == null) return new APIResponse<CmsNoticeDto> { Success = false, Message = "Notice not found.", StatusCode = HttpStatusCode.NotFound };

            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.Category = model.Category;
            entity.PublishedDate = model.PublishedDate;
            entity.ExpiryDate = model.ExpiryDate;
            entity.AttachmentUrl = model.AttachmentUrl;
            entity.IsImportant = model.IsImportant;
            entity.IsActive = model.IsActive;

            await _db.SaveChangesAsync();
            return new APIResponse<CmsNoticeDto> { Data = MapNoticeDto(entity), Success = true, Message = "Notice updated.", StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse> DeleteNoticeAsync(int id, int schoolId)
        {
            var entity = await _db.CmsNotices.FirstOrDefaultAsync(n => n.Id == id && n.SchoolRegistrationId == schoolId);
            if (entity == null) return new APIResponse { Success = false, Message = "Notice not found.", StatusCode = HttpStatusCode.NotFound };

            _db.CmsNotices.Remove(entity);
            await _db.SaveChangesAsync();
            return new APIResponse { Success = true, Message = "Notice deleted.", StatusCode = HttpStatusCode.OK };
        }
        #endregion

        private static CmsPageDto MapPageDto(CmsPage p) => new()
        {
            Id = p.Id,
            SchoolRegistrationId = p.SchoolRegistrationId,
            Title = p.Title,
            Slug = p.Slug,
            ContentHtml = p.ContentHtml,
            MetaTitle = p.MetaTitle,
            MetaDescription = p.MetaDescription,
            BannerImageUrl = p.BannerImageUrl,
            IsPublished = p.IsPublished,
            DisplayOrder = p.DisplayOrder
        };

        private static CmsBannerDto MapBannerDto(CmsBanner b) => new()
        {
            Id = b.Id,
            SchoolRegistrationId = b.SchoolRegistrationId,
            Title = b.Title,
            Subtitle = b.Subtitle,
            ImageUrl = b.ImageUrl,
            ButtonText = b.ButtonText,
            ButtonUrl = b.ButtonUrl,
            DisplayOrder = b.DisplayOrder,
            IsActive = b.IsActive
        };

        private static CmsNoticeDto MapNoticeDto(CmsNotice n) => new()
        {
            Id = n.Id,
            SchoolRegistrationId = n.SchoolRegistrationId,
            Title = n.Title,
            Content = n.Content,
            Category = n.Category,
            PublishedDate = n.PublishedDate,
            ExpiryDate = n.ExpiryDate,
            AttachmentUrl = n.AttachmentUrl,
            IsImportant = n.IsImportant,
            IsActive = n.IsActive
        };
    }
}
