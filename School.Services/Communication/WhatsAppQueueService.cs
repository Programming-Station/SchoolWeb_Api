using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Communication;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs.Communication;

namespace School.Services.Communication
{
    public class WhatsAppQueueService : IWhatsAppQueueService
    {
        private readonly SchoolDbContext _context;

        public WhatsAppQueueService(SchoolDbContext context)
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

        public async Task<bool> EnqueueMessageAsync(string recipientPhone, string payloadJson, DateTime? scheduledTime = null)
        {
            var schoolId = GetCurrentSchoolId();
            var queue = new WhatsAppQueue
            {
                SchoolRegistrationId = schoolId,
                RecipientPhone = recipientPhone,
                MessagePayload = payloadJson,
                ScheduledTime = scheduledTime ?? DateTime.UtcNow,
                RetryCount = 0,
                Status = "Pending",
                CreatedDate = DateTime.UtcNow
            };

            _context.WhatsAppQueues.Add(queue);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<WhatsAppQueueDto>> GetPendingQueueAsync()
        {
            var schoolId = GetCurrentSchoolId();
            var queues = await _context.WhatsAppQueues
                .Where(x => x.SchoolRegistrationId == schoolId && x.Status == "Pending" && x.ScheduledTime <= DateTime.UtcNow && !x.IsDeleted)
                .OrderBy(x => x.ScheduledTime)
                .ToListAsync();

            return queues.Select(x => new WhatsAppQueueDto
            {
                Id = x.Id,
                RecipientPhone = x.RecipientPhone,
                MessagePayload = x.MessagePayload,
                ScheduledTime = x.ScheduledTime,
                RetryCount = x.RetryCount,
                Status = x.Status
            });
        }

        public async Task<bool> UpdateQueueStatusAsync(int id, string status, int retryCount)
        {
            var queue = await _context.WhatsAppQueues.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (queue == null) return false;

            queue.Status = status;
            queue.RetryCount = retryCount;
            queue.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
