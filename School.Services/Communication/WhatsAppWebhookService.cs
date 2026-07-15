using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Communication;
using School.Infrastructure;
using School.Services.Interfaces;

namespace School.Services.Communication
{
    public class WhatsAppWebhookService : IWhatsAppWebhookService
    {
        private readonly SchoolDbContext _context;

        public WhatsAppWebhookService(SchoolDbContext context)
        {
            _context = context;
        }

        private int GetCurrentSchoolId()
        {
            if (_context.CurrentTenantId.HasValue)
                return _context.CurrentTenantId.Value;
            var school = _context.SchoolRegistrations.IgnoreQueryFilters().FirstOrDefault();
            return school?.Id ?? 1;
        }

        public async Task<bool> LogWebhookEventAsync(string eventType, string payloadJson)
        {
            var schoolId = GetCurrentSchoolId();
            var ev = new WhatsAppWebhookEvent
            {
                SchoolRegistrationId = schoolId,
                EventType = eventType,
                Payload = payloadJson,
                Processed = true,
                ReceivedDate = DateTime.UtcNow
            };
            _context.WhatsAppWebhookEvents.Add(ev);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateMessageStatusFromWebhookAsync(string metaMessageId, string status, string recipientPhone)
        {
            var schoolId = GetCurrentSchoolId();
            var message = await _context.WhatsAppMessages.FirstOrDefaultAsync(m => m.MetaMessageId == metaMessageId && !m.IsDeleted);

            if (message != null)
            {
                message.Status = status switch
                {
                    "delivered" => "Delivered",
                    "read" => "Read",
                    "failed" => "Failed",
                    _ => message.Status
                };
            }

            var deliveryLog = new WhatsAppDeliveryLog
            {
                SchoolRegistrationId = message?.SchoolRegistrationId ?? schoolId,
                MetaMessageId = metaMessageId,
                RecipientPhone = recipientPhone,
                Status = status switch
                {
                    "delivered" => "Delivered",
                    "read" => "Read",
                    "failed" => "Failed",
                    _ => "Sent"
                },
                StatusTimestamp = DateTime.UtcNow
            };
            _context.WhatsAppDeliveryLogs.Add(deliveryLog);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
