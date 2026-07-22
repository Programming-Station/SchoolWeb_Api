using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using School.Services.Interfaces;

namespace School.Services.Communication
{
    public class WhatsAppQueueProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WhatsAppQueueProcessor> _logger;

        public WhatsAppQueueProcessor(IServiceProvider serviceProvider, ILogger<WhatsAppQueueProcessor> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("WhatsApp Queue background processor has started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var queueService = scope.ServiceProvider.GetRequiredService<IWhatsAppQueueService>();
                        var whatsappService = scope.ServiceProvider.GetRequiredService<IWhatsAppService>();

                        var pendingItems = await queueService.GetPendingQueueAsync();

                        foreach (var item in pendingItems)
                        {
                            if (stoppingToken.IsCancellationRequested) break;

                            _logger.LogInformation("Processing queued WhatsApp message ID {QueueId} to {Recipient}", item.Id, item.RecipientPhone);

                            // Execute dispatch
                            var res = await whatsappService.SendTextAsync(item.RecipientPhone, item.MessagePayload, "BackgroundQueueJob");

                            if (res.Success)
                            {
                                await queueService.UpdateQueueStatusAsync(item.Id, "Sent", item.RetryCount);
                            }
                            else
                            {
                                int nextRetry = item.RetryCount + 1;
                                string status = nextRetry >= 3 ? "DeadLetter" : "Pending";
                                await queueService.UpdateQueueStatusAsync(item.Id, status, nextRetry);
                                _logger.LogWarning("WhatsApp queued dispatch failed. Rescheduled status set to {Status}.", status);
                            }

                            // Rate limit protection - small delay between sends
                            await Task.Delay(1000, stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during WhatsApp Queue processing loop.");
                }

                // Poll every 30 seconds
                await Task.Delay(30000, stoppingToken);
            }

            _logger.LogInformation("WhatsApp Queue background processor has stopped.");
        }
    }
}
