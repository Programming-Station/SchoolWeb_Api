namespace School.Services.Interfaces
{
    public class EmailQueueItem
    {
        public int TenantId { get; set; }
        public string RecipientEmail { get; set; } = string.Empty;
        public string TemplateName { get; set; } = string.Empty;
        public Dictionary<string, string>? Placeholders { get; set; }
        public byte[]? AttachmentBytes { get; set; }
        public string? AttachmentName { get; set; }
    }

    public interface IEmailQueue
    {
        void QueueEmail(EmailQueueItem emailItem);
        ValueTask<EmailQueueItem> DequeueAsync(CancellationToken cancellationToken);
    }
}
