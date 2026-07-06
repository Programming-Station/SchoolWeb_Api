using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School_DTOs.Hr;
using School_DTOs.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School_API.Controllers.Hr
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(
            IEmployeeService employeeService,
            ICurrentUserService currentUserService) : base(currentUserService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto model)
        {
            var result = await _employeeService.CreateEmployeeAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeDto model)
        {
            var result = await _employeeService.UpdateEmployeeAsync(model, UserName);
            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var result = await _employeeService.GetEmployeeByIdAsync(id);
            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees([FromQuery] PaginationFilterDto filter)
        {
            var result = await _employeeService.GetAllEmployeesAsync(filter);
            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id, UserName);
            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleEmployeeStatus(int id)
        {
            var result = await _employeeService.ToggleEmployeeStatusAsync(id, UserName);
            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("bulk-delete")]
        public async Task<IActionResult> BulkDelete([FromBody] IEnumerable<int> ids)
        {
            var result = await _employeeService.BulkDeleteAsync(ids, UserName);
            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("bulk-restore")]
        public async Task<IActionResult> BulkRestore([FromBody] IEnumerable<int> ids)
        {
            var result = await _employeeService.BulkRestoreAsync(ids, UserName);
            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch("bulk-status")]
        public async Task<IActionResult> BulkStatusUpdate([FromBody] IEnumerable<int> ids, [FromQuery] string status)
        {
            var result = await _employeeService.BulkStatusUpdateAsync(ids, status, UserName);
            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
