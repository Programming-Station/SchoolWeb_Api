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

            // 4. Seed Admission Module Templates
            if (!existingNames.Contains("Admission Application Submitted"))
            {
                context.EmailTemplates.Add(new EmailTemplate
                {
                    SchoolRegistrationId = schoolId,
                    TemplateName = "Admission Application Submitted",
                    Subject = "Admission Application Received - {{ApplicationNo}}",
                    BodyHtml = @"<div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; background-color: #0f172a; color: #f8fafc; border: 1px solid #334155; border-radius: 8px;""><h2 style=""color: #38bdf8; border-bottom: 1px solid #334155; padding-bottom: 10px;"">Application Submitted Successfully</h2><p>Dear <strong>{{CandidateName}}</strong>,</p><p>Thank you for choosing our institution! We have successfully received your admission application.</p><div style=""background-color: #1e293b; padding: 15px; border-radius: 6px; margin: 20px 0; border: 1px solid #475569;""><table style=""width: 100%; border-collapse: collapse;""><tr><td style=""color: #94a3b8; padding: 5px 0;""><strong>Application Number:</strong></td><td style=""color: #f8fafc; padding: 5px 0;"">{{ApplicationNo}}</td></tr><tr><td style=""color: #94a3b8; padding: 5px 0;""><strong>Registration Number:</strong></td><td style=""color: #f8fafc; padding: 5px 0;"">{{RegistrationNo}}</td></tr><tr><td style=""color: #94a3b8; padding: 5px 0;""><strong>Status:</strong></td><td style=""color: #38bdf8; padding: 5px 0;""><strong>Under Review</strong></td></tr></table></div><p>Our admission verification team is currently verifying your details and credentials. You can log in to the Student Panel to track your verification checklist, status updates, and next actions.</p><p style=""margin-top: 30px; font-size: 12px; color: #64748b; text-align: center;"">This is an automated notification. Please do not reply directly to this email.</p></div>",
                    Placeholder = "{{CandidateName}}, {{ApplicationNo}}, {{RegistrationNo}}",
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                });
            }

            if (!existingNames.Contains("Admission Status Updated"))
            {
                context.EmailTemplates.Add(new EmailTemplate
                {
                    SchoolRegistrationId = schoolId,
                    TemplateName = "Admission Status Updated",
                    Subject = "Admission Application Status Update - {{ApplicationNo}}",
                    BodyHtml = @"<div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; background-color: #0f172a; color: #f8fafc; border: 1px solid #334155; border-radius: 8px;""><h2 style=""color: #38bdf8; border-bottom: 1px solid #334155; padding-bottom: 10px;"">Admission Status Notification</h2><p>Dear <strong>{{CandidateName}}</strong>,</p><p>There is an update on your admission application with reference number <strong>{{ApplicationNo}}</strong>.</p><div style=""background-color: #1e293b; padding: 15px; border-radius: 6px; margin: 20px 0; border: 1px solid #475569;""><table style=""width: 100%; border-collapse: collapse;""><tr><td style=""color: #94a3b8; padding: 5px 0;""><strong>Current Status:</strong></td><td style=""color: #f8fafc; padding: 5px 0; font-size: 16px;""><strong>{{Status}}</strong></td></tr><tr><td style=""color: #94a3b8; padding: 5px 0; vertical-align: top;""><strong>Remarks:</strong></td><td style=""color: #cbd5e1; padding: 5px 0;"">{{Remarks}}</td></tr></table></div><p>Please log in to your admission portal page to view detailed information and complete any pending tasks if required.</p><p style=""margin-top: 30px; font-size: 12px; color: #64748b; text-align: center;"">This is an automated notification. Please do not reply directly to this email.</p></div>",
                    Placeholder = "{{CandidateName}}, {{ApplicationNo}}, {{Status}}, {{Remarks}}",
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
