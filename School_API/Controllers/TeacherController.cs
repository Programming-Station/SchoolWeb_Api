using School.Models.Teacher;
using School.Services.Interfaces;
using School_API.Common.Interface;
using Microsoft.AspNetCore.Mvc;

namespace School_API.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService, ICurrentUserService currentUser) : base(currentUser)
        {
            _teacherService = teacherService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacher([FromBody] TeacherModel model, [FromQuery] string? password = null)
        {
            model.CreatedBy = UserName;
            var result = await _teacherService.AddTeacherAsync(model, password);

            if (result.Success)
                return StatusCode((int)result.StatusCode, result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTeacherById(int id)
        {
            var result = await _teacherService.GetTeacherByIdAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTeacherByTeacherId([FromQuery] string teacherId)
        {
            var result = await _teacherService.GetTeacherByTeacherIdAsync(teacherId);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTeacherByEmail([FromQuery] string email)
        {
            var result = await _teacherService.GetTeacherByEmailAsync(email);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllTeachers([FromBody] TeacherFilter filter)
        {
            if (filter == null)
            {
                filter = new TeacherFilter
                {
                    PageIndex = 1,
                    PageSize = 10
                };
            }

            var result = await _teacherService.GetAllTeachersAsync(filter);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTeacher([FromBody] TeacherModel model)
        {
            model.UpdatedBy = UserName;
            var result = await _teacherService.UpdateTeacherAsync(model);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var result = await _teacherService.DeleteTeacherAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch]
        public async Task<IActionResult> ToggleTeacherStatus(int id)
        {
            var result = await _teacherService.ToggleTeacherStatusAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GenerateTeacherId()
        {
            var result = await _teacherService.GenerateTeacherIdAsync();

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}

