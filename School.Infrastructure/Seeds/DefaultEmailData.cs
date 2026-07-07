using Microsoft.EntityFrameworkCore;
using School.Domain.Email;
using School.Domain.School;
using School.Utilities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Seeds
{
    /// <summary>
    /// Seeds realistic default Email data: SMTP server settings, email templates, and branding.
    /// Runs incremental (only inserts missing records, never duplicates).
    /// </summary>
    public static class DefaultEmailData
    {
        public static async Task SeedAsync(SchoolDbContext context, IEncryptionService encryptionService)
        {
            var defaultSchool = await context.SchoolRegistrations
                .FirstOrDefaultAsync(s => s.SchoolCode == "DEF001");
            int schoolId = defaultSchool?.Id ?? 1;

            // ─── 1. Email Server Settings ────────────────────────────────────────────────
            if (!await context.EmailServerSettings.AnyAsync(e => e.SchoolRegistrationId == schoolId))
            {
                var smtpSettings = new List<EmailServerSetting>
                {
                    // Primary: Gmail SMTP (app password)
                    new EmailServerSetting
                    {
                        SchoolRegistrationId = schoolId,
                        DisplayName          = "Default School - Notifications",
                        FromEmail            = "notifications@defaultschool.edu.in",
                        HostName             = "smtp.gmail.com",
                        Port                 = 587,
                        UserName             = "notifications@defaultschool.edu.in",
                        Password             = encryptionService.Encrypt("App@Password#2024"),
                        EnableSSL            = true,
                        UseDefaultCredential = false,
                        IsActive             = true,
                        CreatedBy            = "System",
                        CreatedDate          = DateTime.UtcNow
                    },
                    // Secondary: Outlook / Microsoft 365 SMTP
                    new EmailServerSetting
                    {
                        SchoolRegistrationId = schoolId,
                        DisplayName          = "Default School - Admin Alerts",
                        FromEmail            = "admin@defaultschool.edu.in",
                        HostName             = "smtp.office365.com",
                        Port                 = 587,
                        UserName             = "admin@defaultschool.edu.in",
                        Password             = encryptionService.Encrypt("Office365@Secure!"),
                        EnableSSL            = true,
                        UseDefaultCredential = false,
                        IsActive             = false,   // disabled by default; activate when needed
                        CreatedBy            = "System",
                        CreatedDate          = DateTime.UtcNow
                    }
                };

                context.EmailServerSettings.AddRange(smtpSettings);
                await context.SaveChangesAsync();
            }

            // ─── 2. Email Templates ──────────────────────────────────────────────────────
            var existingTemplateNames = await context.EmailTemplates
                .Where(t => t.SchoolRegistrationId == schoolId)
                .Select(t => t.TemplateName)
                .ToHashSetAsync();

            var templates = BuildEmailTemplates(schoolId);
            var newTemplates = templates.Where(t => !existingTemplateNames.Contains(t.TemplateName)).ToList();

            if (newTemplates.Any())
            {
                context.EmailTemplates.AddRange(newTemplates);
                await context.SaveChangesAsync();
            }

            // ─── 3. Email Branding ────────────────────────────────────────────────────────
            if (!await context.EmailBrandings.AnyAsync(b => b.SchoolRegistrationId == schoolId))
            {
                var branding = new EmailBranding
                {
                    SchoolRegistrationId = schoolId,
                    ThemeColor           = "#1e40af",   // Deep Indigo — professional school color
                    SupportEmail         = defaultSchool?.Email       ?? "support@defaultschool.edu.in",
                    SupportPhone         = defaultSchool?.PhoneNumber ?? "+91-9800000000",
                    PrincipalName        = defaultSchool?.ContactPersonName ?? "Dr. Rajesh Kumar",
                    HeaderHtml           = @"
<table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
  <tr>
    <td style=""background: linear-gradient(135deg, #1e40af 0%, #1e3a8a 100%); padding: 24px 32px; border-radius: 8px 8px 0 0;"">
      <table width=""100%"" cellpadding=""0"" cellspacing=""0"">
        <tr>
          <td style=""vertical-align: middle;"">
            <h1 style=""margin: 0; font-family: 'Segoe UI', Arial, sans-serif; font-size: 22px; font-weight: 700; color: #ffffff; letter-spacing: 0.5px;"">
              {{SchoolName}}
            </h1>
            <p style=""margin: 4px 0 0 0; font-family: Arial, sans-serif; font-size: 12px; color: #bfdbfe;"">
              Excellence in Education
            </p>
          </td>
          <td style=""text-align: right; vertical-align: middle;"">
            <span style=""display: inline-block; background: rgba(255,255,255,0.15); color: #ffffff; font-size: 11px; font-family: Arial, sans-serif; padding: 4px 12px; border-radius: 20px; border: 1px solid rgba(255,255,255,0.3);"">
              Official Communication
            </span>
          </td>
        </tr>
      </table>
    </td>
  </tr>
</table>",
                    FooterHtml = @"
<table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
  <tr>
    <td style=""background-color: #f8fafc; border-top: 3px solid #1e40af; padding: 24px 32px; border-radius: 0 0 8px 8px;"">
      <table width=""100%"" cellpadding=""0"" cellspacing=""0"">
        <tr>
          <td style=""text-align: center; padding-bottom: 12px;"">
            <p style=""margin: 0; font-family: Arial, sans-serif; font-size: 13px; color: #374151; font-weight: 600;"">
              {{SchoolName}}
            </p>
            <p style=""margin: 4px 0 0 0; font-family: Arial, sans-serif; font-size: 12px; color: #6b7280;"">
              {{SchoolAddress}}
            </p>
          </td>
        </tr>
        <tr>
          <td style=""text-align: center; padding-bottom: 12px;"">
            <a href=""mailto:{{SupportEmail}}"" style=""font-family: Arial, sans-serif; font-size: 12px; color: #1e40af; text-decoration: none;"">{{SupportEmail}}</a>
            <span style=""color: #d1d5db; margin: 0 8px;"">|</span>
            <span style=""font-family: Arial, sans-serif; font-size: 12px; color: #6b7280;"">{{SupportPhone}}</span>
          </td>
        </tr>
        <tr>
          <td style=""text-align: center;"">
            <p style=""margin: 0; font-family: Arial, sans-serif; font-size: 11px; color: #9ca3af;"">
              &copy; {{CurrentYear}} {{SchoolName}}. All rights reserved. | This is an automated email — please do not reply directly.
            </p>
          </td>
        </tr>
      </table>
    </td>
  </tr>
</table>",
                    CreatedBy   = "System",
                    CreatedDate = DateTime.UtcNow
                };

                context.EmailBrandings.Add(branding);
                await context.SaveChangesAsync();
            }
        }

        // ─── Private: Build All Email Templates ─────────────────────────────────────────
        private static List<EmailTemplate> BuildEmailTemplates(int schoolId)
        {
            // Shared inline CSS constants
            const string btnStyle = @"display:inline-block; background-color:#1e40af; color:#ffffff; font-family:Arial,sans-serif; font-size:14px; font-weight:600; text-decoration:none; padding:12px 28px; border-radius:6px; margin-top:16px;";
            const string bodyFont = "font-family:'Segoe UI',Arial,sans-serif; font-size:15px; line-height:1.7; color:#1f2937;";
            const string cardStyle = "background:#ffffff; border-radius:10px; box-shadow:0 2px 8px rgba(0,0,0,0.08); padding:32px 36px; max-width:620px; margin:0 auto;";

            string Wrap(string innerHtml) => $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width,initial-scale=1.0"">
</head>
<body style=""margin:0;padding:20px 0;background-color:#f1f5f9;"">
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
    <tr><td align=""center"" style=""padding:20px 16px;"">
      <div style=""{cardStyle}"">
        {{{{EmailHeader}}}}
        <div style=""padding-top:24px; {bodyFont}"">
          {innerHtml}
        </div>
        {{{{EmailFooter}}}}
      </div>
    </td></tr>
  </table>
</body>
</html>";

            return new List<EmailTemplate>
            {
                // ── 1. OTP Verification ─────────────────────────────────────────────
                new EmailTemplate
                {
                    SchoolRegistrationId = schoolId,
                    TemplateName         = "OTP_VERIFICATION",
                    Subject              = "{{SchoolName}} — Your OTP Code: {{Otp}}",
                    Placeholder          = "{{UserName}}, {{Otp}}, {{SchoolName}}, {{ExpiryMinutes}}",
                    IsActive             = true,
                    CreatedBy            = "System",
                    CreatedDate          = DateTime.UtcNow,
                    BodyHtml             = Wrap(@"
<p>Dear <strong>{{UserName}}</strong>,</p>
<p>Please use the one-time password (OTP) below to complete your verification. This code expires in <strong>{{ExpiryMinutes}} minutes</strong>.</p>
<div style=""text-align:center;margin:28px 0;"">
  <div style=""display:inline-block;background:#eff6ff;border:2px dashed #1e40af;border-radius:10px;padding:18px 40px;"">
    <span style=""font-size:36px;font-weight:800;letter-spacing:10px;color:#1e40af;font-family:'Courier New',monospace;"">{{Otp}}</span>
  </div>
</div>
<p style=""color:#dc2626;font-size:13px;"">⚠️ Do not share this code with anyone. Our team will never ask for your OTP.</p>
<p>If you did not request this, please ignore this email or contact our support team immediately.</p>")
                },

                // ── 2. Welcome / Account Created ────────────────────────────────────
                new EmailTemplate
                {
                    SchoolRegistrationId = schoolId,
                    TemplateName         = "ACCOUNT_WELCOME",
                    Subject              = "Welcome to {{SchoolName}} — Your Account is Ready!",
                    Placeholder          = "{{UserName}}, {{LoginUrl}}, {{TempPassword}}, {{Role}}, {{SchoolName}}",
                    IsActive             = true,
                    CreatedBy            = "System",
                    CreatedDate          = DateTime.UtcNow,
                    BodyHtml             = Wrap(@"
<p>Dear <strong>{{UserName}}</strong>,</p>
<p>Welcome to <strong>{{SchoolName}}</strong>! Your <strong>{{Role}}</strong> account has been successfully created.</p>
<table style=""background:#f8fafc;border-radius:8px;padding:20px;width:100%;border:1px solid #e2e8f0;"" cellpadding=""0"" cellspacing=""0"">
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>🔗 Login URL:</strong> <a href=""{{LoginUrl}}"" style=""color:#1e40af;"">{{LoginUrl}}</a></td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>👤 Username:</strong> {{UserName}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>🔑 Temporary Password:</strong> <code style=""background:#fef3c7;padding:2px 6px;border-radius:4px;"">{{TempPassword}}</code></td></tr>
</table>
<p style=""color:#dc2626;font-size:13px;margin-top:16px;"">🔐 Please log in and change your password immediately for security purposes.</p>
<div style=""text-align:center;""><a href=""{{LoginUrl}}"" style=""{btnStyle}"">Login to Your Account →</a></div>")
                },

                // ── 3. Password Reset ────────────────────────────────────────────────
                new EmailTemplate
                {
                    SchoolRegistrationId = schoolId,
                    TemplateName         = "PASSWORD_RESET",
                    Subject              = "{{SchoolName}} — Password Reset Request",
                    Placeholder          = "{{UserName}}, {{ResetLink}}, {{ExpiryMinutes}}, {{SchoolName}}",
                    IsActive             = true,
                    CreatedBy            = "System",
                    CreatedDate          = DateTime.UtcNow,
                    BodyHtml             = Wrap(@"
<p>Dear <strong>{{UserName}}</strong>,</p>
<p>We received a request to reset the password for your account at <strong>{{SchoolName}}</strong>.</p>
<p>Click the button below to set a new password. This link is valid for <strong>{{ExpiryMinutes}} minutes</strong>.</p>
<div style=""text-align:center;""><a href=""{{ResetLink}}"" style=""{btnStyle}"">Reset My Password →</a></div>
<p style=""font-size:13px;color:#6b7280;margin-top:20px;"">If the button doesn't work, copy and paste this URL into your browser:<br>
<a href=""{{ResetLink}}"" style=""color:#1e40af;word-break:break-all;"">{{ResetLink}}</a></p>
<p style=""color:#dc2626;font-size:13px;"">⚠️ If you did not request a password reset, you can safely ignore this email. Your password will not change.</p>")
                },

                // ── 4. Student Admission Confirmation ───────────────────────────────
                new EmailTemplate
                {
                    SchoolRegistrationId = schoolId,
                    TemplateName         = "STUDENT_ADMISSION_CONFIRMATION",
                    Subject              = "Admission Confirmed — {{StudentName}} | {{SchoolName}}",
                    Placeholder          = "{{StudentName}}, {{EnrollmentNumber}}, {{CourseName}}, {{AcademicYear}}, {{AdmissionDate}}, {{SchoolName}}, {{SupportEmail}}",
                    IsActive             = true,
                    CreatedBy            = "System",
                    CreatedDate          = DateTime.UtcNow,
                    BodyHtml             = Wrap(@"
<p>Dear Parent / Guardian of <strong>{{StudentName}}</strong>,</p>
<p>We are pleased to confirm that the admission for <strong>{{StudentName}}</strong> has been successfully processed at <strong>{{SchoolName}}</strong>.</p>
<table style=""background:#f0fdf4;border:1px solid #bbf7d0;border-radius:8px;padding:20px;width:100%;"" cellpadding=""0"" cellspacing=""0"">
  <tr><td style=""padding:5px 0;font-size:14px;""><strong>📋 Enrollment No.:</strong> {{EnrollmentNumber}}</td></tr>
  <tr><td style=""padding:5px 0;font-size:14px;""><strong>📚 Course / Class:</strong> {{CourseName}}</td></tr>
  <tr><td style=""padding:5px 0;font-size:14px;""><strong>🎓 Academic Year:</strong> {{AcademicYear}}</td></tr>
  <tr><td style=""padding:5px 0;font-size:14px;""><strong>📅 Admission Date:</strong> {{AdmissionDate}}</td></tr>
</table>
<p style=""margin-top:16px;"">Please keep the enrollment number for all future correspondence with the school. For any queries, reach us at <a href=""mailto:{{SupportEmail}}"" style=""color:#1e40af;"">{{SupportEmail}}</a>.</p>
<p>Warm regards,<br><strong>Admissions Office</strong><br>{{SchoolName}}</p>")
                },

                // ── 5. Fee Payment Receipt ───────────────────────────────────────────
                new EmailTemplate
                {
                    SchoolRegistrationId = schoolId,
                    TemplateName         = "FEE_PAYMENT_RECEIPT",
                    Subject              = "Fee Receipt #{{ReceiptNo}} — {{StudentName}} | {{SchoolName}}",
                    Placeholder          = "{{StudentName}}, {{ReceiptNo}}, {{AmountPaid}}, {{PaymentDate}}, {{PaymentMode}}, {{FeeType}}, {{AcademicYear}}, {{SchoolName}}",
                    IsActive             = true,
                    CreatedBy            = "System",
                    CreatedDate          = DateTime.UtcNow,
                    BodyHtml             = Wrap(@"
<p>Dear Parent / Guardian of <strong>{{StudentName}}</strong>,</p>
<p>Thank you! We have received your fee payment. Please find the receipt details below.</p>
<table style=""width:100%;border-collapse:collapse;margin-top:16px;"" cellpadding=""0"" cellspacing=""0"">
  <thead>
    <tr style=""background:#1e40af;color:#fff;"">
      <th style=""padding:10px 14px;text-align:left;font-family:Arial;font-size:13px;"">Detail</th>
      <th style=""padding:10px 14px;text-align:left;font-family:Arial;font-size:13px;"">Value</th>
    </tr>
  </thead>
  <tbody>
    <tr style=""background:#f8fafc;""><td style=""padding:10px 14px;font-size:13px;border-bottom:1px solid #e2e8f0;"">Receipt No.</td><td style=""padding:10px 14px;font-size:13px;border-bottom:1px solid #e2e8f0;""><strong>{{ReceiptNo}}</strong></td></tr>
    <tr><td style=""padding:10px 14px;font-size:13px;border-bottom:1px solid #e2e8f0;"">Student Name</td><td style=""padding:10px 14px;font-size:13px;border-bottom:1px solid #e2e8f0;"">{{StudentName}}</td></tr>
    <tr style=""background:#f8fafc;""><td style=""padding:10px 14px;font-size:13px;border-bottom:1px solid #e2e8f0;"">Fee Type</td><td style=""padding:10px 14px;font-size:13px;border-bottom:1px solid #e2e8f0;"">{{FeeType}}</td></tr>
    <tr><td style=""padding:10px 14px;font-size:13px;border-bottom:1px solid #e2e8f0;"">Academic Year</td><td style=""padding:10px 14px;font-size:13px;border-bottom:1px solid #e2e8f0;"">{{AcademicYear}}</td></tr>
    <tr style=""background:#f8fafc;""><td style=""padding:10px 14px;font-size:13px;border-bottom:1px solid #e2e8f0;"">Payment Mode</td><td style=""padding:10px 14px;font-size:13px;border-bottom:1px solid #e2e8f0;"">{{PaymentMode}}</td></tr>
    <tr><td style=""padding:10px 14px;font-size:13px;border-bottom:1px solid #e2e8f0;"">Date</td><td style=""padding:10px 14px;font-size:13px;border-bottom:1px solid #e2e8f0;"">{{PaymentDate}}</td></tr>
    <tr style=""background:#ecfdf5;""><td style=""padding:12px 14px;font-size:14px;font-weight:700;"">Amount Paid</td><td style=""padding:12px 14px;font-size:14px;font-weight:700;color:#065f46;"">₹ {{AmountPaid}}</td></tr>
  </tbody>
</table>
<p style=""font-size:13px;color:#6b7280;margin-top:16px;"">This is a system-generated receipt and does not require a signature. Please retain this for your records.</p>")
                },

                // ── 6. Exam Result Notification ──────────────────────────────────────
                new EmailTemplate
                {
                    SchoolRegistrationId = schoolId,
                    TemplateName         = "EXAM_RESULT_NOTIFICATION",
                    Subject              = "Exam Results Published — {{ExamName}} | {{SchoolName}}",
                    Placeholder          = "{{StudentName}}, {{ExamName}}, {{AcademicYear}}, {{TotalMarks}}, {{ObtainedMarks}}, {{Percentage}}, {{Grade}}, {{Result}}, {{SchoolName}}",
                    IsActive             = true,
                    CreatedBy            = "System",
                    CreatedDate          = DateTime.UtcNow,
                    BodyHtml             = Wrap(@"
<p>Dear Parent / Guardian of <strong>{{StudentName}}</strong>,</p>
<p>The results for <strong>{{ExamName}}</strong> ({{AcademicYear}}) have been published. Please find the summary below.</p>
<table style=""background:#f8fafc;border-radius:8px;padding:20px;width:100%;border:1px solid #e2e8f0;"" cellpadding=""0"" cellspacing=""0"">
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>🎓 Student:</strong> {{StudentName}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>📝 Exam:</strong> {{ExamName}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>📊 Total Marks:</strong> {{TotalMarks}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>✅ Obtained Marks:</strong> {{ObtainedMarks}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>📈 Percentage:</strong> {{Percentage}}%</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>🏆 Grade:</strong> {{Grade}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>🎯 Result:</strong> <span style=""color:{{ResultColor}};font-weight:700;"">{{Result}}</span></td></tr>
</table>
<p style=""margin-top:16px;"">Detailed mark sheet is available on the school portal. For any discrepancy, contact the examination department within 7 days.</p>")
                },

                // ── 7. Leave Application Approved ────────────────────────────────────
                new EmailTemplate
                {
                    SchoolRegistrationId = schoolId,
                    TemplateName         = "LEAVE_APPLICATION_APPROVED",
                    Subject              = "Leave Approved — {{EmployeeName}} | {{SchoolName}}",
                    Placeholder          = "{{EmployeeName}}, {{LeaveType}}, {{FromDate}}, {{ToDate}}, {{TotalDays}}, {{ApprovedBy}}, {{Remarks}}, {{SchoolName}}",
                    IsActive             = true,
                    CreatedBy            = "System",
                    CreatedDate          = DateTime.UtcNow,
                    BodyHtml             = Wrap(@"
<p>Dear <strong>{{EmployeeName}}</strong>,</p>
<p>Your leave application has been <strong style=""color:#16a34a;"">✅ Approved</strong>. Please find the details below.</p>
<table style=""background:#f0fdf4;border:1px solid #bbf7d0;border-radius:8px;padding:20px;width:100%;"" cellpadding=""0"" cellspacing=""0"">
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>🏷️ Leave Type:</strong> {{LeaveType}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>📅 From:</strong> {{FromDate}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>📅 To:</strong> {{ToDate}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>🗓️ Total Days:</strong> {{TotalDays}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>👤 Approved By:</strong> {{ApprovedBy}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>💬 Remarks:</strong> {{Remarks}}</td></tr>
</table>
<p style=""margin-top:16px;"">Please ensure proper handover of your responsibilities before going on leave.</p>
<p>Regards,<br><strong>HR Department</strong><br>{{SchoolName}}</p>")
                },

                // ── 8. Leave Application Rejected ────────────────────────────────────
                new EmailTemplate
                {
                    SchoolRegistrationId = schoolId,
                    TemplateName         = "LEAVE_APPLICATION_REJECTED",
                    Subject              = "Leave Request Update — {{EmployeeName}} | {{SchoolName}}",
                    Placeholder          = "{{EmployeeName}}, {{LeaveType}}, {{FromDate}}, {{ToDate}}, {{RejectedBy}}, {{Reason}}, {{SchoolName}}",
                    IsActive             = true,
                    CreatedBy            = "System",
                    CreatedDate          = DateTime.UtcNow,
                    BodyHtml             = Wrap(@"
<p>Dear <strong>{{EmployeeName}}</strong>,</p>
<p>We regret to inform you that your leave application has been <strong style=""color:#dc2626;"">❌ Rejected</strong>.</p>
<table style=""background:#fef2f2;border:1px solid #fecaca;border-radius:8px;padding:20px;width:100%;"" cellpadding=""0"" cellspacing=""0"">
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>🏷️ Leave Type:</strong> {{LeaveType}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>📅 Requested From:</strong> {{FromDate}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>📅 Requested To:</strong> {{ToDate}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>👤 Reviewed By:</strong> {{RejectedBy}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>💬 Reason:</strong> {{Reason}}</td></tr>
</table>
<p style=""margin-top:16px;"">For further clarification, please contact your reporting manager or the HR department.</p>
<p>Regards,<br><strong>HR Department</strong><br>{{SchoolName}}</p>")
                },

                // ── 9. Staff Appointment Letter ──────────────────────────────────────
                new EmailTemplate
                {
                    SchoolRegistrationId = schoolId,
                    TemplateName         = "STAFF_APPOINTMENT_LETTER",
                    Subject              = "Appointment Letter — {{EmployeeName}} | {{SchoolName}}",
                    Placeholder          = "{{EmployeeName}}, {{Designation}}, {{Department}}, {{JoiningDate}}, {{Salary}}, {{EmployeeCode}}, {{SchoolName}}, {{PrincipalName}}",
                    IsActive             = true,
                    CreatedBy            = "System",
                    CreatedDate          = DateTime.UtcNow,
                    BodyHtml             = Wrap(@"
<p>Dear <strong>{{EmployeeName}}</strong>,</p>
<p>We are delighted to offer you the position of <strong>{{Designation}}</strong> in the <strong>{{Department}}</strong> department at <strong>{{SchoolName}}</strong>.</p>
<table style=""background:#f8fafc;border:1px solid #e2e8f0;border-radius:8px;padding:20px;width:100%;"" cellpadding=""0"" cellspacing=""0"">
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>🪪 Employee Code:</strong> {{EmployeeCode}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>💼 Designation:</strong> {{Designation}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>🏢 Department:</strong> {{Department}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>📅 Date of Joining:</strong> {{JoiningDate}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>💰 Monthly Salary (CTC):</strong> ₹ {{Salary}}</td></tr>
</table>
<p style=""margin-top:16px;"">Please report to the HR department on your joining date with all required documents. We look forward to having you on our team.</p>
<p>Sincerely,<br><strong>{{PrincipalName}}</strong><br>Principal, {{SchoolName}}</p>")
                },

                // ── 10. Attendance Alert ─────────────────────────────────────────────
                new EmailTemplate
                {
                    SchoolRegistrationId = schoolId,
                    TemplateName         = "ATTENDANCE_ALERT",
                    Subject              = "Attendance Alert — {{StudentName}} | {{SchoolName}}",
                    Placeholder          = "{{StudentName}}, {{Date}}, {{Status}}, {{CourseName}}, {{AttendancePercentage}}, {{SchoolName}}, {{SupportEmail}}",
                    IsActive             = true,
                    CreatedBy            = "System",
                    CreatedDate          = DateTime.UtcNow,
                    BodyHtml             = Wrap(@"
<p>Dear Parent / Guardian of <strong>{{StudentName}}</strong>,</p>
<p>This is an automated attendance notification from <strong>{{SchoolName}}</strong>.</p>
<table style=""background:#fef3c7;border:1px solid #fde68a;border-radius:8px;padding:20px;width:100%;"" cellpadding=""0"" cellspacing=""0"">
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>🎓 Student:</strong> {{StudentName}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>📚 Class / Course:</strong> {{CourseName}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>📅 Date:</strong> {{Date}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>🔴 Status:</strong> <span style=""font-weight:700;color:#dc2626;"">{{Status}}</span></td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>📊 Overall Attendance:</strong> {{AttendancePercentage}}%</td></tr>
</table>
<p style=""margin-top:16px;font-size:13px;color:#92400e;"">⚠️ A minimum of 75% attendance is required to be eligible for examinations. Please ensure regular attendance.</p>
<p>For any concerns, contact us at <a href=""mailto:{{SupportEmail}}"" style=""color:#1e40af;"">{{SupportEmail}}</a>.</p>")
                },

                // ── 11. School Event Announcement ────────────────────────────────────
                new EmailTemplate
                {
                    SchoolRegistrationId = schoolId,
                    TemplateName         = "EVENT_ANNOUNCEMENT",
                    Subject              = "📢 {{EventName}} — {{SchoolName}}",
                    Placeholder          = "{{RecipientName}}, {{EventName}}, {{EventDate}}, {{EventTime}}, {{EventVenue}}, {{EventDescription}}, {{SchoolName}}",
                    IsActive             = true,
                    CreatedBy            = "System",
                    CreatedDate          = DateTime.UtcNow,
                    BodyHtml             = Wrap(@"
<p>Dear <strong>{{RecipientName}}</strong>,</p>
<p>We are excited to announce the upcoming event at <strong>{{SchoolName}}</strong>:</p>
<div style=""background:linear-gradient(135deg,#1e40af,#3b82f6);border-radius:10px;padding:24px;text-align:center;margin:20px 0;"">
  <h2 style=""margin:0;font-family:Arial;font-size:22px;color:#fff;font-weight:700;"">{{EventName}}</h2>
  <p style=""margin:8px 0 0 0;color:#bfdbfe;font-size:14px;"">{{EventDate}} at {{EventTime}}</p>
</div>
<table style=""background:#f8fafc;border:1px solid #e2e8f0;border-radius:8px;padding:20px;width:100%;"" cellpadding=""0"" cellspacing=""0"">
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>📍 Venue:</strong> {{EventVenue}}</td></tr>
  <tr><td style=""padding:6px 0;font-size:14px;""><strong>📝 Details:</strong> {{EventDescription}}</td></tr>
</table>
<p style=""margin-top:16px;"">Your presence is highly encouraged. Please make necessary arrangements to attend.</p>
<p>Warm regards,<br><strong>Management Team</strong><br>{{SchoolName}}</p>")
                },

                // ── 12. Holiday Notification ─────────────────────────────────────────
                new EmailTemplate
                {
                    SchoolRegistrationId = schoolId,
                    TemplateName         = "HOLIDAY_NOTIFICATION",
                    Subject              = "Holiday Notice — {{HolidayName}} | {{SchoolName}}",
                    Placeholder          = "{{RecipientName}}, {{HolidayName}}, {{HolidayDate}}, {{Reason}}, {{ResumeDate}}, {{SchoolName}}",
                    IsActive             = true,
                    CreatedBy            = "System",
                    CreatedDate          = DateTime.UtcNow,
                    BodyHtml             = Wrap(@"
<p>Dear <strong>{{RecipientName}}</strong>,</p>
<p>Please be informed that <strong>{{SchoolName}}</strong> will remain closed on the following date:</p>
<div style=""background:#f0f9ff;border-left:4px solid #0ea5e9;border-radius:4px;padding:16px 20px;margin:20px 0;"">
  <p style=""margin:0;font-size:15px;""><strong>🎉 Holiday:</strong> {{HolidayName}}</p>
  <p style=""margin:6px 0 0 0;font-size:14px;""><strong>📅 Date:</strong> {{HolidayDate}}</p>
  <p style=""margin:6px 0 0 0;font-size:14px;""><strong>📌 Reason:</strong> {{Reason}}</p>
  <p style=""margin:6px 0 0 0;font-size:14px;""><strong>🔄 School Resumes:</strong> {{ResumeDate}}</p>
</div>
<p>We wish you a pleasant holiday. Please plan your work accordingly.</p>
<p>Regards,<br><strong>Administration</strong><br>{{SchoolName}}</p>")
                }
            };
        }
    }
}
