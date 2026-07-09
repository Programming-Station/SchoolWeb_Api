using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces;
using School_API.Common.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace School_API.Controllers.Fee
{
    public class ReceiptController : BaseController
    {
        private readonly IReceiptService _receiptService;

        public ReceiptController(ICurrentUserService currentUser, IReceiptService receiptService)
            : base(currentUser)
        {
            _receiptService = receiptService;
        }

        /// <summary>Generate a printable PDF receipt for a payment.</summary>
        [HttpGet("/fees/receipt/{paymentId}")]
        public async Task<IActionResult> DownloadReceipt([FromRoute] int paymentId)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var pdfBytes = await _receiptService.GenerateReceiptPdfAsync(paymentId, baseUrl);
            if (pdfBytes == null || pdfBytes.Length == 0)
                return NotFound(new { message = "Payment receipt not found or failed to generate." });
            return File(pdfBytes, "application/pdf", $"Fee_Receipt_{paymentId}.pdf");
        }
    }
}
