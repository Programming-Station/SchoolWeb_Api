using System.Threading.Tasks;

namespace School.Services.Interfaces
{
    public interface IWhatsAppWebhookService
    {
        Task<bool> LogWebhookEventAsync(string eventType, string payloadJson);
        Task<bool> UpdateMessageStatusFromWebhookAsync(string metaMessageId, string status, string recipientPhone);
    }
}
