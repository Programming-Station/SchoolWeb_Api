using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School_API.Common.Interface;
using School.Models.Website;
using School.Services.Interfaces;
using School_DTOs;

namespace School_API.Controllers
{
    public class WebsiteController : BaseController
    {
        private readonly IWebsiteService _websiteService;
        private readonly IImageService _imageService;

        public WebsiteController(
            IWebsiteService websiteService,
            IImageService imageService,
            ICurrentUserService currentUser) : base(currentUser)
        {
            _websiteService = websiteService;
            _imageService = imageService;
        }

        #region Slider Images 
        /// <summary>
        /// Add a new slider image
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddSliderImage([FromBody] SliderImageModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.CreatedBy = UserName;
            var result = await _websiteService.AddSliderImageAsync(model);

            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all active slider images
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllSliderImages()
        {
            var result = await _websiteService.GetAllSliderImagesAsync();

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Update an existing slider image
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateSliderImage([FromBody] SliderImageModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.UpdatedBy = UserName;
            var result = await _websiteService.UpdateSliderImageAsync(model);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Delete a slider image
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteSliderImage(int id)
        {
            var result = await _websiteService.DeleteSliderImageAsync(id, UserName);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Update the display order of slider images
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateSliderOrder([FromBody] List<int> sliderIds)
        {
            if (sliderIds == null || sliderIds.Count == 0)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = "Slider IDs are required",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }

            var result = await _websiteService.UpdateSliderOrderAsync(sliderIds, UserName);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region Hero Section

        /// <summary>
        /// Get the active hero section
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetHeroSection()
        {
            var result = await _websiteService.GetHeroSectionAsync();

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Save or update hero section
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SaveHeroSection([FromBody] HeroSectionModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Id.HasValue)
            {
                model.UpdatedBy = UserName;
            }
            else
            {
                model.CreatedBy = UserName;
            }

            var result = await _websiteService.SaveHeroSectionAsync(model);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region Notice Bar

        /// <summary>
        /// Get the active notice bar
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetNoticeBar()
        {
            var result = await _websiteService.GetNoticeBarAsync();

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Save or update notice bar
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SaveNoticeBar([FromBody] NoticeBarModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Id.HasValue)
            {
                model.UpdatedBy = UserName;
            }
            else
            {
                model.CreatedBy = UserName;
            }

            var result = await _websiteService.SaveNoticeBarAsync(model);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region Welcome Section

        /// <summary>
        /// Get the active welcome section
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetWelcomeSection()
        {
            var result = await _websiteService.GetWelcomeSectionAsync();

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Save or update welcome section
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SaveWelcomeSection([FromBody] WelcomeSectionModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Id.HasValue)
            {
                model.UpdatedBy = UserName;
            }
            else
            {
                model.CreatedBy = UserName;
            }

            var result = await _websiteService.SaveWelcomeSectionAsync(model);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region About Section (Home Page)

        /// <summary>
        /// Get the active about section
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAboutSection()
        {
            var result = await _websiteService.GetAboutSectionAsync();

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Save or update about section
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SaveAboutSection([FromBody] AboutSectionModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Id.HasValue)
            {
                model.UpdatedBy = UserName;
            }
            else
            {
                model.CreatedBy = UserName;
            }

            var result = await _websiteService.SaveAboutSectionAsync(model);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region About Page

        /// <summary>
        /// Get the active about page content
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAboutPage()
        {
            var result = await _websiteService.GetAboutPageAsync();

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Save or update about page
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SaveAboutPage([FromBody] AboutPageModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Id.HasValue)
            {
                model.UpdatedBy = UserName;
            }
            else
            {
                model.CreatedBy = UserName;
            }

            var result = await _websiteService.SaveAboutPageAsync(model);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region Contact Info

        /// <summary>
        /// Get the active contact information
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetContactInfo()
        {
            var result = await _websiteService.GetContactInfoAsync();

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Save or update contact information
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SaveContactInfo([FromBody] ContactInfoModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Id.HasValue)
            {
                model.UpdatedBy = UserName;
            }
            else
            {
                model.CreatedBy = UserName;
            }

            var result = await _websiteService.SaveContactInfoAsync(model);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region Gallery Images

        /// <summary>
        /// Add a new gallery image
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddGalleryImage([FromBody] GalleryImageModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.CreatedBy = UserName;
            var result = await _websiteService.AddGalleryImageAsync(model);

            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all active gallery images
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllGalleryImages()
        {
            var result = await _websiteService.GetAllGalleryImagesAsync();

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Delete a gallery image
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteGalleryImage(int id)
        {
            var result = await _websiteService.DeleteGalleryImageAsync(id);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region Achievements

        /// <summary>
        /// Add a new achievement
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddAchievement([FromBody] AchievementModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.CreatedBy = UserName;
            var result = await _websiteService.AddAchievementAsync(model);

            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all active achievements
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAchievements()
        {
            var result = await _websiteService.GetAllAchievementsAsync();

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Update an existing achievement
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateAchievement([FromBody] AchievementModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.UpdatedBy = UserName;
            var result = await _websiteService.UpdateAchievementAsync(model);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Delete an achievement
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteAchievement(int id)
        {
            var result = await _websiteService.DeleteAchievementAsync(id, UserName);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region Team Members

        /// <summary>
        /// Add a new team member
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddTeamMember([FromBody] TeamMemberModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.CreatedBy = UserName;
            var result = await _websiteService.AddTeamMemberAsync(model);

            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all active team members
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTeamMembers()
        {
            var result = await _websiteService.GetAllTeamMembersAsync();

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Update an existing team member
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateTeamMember([FromBody] TeamMemberModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.UpdatedBy = UserName;
            var result = await _websiteService.UpdateTeamMemberAsync(model);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Delete a team member
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteTeamMember(int id)
        {
            var result = await _websiteService.DeleteTeamMemberAsync(id, UserName);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        #endregion
    }
}
