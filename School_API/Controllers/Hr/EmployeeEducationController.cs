using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces.Hr;
using School_API.Common.Interface;
using School_DTOs.Common;
using School_DTOs.Hr;
using System.Threading.Tasks;

namespace School_API.Controllers.Hr
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeEducationController : BaseController
    {
        private readonly IEmployeeEducationService _service;

        public EmployeeEducationController(IEmployeeEducationService service, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _service = service;
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetAllByEmployeeId(int employeeId)
        {
            var result = await _service.GetAllByEmployeeIdAsync(employeeId);
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
        public async Task<IActionResult> Create([FromBody] CreateEmployeeEducationDto model)
        {
            var result = await _service.CreateAsync(model, UserName);
            if (result.Success) return StatusCode((int)result.StatusCode, result);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeEducationDto model)
        {
            var result = await _service.UpdateAsync(model.Id, model, UserName);
            if (result.Success) return Ok(result);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id, UserName);
            if (result.Success) return Ok(result);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}