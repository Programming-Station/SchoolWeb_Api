using Microsoft.AspNetCore.Mvc;
using School.Models.Course;
using School.Services.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers
{
    public class CourseController : BaseController
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService, ICurrentUserService currentUser) : base(currentUser)
        {
            _courseService = courseService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseModel model)
        {
            model.CreatedBy = UserName;
            var result = await _courseService.AddCourseAsync(model);

            if (result.Success)
                return StatusCode((int)result.StatusCode, result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var result = await _courseService.GetCourseByIdAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses([FromQuery] int? courseType = null)
        {
            var result = await _courseService.GetAllCoursesAsync(courseType);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourse([FromBody] CourseModel model)
        {
            model.UpdatedBy = UserName;
            var result = await _courseService.UpdateCourseAsync(model);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await _courseService.DeleteCourseAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch]
        public async Task<IActionResult> ToggleCourseStatus(int id)
        {
            var result = await _courseService.ToggleCourseStatusAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}

