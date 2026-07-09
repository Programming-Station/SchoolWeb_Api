using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers.Fee
{
    public class FeeCollectionController : BaseController
    {
        private readonly IFeeCollectionService _service;
        private readonly ITenantService _tenant;
        private readonly IPdfCertificateService _pdfService;

        public FeeCollectionController(ICurrentUserService currentUser, IFeeCollectionService service, ITenantService tenant, IPdfCertificateService pdfService)
            : base(currentUser)
        {
            _service = service;
            _tenant  = tenant;
            _pdfService = pdfService;
        }

        /// <summary>Generate installments for a student based on fee structure.</summary>
        [HttpPost]
        public async Task<IActionResult> GenerateInstallments([FromBody] GenerateInstallmentsRequest request)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var (success, message, installments) = await _service.GenerateInstallmentsAsync(request, UserName, schoolId);
            return success ? Ok(new { message, installments }) : BadRequest(new { message });
        }

        /// <summary>Collect fee payment for an installment — generates receipt automatically.</summary>
        [HttpPost]
        public async Task<IActionResult> Collect([FromBody] CollectFeeRequest request)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var (success, message, payment) = await _service.CollectFeeAsync(request, UserName, schoolId);
            return success ? Ok(new { message, payment }) : BadRequest(new { message });
        }

        /// <summary>Get all installments for a student.</summary>
        [HttpGet]
        public async Task<IActionResult> GetInstallments([FromQuery] int studentId)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var result = await _service.GetInstallmentsByStudentAsync(studentId, schoolId);
            return Ok(result);
        }

        /// <summary>Get only pending/overdue installments for a student.</summary>
        [HttpGet]
        public async Task<IActionResult> GetPending([FromQuery] int studentId)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var result = await _service.GetPendingByStudentAsync(studentId, schoolId);
            return Ok(result);
        }

        /// <summary>Get all overdue installments for the school (for admin follow-up).</summary>
        [HttpGet]
        public async Task<IActionResult> GetOverdue()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var result = await _service.GetOverdueAsync(schoolId);
            return Ok(result);
        }

        /// <summary>Get all payments made by a student.</summary>
        [HttpGet]
        public async Task<IActionResult> GetPaymentsByStudent([FromQuery] int studentId)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var result = await _service.GetPaymentsByStudentAsync(studentId, schoolId);
            return Ok(result);
        }

        /// <summary>Get all payments in a date range (daily collection report).</summary>
        [HttpGet]
        public async Task<IActionResult> GetPaymentsByDate([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var result = await _service.GetPaymentsByDateRangeAsync(from, to, schoolId);
            return Ok(result);
        }

        /// <summary>Get fee collection summary/dashboard stats.</summary>
        [HttpGet]
        public async Task<IActionResult> GetSummary([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var result = await _service.GetCollectionSummaryAsync(from, to, schoolId);
            return Ok(result);
        }

        /// <summary>Look up a payment by receipt number.</summary>
        [HttpGet]
        public async Task<IActionResult> GetByReceipt([FromQuery] string receiptNo)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var result = await _service.GetPaymentByReceiptAsync(receiptNo, schoolId);
            if (result == null) return NotFound(new { message = "Receipt not found." });
            return Ok(result);
        }

        /// <summary>Get pending fees report for all students in a class.</summary>
        [HttpGet]
        public async Task<IActionResult> GetPendingByClass([FromQuery] int classId)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var result = await _service.GetPendingByClassAsync(classId, schoolId);
            return Ok(result);
        }

        /// <summary>Generate a printable PDF receipt for a payment.</summary>
        [HttpGet("{paymentId}")]
        public async Task<IActionResult> DownloadReceiptPdf([FromRoute] int paymentId)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var pdfBytes = await _pdfService.GenerateFeeReceiptPdfAsync(paymentId, baseUrl);
            if (pdfBytes == null || pdfBytes.Length == 0)
                return NotFound(new { message = "Payment receipt not found or failed to generate." });
            return File(pdfBytes, "application/pdf", $"Fee_Receipt_{paymentId}.pdf");
        }
    }
}
