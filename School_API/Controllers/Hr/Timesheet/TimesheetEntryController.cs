using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces.Hr.Timesheet;
using School_API.Common.Interface;
using School_DTOs.Common;
using School_DTOs.Hr.Timesheet;
using System.Threading.Tasks;

namespace School_API.Controllers.Hr.Timesheet
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetEntryController : BaseController
    {
        private readonly ITimesheetEntryService _service;

        public TimesheetEntryController(ITimesheetEntryService service, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _service = service;
        }

        [HttpGet("parent/{foreignKeyId}")]
        public async Task<IActionResult> GetAllByForeignKeyId(int foreignKeyId)
        {
            var result = await _service.GetAllByTimesheetIdAsync(foreignKeyId);
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
        public async Task<IActionResult> Create([FromBody] CreateTimesheetEntryDto model)
        {
            var result = await _service.CreateAsync(model, UserName);
            if (result.Success) return StatusCode((int)result.StatusCode, result);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTimesheetEntryDto model)
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