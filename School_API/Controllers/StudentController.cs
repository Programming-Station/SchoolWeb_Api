using School.Models.Student;
using School.Services.Interfaces;
using School_API.Common.Interface;
using Microsoft.AspNetCore.Mvc;

namespace School_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;

        public StudentController(
            IStudentService studentService,
            ICurrentUserService currentUserService)
            : base(currentUserService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Create a new student (Registration/Admission)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.CreatedBy = UserName;
            var result = await _studentService.CreateStudentAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get student by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _studentService.GetStudentByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get student by Student ID (e.g., STU24001)
        /// </summary>
        [HttpGet("studentid/{studentId}")]
        public async Task<IActionResult> GetByStudentId(string studentId)
        {
            var result = await _studentService.GetStudentByStudentIdAsync(studentId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all students with pagination and filters
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? status = null,
            [FromQuery] string? classFilter = null)
        {
            var result = await _studentService.GetAllStudentsAsync(pageNumber, pageSize, searchTerm, status, classFilter);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Update student information
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] StudentModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.UpdatedBy = UserName;
            var result = await _studentService.UpdateStudentAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Delete student (Soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Generate a new Student ID
        /// </summary>
        [HttpGet("generate-id")]
        public async Task<IActionResult> GenerateStudentId()
        {
            var result = await _studentService.GenerateStudentIdAsync();
            return StatusCode((int)result.StatusCode, result);
        }
    }
}

