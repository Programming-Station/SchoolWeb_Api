using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using School.Domain.Email;

namespace School.Infrastructure.Email
{
    public class SmtpEmailProvider
    {
        private readonly ILogger<SmtpEmailProvider> _logger;

        public SmtpEmailProvider(ILogger<SmtpEmailProvider> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailServerSetting setting, string recipientEmail, string subject, string bodyHtml, byte[]? attachmentBytes = null, string? attachmentName = null)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(nameof(setting), "SMTP configuration is not available.");
            }

            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(recipientEmail));
                
                string fromDisplayName = !string.IsNullOrWhiteSpace(setting.DisplayName) 
                     ? setting.DisplayName 
                     : setting.FromEmail;
                
                message.From = new MailAddress(setting.FromEmail, fromDisplayName);
                message.Subject = subject;
                message.Body = bodyHtml;
                message.IsBodyHtml = true;

                if (attachmentBytes != null && attachmentBytes.Length > 0 && !string.IsNullOrEmpty(attachmentName))
                {
                    message.Attachments.Add(new Attachment(new MemoryStream(attachmentBytes), attachmentName, "application/pdf"));
                }

                using (var client = new SmtpClient(setting.HostName, setting.Port))
                {
                    client.EnableSsl = setting.EnableSSL;
                    
                    if (setting.UseDefaultCredential)
                    {
                        client.UseDefaultCredentials = true;
                    }
                    else
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(setting.UserName, setting.Password);
                    }

                    // Security check: Log host and port, but never log credentials or password.
                    _logger.LogInformation("Sending SMTP email from {FromEmail} to {Recipient} via {Host}:{Port} (SSL={SSL})", 
                        setting.FromEmail, recipientEmail, setting.HostName, setting.Port, setting.EnableSSL);

                    await client.SendMailAsync(message);
                }
            }
        }
    }
}
