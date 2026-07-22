using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Fee;
using School.Services.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers.Fee
{
    public class PaymentController : BaseController
    {
        private readonly IOnlinePaymentService _service;
        private readonly ITenantService _tenant;

        public PaymentController(
            ICurrentUserService currentUser,
            IOnlinePaymentService service,
            ITenantService tenant) : base(currentUser)
        {
            _service = service;
            _tenant = tenant;
        }

        [HttpPost]
        public async Task<IActionResult> ConfigureGateway([FromBody] PaymentGatewayDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var (success, message, gateway) = await _service.ConfigureGatewayAsync(dto, UserName, schoolId);
            if (!success) return BadRequest(new { message });
            return Ok(new { success, message, gateway });
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveGateway()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var gateway = await _service.GetActiveGatewayAsync(schoolId);
            if (gateway == null) return NotFound(new { message = "No active gateway configured." });
            return Ok(gateway);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGateways()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var gateways = await _service.GetAllGatewaysAsync(schoolId);
            return Ok(gateways);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromQuery] int studentId, [FromQuery] int feeInstallmentId)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var (success, message, order) = await _service.CreateOrderAsync(studentId, feeInstallmentId, schoolId);
            if (!success) return BadRequest(new { message });
            return Ok(new { success, message, order });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPayment([FromQuery] string orderId, [FromQuery] string paymentId, [FromQuery] string signature)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var (success, message) = await _service.ConfirmPaymentAsync(orderId, paymentId, signature, schoolId);
            if (!success) return BadRequest(new { message });
            return Ok(new { success, message });
        }

        [AllowAnonymous]
        [HttpPost("{gatewayName}")]
        public async Task<IActionResult> Webhook(string gatewayName, [FromQuery] int schoolId)
        {
            string payload;
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                payload = await reader.ReadToEndAsync();
            }

            var signatureHeader = Request.Headers["X-Razorpay-Signature"].ToString();
            if (string.IsNullOrEmpty(signatureHeader))
            {
                signatureHeader = Request.Headers["Stripe-Signature"].ToString();
            }

            var (success, message) = await _service.ProcessWebhookAsync(gatewayName, payload, signatureHeader, schoolId);
            if (!success) return BadRequest(new { message });
            return Ok(new { success, message });
        }
    }
}
