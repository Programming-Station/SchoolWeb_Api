using School.Models.Faculty;
using School.Services.Interfaces;
using School_API.Common.Interface;
using Microsoft.AspNetCore.Mvc;

namespace School_API.Controllers
{
    public class FacultyController : BaseController
    {
        private readonly IFacultyService _facultyService;

        public FacultyController(IFacultyService facultyService, ICurrentUserService currentUser) : base(currentUser)
        {
            _facultyService = facultyService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFaculty([FromBody] FacultyModel model)
        {
            model.CreatedBy = UserName;
            var result = await _facultyService.AddFacultyAsync(model);

            if (result.Success)
                return StatusCode((int)result.StatusCode, result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetFacultyById(int id)
        {
            var result = await _facultyService.GetFacultyByIdAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFaculties()
        {
            var result = await _facultyService.GetAllFacultiesAsync();

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFaculty([FromBody] FacultyModel model)
        {
            model.UpdatedBy = UserName;
            var result = await _facultyService.UpdateFacultyAsync(model);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFaculty(int id)
        {
            var result = await _facultyService.DeleteFacultyAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch]
        public async Task<IActionResult> ToggleFacultyStatus(int id)
        {
            var result = await _facultyService.ToggleFacultyStatusAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}

