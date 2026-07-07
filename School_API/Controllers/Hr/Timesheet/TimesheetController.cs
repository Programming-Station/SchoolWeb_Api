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
    public class TimesheetController : BaseController
    {
        private readonly ITimesheetService _service;

        public TimesheetController(ITimesheetService service, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _service = service;
        }

        [HttpGet("parent/{foreignKeyId}")]
        public async Task<IActionResult> GetAllByForeignKeyId(int foreignKeyId)
        {
            var result = await _service.GetAllByEmployeeIdAsync(foreignKeyId);
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
        public async Task<IActionResult> Create([FromBody] CreateTimesheetDto model)
        {
            var result = await _service.CreateAsync(model, UserName);
            if (result.Success) return StatusCode((int)result.StatusCode, result);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTimesheetDto model)
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

        [HttpPut("{id}/submit")]
        public async Task<IActionResult> Submit(int id)
        {
            var result = await _service.SubmitTimesheetAsync(id, UserName);
            if (result.Success) return Ok(result);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(int id, [FromQuery] int approverEmployeeId)
        {
            var result = await _service.ApproveTimesheetAsync(id, approverEmployeeId, UserName);
            if (result.Success) return Ok(result);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(int id, [FromQuery] int approverEmployeeId, [FromQuery] string reason)
        {
            var result = await _service.RejectTimesheetAsync(id, approverEmployeeId, reason, UserName);
            if (result.Success) return Ok(result);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}