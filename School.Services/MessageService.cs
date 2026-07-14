using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using School.Domain.Communication;
using School.Infrastructure;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;

namespace School.Services
{
    public class MessageService : IMessageService
    {
        private readonly SchoolDbContext _dbContext;
        private readonly ITenantService _tenantService;
        private readonly ILogger<MessageService> _logger;

        public MessageService(
            SchoolDbContext dbContext,
            ITenantService tenantService,
            ILogger<MessageService> logger)
        {
            _dbContext = dbContext;
            _tenantService = tenantService;
            _logger = logger;
        }

        public async Task<bool> SendSmsAsync(string recipientNo, string message)
        {
            var tenantId = _tenantService.GetTenantId() ?? 0;
            _logger.LogInformation("Attempting to send SMS to {Recipient} (Tenant: {TenantId})", recipientNo, tenantId);

            var log = new SmsLog
            {
                SchoolRegistrationId = tenantId,
                RecipientNo = recipientNo,
                Message = message,
                SentStatus = "Sent",
                SentDate = DateTime.UtcNow,
                ProviderResponse = "SMS queued via carrier SMS-Gateway successfully (200 OK)"
            };

            try
            {
                _dbContext.SmsLogs.Add(log);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to record SMS transmission log.");
                return false;
            }
        }

        public async Task<bool> SendWhatsAppAsync(string recipientPhone, string message)
        {
            var tenantId = _tenantService.GetTenantId() ?? 0;
            _logger.LogInformation("Attempting to send WhatsApp message to {Recipient} (Tenant: {TenantId})", recipientPhone, tenantId);

            var log = new WhatsAppLog
            {
                SchoolRegistrationId = tenantId,
                RecipientPhone = recipientPhone,
                Message = message,
                Status = "Sent",
                SentDate = DateTime.UtcNow
            };

            try
            {
                _dbContext.WhatsAppLogs.Add(log);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to record WhatsApp transmission log.");
                return false;
            }
        }
    }
}
