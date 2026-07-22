using Microsoft.AspNetCore.Mvc;
using School.Services.School.ISchoolServices;
using School_API.Common.Interface;
using School_DTOs;
using School_DTOs.School;

namespace School_API.Controllers.School
{
    public class AffiliationBoardsController : BaseController
    {
        private readonly IAffiliationBoardService _service;

        public AffiliationBoardsController(IAffiliationBoardService service, ICurrentUserService currentUserService)
            : base(currentUserService)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<AffiliationBoardDto>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null, [FromQuery] bool? isActive = null)
        {
            return Ok(await _service.GetAllsAsync(pageNumber, pageSize, searchTerm, isActive));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<AffiliationBoardDto>>> GetById(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse<AffiliationBoardDto>>> Create([FromBody] AffiliationBoardModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse<AffiliationBoardDto>>> Update([FromBody] AffiliationBoardModel model)
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


