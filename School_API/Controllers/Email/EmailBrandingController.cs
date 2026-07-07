using Microsoft.AspNetCore.Mvc;
using School.Models.Email;
using School.Services.Interfaces.Email;
using School_API.Common.Interface;

namespace School_API.Controllers.Email
{
    public class EmailBrandingController : BaseController
    {
        private readonly IEmailBrandingService _service;

        public EmailBrandingController(IEmailBrandingService service, ICurrentUserService currentUser)
            : base(currentUser)
        {
            _service = service;
        }

        /// <summary>Get branding for a school (one per school)</summary>
        [HttpGet]
        public async Task<IActionResult> GetBySchool([FromQuery] int schoolId)
        {
            var result = await _service.GetBySchoolIdAsync(schoolId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Get branding by ID</summary>
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Create school email branding (only if none exists)</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmailBrandingModel model)
        {
            model.CreatedBy = UserName;
            var result = await _service.AddAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>Update existing school email branding</summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EmailBrandingModel model)
        {
            model.UpdatedBy = UserName;
            var result = await _service.UpdateAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
