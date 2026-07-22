using Microsoft.AspNetCore.Mvc;
using School.Services.School.ISchoolServices;
using School_API.Common.Interface;
using School_DTOs;
using School_DTOs.School;

namespace School_API.Controllers.School
{
    public class SchoolProfileSettingsController : BaseController
    {
        private readonly ISchoolProfileSettingService _profileSettingService;

        public SchoolProfileSettingsController(ISchoolProfileSettingService profileSettingService, ICurrentUserService currentUserService)
            : base(currentUserService)
        {
            _profileSettingService = profileSettingService;
        }

        [HttpGet("my-profile")]
        public async Task<ActionResult<APIResponse<SchoolProfileSettingDto>>> GetMyProfileSettings()
        {
            var response = await _profileSettingService.GetMyProfileSettingsAsync();
            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<ActionResult<APIResponse<SchoolProfileSettingDto>>> UpdateMyProfileSettings([FromBody] SchoolProfileSettingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _profileSettingService.UpdateMyProfileSettingsAsync(model);
            return Ok(response);
        }
    }
}


