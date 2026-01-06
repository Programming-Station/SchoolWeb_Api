using School.Models.Website;
using School.Services.Interfaces;
using School_API.Common.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace School_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EnquiryController : ControllerBase
    {
        private readonly IEnquiryService _enquiryService;

        public EnquiryController(IEnquiryService enquiryService)
        {
            _enquiryService = enquiryService;
        }

        /// <summary>
        /// Submit a new enquiry (Public endpoint - no authentication required)
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SubmitEnquiry([FromBody] EnquiryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get IP address and User Agent from request
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            var result = await _enquiryService.AddEnquiryAsync(model, ipAddress, userAgent);

            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get enquiry by ID (Admin only)
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer,Basic")]
        [HttpGet]
        public async Task<IActionResult> GetEnquiryById(int id)
        {
            var result = await _enquiryService.GetEnquiryByIdAsync(id);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all enquiries with optional filters (Admin only)
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer,Basic")]
        [HttpGet]
        public async Task<IActionResult> GetAllEnquiries(
            [FromQuery] int? statusId = null,
            [FromQuery] int? pageNumber = null,
            [FromQuery] int? pageSize = null)
        {
            var result = await _enquiryService.GetAllEnquiriesAsync(statusId, pageNumber, pageSize);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Update enquiry status and add admin reply (Admin only)
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer,Basic")]
        [HttpPut]
        public async Task<IActionResult> UpdateEnquiryStatus(
            [FromBody] UpdateEnquiryStatusModel model,
            [FromServices] ICurrentUserService currentUserService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _enquiryService.UpdateEnquiryStatusAsync(
                model.Id, 
                model.StatusId, 
                model.AdminReply, 
                currentUserService.UserName);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Delete enquiry (Admin only)
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer,Basic")]
        [HttpDelete]
        public async Task<IActionResult> DeleteEnquiry(int id)
        {
            var result = await _enquiryService.DeleteEnquiryAsync(id);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get enquiry count by status (Admin only)
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer,Basic")]
        [HttpGet]
        public async Task<IActionResult> GetEnquiryCount([FromQuery] int? statusId = null)
        {
            var result = await _enquiryService.GetEnquiryCountAsync(statusId);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode((int)result.StatusCode, result);
        }
    }

    // Model for updating enquiry status
    public class UpdateEnquiryStatusModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int StatusId { get; set; } // Status ID from Status table

        [MaxLength(2000)]
        public string? AdminReply { get; set; }
    }
}

