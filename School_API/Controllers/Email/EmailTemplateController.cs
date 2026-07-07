using Microsoft.AspNetCore.Mvc;
using School.Models.Email;
using School.Services.Interfaces.Email;
using School_API.Common.Interface;

namespace School_API.Controllers.Email
{
    public class EmailTemplateController : BaseController
    {
        private readonly IEmailTemplateService _service;

        public EmailTemplateController(IEmailTemplateService service, ICurrentUserService currentUser)
            : base(currentUser)
        {
            _service = service;
        }

        /// <summary>Get all email templates for a school</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int schoolId)
        {
            var result = await _service.GetAllBySchoolIdAsync(schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Get single template by ID (includes full BodyHtml for preview)</summary>
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Create a new email template</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmailTemplateModel model)
        {
            model.CreatedBy = UserName;
            var result = await _service.AddAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Update existing email template</summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EmailTemplateModel model)
        {
            model.UpdatedBy = UserName;
            var result = await _service.UpdateAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Soft delete email template</summary>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _service.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Toggle IsActive status of email template</summary>
        [HttpPatch]
        public async Task<IActionResult> ToggleStatus([FromQuery] int id)
        {
            var result = await _service.ToggleStatusAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
