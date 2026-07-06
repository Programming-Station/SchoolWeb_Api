using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using School.Services.Interfaces;

namespace School.Services
{
    public class EmailQueue : IEmailQueue
    {
        private readonly Channel<EmailQueueItem> _queue;

        public EmailQueue()
        {
            // Set unbounded channel or bounded channel. Bounded is safer for production memory, e.g. 50,000 capacity.
            var options = new BoundedChannelOptions(50000)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true, // We will have a single hosted service reading from it
                SingleWriter = false // Multiple APIs can write to it
            };
            _queue = Channel.CreateBounded<EmailQueueItem>(options);
        }

        public void QueueEmail(EmailQueueItem emailItem)
        {
            if (emailItem == null) throw new ArgumentNullException(nameof(emailItem));
            
            // Try to write. If full, we write synchronously/wait or discard. Since it's BoundedChannelFullMode.Wait,
            // we use TryWrite. If it fails, we write async.
            if (!_queue.Writer.TryWrite(emailItem))
            {
                // Fallback to async write in a fire-and-forget task
                _ = Task.Run(async () => await _queue.Writer.WriteAsync(emailItem));
            }
        }

        public async ValueTask<EmailQueueItem> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}
