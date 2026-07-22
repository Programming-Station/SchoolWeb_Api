using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Models.Student;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School_DTOs.Student;

namespace School_API.Controllers
{
    public class AdmissionController : BaseController
    {
        private readonly IAdmissionService _admissionService;
        private readonly IEnrollmentService _enrollmentService;

        public AdmissionController(
            IAdmissionService admissionService,
            IEnrollmentService enrollmentService,
            ICurrentUserService currentUserService)
            : base(currentUserService)
        {
            _admissionService = admissionService;
            _enrollmentService = enrollmentService;
        }

        /// <summary>
        /// Save application draft (Supports public access, tenant ID must be passed in model)
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveDraft([FromBody] AdmissionApplicationModel model, [FromQuery] int tenantId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string user = IsAuthenticated ? UserName : "ApplicantPublic";
            var result = await _admissionService.SaveDraftAsync(model, user, tenantId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Submit completed application (Supports public access, tenant ID must be passed in model)
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Submit([FromBody] AdmissionApplicationModel model, [FromQuery] int tenantId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string user = IsAuthenticated ? UserName : "ApplicantPublic";
            var result = await _admissionService.SubmitApplicationAsync(model, user, tenantId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get application by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _admissionService.GetApplicationByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get applications list with filters and paging
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetApplicationsList(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? status = null,
            [FromQuery] int? campusId = null,
            [FromQuery] int? programId = null)
        {
            var result = await _admissionService.GetApplicationsListAsync(pageNumber, pageSize, searchTerm, status, campusId, programId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Verify checklist document status
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> VerifyDocument([FromQuery] int applicationId, [FromQuery] string documentName, [FromQuery] string status, [FromQuery] string? notes = null)
        {
            var result = await _admissionService.VerifyDocumentAsync(applicationId, documentName, status, notes, UserName);
            return Ok(result);
        }

        /// <summary>
        /// Assign custom installment fees structure
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AssignFee([FromQuery] int applicationId, [FromBody] string assignedFeesJson)
        {
            var result = await _admissionService.AssignFeeAsync(applicationId, assignedFeesJson, UserName);
            return Ok(result);
        }

        /// <summary>
        /// Update application review status
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateAdmissionApplicationStatusDto dto)
        {
            var result = await _admissionService.UpdateApplicationStatusAsync(dto, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get application status audit logs
        /// </summary>
        [HttpGet("{applicationId}")]
        public async Task<IActionResult> GetAuditLogs(int applicationId)
        {
            var result = await _admissionService.GetAuditLogsAsync(applicationId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get admission statistics for dashboard
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDashboardStats([FromQuery] int tenantId)
        {
            var result = await _admissionService.GetDashboardStatsAsync(tenantId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Finalize enrollment, generate barcodes/QRs, trigger logins, copy to active student table
        /// </summary>
        [HttpPost("{applicationId}")]
        public async Task<IActionResult> Enroll(int applicationId)
        {
            var result = await _enrollmentService.EnrollStudentAsync(applicationId, UserName);
            return Ok(result);
        }

        /// <summary>
        /// Utilities: generate barcode base64 SVG
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBarcode([FromQuery] string data)
        {
            var result = await _admissionService.GenerateBarcodeBase64Async(data);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Utilities: generate QR code base64 PNG
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetQrCode([FromQuery] string url)
        {
            var result = await _admissionService.GenerateQrCodeBase64Async(url);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
