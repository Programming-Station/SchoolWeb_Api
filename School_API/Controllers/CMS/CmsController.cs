using Microsoft.AspNetCore.Mvc;
using School.Models.CMS;
using School.Services.CMS;
using School_API.Common.Interface;

namespace School_API.Controllers.CMS
{
    
    public class CmsController : BaseController
    {
        private readonly ICmsService _cmsService;

        public CmsController(ICmsService cmsService, ICurrentUserService currentUserService)
            : base(currentUserService)
        {
            _cmsService = cmsService;
        }

        private int CurrentSchoolId => TenantId ?? 1;

        #region Pages
        [HttpGet]
        public async Task<IActionResult> GetPages([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var result = await _cmsService.GetPagesAsync(CurrentSchoolId, pageNumber, pageSize, search);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPageById(int id)
        {
            var result = await _cmsService.GetPageByIdAsync(id, CurrentSchoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetPageBySlug(string slug)
        {
            var result = await _cmsService.GetPageBySlugAsync(slug, CurrentSchoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePage([FromBody] CmsPageModel model)
        {
            var result = await _cmsService.CreatePageAsync(model, CurrentSchoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePage([FromBody] CmsPageModel model)
        {
            var result = await _cmsService.UpdatePageAsync(model, CurrentSchoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePage(int id)
        {
            var result = await _cmsService.DeletePageAsync(id, CurrentSchoolId);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Banners
        [HttpGet]
        public async Task<IActionResult> GetBanners([FromQuery] bool activeOnly = false)
        {
            var result = await _cmsService.GetBannersAsync(CurrentSchoolId, activeOnly);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBanner([FromBody] CmsBannerModel model)
        {
            var result = await _cmsService.CreateBannerAsync(model, CurrentSchoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBanner([FromBody] CmsBannerModel model)
        {
            var result = await _cmsService.UpdateBannerAsync(model, CurrentSchoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            var result = await _cmsService.DeleteBannerAsync(id, CurrentSchoolId);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Notices
        [HttpGet]
        public async Task<IActionResult> GetNotices([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? category = null)
        {
            var result = await _cmsService.GetNoticesAsync(CurrentSchoolId, pageNumber, pageSize, category);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotice([FromBody] CmsNoticeModel model)
        {
            var result = await _cmsService.CreateNoticeAsync(model, CurrentSchoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNotice([FromBody] CmsNoticeModel model)
        {
            var result = await _cmsService.UpdateNoticeAsync(model, CurrentSchoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotice(int id)
        {
            var result = await _cmsService.DeleteNoticeAsync(id, CurrentSchoolId);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}
