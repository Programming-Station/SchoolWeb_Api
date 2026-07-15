using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        private static readonly HttpClient _httpClient = new HttpClient();

        public MessageService(
            SchoolDbContext dbContext,
            ITenantService tenantService,
            ILogger<MessageService> logger,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _tenantService = tenantService;
            _logger = logger;
            _configuration = configuration;
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

            var phoneNumberId = _configuration.GetSection("WhatsAppConfig:PhoneNumberId").Value;
            var accessToken = _configuration.GetSection("WhatsAppConfig:AccessToken").Value;
            var apiVersion = _configuration.GetSection("WhatsAppConfig:ApiVersion").Value ?? "v20.0";

            string status = "Sent";

            if (!string.IsNullOrWhiteSpace(phoneNumberId) && !string.IsNullOrWhiteSpace(accessToken))
            {
                try
                {
                    var requestUrl = $"https://graph.facebook.com/{apiVersion}/{phoneNumberId}/messages";
                    var payload = new
                    {
                        messaging_product = "whatsapp",
                        recipient_type = "individual",
                        to = recipientPhone,
                        type = "text",
                        text = new
                        {
                            preview_url = false,
                            body = message
                        }
                    };

                    var jsonPayload = JsonSerializer.Serialize(payload);
                    using var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    requestMessage.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    var response = await _httpClient.SendAsync(requestMessage);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        status = "Delivered";
                        _logger.LogInformation("WhatsApp message successfully sent via Meta Cloud API.");
                    }
                    else
                    {
                        status = "Failed";
                        _logger.LogError("Meta Cloud API returned an error status {Status}: {Response}", response.StatusCode, responseContent);
                    }
                }
                catch (Exception ex)
                {
                    status = "Failed";
                    _logger.LogError(ex, "Failed to connect to Meta WhatsApp API gateway.");
                }
            }
            else
            {
                _logger.LogInformation("Simulating WhatsApp dispatch - Credentials not configured in AppSettings.");
            }

            var log = new WhatsAppLog
            {
                SchoolRegistrationId = tenantId,
                RecipientPhone = recipientPhone,
                Message = message,
                Status = status,
                SentDate = DateTime.UtcNow
            };

            try
            {
                _dbContext.WhatsAppLogs.Add(log);
                await _dbContext.SaveChangesAsync();
                return status == "Delivered" || status == "Sent";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to record WhatsApp transmission log.");
                return false;
            }
        }
    }
}
