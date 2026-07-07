using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace School.Services.Interfaces
{
    public class EmailQueueItem
    {
        public int TenantId { get; set; }
        public string RecipientEmail { get; set; } = string.Empty;
        public string TemplateName { get; set; } = string.Empty;
        public Dictionary<string, string>? Placeholders { get; set; }
    }

    public interface IEmailQueue
    {
        void QueueEmail(EmailQueueItem emailItem);
        ValueTask<EmailQueueItem> DequeueAsync(CancellationToken cancellationToken);
    }
}
