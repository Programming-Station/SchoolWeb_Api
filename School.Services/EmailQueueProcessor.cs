using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using School.Domain.Email;
using School.Infrastructure;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;

namespace School.Services
{
    public class EmailQueueProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEmailQueue _emailQueue;
        private readonly ILogger<EmailQueueProcessor> _logger;

        public EmailQueueProcessor(
            IServiceProvider serviceProvider,
            IEmailQueue emailQueue,
            ILogger<EmailQueueProcessor> logger)
        {
            _serviceProvider = serviceProvider;
            _emailQueue = emailQueue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Enterprise Email Background Queue Processor is starting...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Dequeue the next email request
                    var queueItem = await _emailQueue.DequeueAsync(stoppingToken);

                    // Process email sequentially in the background
                    await ProcessQueueItemWithRetryAsync(queueItem);
                }
                catch (OperationCanceledException)
                {
                    // Graceful shutdown
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing email background queue item.");
                }
            }

            _logger.LogInformation("Enterprise Email Background Queue Processor is stopped.");
        }

        private async Task ProcessQueueItemWithRetryAsync(EmailQueueItem item)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var tenantService = scope.ServiceProvider.GetRequiredService<ITenantService>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var dbContext = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<EmailQueueProcessor>>();

                // 1. Establish tenant context in the background scope
                tenantService.SetTenantId(item.TenantId);

                // 2. Create the initial EmailLog record in the database
                var log = new EmailLog
                {
                    SchoolRegistrationId = item.TenantId,
                    TemplateName = item.TemplateName,
                    RecipientEmail = item.RecipientEmail,
                    Subject = item.TemplateName, // Will be updated to rendered subject after sending
                    BodyHtml = string.Empty,     // Will be updated after sending
                    Status = "Pending",
                    CreatedBy = "SystemQueue",
                    CreatedDate = DateTime.UtcNow
                };

                dbContext.EmailLogs.Add(log);
                await dbContext.SaveChangesAsync();

                int maxRetries = 3;
                bool isSuccess = false;
                string? errorMessage = null;
                string? smtpResponse = null;

                for (int attempt = 1; attempt <= maxRetries; attempt++)
                {
                    try
                    {
                        // Get template to log rendered fields
                        var template = await dbContext.EmailTemplates
                            .FirstOrDefaultAsync(t => t.SchoolRegistrationId == item.TenantId && t.TemplateName.ToLower() == item.TemplateName.ToLower() && t.IsActive);

                        if (template == null)
                        {
                            // Fallback to system default template
                            var defaultSchool = await dbContext.SchoolRegistrations
                                .IgnoreQueryFilters()
                                .FirstOrDefaultAsync(s => s.SchoolCode == "DEF001");
                            int defaultSchoolId = defaultSchool?.Id ?? 1;

                            template = await dbContext.EmailTemplates
                                .IgnoreQueryFilters()
                                .FirstOrDefaultAsync(t => t.SchoolRegistrationId == defaultSchoolId && t.TemplateName.ToLower() == item.TemplateName.ToLower() && t.IsActive);
                        }

                        if (template == null)
                        {
                            throw new InvalidOperationException($"Email template '{item.TemplateName}' was not found in database.");
                        }

                        // Send the email template
                        isSuccess = await emailService.SendTemplateAsync(item.RecipientEmail, item.TemplateName, item.Placeholders, item.AttachmentBytes, item.AttachmentName);

                        if (isSuccess)
                        {
                            smtpResponse = "250 OK - Message accepted for delivery";
                            
                            // Re-query or construct subject and body to save in audit logs
                            var renderer = scope.ServiceProvider.GetRequiredService<global::School.Infrastructure.Email.ITemplateRenderer>();
                            
                            // Load branding options to mock placeholders
                            var school = await dbContext.SchoolRegistrations.FindAsync(item.TenantId)
                                         ?? await dbContext.SchoolRegistrations.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.SchoolCode == "DEF001");

                            var finalPlaceholders = item.Placeholders != null 
                                ? new Dictionary<string, string>(item.Placeholders, StringComparer.OrdinalIgnoreCase)
                                : new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                            if (school != null)
                            {
                                if (!finalPlaceholders.ContainsKey("SchoolName")) finalPlaceholders["SchoolName"] = school.SchoolName;
                                if (!finalPlaceholders.ContainsKey("SchoolLogo")) finalPlaceholders["SchoolLogo"] = school.Logo ?? "";
                                if (!finalPlaceholders.ContainsKey("SchoolAddress")) finalPlaceholders["SchoolAddress"] = school.Address ?? "";
                                if (!finalPlaceholders.ContainsKey("Website")) finalPlaceholders["Website"] = school.WebsiteUrl ?? "#";
                                if (!finalPlaceholders.ContainsKey("SupportEmail")) finalPlaceholders["SupportEmail"] = school.Email;
                                if (!finalPlaceholders.ContainsKey("SupportPhone")) finalPlaceholders["SupportPhone"] = school.PhoneNumber;
                                if (!finalPlaceholders.ContainsKey("PrincipalName")) finalPlaceholders["PrincipalName"] = school.ContactPersonName ?? "Principal";
                            }
                            if (!finalPlaceholders.ContainsKey("CurrentYear")) finalPlaceholders["CurrentYear"] = DateTime.UtcNow.Year.ToString();

                            log.Subject = renderer.Render(template.Subject, finalPlaceholders);
                            log.BodyHtml = renderer.Render(template.BodyHtml, finalPlaceholders);
                            log.Status = "Sent";
                            log.SentTime = DateTime.UtcNow;
                            log.SmtpResponse = smtpResponse;
                            
                            break;
                        }
                        else
                        {
                            errorMessage = "EmailService returned false. Check logs for details.";
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                        logger.LogError(ex, "Attempt {Attempt} failed sending email '{TemplateName}' to {Recipient}.", attempt, item.TemplateName, item.RecipientEmail);
                    }

                    log.RetryCount = attempt;
                    log.ErrorMessage = errorMessage;
                    log.Status = "Retrying";
                    await dbContext.SaveChangesAsync();

                    if (attempt < maxRetries)
                    {
                        // Exponential backoff: 5s, 15s, 45s
                        int backoffSeconds = (int)Math.Pow(3, attempt) * 2; 
                        await Task.Delay(TimeSpan.FromSeconds(backoffSeconds));
                    }
                }

                if (!isSuccess)
                {
                    log.Status = "Failed";
                    log.ErrorMessage = errorMessage ?? "Unknown error occurred.";
                }

                log.UpdatedDate = DateTime.UtcNow;
                log.UpdatedBy = "SystemQueue";
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
