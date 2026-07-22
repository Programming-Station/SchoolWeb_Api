using Microsoft.AspNetCore.Mvc;
using School.Models.School;
using School.Services.School.ISchoolServices;
using School_API.Common.Interface;
using School_DTOs;
using School_DTOs.School;

namespace School_API.Controllers.School
{
    public class SchoolsController : BaseController
    {
        private readonly ISchoolService _schoolService;

        public SchoolsController(ISchoolService schoolService, ICurrentUserService currentUserService)
            : base(currentUserService)
        {
            _schoolService = schoolService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<SchoolRegistrationDto>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null, [FromQuery] bool? isActive = null)
        {
            var response = await _schoolService.GetAllsAsync(pageNumber, pageSize, searchTerm, isActive);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<SchoolRegistrationDto>>> GetById(int id)
        {
            var response = await _schoolService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse<SchoolRegistrationDto>>> Create([FromBody] SchoolRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _schoolService.AddAsync(model);
            if (response.Success)
                return CreatedAtAction(nameof(GetById), new { id = response.Data?.Id }, response);

            return BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse<SchoolRegistrationDto>>> Update([FromBody] SchoolRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _schoolService.EditAsync(model);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            var response = await _schoolService.DeleteAsync(id);
            return Ok(response);
        }
    }
}


