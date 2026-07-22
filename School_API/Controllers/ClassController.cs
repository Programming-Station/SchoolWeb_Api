using Microsoft.AspNetCore.Mvc;
using School.Models.Class;
using School.Services.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers
{
    public class ClassController : BaseController
    {
        private readonly IClassService _classService;

        public ClassController(
            IClassService classService,
            ICurrentUserService currentUser) : base(currentUser)
        {
            _classService = classService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClass([FromBody] ClassModel model)
        {
            model.CreatedBy = UserName;
            var result = await _classService.AddClassAsync(model);

            if (result.Success)
                return StatusCode((int)result.StatusCode, result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetClassById(int id)
        {
            var result = await _classService.GetClassByIdAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClasses()
        {
            var result = await _classService.GetAllClassesAsync();

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetClassesByCourseId(int courseId)
        {
            var result = await _classService.GetClassesByCourseIdAsync(courseId);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClass([FromBody] ClassModel model)
        {
            model.UpdatedBy = UserName;
            var result = await _classService.UpdateClassAsync(model);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var result = await _classService.DeleteClassAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch]
        public async Task<IActionResult> ToggleClassStatus(int id)
        {
            var result = await _classService.ToggleClassStatusAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateClassStrength(int id, [FromBody] int newStrength)
        {
            var result = await _classService.UpdateClassStrengthAsync(id, newStrength);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}

