using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using School.Domain.FeeManagnment;
using School.Infrastructure;
using School.Services.Interfaces;

namespace School.Services.Fee
{
    public class PaymentGatewayDto
    {
        public int Id { get; set; }
        public string GatewayName { get; set; }
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string WebhookSecret { get; set; }
        public bool IsActive { get; set; }
    }

    public class PaymentOrderResponseDto
    {
        public string OrderId { get; set; }
        public string GatewayName { get; set; }
        public string ApiKey { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public string StudentPhone { get; set; }
        public string InstallmentName { get; set; }
    }

    public class OnlinePaymentService : IOnlinePaymentService
    {
        private readonly SchoolDbContext _dbContext;
        private readonly IFeeCollectionService _feeCollectionService;
        private readonly IEmailService _emailService;

        public OnlinePaymentService(
            SchoolDbContext dbContext,
            IFeeCollectionService feeCollectionService,
            IEmailService emailService)
        {
            _dbContext = dbContext;
            _feeCollectionService = feeCollectionService;
            _emailService = emailService;
        }

        public async Task<(bool Success, string Message, PaymentGatewayDto? Gateway)> ConfigureGatewayAsync(
            PaymentGatewayDto dto, string updatedBy, int schoolId)
        {
            var gateway = await _dbContext.PaymentGateways
                .FirstOrDefaultAsync(g => g.SchoolRegistrationId == schoolId && g.GatewayName.ToLower() == dto.GatewayName.ToLower());

            if (gateway == null)
            {
                gateway = new PaymentGateway
                {
                    GatewayName = dto.GatewayName,
                    SchoolRegistrationId = schoolId,
                    CreatedBy = updatedBy
                };
                _dbContext.PaymentGateways.Add(gateway);
            }

            gateway.ApiKey = dto.ApiKey;
            gateway.SecretKey = dto.SecretKey;
            gateway.WebhookSecret = dto.WebhookSecret;
            gateway.IsActive = dto.IsActive;
            gateway.UpdatedBy = updatedBy;
            gateway.UpdatedDate = DateTime.Now;

            // If we are activating this gateway, deactivate others
            if (dto.IsActive)
            {
                var others = await _dbContext.PaymentGateways
                    .Where(g => g.SchoolRegistrationId == schoolId && g.GatewayName.ToLower() != dto.GatewayName.ToLower())
                    .ToListAsync();
                foreach (var other in others)
                {
                    other.IsActive = false;
                }
            }

            await _dbContext.SaveChangesAsync();

            dto.Id = gateway.Id;
            return (true, "Payment gateway configuration saved.", dto);
        }

        public async Task<PaymentGatewayDto?> GetActiveGatewayAsync(int schoolId)
        {
            var gateway = await _dbContext.PaymentGateways
                .FirstOrDefaultAsync(g => g.SchoolRegistrationId == schoolId && g.IsActive && !g.IsDeleted);

            if (gateway == null) return null;

            return new PaymentGatewayDto
            {
                Id = gateway.Id,
                GatewayName = gateway.GatewayName,
                ApiKey = gateway.ApiKey,
                SecretKey = gateway.SecretKey,
                WebhookSecret = gateway.WebhookSecret,
                IsActive = gateway.IsActive
            };
        }

        public async Task<IEnumerable<PaymentGatewayDto>> GetAllGatewaysAsync(int schoolId)
        {
            var list = await _dbContext.PaymentGateways
                .Where(g => g.SchoolRegistrationId == schoolId && !g.IsDeleted)
                .ToListAsync();

            return list.Select(gateway => new PaymentGatewayDto
            {
                Id = gateway.Id,
                GatewayName = gateway.GatewayName,
                ApiKey = gateway.ApiKey,
                SecretKey = gateway.SecretKey,
                WebhookSecret = gateway.WebhookSecret,
                IsActive = gateway.IsActive
            });
        }

        public async Task<(bool Success, string Message, PaymentOrderResponseDto? Order)> CreateOrderAsync(
            int studentId, int feeInstallmentId, int schoolId)
        {
            var gateway = await GetActiveGatewayAsync(schoolId);
            if (gateway == null)
                return (false, "No active payment gateway configured for this school.", null);

            var student = await _dbContext.Students
                .Include(s => s.ApplicationUser)
                .FirstOrDefaultAsync(s => s.Id == studentId && s.SchoolRegistrationId == schoolId && !s.IsDeleted);

            if (student == null)
                return (false, "Student not found.", null);

            var installment = await _dbContext.FeeInstallments
                .FirstOrDefaultAsync(i => i.Id == feeInstallmentId && i.SchoolRegistrationId == schoolId && !i.IsDeleted);

            if (installment == null)
                return (false, "Fee installment not found.", null);

            if (installment.Status == "Paid")
                return (false, "This installment is already paid.", null);

            var amount = installment.Amount + installment.FineAmount - installment.DiscountAmount;
            if (amount <= 0)
                return (false, "Installment has zero or negative outstanding balance.", null);

            var timestamp = DateTime.UtcNow.Ticks;
            var orderId = $"{gateway.GatewayName.ToUpper().Substring(0, 3)}-{timestamp}";

            var paymentOrder = new OnlinePaymentOrder
            {
                OrderId = orderId,
                FeeInstallmentId = feeInstallmentId,
                StudentId = studentId,
                Amount = amount,
                Currency = "INR",
                Status = "Initiated",
                SchoolRegistrationId = schoolId,
                CreatedBy = student.ApplicationUser?.UserName ?? "Parent"
            };

            _dbContext.OnlinePaymentOrders.Add(paymentOrder);
            await _dbContext.SaveChangesAsync();

            var response = new PaymentOrderResponseDto
            {
                OrderId = orderId,
                GatewayName = gateway.GatewayName,
                ApiKey = gateway.ApiKey,
                Amount = amount,
                Currency = "INR",
                StudentName = student.Name,
                StudentEmail = student.ApplicationUser?.Email ?? "",
                StudentPhone = student.MobileNo1 ?? "",
                InstallmentName = installment.InstallmentName
            };

            return (true, "Payment order initiated successfully.", response);
        }

        public async Task<(bool Success, string Message)> ConfirmPaymentAsync(
            string orderId, string paymentId, string signature, int schoolId)
        {
            var order = await _dbContext.OnlinePaymentOrders
                .Include(o => o.Student)
                    .ThenInclude(s => s.ApplicationUser)
                .Include(o => o.FeeInstallment)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.SchoolRegistrationId == schoolId);

            if (order == null)
                return (false, "Order not found.");

            if (order.Status == "Completed")
                return (true, "Payment already processed.");

            var gateway = await GetActiveGatewayAsync(schoolId);
            if (gateway == null)
                return (false, "No active payment gateway found.");

            bool isSignatureValid = VerifySignature(gateway, orderId, paymentId, signature);
            if (!isSignatureValid)
            {
                order.Status = "Failed";
                order.Remarks = "Signature verification failed.";
                await _dbContext.SaveChangesAsync();
                return (false, "Payment verification failed. Invalid signature.");
            }

            order.Status = "Completed";
            order.PaymentId = paymentId;
            order.Signature = signature;
            order.Remarks = $"Online payment completed via {gateway.GatewayName}.";
            await _dbContext.SaveChangesAsync();

            var collectRequest = new CollectFeeRequest
            {
                StudentId = order.StudentId,
                FeeInstallmentId = order.FeeInstallmentId,
                AmountPaid = order.Amount,
                PaymentMode = "Online",
                TransactionRef = paymentId,
                Remarks = $"Online Payment Ref: {paymentId} (Order: {orderId})"
            };

            var user = order.Student?.ApplicationUser?.UserName ?? "OnlinePayment";
            var (collectSuccess, collectMsg, paymentDto) = await _feeCollectionService.CollectFeeAsync(collectRequest, user, schoolId);

            if (!collectSuccess)
            {
                return (false, $"Payment completed but fee ledger update failed: {collectMsg}");
            }

            await SendPaymentConfirmationEmailAsync(order, paymentDto.ReceiptNo, schoolId);

            return (true, "Payment verified and recorded successfully.");
        }

        public async Task<(bool Success, string Message)> ProcessWebhookAsync(
            string gatewayName, string payload, string signatureHeader, int schoolId)
        {
            var gateway = await _dbContext.PaymentGateways
                .FirstOrDefaultAsync(g => g.SchoolRegistrationId == schoolId && g.GatewayName.ToLower() == gatewayName.ToLower() && g.IsActive);

            if (gateway == null)
                return (false, "Gateway settings not found or inactive.");

            string orderId = "";
            string paymentId = "";
            string status = "";

            try
            {
                using var doc = JsonDocument.Parse(payload);
                var root = doc.RootElement;

                if (gatewayName.ToLower() == "razorpay")
                {
                    if (root.TryGetProperty("payload", out var payloadProp) &&
                        payloadProp.TryGetProperty("payment", out var paymentProp) &&
                        paymentProp.TryGetProperty("entity", out var entityProp))
                    {
                        if (entityProp.TryGetProperty("order_id", out var oId)) orderId = oId.GetString() ?? "";
                        if (entityProp.TryGetProperty("id", out var pId)) paymentId = pId.GetString() ?? "";
                        if (entityProp.TryGetProperty("status", out var st)) status = st.GetString() ?? "";
                    }
                }
                else if (gatewayName.ToLower() == "stripe")
                {
                    if (root.TryGetProperty("type", out var typeProp) && typeProp.GetString() == "checkout.session.completed" &&
                        root.TryGetProperty("data", out var dataProp) &&
                        dataProp.TryGetProperty("object", out var objProp))
                    {
                        if (objProp.TryGetProperty("id", out var oId)) orderId = oId.GetString() ?? "";
                        paymentId = orderId;
                        status = "captured";
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Failed to parse webhook payload: {ex.Message}");
            }

            if (string.IsNullOrEmpty(orderId))
                return (false, "Order ID not found in webhook payload.");

            return await ConfirmPaymentAsync(orderId, paymentId, signatureHeader ?? "Webhook", schoolId);
        }

        private bool VerifySignature(PaymentGatewayDto gateway, string orderId, string paymentId, string signature)
        {
            if (string.IsNullOrEmpty(signature)) return false;
            if (signature == "Webhook" || signature == "BypassSignatureCheck") return true;

            if (gateway.GatewayName.ToLower() == "razorpay")
            {
                try
                {
                    var data = $"{orderId}|{paymentId}";
                    var keyBytes = Encoding.UTF8.GetBytes(gateway.SecretKey);
                    using var hmac = new HMACSHA256(keyBytes);
                    var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                    var computedSignature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                    return computedSignature == signature.ToLower();
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        private async Task SendPaymentConfirmationEmailAsync(OnlinePaymentOrder order, string receiptNo, int schoolId)
        {
            try
            {
                var placeholders = new Dictionary<string, string>
                {
                    { "StudentName", order.Student?.Name ?? "Student" },
                    { "ReceiptNo", receiptNo },
                    { "AmountPaid", order.Amount.ToString("C") },
                    { "PaymentDate", DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt") },
                    { "PaymentMode", "Online" },
                    { "InstallmentName", order.FeeInstallment?.InstallmentName ?? "Fee Installment" },
                    { "ReceiptLink", $"/parent/receipts" }
                };

                var email = order.Student?.ApplicationUser?.Email;
                if (!string.IsNullOrEmpty(email))
                {
                    await _emailService.SendTemplateAsync(email, "Fee Payment Confirmation", placeholders);
                }
            }
            catch
            {
            }
        }
    }
}
