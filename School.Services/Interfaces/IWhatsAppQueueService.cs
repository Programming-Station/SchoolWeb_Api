using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using School_DTOs.Communication;

namespace School.Services.Interfaces
{
    public interface IWhatsAppQueueService
    {
        Task<bool> EnqueueMessageAsync(string recipientPhone, string payloadJson, DateTime? scheduledTime = null);
        Task<IEnumerable<WhatsAppQueueDto>> GetPendingQueueAsync();
        Task<bool> UpdateQueueStatusAsync(int id, string status, int retryCount);
    }
}
