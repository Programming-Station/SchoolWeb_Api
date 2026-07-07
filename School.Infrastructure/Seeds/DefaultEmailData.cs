using Microsoft.EntityFrameworkCore;
using School.Domain.Email;
using School.Utilities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Seeds
{
    public static class DefaultEmailData
    {
        public static async Task SeedAsync(SchoolDbContext context, IEncryptionService encryptionService)
        {
            var defaultSchool = await context.SchoolRegistrations.FirstOrDefaultAsync(s => s.SchoolCode == "DPSVAR001") 
                                ?? await context.SchoolRegistrations.FirstOrDefaultAsync(s => s.SchoolCode == "DEF001") 
                                ?? await context.SchoolRegistrations.FirstOrDefaultAsync();
            int schoolId = defaultSchool?.Id ?? 1;

            // 1. Seed SMTP Server Settings
            if (!await context.EmailServerSettings.AnyAsync(e => e.SchoolRegistrationId == schoolId))
            {
                var smtpSettings = new List<EmailServerSetting>
                {
                    new EmailServerSetting
                    {
                        SchoolRegistrationId = schoolId,
                        DisplayName          = "SchoolSaaas Support",
                        FromEmail            = "titwig365@gmail.com",
                        HostName             = "smtp.gmail.com",
                        Port                 = 587,
                        UserName             = "titwig365@gmail.com",
                        Password             = encryptionService.Encrypt("koglixeavvrnmsle"),
                        EnableSSL            = true,
                        UseDefaultCredential = false,
                        IsActive             = true,
                        CreatedBy            = "System",
                        CreatedDate          = DateTime.UtcNow,
                        UpdatedDate          = DateTime.UtcNow
                    }
                };

                context.EmailServerSettings.AddRange(smtpSettings);
                await context.SaveChangesAsync();
            }

            // 2. Seed Email Branding
            if (!await context.EmailBrandings.AnyAsync(b => b.SchoolRegistrationId == schoolId))
            {
                var branding = new EmailBranding
                {
                    SchoolRegistrationId = schoolId,
                    ThemeColor           = "#1e40af", // Deep Indigo
                    SupportEmail         = defaultSchool?.Email ?? "support@schoolsaas.com",
                    SupportPhone         = defaultSchool?.PhoneNumber ?? "9876543210",
                    PrincipalName        = defaultSchool?.ContactPersonName ?? "Principal",
                    HeaderHtml           = @"<div style=""background-color: #1e40af; padding: 20px; text-align: center; border-radius: 4px 4px 0 0;""><h1 style=""color: #ffffff; margin: 0; font-family: Arial, sans-serif;"">{{SchoolName}}</h1></div>",
                    FooterHtml           = @"<div style=""background-color: #f3f4f6; padding: 20px; text-align: center; color: #6b7280; font-size: 12px; border-radius: 0 0 4px 4px; border-top: 1px solid #e5e7eb;""><p style=""margin: 0 0 8px 0;"">This is an automated notification from {{SchoolName}}.</p><p style=""margin: 0;"">{{SchoolAddress}} | Support: {{SupportEmail}} | Phone: {{SupportPhone}}</p><p style=""margin: 8px 0 0 0;"">&copy; {{CurrentYear}} {{SchoolName}}. All rights reserved.</p></div>",
                    CreatedBy            = "System",
                    CreatedDate          = DateTime.UtcNow
                };

                context.EmailBrandings.Add(branding);
                await context.SaveChangesAsync();
            }

            // 3. Seed Email Templates incrementally if missing
            var existingNames = await context.EmailTemplates
                .Where(t => t.SchoolRegistrationId == schoolId)
                .Select(t => t.TemplateName)
                .ToHashSetAsync();

            var allTemplates = DefaultEmailTemplate.GetAllEmailTemplate();
            var newTemplates = allTemplates.Where(t => !existingNames.Contains(t.TemplateName)).ToList();

            if (newTemplates.Any())
            {
                foreach (var template in newTemplates)
                {
                    template.SchoolRegistrationId = schoolId;
                }
                context.EmailTemplates.AddRange(newTemplates);
                await context.SaveChangesAsync();
            }
        }
    }
}
