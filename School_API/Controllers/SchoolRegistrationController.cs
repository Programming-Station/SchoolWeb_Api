using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Models.Website;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Website;
using System.Net;

namespace School_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SchoolRegistrationController : ControllerBase
    {
        private readonly ISchoolRegistrationService _schoolRegistrationService;

        public SchoolRegistrationController(ISchoolRegistrationService schoolRegistrationService)
        {
            _schoolRegistrationService = schoolRegistrationService;
        }

        /// <summary>
        /// Submit a new school registration (Public endpoint - no authentication required)
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SchoolRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get IP address and User Agent from request
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            var result = await _schoolRegistrationService.AddSchoolRegistrationAsync(model, ipAddress, userAgent);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Check if email already exists (Public endpoint)
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new APIResponse<bool>
                {
                    Success = false,
                    Message = "Email is required",
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = false
                });
            }

            var result = await _schoolRegistrationService.CheckEmailExistsAsync(email);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Check if mobile number already exists (Public endpoint)
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> CheckMobileExists([FromQuery] string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                return BadRequest(new APIResponse<bool>
                {
                    Success = false,
                    Message = "Mobile number is required",
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = false
                });
            }

            var result = await _schoolRegistrationService.CheckMobileExistsAsync(mobile);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get school registration by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer,Basic")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _schoolRegistrationService.GetSchoolRegistrationByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all school registrations with optional filters (Admin only)
        /// </summary>
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer,Basic")]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? status = null,
            [FromQuery] int? pageNumber = null,
            [FromQuery] int? pageSize = null)
        {
            // If pagination is provided, use GetAllPagedAsync
            if (pageNumber.HasValue && pageSize.HasValue && pageNumber.Value > 0 && pageSize.Value > 0)
            {
                var pagedResult = await _schoolRegistrationService.GetAllPagedAsync(pageNumber.Value, pageSize.Value, status);
                return StatusCode((int)pagedResult.StatusCode, pagedResult);
            }
            else
            {
                // Otherwise, use GetAllSchoolRegistrationsAsync without pagination
                var result = await _schoolRegistrationService.GetAllSchoolRegistrationsAsync(status, null, null);
                return StatusCode((int)result.StatusCode, result);
            }
        }

        /// <summary>
        /// Update school registration status (Admin only)
        /// </summary>
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer,Basic")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateSchoolRegistrationStatusModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _schoolRegistrationService.UpdateSchoolRegistrationStatusAsync(
                model.Id, 
                model.StatusId, 
                model.ApprovedBy, 
                model.RejectionReason);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Delete school registration (Soft delete - Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer,Basic")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _schoolRegistrationService.DeleteSchoolRegistrationAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Update school registration (Admin only)
        /// </summary>
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer,Basic")]
        public async Task<IActionResult> Update([FromBody] SchoolRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _schoolRegistrationService.UpdateSchoolRegistrationAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get school registration count by status (Admin only)
        /// </summary>
        [HttpGet("count")]
        [Authorize(AuthenticationSchemes = "Bearer,Basic")]
        public async Task<IActionResult> GetCount([FromQuery] string? status = null)
        {
            var result = await _schoolRegistrationService.GetSchoolRegistrationCountAsync(status);
            return StatusCode((int)result.StatusCode, result);
        }
    }

    public class UpdateSchoolRegistrationStatusModel
    {
        public int Id { get; set; }
        public int StatusId { get; set; } // StatusId from Status table (e.g., 2=Pending, 8=Approved, 9=Rejected)
        public string? ApprovedBy { get; set; }
        public string? RejectionReason { get; set; }
    }
}
