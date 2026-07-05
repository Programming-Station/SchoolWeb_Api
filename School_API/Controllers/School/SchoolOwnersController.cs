using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Services.School.ISchoolServices;
using School_DTOs;
using School_DTOs.School;
using System.Threading.Tasks;
using School_API.Common.Interface;

namespace School_API.Controllers.School
{
    [Authorize(Roles = "Owner,Superadmin")]
    public class SchoolOwnersController : BaseController
    {
        private readonly ISchoolOwnerService _service;

        public SchoolOwnersController(ISchoolOwnerService service, ICurrentUserService currentUserService)
            : base(currentUserService)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<SchoolOwnerDto>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null, [FromQuery] bool? isActive = null)
        {
            return Ok(await _service.GetAllsAsync(pageNumber, pageSize, searchTerm, isActive));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<SchoolOwnerDto>>> GetById(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse<SchoolOwnerDto>>> Create([FromBody] SchoolOwnerModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse<SchoolOwnerDto>>> Update([FromBody] SchoolOwnerModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _service.EditAsync(model));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            return Ok(await _service.DeleteAsync(id));
        }
    }
}


