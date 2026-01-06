using School.Models.AcademicYear;
using School.Services.Interfaces;
using School_API.Common.Interface;
using Microsoft.AspNetCore.Mvc;

namespace School_API.Controllers
{
    public class AcademicYearController : BaseController
    {
        private readonly IAcademicYearService _academicYearService;

        public AcademicYearController(IAcademicYearService academicYearService, ICurrentUserService currentUser) : base(currentUser)
        {
            _academicYearService = academicYearService;
        }

        /// <summary>
        /// Create a new academic year
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAcademicYear([FromBody] AcademicYearModel model)
        {
            model.CreatedBy = UserName;
            var result = await _academicYearService.AddAcademicYearAsync(model);

            if (result.Success)
                return StatusCode((int)result.StatusCode, result);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get academic year by ID
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAcademicYearById(int id)
        {
            var result = await _academicYearService.GetAcademicYearByIdAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all academic years
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAcademicYears([FromQuery] bool? isActive = null)
        {
            var result = await _academicYearService.GetAllAcademicYearsAsync(isActive);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get current academic year
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCurrentAcademicYear()
        {
            var result = await _academicYearService.GetCurrentAcademicYearAsync();

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Update academic year
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateAcademicYear([FromBody] AcademicYearModel model)
        {
            model.UpdatedBy = UserName;
            var result = await _academicYearService.UpdateAcademicYearAsync(model);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Delete academic year (soft delete)
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteAcademicYear(int id)
        {
            var result = await _academicYearService.DeleteAcademicYearAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Toggle academic year active status
        /// </summary>
        [HttpPatch]
        public async Task<IActionResult> ToggleAcademicYearStatus(int id)
        {
            var result = await _academicYearService.ToggleAcademicYearStatusAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Set academic year as current
        /// </summary>
        [HttpPatch]
        public async Task<IActionResult> SetCurrentAcademicYear(int id)
        {
            var result = await _academicYearService.SetCurrentAcademicYearAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}

