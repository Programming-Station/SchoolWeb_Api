using School_DTOs;
using School_DTOs.Communication;

namespace School.Services.Interfaces
{
    public interface IWhatsAppService
    {
        // Credentials Config Setup
        Task<APIResponse<WhatsAppAccountDto>> GetWhatsAppAccountAsync();
        Task<APIResponse<WhatsAppAccountDto>> SaveWhatsAppAccountAsync(WhatsAppAccountDto dto, string userName);

        // Meta WhatsApp Templates Synchronization
        Task<APIResponse<IEnumerable<WhatsAppTemplateDto>>> GetTemplatesAsync();
        Task<APIResponse<WhatsAppTemplateDto>> CreateTemplateAsync(WhatsAppTemplateDto dto, string userName);

        // Core Outbound Dispatch Engines
        Task<APIResponse<bool>> SendTextAsync(string recipientPhone, string message, string userName);
        Task<APIResponse<bool>> SendTemplateAsync(string recipientPhone, string templateName, Dictionary<string, string> variables, string userName);
        Task<APIResponse<bool>> SendMediaAsync(string recipientPhone, string mediaUrl, string mimeType, string filename, string userName);

        // Queue / Asynchronous retries
        Task<APIResponse<IEnumerable<WhatsAppQueueDto>>> GetQueuedMessagesAsync();
        Task<APIResponse<bool>> RetryFailedMessageAsync(int queueId, string userName);
        Task<APIResponse<bool>> CancelQueuedMessageAsync(int queueId, string userName);

        // History logs
        Task<APIResponse<IEnumerable<WhatsAppMessageDto>>> GetLogsAsync(GatewayLogFilterDto? filter = null);

        // Webhook Events
        Task<APIResponse<bool>> ProcessWebhookCallbackAsync(string payloadJson);
    }
}
