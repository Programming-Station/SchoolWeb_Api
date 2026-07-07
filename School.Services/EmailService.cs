using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using School.Domain.Email;
using School.Domain.School;
using School.Infrastructure;
using School.Infrastructure.Email;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using School.Utilities.Security;

namespace School.Services
{
    public class EmailService : IEmailService
    {
        private readonly SchoolDbContext _dbContext;
        private readonly ITemplateRenderer _templateRenderer;
        private readonly SmtpEmailProvider _emailProvider;
        private readonly IMemoryCache _cache;
        private readonly ILogger<EmailService> _logger;
        private readonly ITenantService _tenantService;
        private readonly IEncryptionService _encryptionService;
        private readonly IEmailQueue _emailQueue;

        private const string SmtpSettingsCacheKeyPrefix = "ActiveSmtpSettings_";
        private const string EmailTemplateCacheKeyPrefix = "EmailTemplate_";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1); // Cache settings and templates for 1 hour

        public EmailService(
            SchoolDbContext dbContext,
            ITemplateRenderer templateRenderer,
            SmtpEmailProvider emailProvider,
            IMemoryCache cache,
            ILogger<EmailService> logger,
            ITenantService tenantService,
            IEncryptionService encryptionService,
            IEmailQueue emailQueue)
        {
            _dbContext = dbContext;
            _templateRenderer = templateRenderer;
            _emailProvider = emailProvider;
            _cache = cache;
            _logger = logger;
            _tenantService = tenantService;
            _encryptionService = encryptionService;
            _emailQueue = emailQueue;
        }

        public void InvalidateTemplateCache(string templateName)
        {
            if (string.IsNullOrWhiteSpace(templateName)) return;
            var tenantId = _tenantService.GetTenantId() ?? 0;
            string cacheKey = $"{EmailTemplateCacheKeyPrefix}{tenantId}_{templateName.Trim().ToLower()}";
            _cache.Remove(cacheKey);
            _logger.LogInformation("Invalidated cache for email template: {TemplateName} (Tenant: {TenantId})", templateName, tenantId);
        }

        public void InvalidateSmtpCache()
        {
            var tenantId = _tenantService.GetTenantId() ?? 0;
            string cacheKey = $"{SmtpSettingsCacheKeyPrefix}{tenantId}";
            _cache.Remove(cacheKey);
            _logger.LogInformation("Invalidated SMTP settings cache (Tenant: {TenantId}).", tenantId);
        }

        public void QueueTemplateEmail(string recipientEmail, string templateName, Dictionary<string, string>? placeholders)
        {
            var tenantId = _tenantService.GetTenantId() ?? 0;
            _emailQueue.QueueEmail(new EmailQueueItem
            {
                TenantId = tenantId,
                RecipientEmail = recipientEmail,
                TemplateName = templateName,
                Placeholders = placeholders
            });
            _logger.LogInformation("Email queued in background. Template: {TemplateName}, Recipient: {Recipient}, Tenant: {TenantId}",
                templateName, recipientEmail, tenantId);
        }

        private async Task<EmailServerSetting?> GetSmtpSettingAsync()
        {
            var tenantId = _tenantService.GetTenantId() ?? 0;
            string cacheKey = $"{SmtpSettingsCacheKeyPrefix}{tenantId}";

            if (!_cache.TryGetValue(cacheKey, out EmailServerSetting? setting))
            {
                // 1. Try to find active settings for the current tenant
                if (tenantId > 0)
                {
                    setting = await _dbContext.EmailServerSettings
                        .FirstOrDefaultAsync(s => s.SchoolRegistrationId == tenantId && s.IsActive);
                }

                // 2. If not found or not tenant context, fall back to default school settings
                if (setting == null)
                {
                    var defaultSchool = await _dbContext.SchoolRegistrations
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(s => s.SchoolCode == "DEF001");
                    int defaultSchoolId = defaultSchool?.Id ?? 1;

                    setting = await _dbContext.EmailServerSettings
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(s => s.SchoolRegistrationId == defaultSchoolId && s.IsActive);
                }

                if (setting != null)
                {
                    // Clone setting to avoid modifying cached tracker object values
                    setting = new EmailServerSetting
                    {
                        Id = setting.Id,
                        HostName = setting.HostName,
                        Port = setting.Port,
                        UserName = setting.UserName,
                        Password = setting.Password,
                        FromEmail = setting.FromEmail,
                        DisplayName = setting.DisplayName,
                        EnableSSL = setting.EnableSSL,
                        IsActive = setting.IsActive,
                        UseDefaultCredential = setting.UseDefaultCredential,
                        SchoolRegistrationId = setting.SchoolRegistrationId
                    };

                    // Decrypt SMTP password securely
                    try
                    {
                        setting.Password = _encryptionService.Decrypt(setting.Password);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("Failed to decrypt SMTP password. It might be stored as plain text. Error: {Msg}", ex.Message);
                    }

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(CacheDuration);
                    _cache.Set(cacheKey, setting, cacheEntryOptions);
                    _logger.LogInformation("Loaded active SMTP settings for Tenant {TenantId} from database and cached.", tenantId);
                }
                else
                {
                    _logger.LogWarning("No active SMTP setting found for Tenant {TenantId} or system default.", tenantId);
                }
            }
            return setting;
        }

        private async Task<EmailTemplate?> GetTemplateAsync(string templateName)
        {
            if (string.IsNullOrWhiteSpace(templateName)) return null;

            string normalizedTemplateName = templateName.Trim().ToLower();
            var tenantId = _tenantService.GetTenantId() ?? 0;
            string cacheKey = $"{EmailTemplateCacheKeyPrefix}{tenantId}_{normalizedTemplateName}";

            if (!_cache.TryGetValue(cacheKey, out EmailTemplate? template))
            {
                // 1. Try to find template override for the current tenant
                if (tenantId > 0)
                {
                    template = await _dbContext.EmailTemplates
                        .FirstOrDefaultAsync(t => t.SchoolRegistrationId == tenantId && t.TemplateName.ToLower() == normalizedTemplateName && t.IsActive);
                }

                // 2. If not found or not tenant context, fall back to default school template
                if (template == null)
                {
                    var defaultSchool = await _dbContext.SchoolRegistrations
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(s => s.SchoolCode == "DEF001");
                    int defaultSchoolId = defaultSchool?.Id ?? 1;

                    template = await _dbContext.EmailTemplates
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(t => t.SchoolRegistrationId == defaultSchoolId && t.TemplateName.ToLower() == normalizedTemplateName && t.IsActive);
                }

                if (template != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(CacheDuration);
                    _cache.Set(cacheKey, template, cacheEntryOptions);
                    _logger.LogInformation("Loaded active email template '{TemplateName}' for Tenant {TenantId} from database and cached.", templateName, tenantId);
                }
                else
                {
                    _logger.LogWarning("Active email template '{TemplateName}' not found for Tenant {TenantId} or system default.", templateName, tenantId);
                }
            }
            return template;
        }

        public async Task<bool> SendTemplateAsync(string recipientEmail, string templateName, Dictionary<string, string>? placeholders)
        {
            var sendTime = DateTime.UtcNow;
            if (string.IsNullOrWhiteSpace(recipientEmail))
            {
                _logger.LogWarning("Recipient email is empty. Cannot send email. Template: {TemplateName}", templateName);
                return false;
            }

            try
            {
                var template = await GetTemplateAsync(templateName);
                if (template == null)
                {
                    _logger.LogWarning("Email template '{TemplateName}' not found or inactive. Skipping email sending to {Recipient}.", templateName, recipientEmail);
                    return false;
                }

                var setting = await GetSmtpSettingAsync();
                if (setting == null)
                {
                    _logger.LogError("Active SMTP setting not found in database. Cannot send email '{TemplateName}' to {Recipient}.", templateName, recipientEmail);
                    return false;
                }

                // Clone placeholders dictionary to prevent modifying the caller's reference
                var finalPlaceholders = placeholders != null
                    ? new Dictionary<string, string>(placeholders, StringComparer.OrdinalIgnoreCase)
                    : new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                // Inject default system/branding placeholders
                var tenantId = _tenantService.GetTenantId();
                SchoolRegistration? school = null;
                if (tenantId.HasValue && tenantId.Value > 0)
                {
                    string schoolCacheKey = $"SchoolRegistration_{tenantId.Value}";
                    if (!_cache.TryGetValue(schoolCacheKey, out school))
                    {
                        school = await _dbContext.SchoolRegistrations
                            .FirstOrDefaultAsync(s => s.Id == tenantId.Value);

                        if (school != null)
                        {
                            var cacheEntryOptions = new MemoryCacheEntryOptions()
                                .SetAbsoluteExpiration(CacheDuration);
                            _cache.Set(schoolCacheKey, school, cacheEntryOptions);
                        }
                    }
                }

                // If no tenant school found, fall back to default school
                if (school == null)
                {
                    string defaultSchoolCacheKey = "SchoolRegistration_Default";
                    if (!_cache.TryGetValue(defaultSchoolCacheKey, out school))
                    {
                        school = await _dbContext.SchoolRegistrations
                            .IgnoreQueryFilters()
                            .FirstOrDefaultAsync(s => s.SchoolCode == "DEF001");

                        if (school != null)
                        {
                            var cacheEntryOptions = new MemoryCacheEntryOptions()
                                .SetAbsoluteExpiration(CacheDuration);
                            _cache.Set(defaultSchoolCacheKey, school, cacheEntryOptions);
                        }
                    }
                }

                // Fetch branding details
                EmailBranding? branding = null;
                if (tenantId.HasValue && tenantId.Value > 0)
                {
                    string brandingCacheKey = $"EmailBranding_{tenantId.Value}";
                    if (!_cache.TryGetValue(brandingCacheKey, out branding))
                    {
                        branding = await _dbContext.EmailBrandings
                            .FirstOrDefaultAsync(b => b.SchoolRegistrationId == tenantId.Value);

                        if (branding != null)
                        {
                            var cacheEntryOptions = new MemoryCacheEntryOptions()
                                .SetAbsoluteExpiration(CacheDuration);
                            _cache.Set(brandingCacheKey, branding, cacheEntryOptions);
                        }
                    }
                }

                // Fallback to default branding if tenant hasn't defined any
                if (branding == null)
                {
                    string defaultBrandingCacheKey = "EmailBranding_Default";
                    if (!_cache.TryGetValue(defaultBrandingCacheKey, out branding))
                    {
                        var defaultSchool = await _dbContext.SchoolRegistrations
                            .IgnoreQueryFilters()
                            .FirstOrDefaultAsync(s => s.SchoolCode == "DEF001");
                        int defaultSchoolId = defaultSchool?.Id ?? 1;

                        branding = await _dbContext.EmailBrandings
                            .IgnoreQueryFilters()
                            .FirstOrDefaultAsync(b => b.SchoolRegistrationId == defaultSchoolId);

                        if (branding != null)
                        {
                            var cacheEntryOptions = new MemoryCacheEntryOptions()
                                .SetAbsoluteExpiration(CacheDuration);
                            _cache.Set(defaultBrandingCacheKey, branding, cacheEntryOptions);
                        }
                    }
                }

                if (school != null)
                {
                    // Add branding placeholders if not already provided by the service
                    if (!finalPlaceholders.ContainsKey("SchoolName"))
                        finalPlaceholders["SchoolName"] = school.SchoolName;
                    if (!finalPlaceholders.ContainsKey("SchoolLogo"))
                        finalPlaceholders["SchoolLogo"] = school.Logo ?? "";
                    if (!finalPlaceholders.ContainsKey("SchoolAddress"))
                        finalPlaceholders["SchoolAddress"] = school.Address ?? "";
                    if (!finalPlaceholders.ContainsKey("Website"))
                        finalPlaceholders["Website"] = school.WebsiteUrl ?? "#";
                    
                    if (branding != null)
                    {
                        if (!finalPlaceholders.ContainsKey("SupportEmail"))
                            finalPlaceholders["SupportEmail"] = branding.SupportEmail ?? school.Email;
                        if (!finalPlaceholders.ContainsKey("SupportPhone"))
                            finalPlaceholders["SupportPhone"] = branding.SupportPhone ?? school.PhoneNumber;
                        if (!finalPlaceholders.ContainsKey("PrincipalName"))
                            finalPlaceholders["PrincipalName"] = branding.PrincipalName ?? school.ContactPersonName ?? "Principal";
                        if (!finalPlaceholders.ContainsKey("ThemeColor"))
                            finalPlaceholders["ThemeColor"] = branding.ThemeColor;
                    }
                    else
                    {
                        if (!finalPlaceholders.ContainsKey("SupportEmail"))
                            finalPlaceholders["SupportEmail"] = school.Email;
                        if (!finalPlaceholders.ContainsKey("SupportPhone"))
                            finalPlaceholders["SupportPhone"] = school.PhoneNumber;
                        if (!finalPlaceholders.ContainsKey("PrincipalName"))
                            finalPlaceholders["PrincipalName"] = school.ContactPersonName ?? "Principal";
                        if (!finalPlaceholders.ContainsKey("ThemeColor"))
                            finalPlaceholders["ThemeColor"] = "#1e3a8a";
                    }
                }

                if (!finalPlaceholders.ContainsKey("CurrentYear"))
                    finalPlaceholders["CurrentYear"] = DateTime.UtcNow.Year.ToString();
                if (!finalPlaceholders.ContainsKey("CurrentDate"))
                    finalPlaceholders["CurrentDate"] = DateTime.UtcNow.ToString("dd MMM yyyy");

                // Render branding header and footer blocks
                string headerHtml = branding != null && !string.IsNullOrEmpty(branding.HeaderHtml)
                    ? _templateRenderer.Render(branding.HeaderHtml, finalPlaceholders)
                    : string.Empty;

                string footerHtml = branding != null && !string.IsNullOrEmpty(branding.FooterHtml)
                    ? _templateRenderer.Render(branding.FooterHtml, finalPlaceholders)
                    : string.Empty;

                // Process layout injection
                string rawBody = template.BodyHtml;
                string processedBody;

                if (rawBody.Contains("{{EmailHeader}}") || rawBody.Contains("{{EmailFooter}}"))
                {
                    // Case A: Template explicitly defines header and footer slots
                    finalPlaceholders["EmailHeader"] = headerHtml;
                    finalPlaceholders["EmailFooter"] = footerHtml;
                    processedBody = _templateRenderer.Render(rawBody, finalPlaceholders);
                }
                else if (rawBody.Trim().StartsWith("<!DOCTYPE html", StringComparison.OrdinalIgnoreCase) || rawBody.Contains("<html", StringComparison.OrdinalIgnoreCase))
                {
                    // Case B: Template is already a full standalone HTML document
                    processedBody = _templateRenderer.Render(rawBody, finalPlaceholders);
                }
                else
                {
                    // Case C: Template is a content block; dynamically wrap it with header and footer
                    string renderedBody = _templateRenderer.Render(rawBody, finalPlaceholders);
                    processedBody = $"{headerHtml}{renderedBody}{footerHtml}";
                }

                string subject = _templateRenderer.Render(template.Subject, finalPlaceholders);

                await _emailProvider.SendEmailAsync(setting, recipientEmail, subject, processedBody);

                _logger.LogInformation(
                    "Email sent successfully. Template: {TemplateName}, Recipient: {Recipient}, SendTime: {SendTime}, Tenant: {TenantId}, Status: Success",
                    templateName, recipientEmail, sendTime, tenantId ?? 0);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to send email. Template: {TemplateName}, Recipient: {Recipient}, SendTime: {SendTime}, SMTP Error: {ErrorMsg}, Status: Failure",
                    templateName, recipientEmail, sendTime, ex.Message);

                return false;
            }
        }

        public async Task<bool> SendWelcomeEmailAsync(string recipientEmail, string userName, string password, string status)
        {
            var placeholders = new Dictionary<string, string>
            {
                { "UserName", userName },
                { "EmployeeCode", userName },
                { "Password", password },
                { "Status", status },
                { "CurrentDate", DateTime.Now.ToString("dd MMM yyyy") }
            };
            return await SendTemplateAsync(recipientEmail, "Employee Account Created", placeholders);
        }

        public async Task<bool> SendForgotPasswordAsync(string recipientEmail, string userName, string resetLink)
        {
            var placeholders = new Dictionary<string, string>
            {
                { "UserName", userName },
                { "ResetLink", resetLink },
                { "ResetPasswordLink", resetLink },
                { "SupportEmail", "support@cipcvns.com" }
            };
            return await SendTemplateAsync(recipientEmail, "Forgot Password", placeholders);
        }

        public async Task<bool> SendResetPasswordAsync(string recipientEmail, string userName)
        {
            var placeholders = new Dictionary<string, string>
            {
                { "UserName", userName },
                { "CurrentDate", DateTime.Now.ToString("dd MMM yyyy") },
                { "CurrentTime", DateTime.Now.ToString("HH:mm") }
            };
            return await SendTemplateAsync(recipientEmail, "Password Changed", placeholders);
        }

        public async Task<bool> SendOtpAsync(string recipientEmail, string userName, string otp)
        {
            var placeholders = new Dictionary<string, string>
            {
                { "UserName", userName },
                { "Otp", otp },
                { "OtpSendFrom", "System" }
            };
            return await SendTemplateAsync(recipientEmail, "Otp", placeholders);
        }

        public async Task<bool> SendVerificationAsync(string recipientEmail, string userName, string verificationLink)
        {
            var placeholders = new Dictionary<string, string>
            {
                { "UserName", userName },
                { "VerificationCode", verificationLink },
                { "VerificationLink", verificationLink }
            };
            return await SendTemplateAsync(recipientEmail, "Email Verification", placeholders);
        }

        public async Task<bool> SendGenericTemplateAsync(string recipientEmail, string templateName, Dictionary<string, string>? placeholders)
        {
            return await SendTemplateAsync(recipientEmail, templateName, placeholders);
        }
    }
}
