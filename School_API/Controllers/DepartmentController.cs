using School.Models.Department;
using School.Services.Interfaces;
using School_API.Common.Interface;
using Microsoft.AspNetCore.Mvc;

namespace School_API.Controllers
{
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService, ICurrentUserService currentUser) : base(currentUser)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentModel model)
        {
            model.CreatedBy = UserName;
            var result = await _departmentService.AddDepartmentAsync(model);

            if (result.Success)
                return StatusCode((int)result.StatusCode, result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var result = await _departmentService.GetDepartmentByIdAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var result = await _departmentService.GetAllDepartmentsAsync();

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartmentsByFacultyId([FromQuery] int facultyId)
        {
            var result = await _departmentService.GetDepartmentsByFacultyIdAsync(facultyId);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentModel model)
        {
            model.UpdatedBy = UserName;
            var result = await _departmentService.UpdateDepartmentAsync(model);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var result = await _departmentService.DeleteDepartmentAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch]
        public async Task<IActionResult> ToggleDepartmentStatus(int id)
        {
            var result = await _departmentService.ToggleDepartmentStatusAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}

