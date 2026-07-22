using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using School.Domain.Communication;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Communication;

namespace School.Services.Communication
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly SchoolDbContext _context;
        private readonly ILogger<WhatsAppService> _logger;
        private static readonly HttpClient _httpClient = new HttpClient();

        public WhatsAppService(SchoolDbContext context, ILogger<WhatsAppService> logger)
        {
            _context = context;
            _logger = logger;
        }

        private int GetCurrentSchoolId()
        {
            if (_context.CurrentTenantId.HasValue)
                return _context.CurrentTenantId.Value;
            var school = _context.SchoolRegistrations.IgnoreQueryFilters().FirstOrDefault();
            return school?.Id ?? 1;
        }

        public async Task<APIResponse<WhatsAppAccountDto>> GetWhatsAppAccountAsync()
        {
            var schoolId = GetCurrentSchoolId();
            var account = await _context.WhatsAppAccounts.FirstOrDefaultAsync(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted);

            if (account == null)
            {
                return new APIResponse<WhatsAppAccountDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "WhatsApp Account credentials not configured for this tenant."
                };
            }

            return new APIResponse<WhatsAppAccountDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = new WhatsAppAccountDto
                {
                    Id = account.Id,
                    PhoneNumberId = account.PhoneNumberId,
                    BusinessAccountId = account.BusinessAccountId,
                    PermanentAccessToken = account.PermanentAccessToken,
                    WebhookVerifyToken = account.WebhookVerifyToken,
                    WebhookSecret = account.WebhookSecret,
                    BaseUrl = account.BaseUrl,
                    IsSandbox = account.IsSandbox,
                    Status = account.Status
                }
            };
        }

        public async Task<APIResponse<WhatsAppAccountDto>> SaveWhatsAppAccountAsync(WhatsAppAccountDto dto, string userName)
        {
            var schoolId = GetCurrentSchoolId();
            var account = await _context.WhatsAppAccounts.FirstOrDefaultAsync(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted);

            if (account == null)
            {
                account = new WhatsAppAccount
                {
                    SchoolRegistrationId = schoolId,
                    CreatedBy = userName,
                    CreatedDate = DateTime.UtcNow
                };
                _context.WhatsAppAccounts.Add(account);
            }
            else
            {
                account.UpdatedBy = userName;
                account.UpdatedDate = DateTime.UtcNow;
            }

            account.PhoneNumberId = dto.PhoneNumberId;
            account.BusinessAccountId = dto.BusinessAccountId;
            account.PermanentAccessToken = dto.PermanentAccessToken;
            account.WebhookVerifyToken = dto.WebhookVerifyToken;
            account.WebhookSecret = dto.WebhookSecret;
            account.BaseUrl = dto.BaseUrl;
            account.IsSandbox = dto.IsSandbox;
            account.Status = dto.Status;

            await _context.SaveChangesAsync();

            dto.Id = account.Id;
            return new APIResponse<WhatsAppAccountDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "WhatsApp configuration saved successfully.",
                Data = dto
            };
        }

        public async Task<APIResponse<IEnumerable<WhatsAppTemplateDto>>> GetTemplatesAsync()
        {
            var schoolId = GetCurrentSchoolId();
            var templates = await _context.WhatsAppTemplates
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .OrderBy(x => x.TemplateName)
                .ToListAsync();

            return new APIResponse<IEnumerable<WhatsAppTemplateDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = templates.Select(x => new WhatsAppTemplateDto
                {
                    Id = x.Id,
                    TemplateName = x.TemplateName,
                    Category = x.Category,
                    LanguageCode = x.LanguageCode,
                    BodyTemplate = x.BodyTemplate,
                    Status = x.Status
                })
            };
        }

        public async Task<APIResponse<WhatsAppTemplateDto>> CreateTemplateAsync(WhatsAppTemplateDto dto, string userName)
        {
            var schoolId = GetCurrentSchoolId();
            var template = new WhatsAppTemplate
            {
                SchoolRegistrationId = schoolId,
                TemplateName = dto.TemplateName,
                Category = dto.Category,
                LanguageCode = dto.LanguageCode,
                BodyTemplate = dto.BodyTemplate,
                Status = dto.Status,
                CreatedBy = userName,
                CreatedDate = DateTime.UtcNow
            };

            _context.WhatsAppTemplates.Add(template);
            await _context.SaveChangesAsync();

            dto.Id = template.Id;
            return new APIResponse<WhatsAppTemplateDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "WhatsApp template registered.",
                Data = dto
            };
        }

        public async Task<APIResponse<bool>> SendTextAsync(string recipientPhone, string message, string userName)
        {
            var schoolId = GetCurrentSchoolId();
            var account = await _context.WhatsAppAccounts.FirstOrDefaultAsync(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted);

            string status = "Sent";
            string? metaMessageId = null;

            if (account != null && !string.IsNullOrWhiteSpace(account.PhoneNumberId) && !string.IsNullOrWhiteSpace(account.PermanentAccessToken))
            {
                try
                {
                    var apiVersion = "v20.0";
                    var requestUrl = $"https://graph.facebook.com/{apiVersion}/{account.PhoneNumberId}/messages";
                    var payload = new
                    {
                        messaging_product = "whatsapp",
                        recipient_type = "individual",
                        to = recipientPhone,
                        type = "text",
                        text = new { preview_url = false, body = message }
                    };

                    var jsonPayload = JsonSerializer.Serialize(payload);
                    using var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", account.PermanentAccessToken);
                    requestMessage.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    var response = await _httpClient.SendAsync(requestMessage);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        status = "Delivered";
                        using var doc = JsonDocument.Parse(responseContent);
                        if (doc.RootElement.TryGetProperty("messages", out var msgs) && msgs.GetArrayLength() > 0)
                        {
                            metaMessageId = msgs[0].GetProperty("id").GetString();
                        }
                    }
                    else
                    {
                        status = "Failed";
                        _logger.LogError("WhatsApp Gateway Send failed: {Response}", responseContent);
                    }
                }
                catch (Exception ex)
                {
                    status = "Failed";
                    _logger.LogError(ex, "WhatsApp Meta HTTP Dispatch Crash.");
                }
            }

            var msg = new WhatsAppMessage
            {
                SchoolRegistrationId = schoolId,
                RecipientPhone = recipientPhone,
                MessageText = message,
                MessageType = "Text",
                MetaMessageId = metaMessageId,
                Status = status,
                SentDate = DateTime.UtcNow,
                CreatedBy = userName,
                CreatedDate = DateTime.UtcNow
            };

            _context.WhatsAppMessages.Add(msg);
            await _context.SaveChangesAsync();

            return new APIResponse<bool>
            {
                Success = status == "Delivered" || status == "Sent",
                StatusCode = HttpStatusCode.OK,
                Data = true
            };
        }

        public async Task<APIResponse<bool>> SendTemplateAsync(string recipientPhone, string templateName, Dictionary<string, string> variables, string userName)
        {
            var schoolId = GetCurrentSchoolId();
            var account = await _context.WhatsAppAccounts.FirstOrDefaultAsync(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted);

            // Construct template preview message for DB storage
            var template = await _context.WhatsAppTemplates.FirstOrDefaultAsync(t => t.SchoolRegistrationId == schoolId && t.TemplateName == templateName && !t.IsDeleted);
            string messageBody = template?.BodyTemplate ?? $"Template: {templateName}";

            if (variables != null)
            {
                foreach (var v in variables)
                {
                    messageBody = messageBody.Replace($"{{{{{v.Key}}}}}", v.Value);
                }
            }

            string status = "Sent";
            string? metaMessageId = null;

            if (account != null && !string.IsNullOrWhiteSpace(account.PhoneNumberId) && !string.IsNullOrWhiteSpace(account.PermanentAccessToken))
            {
                try
                {
                    var apiVersion = "v20.0";
                    var requestUrl = $"https://graph.facebook.com/{apiVersion}/{account.PhoneNumberId}/messages";

                    var components = new List<object>();
                    if (variables != null && variables.Count > 0)
                    {
                        var parameters = variables.Select(v => new { type = "text", text = v.Value }).ToList();
                        components.Add(new
                        {
                            type = "body",
                            parameters = parameters
                        });
                    }

                    var payload = new
                    {
                        messaging_product = "whatsapp",
                        recipient_type = "individual",
                        to = recipientPhone,
                        type = "template",
                        template = new
                        {
                            name = templateName,
                            language = new { code = template?.LanguageCode ?? "en_US" },
                            components = components
                        }
                    };

                    var jsonPayload = JsonSerializer.Serialize(payload);
                    using var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", account.PermanentAccessToken);
                    requestMessage.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    var response = await _httpClient.SendAsync(requestMessage);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        status = "Delivered";
                        using var doc = JsonDocument.Parse(responseContent);
                        if (doc.RootElement.TryGetProperty("messages", out var msgs) && msgs.GetArrayLength() > 0)
                        {
                            metaMessageId = msgs[0].GetProperty("id").GetString();
                        }
                    }
                    else
                    {
                        status = "Failed";
                        _logger.LogError("WhatsApp Gateway Template Send failed: {Response}", responseContent);
                    }
                }
                catch (Exception ex)
                {
                    status = "Failed";
                    _logger.LogError(ex, "WhatsApp Meta HTTP Template Dispatch Crash.");
                }
            }

            var msg = new WhatsAppMessage
            {
                SchoolRegistrationId = schoolId,
                RecipientPhone = recipientPhone,
                MessageText = messageBody,
                MessageType = "Template",
                MetaMessageId = metaMessageId,
                Status = status,
                SentDate = DateTime.UtcNow,
                CreatedBy = userName,
                CreatedDate = DateTime.UtcNow
            };

            _context.WhatsAppMessages.Add(msg);
            await _context.SaveChangesAsync();

            return new APIResponse<bool>
            {
                Success = status == "Delivered" || status == "Sent",
                StatusCode = HttpStatusCode.OK,
                Data = true
            };
        }

        public async Task<APIResponse<bool>> SendMediaAsync(string recipientPhone, string mediaUrl, string mimeType, string filename, string userName)
        {
            // Simplified Media Sending logs fallback
            var schoolId = GetCurrentSchoolId();
            var account = await _context.WhatsAppAccounts.FirstOrDefaultAsync(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted);

            string status = "Sent";
            string? metaMessageId = null;

            if (account != null && !string.IsNullOrWhiteSpace(account.PhoneNumberId) && !string.IsNullOrWhiteSpace(account.PermanentAccessToken))
            {
                try
                {
                    var apiVersion = "v20.0";
                    var requestUrl = $"https://graph.facebook.com/{apiVersion}/{account.PhoneNumberId}/messages";

                    var payload = new
                    {
                        messaging_product = "whatsapp",
                        recipient_type = "individual",
                        to = recipientPhone,
                        type = mimeType.StartsWith("image/") ? "image" : "document",
                        image = mimeType.StartsWith("image/") ? new { link = mediaUrl } : null,
                        document = !mimeType.StartsWith("image/") ? new { link = mediaUrl, filename = filename } : null
                    };

                    var jsonPayload = JsonSerializer.Serialize(payload);
                    using var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", account.PermanentAccessToken);
                    requestMessage.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    var response = await _httpClient.SendAsync(requestMessage);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        status = "Delivered";
                        using var doc = JsonDocument.Parse(responseContent);
                        if (doc.RootElement.TryGetProperty("messages", out var msgs) && msgs.GetArrayLength() > 0)
                        {
                            metaMessageId = msgs[0].GetProperty("id").GetString();
                        }
                    }
                    else
                    {
                        status = "Failed";
                        _logger.LogError("WhatsApp Gateway Media Send failed: {Response}", responseContent);
                    }
                }
                catch (Exception ex)
                {
                    status = "Failed";
                    _logger.LogError(ex, "WhatsApp Meta HTTP Media Dispatch Crash.");
                }
            }

            var msg = new WhatsAppMessage
            {
                SchoolRegistrationId = schoolId,
                RecipientPhone = recipientPhone,
                MessageText = $"Media Attachment: {filename} ({mediaUrl})",
                MessageType = mimeType.StartsWith("image/") ? "Image" : "Document",
                MetaMessageId = metaMessageId,
                Status = status,
                SentDate = DateTime.UtcNow,
                CreatedBy = userName,
                CreatedDate = DateTime.UtcNow
            };

            _context.WhatsAppMessages.Add(msg);
            await _context.SaveChangesAsync();

            return new APIResponse<bool>
            {
                Success = status == "Delivered" || status == "Sent",
                StatusCode = HttpStatusCode.OK,
                Data = true
            };
        }

        public async Task<APIResponse<IEnumerable<WhatsAppQueueDto>>> GetQueuedMessagesAsync()
        {
            var schoolId = GetCurrentSchoolId();
            var queues = await _context.WhatsAppQueues
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .OrderBy(x => x.ScheduledTime)
                .ToListAsync();

            return new APIResponse<IEnumerable<WhatsAppQueueDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = queues.Select(x => new WhatsAppQueueDto
                {
                    Id = x.Id,
                    RecipientPhone = x.RecipientPhone,
                    MessagePayload = x.MessagePayload,
                    ScheduledTime = x.ScheduledTime,
                    RetryCount = x.RetryCount,
                    Status = x.Status
                })
            };
        }

        public async Task<APIResponse<bool>> RetryFailedMessageAsync(int queueId, string userName)
        {
            var schoolId = GetCurrentSchoolId();
            var queue = await _context.WhatsAppQueues.FirstOrDefaultAsync(x => x.Id == queueId && x.SchoolRegistrationId == schoolId && !x.IsDeleted);

            if (queue == null)
            {
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Queued message not found." };
            }

            queue.Status = "Pending";
            queue.RetryCount = 0;
            queue.ScheduledTime = DateTime.UtcNow;
            queue.UpdatedBy = userName;
            queue.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Message rescheduled.", Data = true };
        }

        public async Task<APIResponse<bool>> CancelQueuedMessageAsync(int queueId, string userName)
        {
            var schoolId = GetCurrentSchoolId();
            var queue = await _context.WhatsAppQueues.FirstOrDefaultAsync(x => x.Id == queueId && x.SchoolRegistrationId == schoolId && !x.IsDeleted);

            if (queue == null)
            {
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Queued message not found." };
            }

            queue.IsDeleted = true;
            queue.UpdatedBy = userName;
            queue.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Queued message cancelled.", Data = true };
        }

        public async Task<APIResponse<IEnumerable<WhatsAppMessageDto>>> GetLogsAsync(GatewayLogFilterDto? filter = null)
        {
            var schoolId = GetCurrentSchoolId();
            var q = _context.WhatsAppMessages.Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted).AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.RecipientNo))
                    q = q.Where(x => x.RecipientPhone.Contains(filter.RecipientNo));
                if (!string.IsNullOrWhiteSpace(filter.Status))
                    q = q.Where(x => x.Status == filter.Status);
                if (filter.FromDate.HasValue)
                    q = q.Where(x => x.SentDate >= filter.FromDate.Value);
                if (filter.ToDate.HasValue)
                    q = q.Where(x => x.SentDate <= filter.ToDate.Value);
            }

            var list = await q.OrderByDescending(x => x.SentDate).ToListAsync();
            return new APIResponse<IEnumerable<WhatsAppMessageDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = list.Select(x => new WhatsAppMessageDto
                {
                    Id = x.Id,
                    RecipientPhone = x.RecipientPhone,
                    MessageText = x.MessageText,
                    MessageType = x.MessageType,
                    MetaMessageId = x.MetaMessageId,
                    Status = x.Status,
                    SentDate = x.SentDate
                })
            };
        }

        public async Task<APIResponse<bool>> ProcessWebhookCallbackAsync(string payloadJson)
        {
            try
            {
                var doc = JsonDocument.Parse(payloadJson);
                var entry = doc.RootElement.GetProperty("entry")[0];
                var changes = entry.GetProperty("changes")[0];
                var value = changes.GetProperty("value");

                if (value.TryGetProperty("statuses", out var statuses) && statuses.GetArrayLength() > 0)
                {
                    var statusObj = statuses[0];
                    var metaMessageId = statusObj.GetProperty("id").GetString();
                    var status = statusObj.GetProperty("status").GetString();
                    var recipientPhone = statusObj.GetProperty("recipient_id").GetString();

                    // Log webhook event
                    var schoolId = 1; // Default
                    var ev = new WhatsAppWebhookEvent
                    {
                        SchoolRegistrationId = schoolId,
                        EventType = $"status_{status}",
                        Payload = payloadJson,
                        Processed = true,
                        ReceivedDate = DateTime.UtcNow
                    };
                    _context.WhatsAppWebhookEvents.Add(ev);

                    // Update corresponding message
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

                    // Log status trace
                    var deliveryLog = new WhatsAppDeliveryLog
                    {
                        SchoolRegistrationId = message?.SchoolRegistrationId ?? schoolId,
                        MetaMessageId = metaMessageId!,
                        RecipientPhone = recipientPhone!,
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
                }

                return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Webhook Event process failed.");
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.BadRequest, Message = ex.Message };
            }
        }
    }
}
