using Microsoft.AspNetCore.Mvc;
using School.Services.Location;
using School_API.Common.Interface;
using School_DTOs.Common;
using School_DTOs.Location;

namespace School_API.Controllers.Location
{
    [Route("api/location/[controller]")]
    [ApiController]
    public class StateLocationController : BaseController
    {
        private readonly IStateLocationService _service;

        public StateLocationController(IStateLocationService service, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilterDto filter)
        {
            var result = await _service.GetAllAsync(filter);
            if (result.Success) return Ok(result);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result.Success) return Ok(result);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStateLocationDto model)
        {
            var result = await _service.CreateAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateStateLocationDto model)
        {
            var result = await _service.UpdateAsync(model.Id, model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await _service.ToggleStatusAsync(id, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("dropdown")]
        public async Task<IActionResult> GetDropdown([FromQuery] int? countryId = null)
        {
            var result = await _service.GetDropdownAsync(countryId);
            return Ok(result);
        }
    }
}
