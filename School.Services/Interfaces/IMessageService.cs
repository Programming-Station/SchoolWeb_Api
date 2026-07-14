using System.Threading.Tasks;

namespace School.Services.Interfaces
{
    public interface IMessageService
    {
        Task<bool> SendSmsAsync(string recipientNo, string message);
        Task<bool> SendWhatsAppAsync(string recipientPhone, string message);
    }
}
