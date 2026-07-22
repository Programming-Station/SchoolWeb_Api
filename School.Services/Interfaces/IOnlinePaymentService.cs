using School.Services.Fee;

namespace School.Services.Interfaces
{
    public interface IOnlinePaymentService
    {
        Task<(bool Success, string Message, PaymentGatewayDto? Gateway)> ConfigureGatewayAsync(PaymentGatewayDto dto, string updatedBy, int schoolId);
        Task<PaymentGatewayDto?> GetActiveGatewayAsync(int schoolId);
        Task<IEnumerable<PaymentGatewayDto>> GetAllGatewaysAsync(int schoolId);
        Task<(bool Success, string Message, PaymentOrderResponseDto? Order)> CreateOrderAsync(int studentId, int feeInstallmentId, int schoolId);
        Task<(bool Success, string Message)> ConfirmPaymentAsync(string orderId, string paymentId, string signature, int schoolId);
        Task<(bool Success, string Message)> ProcessWebhookAsync(string gatewayName, string payload, string signatureHeader, int schoolId);
    }
}
