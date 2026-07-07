using Microsoft.AspNetCore.Mvc;
using School.Models.Email;
using School.Services.Interfaces.Email;
using School_API.Common.Interface;

namespace School_API.Controllers.Email
{
    public class EmailServerSettingController : BaseController
    {
        private readonly IEmailServerSettingService _service;

        public EmailServerSettingController(IEmailServerSettingService service, ICurrentUserService currentUser)
            : base(currentUser)
        {
            _service = service;
        }

        /// <summary>Get all SMTP settings for a school</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int schoolId)
        {
            var result = await _service.GetAllBySchoolIdAsync(schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Get single SMTP setting by ID</summary>
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Create a new SMTP server setting</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmailServerSettingModel model)
        {
            model.CreatedBy = UserName;
            var result = await _service.AddAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Update existing SMTP setting (does NOT update password)</summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EmailServerSettingModel model)
        {
            model.UpdatedBy = UserName;
            var result = await _service.UpdateAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Update SMTP password separately (encrypted before saving)</summary>
        [HttpPatch]
        public async Task<IActionResult> UpdatePassword([FromQuery] int id, [FromBody] string newPassword)
        {
            var result = await _service.UpdatePasswordAsync(id, newPassword, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Soft delete SMTP setting</summary>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _service.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Toggle IsActive status of SMTP setting</summary>
        [HttpPatch]
        public async Task<IActionResult> ToggleStatus([FromQuery] int id)
        {
            var result = await _service.ToggleStatusAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
