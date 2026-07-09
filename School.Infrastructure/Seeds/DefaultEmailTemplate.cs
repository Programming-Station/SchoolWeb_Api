using School.Domain.Email;
namespace School.Infrastructure.Seeds
{
    public static class DefaultEmailTemplate
    {
        public static List<EmailTemplate> GetAllEmailTemplate()
        {
            var templates = new List<EmailTemplate>()
            {
                new EmailTemplate
                {
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System",
                    BodyHtml = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>CIPC Paramedical Council OTP</title>
</head>
<body style=""margin: 0; padding: 0; background-color: #F2F2F2; font-family: 'Barlow', Arial, sans-serif;"">
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #F2F2F2;"">
        <tr>
            <td align=""center"" style=""padding: 20px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; background-color: #FFFFFF; border-bottom: 5px solid #30b524; border-radius: 4px; overflow: hidden;"">
                    <!-- Body Content -->
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        Dear {{UserName}},
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        Thank you for choosing <strong>CIPC Paramedical Council</strong>!
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 30px;"">
                                        You have requested to unsubscribe from our service notifications. Please confirm your request by entering the following 6-digit OTP:
                                    </td>
                                </tr>
                                <tr>
                                    <td align=""center"" style=""padding-bottom: 30px;"">
                                        <table border=""0"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #EAF8EB; border-radius: 6px;"">
                                            <tr>
                                                <td align=""center"" style=""padding: 15px 40px; font-size: 32px; font-weight: bold; color: #30b524; letter-spacing: 5px;"">
                                                    {{Otp}}
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #555555; font-size: 16px; line-height: 24px; padding-bottom: 20px;"">
                                        This OTP is valid for 5 minutes. If you did not initiate this request, please ignore this email.
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 16px; line-height: 24px; padding-bottom: 20px;"">
                                        If you have any questions, feel free to contact us at <a href=""mailto:support@cipcvns.com"" style=""color: #30b524; text-decoration: none; font-weight: bold;"">support@cipcvns.com</a>.
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 16px; line-height: 24px;"">
                                        Best Regards,<br>
                                        <strong>CIPC Paramedical Council</strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <!-- Footer -->
        <tr>
            <td align=""center"" style=""padding: 20px 0 40px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; text-align: center;"">
                    <tr>
                        <td style=""color: #9B9B9B; font-size: 13px; line-height: 18px; padding-bottom: 10px;"">
                            We respect your privacy and promise never to share your data with any third party.
                        </td>
                    </tr>
                    <tr>
                        <td style=""color: #9B9B9B; font-size: 13px; line-height: 18px;"">
                            CIPC Paramedical Council<br>
                            Varanasi, Uttar Pradesh.<br>
                            <a href=""https://www.cipcvns.org"" style=""color: #7E7E7E; text-decoration: underline;"">cipcvns.org</a> | 
                            <a href=""https://www.cipcvns.org/privacy-policy"" style=""color: #7E7E7E; text-decoration: underline;"">Privacy Policy</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>",
                    EmailServerSettingId = 1,
                    IsActive = true,
                    IsDeleted = false,
                    Placeholder="OTP",
                    Subject="Your OTP for CIPC Paramedical Council {{OtpSendFrom}}",
                    TemplateName="Otp",
                    UpdatedBy= "System",
                },
                new EmailTemplate
                {
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System",
                    BodyHtml = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Welcome to CIPC Paramedical Council</title>
</head>
<body style=""margin: 0; padding: 0; background-color: #F2F2F2; font-family: 'Barlow', Arial, sans-serif;"">
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #F2F2F2;"">
        <tr>
            <td align=""center"" style=""padding: 20px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; background-color: #FFFFFF; border-bottom: 5px solid #30b524; border-radius: 4px; overflow: hidden;"">
                    <!-- Body Content -->
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        Dear {{UserName}},
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        Thank you for choosing <strong>CIPC Paramedical Council</strong>!
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 30px;"">
                                        Your account has been successfully created and is currently <strong>under {{Status}}</strong>. You will be notified once it is activated.
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""background-color: #f9f9f9; padding: 20px; border-radius: 6px; margin-bottom: 30px;"">
                                        <h3 style=""margin: 0 0 10px 0; color: #30b524; font-size: 18px;"">Login Details:</h3>
                                        <p style=""margin: 5px 0; color: #333; font-size: 16px;""><strong>User ID:</strong> {{UserId}}</p>
                                        <p style=""margin: 5px 0; color: #333; font-size: 16px;""><strong>Password:</strong> {{Password}}</p>
                                    </td>
                                </tr>
                                <tr>
                                    <td align=""center"" style=""padding: 30px 0;"">
                                        <a href=""{{PortalUrl}}"" target=""_blank"" style=""background-color: #4CAF50; color: #ffffff; padding: 15px 30px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block;"">Go to Web Portal</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 16px; line-height: 24px; padding-bottom: 20px;"">
                                        If you have any questions, feel free to email us at <a href=""mailto:support@cipcvns.com"" style=""color: #30b524; text-decoration: none; font-weight: bold;"">support@cipcvns.com</a>.
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 16px; line-height: 24px;"">
                                        Best Regards,<br>
                                        <strong>CIPC Paramedical Council</strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <!-- Footer -->
        <tr>
            <td align=""center"" style=""padding: 20px 0 40px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; text-align: center;"">
                    <tr>
                        <td style=""color: #9B9B9B; font-size: 13px; line-height: 18px; padding-bottom: 10px;"">
                            We respect your privacy and promise never to share your data with any third party.
                        </td>
                    </tr>
                    <tr>
                        <td style=""color: #9B9B9B; font-size: 13px; line-height: 18px;"">
                            CIPC Paramedical Council<br>
                            Varanasi, Uttar Pradesh.<br>
                            <a href=""https://www.cipcvns.org"" style=""color: #7E7E7E; text-decoration: underline;"">cipcvns.org</a> | 
                            <a href=""https://www.cipcvns.org/privacy-policy"" style=""color: #7E7E7E; text-decoration: underline;"">Privacy Policy</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>",
                    EmailServerSettingId = 1,
                    IsActive = true,
                    IsDeleted = false,
                    Placeholder="Welcome",
                    Subject="Welcome to CIPC Paramedical Council! Your account is under approval",
                    TemplateName="Created",
                    UpdatedBy= "System",
                },
                new EmailTemplate
                {
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System",
                    BodyHtml = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <title>Password Reset</title>
    <style>
        body {
            margin: 0;
            padding: 0;
            background-color: #F2F2F2;
            font-family: Arial, Helvetica, sans-serif;
        }
        table {
            border-collapse: collapse;
        }
        p {
            font-size: 16px;
            line-height: 26px;
            color: #1D1D1D;
            margin: 0 0 12px;
        }
        a {
            color: #30b524;
            text-decoration: none;
        }
    </style>
</head>

<body>
    <table width=""100%"" bgcolor=""#F2F2F2"" cellpadding=""0"" cellspacing=""0"">
        <tr>
            <td align=""center"">

                <table width=""600"" bgcolor=""#FFFFFF"" cellpadding=""0"" cellspacing=""0""
                       style=""max-width:600px;border-bottom:5px solid #30b524;"">
                    <tr>
                        <td style=""padding:20px;"">

                            <p>Dear <strong>{UserName}</strong>,</p>

                            <p>
                                We received a request to reset your account password.
                                Please click the button below to set a new password.
                            </p>

                            <p style=""text-align:center; margin:20px 0;"">
                                <a href=""{ResetLink}"" target=""_blank""
                                   style=""background:#4CAF50;color:#ffffff;padding:12px 25px;
                                          border-radius:5px;display:inline-block;"">
                                    Reset Password
                                </a>
                            </p>

                            <p>
                                This link is valid for a limited time.
                                If you did not request a password reset, please ignore this email.
                            </p>

                            <p>
                                For any assistance, contact us at
                                <a href=""mailto:{SupportEmail}"">{SupportEmail}</a>.
                            </p>

                            <p>
                                Best Regards,<br>
                                <strong>CIPC Paramedical Council</strong>
                            </p>

                        </td>
                    </tr>
                </table>

                <table width=""600"" cellpadding=""0"" cellspacing=""0""
                       style=""max-width:600px;margin-top:20px;"">
                    <tr>
                        <td align=""center"" style=""font-size:13px;color:#9B9B9B;padding:10px;"">
                            <p>
                                CIPC Paramedical Council<br>
                                Varanasi, Uttar Pradesh
                            </p>
                            <p>
                                <a href=""https://www.cipcvns.org"" target=""_blank"">cipcvns.org</a>
                            </p>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
    </table>
</body>
</html>",
                    EmailServerSettingId = 1,
                    IsActive = true,
                    IsDeleted = false,
                    Placeholder="{UserName}, {ResetLink}, {SupportEmail}",
                    Subject="Reset Your Password â€“ CIPC Paramedical Council",
                    TemplateName="Forgot Password",
                    UpdatedBy= "System",
                    },

                // HelpDesk - Ticket Created
                new EmailTemplate
                {
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System",
                    BodyHtml = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>HelpDesk Ticket Created - CIPC</title>
</head>
<body style=""margin: 0; padding: 0; background-color: #F2F2F2; font-family: 'Barlow', Arial, sans-serif;"">
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #F2F2F2;"">
        <tr>
            <td align=""center"" style=""padding: 20px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; background-color: #FFFFFF; border-bottom: 5px solid #30b524; border-radius: 4px; overflow: hidden;"">
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        Dear {{UserName}},
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        Your support ticket has been created successfully.
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""background-color: #f9f9f9; padding: 20px; border-radius: 6px; margin-bottom: 20px;"">
                                        <h3 style=""margin: 0 0 15px 0; color: #30b524; font-size: 18px;"">Ticket Details:</h3>
                                        <p style=""margin: 5px 0; color: #333; font-size: 16px;""><strong>Ticket #:</strong> {{TicketNumber}}</p>
                                        <p style=""margin: 5px 0; color: #333; font-size: 16px;""><strong>Subject:</strong> {{Subject}}</p>
                                        <p style=""margin: 5px 0; color: #333; font-size: 16px;""><strong>Category:</strong> {{Category}}</p>
                                        <p style=""margin: 5px 0; color: #333; font-size: 16px;""><strong>Priority:</strong> {{Priority}}</p>
                                        <p style=""margin: 5px 0; color: #333; font-size: 16px;""><strong>Due Date:</strong> {{DueDate}}</p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #555555; font-size: 16px; line-height: 24px; padding-bottom: 20px;"">
                                        You will receive email updates when the status changes.
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 16px; line-height: 24px;"">
                                        Best Regards,<br>
                                        <strong>CIPC HelpDesk</strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align=""center"" style=""padding: 20px 0 40px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; text-align: center;"">
                    <tr>
                        <td style=""color: #9B9B9B; font-size: 13px; line-height: 18px;"">
                            CIPC Paramedical Council | Varanasi, Uttar Pradesh | <a href=""https://www.cipcvns.org"" style=""color: #7E7E7E; text-decoration: underline;"">cipcvns.org</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>",
                    EmailServerSettingId = 1,
                    IsActive = true,
                    IsDeleted = false,
                    Placeholder = "{{UserName}}, {{TicketNumber}}, {{Subject}}, {{Category}}, {{Priority}}, {{DueDate}}",
                    Subject = "HelpDesk Ticket Created: {{TicketNumber}} - {{Subject}}",
                    TemplateName = "HelpDeskTicketCreated",
                    UpdatedBy = "System",
                },

                // HelpDesk - Status Updated
                new EmailTemplate
                {
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System",
                    BodyHtml = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>HelpDesk Ticket Status Updated - CIPC</title>
</head>
<body style=""margin: 0; padding: 0; background-color: #F2F2F2; font-family: 'Barlow', Arial, sans-serif;"">
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #F2F2F2;"">
        <tr>
            <td align=""center"" style=""padding: 20px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; background-color: #FFFFFF; border-bottom: 5px solid #30b524; border-radius: 4px; overflow: hidden;"">
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        Dear {{UserName}},
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        Your support ticket <strong>{{TicketNumber}}</strong> has been updated.
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""background-color: #f9f9f9; padding: 20px; border-radius: 6px; margin-bottom: 20px;"">
                                        <h3 style=""margin: 0 0 15px 0; color: #30b524; font-size: 18px;"">Update Details:</h3>
                                        <p style=""margin: 5px 0; color: #333; font-size: 16px;""><strong>Subject:</strong> {{Subject}}</p>
                                        <p style=""margin: 5px 0; color: #333; font-size: 16px;""><strong>New Status:</strong> {{StatusName}}</p>
                                        {{ResolutionBlock}}
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 16px; line-height: 24px;"">
                                        Best Regards,<br>
                                        <strong>CIPC HelpDesk</strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align=""center"" style=""padding: 20px 0 40px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; text-align: center;"">
                    <tr>
                        <td style=""color: #9B9B9B; font-size: 13px; line-height: 18px;"">
                            CIPC Paramedical Council | Varanasi, Uttar Pradesh | <a href=""https://www.cipcvns.org"" style=""color: #7E7E7E; text-decoration: underline;"">cipcvns.org</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>",
                    EmailServerSettingId = 1,
                    IsActive = true,
                    IsDeleted = false,
                    Placeholder = "{{UserName}}, {{TicketNumber}}, {{Subject}}, {{StatusName}}, {{ResolutionBlock}}",
                    Subject = "HelpDesk Ticket {{TicketNumber}} - Status: {{StatusName}}",
                    TemplateName = "HelpDeskStatusUpdated",
                    UpdatedBy = "System",
                },

                // HelpDesk - Escalation
                new EmailTemplate
                {
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System",
                    BodyHtml = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>HelpDesk Ticket Escalated - CIPC</title>
</head>
<body style=""margin: 0; padding: 0; background-color: #F2F2F2; font-family: 'Barlow', Arial, sans-serif;"">
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #F2F2F2;"">
        <tr>
            <td align=""center"" style=""padding: 20px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; background-color: #FFFFFF; border-bottom: 5px solid #30b524; border-radius: 4px; overflow: hidden;"">
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        Dear {{UserName}},
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        A support ticket has been escalated and assigned to you.
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""background-color: #f9f9f9; padding: 20px; border-radius: 6px; margin-bottom: 20px;"">
                                        <h3 style=""margin: 0 0 15px 0; color: #30b524; font-size: 18px;"">Ticket Details:</h3>
                                        <p style=""margin: 5px 0; color: #333; font-size: 16px;""><strong>Ticket #:</strong> {{TicketNumber}}</p>
                                        <p style=""margin: 5px 0; color: #333; font-size: 16px;""><strong>Subject:</strong> {{Subject}}</p>
                                        <p style=""margin: 5px 0; color: #333; font-size: 16px;""><strong>Escalation Level:</strong> {{EscalationLevel}}</p>
                                        {{ReasonBlock}}
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #555555; font-size: 16px; line-height: 24px; padding-bottom: 20px;"">
                                        Please review and take appropriate action.
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 16px; line-height: 24px;"">
                                        Best Regards,<br>
                                        <strong>CIPC HelpDesk</strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align=""center"" style=""padding: 20px 0 40px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; text-align: center;"">
                    <tr>
                        <td style=""color: #9B9B9B; font-size: 13px; line-height: 18px;"">
                            CIPC Paramedical Council | Varanasi, Uttar Pradesh | <a href=""https://www.cipcvns.org"" style=""color: #7E7E7E; text-decoration: underline;"">cipcvns.org</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>",
                    EmailServerSettingId = 1,
                    IsActive = true,
                    IsDeleted = false,
                    Placeholder = "{{UserName}}, {{TicketNumber}}, {{Subject}}, {{EscalationLevel}}, {{ReasonBlock}}",
                    Subject = "HelpDesk Escalation: Ticket {{TicketNumber}} Assigned to You",
                    TemplateName = "HelpDeskEscalation",
                    UpdatedBy = "System",
                },
                
                // Registration Rejected
                new EmailTemplate
                {
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System",
                    BodyHtml = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Registration Rejected - CIPC</title>
</head>
<body style=""margin: 0; padding: 0; background-color: #F2F2F2; font-family: 'Barlow', Arial, sans-serif;"">
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #F2F2F2;"">
        <tr>
            <td align=""center"" style=""padding: 20px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; background-color: #FFFFFF; border-bottom: 5px solid #d32f2f; border-radius: 4px; overflow: hidden;"">
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        Dear {{StudentName}},
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        Your registration has been rejected due to the following reason:
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""background-color: #fef2f2; padding: 20px; border-radius: 6px; margin-bottom: 20px; border-left: 4px solid #d32f2f;"">
                                        <p style=""margin: 0; color: #d32f2f; font-size: 16px; font-weight: bold;"">{{Reason}}</p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #555555; font-size: 16px; line-height: 24px; padding-bottom: 20px;"">
                                        Please click the link below to edit and resubmit your application:
                                    </td>
                                </tr>
                                <tr>
                                    <td align=""center"" style=""padding: 20px 0;"">
                                        <a href=""{{EditLink}}"" target=""_blank"" style=""background-color: #4CAF50; color: #ffffff; padding: 15px 30px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block;"">Edit Application</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #555555; font-size: 14px; line-height: 20px; padding-bottom: 20px;"">
                                        If the button doesn't work, copy and paste this link into your browser:<br>
                                        <a href=""{{EditLink}}"" style=""color: #2563eb; word-break: break-all;"">{{EditLink}}</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 16px; line-height: 24px;"">
                                        Best Regards,<br>
                                        <strong>CIPC Paramedical Council</strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align=""center"" style=""padding: 20px 0 40px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; text-align: center;"">
                    <tr>
                        <td style=""color: #9B9B9B; font-size: 13px; line-height: 18px;"">
                            CIPC Paramedical Council | Varanasi, Uttar Pradesh | <a href=""https://www.cipcvns.org"" style=""color: #7E7E7E; text-decoration: underline;"">cipcvns.org</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>",
                    EmailServerSettingId = 1,
                    IsActive = true,
                    IsDeleted = false,
                    Placeholder = "{{StudentName}}, {{Reason}}, {{EditLink}}",
                    Subject = "Registration Rejected - Action Required",
                    TemplateName = "RegistrationRejected",
                    UpdatedBy = "System",
                },

                // ============================================
                // Birthday Email Templates (5 rotating designs)
                // ============================================

                // Birthday Template 1 - Classic Celebration (Gold/Warm)
                new EmailTemplate
                {
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System",
                    BodyHtml = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Happy Birthday! - CIPC</title>
</head>
<body style=""margin: 0; padding: 0; background-color: #FFF8E1; font-family: 'Barlow', Arial, sans-serif;"">
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #FFF8E1;"">
        <tr>
            <td align=""center"" style=""padding: 20px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; background-color: #FFFFFF; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 15px rgba(0,0,0,0.1);"">
                    <!-- Header with gradient -->
                    <tr>
                        <td style=""background: linear-gradient(135deg, #FF9800, #FFB74D); padding: 40px 30px; text-align: center;"">
                            <div style=""font-size: 60px; margin-bottom: 10px;"">ðŸŽ‚</div>
                            <h1 style=""color: #FFFFFF; margin: 0; font-size: 32px; text-shadow: 1px 1px 3px rgba(0,0,0,0.2);"">Happy Birthday!</h1>
                        </td>
                    </tr>
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        Dear <strong>{{UserName}}</strong>,
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #555; font-size: 16px; line-height: 26px; padding-bottom: 20px;"">
                                        ðŸŽ‰ Wishing you a very <strong>Happy Birthday</strong> from the entire <strong>CIPC Paramedical Council</strong> family!
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""background: linear-gradient(135deg, #FFF3E0, #FFE0B2); padding: 25px; border-radius: 10px; text-align: center; margin-bottom: 20px;"">
                                        <p style=""color: #E65100; font-size: 18px; font-style: italic; margin: 0; line-height: 28px;"">
                                            ""May this special day bring you endless joy, good health, and all the success you deserve. Your dedication to the field of paramedical sciences inspires us all.""
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #555; font-size: 16px; line-height: 26px; padding-top: 20px; padding-bottom: 20px;"">
                                        May this year ahead be filled with wonderful achievements and memorable moments. Have a fantastic celebration! ðŸŽˆðŸŽ
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 16px; line-height: 24px;"">
                                        Warm Wishes,<br>
                                        <strong>CIPC Paramedical Council</strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align=""center"" style=""padding: 20px 0 40px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; text-align: center;"">
                    <tr>
                        <td style=""color: #9B9B9B; font-size: 13px; line-height: 18px;"">
                            CIPC Paramedical Council | Varanasi, Uttar Pradesh | <a href=""https://www.cipcvns.org"" style=""color: #7E7E7E; text-decoration: underline;"">cipcvns.org</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>",
                    EmailServerSettingId = 1,
                    IsActive = true,
                    IsDeleted = false,
                    Placeholder = "{{UserName}}",
                    Subject = "ðŸŽ‚ Happy Birthday, {{UserName}}! â€“ CIPC Paramedical Council",
                    TemplateName = "Birthday_1",
                    UpdatedBy = "System",
                },

                // Birthday Template 2 - Starry Night (Dark Blue/Inspirational)
                new EmailTemplate
                {
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System",
                    BodyHtml = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Happy Birthday! - CIPC</title>
</head>
<body style=""margin: 0; padding: 0; background-color: #0D1B2A; font-family: 'Barlow', Arial, sans-serif;"">
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #0D1B2A;"">
        <tr>
            <td align=""center"" style=""padding: 20px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; background-color: #1B2838; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 20px rgba(0,0,0,0.3);"">
                    <tr>
                        <td style=""background: linear-gradient(135deg, #1A237E, #283593); padding: 40px 30px; text-align: center;"">
                            <div style=""font-size: 50px; margin-bottom: 10px;"">ðŸŒŸâœ¨ðŸŒŸ</div>
                            <h1 style=""color: #FFD54F; margin: 0; font-size: 32px;"">Happy Birthday!</h1>
                            <p style=""color: #90CAF9; font-size: 14px; margin: 10px 0 0 0;"">A Star is Celebrating Today â­</p>
                        </td>
                    </tr>
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                    <td style=""color: #E3F2FD; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        Dear <strong style=""color: #FFD54F;"">{{UserName}}</strong>,
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #B0BEC5; font-size: 16px; line-height: 26px; padding-bottom: 20px;"">
                                        On this special day, the entire <strong style=""color: #E3F2FD;"">CIPC Paramedical Council</strong> family sends you heartfelt birthday wishes! ðŸŽ‰
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""background-color: #263238; padding: 25px; border-radius: 10px; border-left: 4px solid #FFD54F; text-align: center;"">
                                        <p style=""color: #FFD54F; font-size: 18px; font-style: italic; margin: 0; line-height: 28px;"">
                                            ""The future belongs to those who believe in the beauty of their dreams. Keep shining bright like the star you are!""
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #B0BEC5; font-size: 16px; line-height: 26px; padding-top: 20px; padding-bottom: 20px;"">
                                        May your birthday be as bright as the stars in the sky. Wishing you a year full of new discoveries, growth, and happiness! ðŸŒ™
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #E3F2FD; font-size: 16px; line-height: 24px;"">
                                        With Best Wishes,<br>
                                        <strong style=""color: #FFD54F;"">CIPC Paramedical Council</strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align=""center"" style=""padding: 20px 0 40px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; text-align: center;"">
                    <tr>
                        <td style=""color: #546E7A; font-size: 13px; line-height: 18px;"">
                            CIPC Paramedical Council | Varanasi, Uttar Pradesh | <a href=""https://www.cipcvns.org"" style=""color: #78909C; text-decoration: underline;"">cipcvns.org</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>",
                    EmailServerSettingId = 1,
                    IsActive = true,
                    IsDeleted = false,
                    Placeholder = "{{UserName}}",
                    Subject = "ðŸŒŸ Happy Birthday, {{UserName}}! â€“ CIPC Paramedical Council",
                    TemplateName = "Birthday_2",
                    UpdatedBy = "System",
                },

                // Birthday Template 3 - Balloon Party (Colorful/Playful)
                new EmailTemplate
                {
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System",
                    BodyHtml = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Happy Birthday! - CIPC</title>
</head>
<body style=""margin: 0; padding: 0; background-color: #F3E5F5; font-family: 'Barlow', Arial, sans-serif;"">
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #F3E5F5;"">
        <tr>
            <td align=""center"" style=""padding: 20px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; background-color: #FFFFFF; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 15px rgba(0,0,0,0.1);"">
                    <tr>
                        <td style=""background: linear-gradient(135deg, #E91E63, #9C27B0, #2196F3); padding: 40px 30px; text-align: center;"">
                            <div style=""font-size: 50px; margin-bottom: 10px;"">ðŸŽˆðŸŽ‰ðŸŽˆ</div>
                            <h1 style=""color: #FFFFFF; margin: 0; font-size: 32px;"">It's Party Time!</h1>
                            <p style=""color: #F8BBD0; font-size: 16px; margin: 10px 0 0 0;"">Happy Birthday to You! ðŸ¥³</p>
                        </td>
                    </tr>
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        Dear <strong style=""color: #E91E63;"">{{UserName}}</strong>,
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #555; font-size: 16px; line-height: 26px; padding-bottom: 20px;"">
                                        ðŸŽŠ Hip Hip Hooray! It's your special day! The <strong>CIPC Paramedical Council</strong> family is thrilled to celebrate YOU today!
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""background: linear-gradient(135deg, #FCE4EC, #F3E5F5, #E3F2FD); padding: 25px; border-radius: 10px; text-align: center;"">
                                        <p style=""color: #7B1FA2; font-size: 18px; margin: 0; line-height: 28px;"">
                                            ðŸŽ <strong>May your birthday be sprinkled with fun, laughter, and lots of cake!</strong> ðŸŽ
                                        </p>
                                        <p style=""color: #AD1457; font-size: 16px; margin: 15px 0 0 0;"">
                                            You bring so much energy and positivity to our community. Keep being awesome! ðŸŒˆ
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #555; font-size: 16px; line-height: 26px; padding-top: 20px; padding-bottom: 20px;"">
                                        Here's to another amazing year of growth, learning, and wonderful memories! Enjoy every moment of your day! ðŸŽ¶ðŸŽ‚
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 16px; line-height: 24px;"">
                                        With Love & Cheers,<br>
                                        <strong style=""color: #9C27B0;"">CIPC Paramedical Council</strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align=""center"" style=""padding: 20px 0 40px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; text-align: center;"">
                    <tr>
                        <td style=""color: #9B9B9B; font-size: 13px; line-height: 18px;"">
                            CIPC Paramedical Council | Varanasi, Uttar Pradesh | <a href=""https://www.cipcvns.org"" style=""color: #7E7E7E; text-decoration: underline;"">cipcvns.org</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>",
                    EmailServerSettingId = 1,
                    IsActive = true,
                    IsDeleted = false,
                    Placeholder = "{{UserName}}",
                    Subject = "ðŸŽˆ Happy Birthday, {{UserName}}! Party Time! â€“ CIPC",
                    TemplateName = "Birthday_3",
                    UpdatedBy = "System",
                },

                // Birthday Template 4 - Floral Elegance (Pink/Purple)
                new EmailTemplate
                {
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System",
                    BodyHtml = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Happy Birthday! - CIPC</title>
</head>
<body style=""margin: 0; padding: 0; background-color: #FDF2F8; font-family: 'Barlow', Arial, sans-serif;"">
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #FDF2F8;"">
        <tr>
            <td align=""center"" style=""padding: 20px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; background-color: #FFFFFF; border-radius: 12px; overflow: hidden; border: 2px solid #F8BBD0; box-shadow: 0 4px 15px rgba(0,0,0,0.08);"">
                    <tr>
                        <td style=""background: linear-gradient(135deg, #EC407A, #AB47BC); padding: 40px 30px; text-align: center;"">
                            <div style=""font-size: 50px; margin-bottom: 10px;"">ðŸŒ¸ðŸŒºðŸŒ¸</div>
                            <h1 style=""color: #FFFFFF; margin: 0; font-size: 32px;"">Happy Birthday!</h1>
                            <p style=""color: #F8BBD0; font-size: 14px; margin: 10px 0 0 0;"">Blooming with Joy on Your Special Day ðŸŒ·</p>
                        </td>
                    </tr>
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        Dear <strong style=""color: #AD1457;"">{{UserName}}</strong>,
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #555; font-size: 16px; line-height: 26px; padding-bottom: 20px;"">
                                        ðŸŒ¹ On the beautiful occasion of your birthday, the <strong>CIPC Paramedical Council</strong> family extends warm and heartfelt wishes to you.
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""background-color: #FDF2F8; padding: 25px; border-radius: 10px; border-left: 4px solid #EC407A;"">
                                        <p style=""color: #880E4F; font-size: 18px; font-style: italic; margin: 0; line-height: 28px;"">
                                            ""Like flowers that bloom in spring, may your life blossom with happiness, love, and endless possibilities.""
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #555; font-size: 16px; line-height: 26px; padding-top: 20px; padding-bottom: 20px;"">
                                        May grace and elegance follow you in everything you do. Wishing you a birthday as beautiful as your spirit! ðŸŒ¼ðŸ’
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 16px; line-height: 24px;"">
                                        With Warmth & Grace,<br>
                                        <strong style=""color: #AD1457;"">CIPC Paramedical Council</strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align=""center"" style=""padding: 20px 0 40px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; text-align: center;"">
                    <tr>
                        <td style=""color: #9B9B9B; font-size: 13px; line-height: 18px;"">
                            CIPC Paramedical Council | Varanasi, Uttar Pradesh | <a href=""https://www.cipcvns.org"" style=""color: #7E7E7E; text-decoration: underline;"">cipcvns.org</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>",
                    EmailServerSettingId = 1,
                    IsActive = true,
                    IsDeleted = false,
                    Placeholder = "{{UserName}}",
                    Subject = "ðŸŒ¸ Happy Birthday, {{UserName}}! â€“ CIPC Paramedical Council",
                    TemplateName = "Birthday_4",
                    UpdatedBy = "System",
                },

                // Birthday Template 5 - Academic Growth (Green/Teal)
                new EmailTemplate
                {
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System",
                    BodyHtml = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Happy Birthday! - CIPC</title>
</head>
<body style=""margin: 0; padding: 0; background-color: #E0F2F1; font-family: 'Barlow', Arial, sans-serif;"">
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #E0F2F1;"">
        <tr>
            <td align=""center"" style=""padding: 20px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; background-color: #FFFFFF; border-bottom: 5px solid #00897B; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 15px rgba(0,0,0,0.1);"">
                    <tr>
                        <td style=""background: linear-gradient(135deg, #00897B, #26A69A); padding: 40px 30px; text-align: center;"">
                            <div style=""font-size: 50px; margin-bottom: 10px;"">ðŸŽ“ðŸŽ‚ðŸŽ“</div>
                            <h1 style=""color: #FFFFFF; margin: 0; font-size: 32px;"">Happy Birthday!</h1>
                            <p style=""color: #B2DFDB; font-size: 14px; margin: 10px 0 0 0;"">Growing Stronger Every Year ðŸŒ¿</p>
                        </td>
                    </tr>
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px;"">
                                        Dear <strong style=""color: #00695C;"">{{UserName}}</strong>,
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #555; font-size: 16px; line-height: 26px; padding-bottom: 20px;"">
                                        ðŸŽ‰ Happy Birthday! On behalf of the entire <strong>CIPC Paramedical Council</strong>, we celebrate your journey of learning and growth.
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""background-color: #E8F5E9; padding: 25px; border-radius: 10px; border-left: 4px solid #00897B;"">
                                        <p style=""color: #1B5E20; font-size: 18px; font-style: italic; margin: 0; line-height: 28px;"">
                                            ""Education is the most powerful weapon which you can use to change the world. May this year bring you closer to all your academic and professional goals.""
                                        </p>
                                        <p style=""color: #2E7D32; font-size: 14px; margin: 10px 0 0 0; text-align: right;"">â€” Inspired by Nelson Mandela</p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #555; font-size: 16px; line-height: 26px; padding-top: 20px; padding-bottom: 20px;"">
                                        Your commitment to paramedical education is truly commendable. May this new year of your life bring you new knowledge, skills, and opportunities to make a difference in healthcare! ðŸ“šðŸ’ª
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 16px; line-height: 24px;"">
                                        With Pride & Best Wishes,<br>
                                        <strong style=""color: #00695C;"">CIPC Paramedical Council</strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align=""center"" style=""padding: 20px 0 40px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; text-align: center;"">
                    <tr>
                        <td style=""color: #9B9B9B; font-size: 13px; line-height: 18px;"">
                            CIPC Paramedical Council | Varanasi, Uttar Pradesh | <a href=""https://www.cipcvns.org"" style=""color: #7E7E7E; text-decoration: underline;"">cipcvns.org</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>",
                    EmailServerSettingId = 1,
                    IsActive = true,
                    IsDeleted = false,
                    Placeholder = "{{UserName}}",
                    Subject = "ðŸŽ“ Happy Birthday, {{UserName}}! â€“ CIPC Paramedical Council",
                    TemplateName = "Birthday_5",
                    UpdatedBy = "System",
                }
            };

            templates.AddRange(GenerateAllModuleTemplates());
            return templates;
        }

        /// <summary>Get only HelpDesk email templates (for seeding when base templates already exist)</summary>
        public static List<EmailTemplate> GetHelpDeskTemplates()
        {
            return GetAllEmailTemplate()
                .Where(t => t.TemplateName is "HelpDeskTicketCreated" or "HelpDeskStatusUpdated" or "HelpDeskEscalation")
                .ToList();
        }

        /// <summary>Get only Birthday email templates (for seeding when base templates already exist)</summary>
        public static List<EmailTemplate> GetBirthdayTemplates()
        {
            return GetAllEmailTemplate()
                .Where(t => t.TemplateName.StartsWith("Birthday_"))
                .ToList();
        }

        private class TemplateDefinition
        {
            public string Name { get; set; } = string.Empty;
            public string Subject { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string ThemeColor { get; set; } = string.Empty;
            public string MessageText { get; set; } = string.Empty;
            public string InfoCardHtml { get; set; } = string.Empty;
            public string ButtonText { get; set; } = string.Empty;
            public string ButtonLink { get; set; } = string.Empty;
            public string Placeholders { get; set; } = string.Empty;
        }

        private static string BuildTemplateHtml(TemplateDefinition def)
        {
            return $@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>{def.Title}</title>
</head>
<body style=""margin: 0; padding: 0; background-color: #F2F2F2; font-family: 'Barlow', Arial, sans-serif;"">
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #F2F2F2;"">
        <tr>
            <td align=""center"" style=""padding: 20px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; background-color: #FFFFFF; border-bottom: 5px solid {def.ThemeColor}; border-radius: 4px; overflow: hidden;"">
                    <!-- Header -->
                    <tr style=""background-color: #1e3a8a; color: #ffffff; text-align: center;"">
                        <td style=""padding: 25px 30px; text-align: center;"">
                            <h2 style=""margin: 0; font-size: 24px; font-weight: bold; letter-spacing: 1px;"">{{{{SchoolName}}}}</h2>
                            <p style=""margin: 5px 0 0 0; font-size: 14px; opacity: 0.8;"">{{{{InstituteName}}}}</p>
                        </td>
                    </tr>
                    <!-- Body Content -->
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                    <td style=""color: #1D1D1D; font-size: 18px; line-height: 28px; padding-bottom: 20px; font-weight: bold;"">
                                        Dear {{{{UserName}}}},
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""color: #333333; font-size: 16px; line-height: 24px; padding-bottom: 20px;"">
                                        {def.MessageText}
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding-bottom: 30px;"">
                                        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #F9FAFB; border-left: 4px solid {def.ThemeColor}; border-radius: 4px; padding: 20px; border-top: 1px solid #E5E7EB; border-right: 1px solid #E5E7EB; border-bottom: 1px solid #E5E7EB;"">
                                            <tr>
                                                <td style=""color: #374151; font-size: 15px; line-height: 22px;"">
                                                    {def.InfoCardHtml}
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                {(string.IsNullOrEmpty(def.ButtonText) ? "" : $@"
                                <tr>
                                    <td align=""center"" style=""padding-bottom: 30px;"">
                                        <table border=""0"" cellpadding=""0"" cellspacing=""0"" style=""border-collapse: separate;"">
                                            <tr>
                                                <td align=""center"" valign=""middle"" style=""border-radius: 4px; background-color: {def.ThemeColor};"">
                                                    <a href=""{def.ButtonLink}"" target=""_blank"" style=""font-size: 16px; font-weight: bold; color: #ffffff; text-decoration: none; padding: 12px 30px; display: inline-block; border-radius: 4px;"">{def.ButtonText}</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                ")}
                                <tr>
                                    <td style=""color: #555555; font-size: 14px; line-height: 20px; padding-top: 20px; border-top: 1px solid #eeeeee;"">
                                        Best Regards,<br>
                                        <strong>{{{{SchoolName}}}} Administration</strong><br>
                                        <span style=""font-size: 12px; color: #9CA3AF;"">Contact: {{{{SupportEmail}}}} | Phone: {{{{SupportPhone}}}}</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <!-- Footer -->
        <tr>
            <td align=""center"" style=""padding: 20px 0 40px 0;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width: 100%; max-width: 600px; text-align: center;"">
                    <tr>
                        <td style=""color: #9B9B9B; font-size: 12px; line-height: 18px;"">
                            This is an automated system notification. Please do not reply directly to this email.<br>
                            &copy; {{{{CurrentYear}}}} {{{{CompanyName}}}}. All Rights Reserved.<br>
                            <a href=""{{{{Website}}}}"" style=""color: #7E7E7E; text-decoration: underline;"">{{{{Website}}}}</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
        }

        private static List<EmailTemplate> GenerateAllModuleTemplates()
        {
            var defs = new List<TemplateDefinition>
            {
                // â”€â”€â”€ AUTH / ACCOUNT â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                new TemplateDefinition {
                    Name = "Email Verification",
                    Subject = "Verify Your Email Address â€“ {{SchoolName}}",
                    Title = "Confirm Your Email",
                    ThemeColor = "#3B82F6",
                    MessageText = "Thank you for joining <strong>{{SchoolName}}</strong>. Please verify your email address to activate your account.",
                    InfoCardHtml = "<strong>Verification Code:</strong> <span style=\"font-size:22px;letter-spacing:4px;font-weight:bold;\">{{VerificationCode}}</span><br/><small>This code expires in 10 minutes.</small>",
                    ButtonText = "Verify Email",
                    ButtonLink = "{{VerificationLink}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{VerificationCode}}, {{VerificationLink}}"
                },
                new TemplateDefinition {
                    Name = "Reset Password",
                    Subject = "Password Reset Request â€“ {{SchoolName}}",
                    Title = "Reset Your Password",
                    ThemeColor = "#EF4444",
                    MessageText = "We received a request to reset the password for your account registered with <strong>{{SchoolName}}</strong>. If you did not request this, please ignore this email.",
                    InfoCardHtml = "<strong>âš  This link is valid for 5 minutes only.</strong><br/>Do not share this link with anyone.",
                    ButtonText = "Reset Password",
                    ButtonLink = "{{ResetPasswordLink}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{ResetPasswordLink}}"
                },
                new TemplateDefinition {
                    Name = "Password Changed",
                    Subject = "Security Alert: Password Changed â€“ {{SchoolName}}",
                    Title = "Password Successfully Changed",
                    ThemeColor = "#10B981",
                    MessageText = "Your account password at <strong>{{SchoolName}}</strong> was changed successfully. If you did not perform this action, contact support immediately.",
                    InfoCardHtml = "<strong>Date:</strong> {{CurrentDate}}<br/><strong>Time:</strong> {{CurrentTime}}<br/><strong>IP Address:</strong> {{IpAddress}}",
                    ButtonText = "Secure My Account",
                    ButtonLink = "{{ResetPasswordLink}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{CurrentDate}}, {{CurrentTime}}, {{IpAddress}}, {{ResetPasswordLink}}"
                },
                new TemplateDefinition {
                    Name = "Account Activated",
                    Subject = "Account Activated â€“ {{SchoolName}}",
                    Title = "Your Account Is Now Active",
                    ThemeColor = "#10B981",
                    MessageText = "Your account at <strong>{{SchoolName}}</strong> has been activated. You can now log in to the portal.",
                    InfoCardHtml = "<strong>Username / Email:</strong> {{UserName}}<br/><strong>Role:</strong> {{Role}}<br/><strong>Portal:</strong> {{LoginUrl}}",
                    ButtonText = "Log In Now",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{Role}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Account Deactivated",
                    Subject = "Account Deactivated â€“ {{SchoolName}}",
                    Title = "Account Deactivated",
                    ThemeColor = "#6B7280",
                    MessageText = "Your account at <strong>{{SchoolName}}</strong> has been deactivated by the administration. Please contact support if you believe this is an error.",
                    InfoCardHtml = "<strong>Support Email:</strong> {{SupportEmail}}<br/><strong>Phone:</strong> {{SupportPhone}}",
                    ButtonText = "Contact Support",
                    ButtonLink = "mailto:{{SupportEmail}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{SupportEmail}}, {{SupportPhone}}"
                },
                new TemplateDefinition {
                    Name = "Login Alert",
                    Subject = "New Login Detected â€“ {{SchoolName}}",
                    Title = "New Login to Your Account",
                    ThemeColor = "#F59E0B",
                    MessageText = "A new login was detected on your <strong>{{SchoolName}}</strong> account. If this was not you, reset your password immediately.",
                    InfoCardHtml = "<strong>Date/Time:</strong> {{CurrentDate}} {{CurrentTime}}<br/><strong>IP Address:</strong> {{IpAddress}}<br/><strong>Device:</strong> {{DeviceType}}",
                    ButtonText = "Reset Password",
                    ButtonLink = "{{ResetPasswordLink}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{CurrentDate}}, {{CurrentTime}}, {{IpAddress}}, {{DeviceType}}, {{ResetPasswordLink}}"
                },

                // â”€â”€â”€ SCHOOL REGISTRATION â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                new TemplateDefinition {
                    Name = "School Registration",
                    Subject = "School Registration Received â€“ {{SchoolName}}",
                    Title = "School Registration Submitted",
                    ThemeColor = "#3B82F6",
                    MessageText = "Your school registration request has been received and is currently under review by the administration.",
                    InfoCardHtml = "<strong>School Name:</strong> {{SchoolName}}<br/><strong>School Code:</strong> {{SchoolCode}}<br/><strong>Contact Email:</strong> {{Email}}<br/><strong>Phone:</strong> {{PhoneNumber}}<br/><strong>Status:</strong> {{ApprovalStatus}}",
                    ButtonText = "Check Status",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{SchoolCode}}, {{Email}}, {{PhoneNumber}}, {{ApprovalStatus}}, {{ContactPersonName}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "School Approved",
                    Subject = "School Registration Approved â€“ {{SchoolName}}",
                    Title = "School Approved ðŸŽ‰",
                    ThemeColor = "#10B981",
                    MessageText = "Congratulations! Your school registration for <strong>{{SchoolName}}</strong> has been approved. Your admin account is ready.",
                    InfoCardHtml = "<strong>School Code:</strong> {{SchoolCode}}<br/><strong>Admin Login:</strong> {{Email}}<br/><strong>Temporary Password:</strong> {{Password}}<br/><strong>Portal URL:</strong> {{LoginUrl}}",
                    ButtonText = "Access Admin Panel",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{SchoolCode}}, {{Email}}, {{Password}}, {{ContactPersonName}}, {{LoginUrl}}"
                },

                // â”€â”€â”€ STUDENT REGISTRATION (StudentRegistration entity) â”€â”€â”€â”€â”€â”€â”€â”€
                new TemplateDefinition {
                    Name = "Student Registration Submitted",
                    Subject = "Registration Submitted â€“ {{InstituteName}}",
                    Title = "Registration Application Received",
                    ThemeColor = "#3B82F6",
                    MessageText = "Dear <strong>{{FullName}}</strong>, your registration application has been submitted successfully to <strong>{{InstituteName}}</strong> and is currently under review.",
                    InfoCardHtml = "<strong>Full Name:</strong> {{FullName}}<br/><strong>Father's Name:</strong> {{FathersName}}<br/><strong>Course:</strong> {{Course}}<br/><strong>Academic Year:</strong> {{AcademicYear}}<br/><strong>Mobile:</strong> {{Mobile}}<br/><strong>Status:</strong> {{RegistrationStatus}}",
                    ButtonText = "Track Application",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{InstituteName}}, {{FullName}}, {{FathersName}}, {{Course}}, {{AcademicYear}}, {{Mobile}}, {{RegistrationStatus}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Student Registration Approved",
                    Subject = "Registration Approved â€“ {{InstituteName}}",
                    Title = "Application Approved âœ…",
                    ThemeColor = "#10B981",
                    MessageText = "Congratulations <strong>{{FullName}}</strong>! Your registration application at <strong>{{InstituteName}}</strong> has been approved.",
                    InfoCardHtml = "<strong>Council Enrollment No:</strong> {{CouncilEnrollmentNo}}<br/><strong>Course:</strong> {{Course}}<br/><strong>Pass Year:</strong> {{PassYear}}<br/><strong>Status:</strong> Approved",
                    ButtonText = "View Application",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{InstituteName}}, {{FullName}}, {{CouncilEnrollmentNo}}, {{Course}}, {{PassYear}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "RegistrationRejected",
                    Subject = "Registration Application Rejected â€“ {{InstituteName}}",
                    Title = "Application Rejected",
                    ThemeColor = "#EF4444",
                    MessageText = "We regret to inform you that your registration application at <strong>{{InstituteName}}</strong> has been rejected.",
                    InfoCardHtml = "<strong>Applicant:</strong> {{FullName}}<br/><strong>Course:</strong> {{Course}}<br/><strong>Reason:</strong> {{Reason}}<br/><br/>You may edit your application and resubmit.",
                    ButtonText = "Edit Application",
                    ButtonLink = "{{EditLink}}",
                    Placeholders = "{{InstituteName}}, {{FullName}}, {{Course}}, {{Reason}}, {{EditLink}}"
                },
                new TemplateDefinition {
                    Name = "Student Registration Under Review",
                    Subject = "Application Under Review â€“ {{InstituteName}}",
                    Title = "Application Under Review",
                    ThemeColor = "#F59E0B",
                    MessageText = "Your registration application is currently being reviewed by the administration of <strong>{{InstituteName}}</strong>. You will be notified once a decision is made.",
                    InfoCardHtml = "<strong>Applicant:</strong> {{FullName}}<br/><strong>Course:</strong> {{Course}}<br/><strong>Status:</strong> Under Review",
                    ButtonText = "Application Portal",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{InstituteName}}, {{FullName}}, {{Course}}, {{LoginUrl}}"
                },

                // â”€â”€â”€ STUDENT (Student entity) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                new TemplateDefinition {
                    Name = "Student Admission Confirmation",
                    Subject = "Admission Confirmed â€“ {{SchoolName}}",
                    Title = "Welcome to {{SchoolName}}",
                    ThemeColor = "#10B981",
                    MessageText = "Congratulations <strong>{{Name}}</strong>! Your admission at <strong>{{SchoolName}}</strong> has been officially confirmed.",
                    InfoCardHtml = "<strong>Student ID:</strong> {{StudentId}}<br/><strong>Enrollment No:</strong> {{EnrollmentNumber}}<br/><strong>Course:</strong> {{Course}}<br/><strong>Class:</strong> {{Class}}<br/><strong>Academic Year:</strong> {{AcademicYear}}",
                    ButtonText = "Student Portal",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{Name}}, {{StudentId}}, {{EnrollmentNumber}}, {{Course}}, {{Class}}, {{AcademicYear}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Student Login Credentials",
                    Subject = "Your Student Portal Login Details â€“ {{SchoolName}}",
                    Title = "Student Portal Access",
                    ThemeColor = "#3B82F6",
                    MessageText = "Your student login credentials for <strong>{{SchoolName}}</strong> portal have been created. Please change your password on first login.",
                    InfoCardHtml = "<strong>Student ID:</strong> {{StudentId}}<br/><strong>Username:</strong> {{UserName}}<br/><strong>Temporary Password:</strong> {{Password}}<br/><strong>Portal:</strong> {{LoginUrl}}",
                    ButtonText = "Log In",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{Name}}, {{StudentId}}, {{UserName}}, {{Password}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Student Profile Updated",
                    Subject = "Student Profile Updated â€“ {{SchoolName}}",
                    Title = "Profile Updated",
                    ThemeColor = "#3B82F6",
                    MessageText = "Your profile records at <strong>{{SchoolName}}</strong> have been updated successfully.",
                    InfoCardHtml = "<strong>Student:</strong> {{Name}}<br/><strong>Student ID:</strong> {{StudentId}}<br/><strong>Updated On:</strong> {{CurrentDate}}",
                    ButtonText = "View Profile",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{Name}}, {{StudentId}}, {{CurrentDate}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Student ID Generated",
                    Subject = "Student ID Card Ready â€“ {{SchoolName}}",
                    Title = "ID Card Generated",
                    ThemeColor = "#10B981",
                    MessageText = "Your official Student ID Card at <strong>{{SchoolName}}</strong> has been generated and is ready to download.",
                    InfoCardHtml = "<strong>Student ID:</strong> {{StudentId}}<br/><strong>Enrollment No:</strong> {{EnrollmentNumber}}<br/><strong>Course:</strong> {{Course}}",
                    ButtonText = "Download ID Card",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{Name}}, {{StudentId}}, {{EnrollmentNumber}}, {{Course}}, {{LoginUrl}}"
                },

                // â”€â”€â”€ EMPLOYEE (Employee entity) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                new TemplateDefinition {
                    Name = "Employee Account Created",
                    Subject = "Staff Account Created â€“ {{SchoolName}}",
                    Title = "Welcome to {{SchoolName}}",
                    ThemeColor = "#3B82F6",
                    MessageText = "Dear <strong>{{FirstName}} {{LastName}}</strong>, your staff account at <strong>{{SchoolName}}</strong> has been created. Please find your credentials below.",
                    InfoCardHtml = "<strong>Employee Code:</strong> {{EmployeeCode}}<br/><strong>Department:</strong> {{Department}}<br/><strong>Designation:</strong> {{Designation}}<br/><strong>Username:</strong> {{EmployeeCode}}<br/><strong>Temporary Password:</strong> {{Password}}<br/><strong>Joining Date:</strong> {{JoiningDate}}",
                    ButtonText = "Staff Portal",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{FirstName}}, {{LastName}}, {{EmployeeCode}}, {{Department}}, {{Designation}}, {{Password}}, {{JoiningDate}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Employee Joining Confirmation",
                    Subject = "Joining Confirmation â€“ {{SchoolName}}",
                    Title = "Joining Confirmation Letter",
                    ThemeColor = "#10B981",
                    MessageText = "We are pleased to confirm your joining as a staff member of <strong>{{SchoolName}}</strong>.",
                    InfoCardHtml = "<strong>Name:</strong> {{FirstName}} {{LastName}}<br/><strong>Employee Code:</strong> {{EmployeeCode}}<br/><strong>Department:</strong> {{Department}}<br/><strong>Designation:</strong> {{Designation}}<br/><strong>Joining Date:</strong> {{JoiningDate}}<br/><strong>Working Hours:</strong> {{WorkingHours}} hrs/day",
                    ButtonText = "Employee Portal",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{FirstName}}, {{LastName}}, {{EmployeeCode}}, {{Department}}, {{Designation}}, {{JoiningDate}}, {{WorkingHours}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Employee Login Credentials",
                    Subject = "Staff Portal Login Credentials â€“ {{SchoolName}}",
                    Title = "Your Login Details",
                    ThemeColor = "#3B82F6",
                    MessageText = "Here are your credentials to access the <strong>{{SchoolName}}</strong> staff portal. Please change your password after first login.",
                    InfoCardHtml = "<strong>Employee Code:</strong> {{EmployeeCode}}<br/><strong>Username:</strong> {{EmployeeCode}}<br/><strong>Temporary Password:</strong> {{Password}}<br/><strong>Portal:</strong> {{LoginUrl}}",
                    ButtonText = "Log In",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{FirstName}}, {{LastName}}, {{EmployeeCode}}, {{Password}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Employee Profile Updated",
                    Subject = "Employee Profile Updated â€“ {{SchoolName}}",
                    Title = "Profile Updated",
                    ThemeColor = "#3B82F6",
                    MessageText = "Your employee profile at <strong>{{SchoolName}}</strong> has been updated successfully.",
                    InfoCardHtml = "<strong>Employee:</strong> {{FirstName}} {{LastName}}<br/><strong>Code:</strong> {{EmployeeCode}}<br/><strong>Department:</strong> {{Department}}<br/><strong>Updated On:</strong> {{CurrentDate}}",
                    ButtonText = "View Profile",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{FirstName}}, {{LastName}}, {{EmployeeCode}}, {{Department}}, {{CurrentDate}}, {{LoginUrl}}"
                },

                // â”€â”€â”€ LEAVE MANAGEMENT (LeaveRequest entity) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                new TemplateDefinition {
                    Name = "Leave Approved",
                    Subject = "Leave Approved â€“ {{SchoolName}}",
                    Title = "Leave Request Approved âœ…",
                    ThemeColor = "#10B981",
                    MessageText = "Your leave request at <strong>{{SchoolName}}</strong> has been approved by the management.",
                    InfoCardHtml = "<strong>Employee:</strong> {{FirstName}} {{LastName}} ({{EmployeeCode}})<br/><strong>Leave Type:</strong> {{LeaveType}}<br/><strong>From:</strong> {{StartDate}}<br/><strong>To:</strong> {{EndDate}}<br/><strong>Total Days:</strong> {{TotalDays}}<br/><strong>Approved By:</strong> {{ApprovedBy}}<br/><strong>Status:</strong> Approved",
                    ButtonText = "View Leave Details",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{FirstName}}, {{LastName}}, {{EmployeeCode}}, {{LeaveType}}, {{StartDate}}, {{EndDate}}, {{TotalDays}}, {{ApprovedBy}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Leave Rejected",
                    Subject = "Leave Request Rejected â€“ {{SchoolName}}",
                    Title = "Leave Request Rejected",
                    ThemeColor = "#EF4444",
                    MessageText = "We regret to inform you that your leave request at <strong>{{SchoolName}}</strong> has been rejected.",
                    InfoCardHtml = "<strong>Employee:</strong> {{FirstName}} {{LastName}} ({{EmployeeCode}})<br/><strong>Leave Type:</strong> {{LeaveType}}<br/><strong>From:</strong> {{StartDate}}<br/><strong>To:</strong> {{EndDate}}<br/><strong>Total Days:</strong> {{TotalDays}}<br/><strong>Reason for Rejection:</strong> {{Remarks}}",
                    ButtonText = "View Leave History",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{FirstName}}, {{LastName}}, {{EmployeeCode}}, {{LeaveType}}, {{StartDate}}, {{EndDate}}, {{TotalDays}}, {{Remarks}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Leave Applied",
                    Subject = "Leave Application Submitted â€“ {{SchoolName}}",
                    Title = "Leave Request Submitted",
                    ThemeColor = "#F59E0B",
                    MessageText = "Your leave application at <strong>{{SchoolName}}</strong> has been submitted and is pending approval.",
                    InfoCardHtml = "<strong>Leave Type:</strong> {{LeaveType}}<br/><strong>From:</strong> {{StartDate}}<br/><strong>To:</strong> {{EndDate}}<br/><strong>Total Days:</strong> {{TotalDays}}<br/><strong>Reason:</strong> {{Reason}}<br/><strong>Status:</strong> Pending",
                    ButtonText = "Track Status",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{FirstName}}, {{LastName}}, {{EmployeeCode}}, {{LeaveType}}, {{StartDate}}, {{EndDate}}, {{TotalDays}}, {{Reason}}, {{LoginUrl}}"
                },

                // â”€â”€â”€ ATTENDANCE â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                new TemplateDefinition {
                    Name = "Attendance Alert",
                    Subject = "Absence Recorded â€“ {{SchoolName}}",
                    Title = "Absence Notification",
                    ThemeColor = "#EF4444",
                    MessageText = "This is to inform you that an absence was recorded at <strong>{{SchoolName}}</strong> today.",
                    InfoCardHtml = "<strong>Employee/Student:</strong> {{UserName}}<br/><strong>Date:</strong> {{CurrentDate}}<br/><strong>Status:</strong> Absent",
                    ButtonText = "View Attendance",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{CurrentDate}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Low Attendance Warning",
                    Subject = "Low Attendance Warning â€“ {{SchoolName}}",
                    Title = "âš  Low Attendance Alert",
                    ThemeColor = "#F59E0B",
                    MessageText = "Your attendance at <strong>{{SchoolName}}</strong> has fallen below the minimum required percentage. Immediate improvement is required.",
                    InfoCardHtml = "<strong>Name:</strong> {{UserName}}<br/><strong>Current Attendance:</strong> {{Attendance}}%<br/><strong>Required:</strong> 75%",
                    ButtonText = "View Attendance Record",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{Attendance}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Monthly Attendance Report",
                    Subject = "Monthly Attendance Summary â€“ {{SchoolName}}",
                    Title = "Attendance Summary Report",
                    ThemeColor = "#3B82F6",
                    MessageText = "Please find the monthly attendance summary report for <strong>{{SchoolName}}</strong> below.",
                    InfoCardHtml = "<strong>Name:</strong> {{UserName}}<br/><strong>Month:</strong> {{Month}} {{Year}}<br/><strong>Present Days:</strong> {{PresentDays}}<br/><strong>Absent Days:</strong> {{AbsentDays}}<br/><strong>Total Working Days:</strong> {{TotalDays}}<br/><strong>Attendance %:</strong> {{Attendance}}%",
                    ButtonText = "View Full Report",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{Month}}, {{Year}}, {{PresentDays}}, {{AbsentDays}}, {{TotalDays}}, {{Attendance}}, {{LoginUrl}}"
                },

                // â”€â”€â”€ EXAMINATION (Exam / ExamResult entities) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                new TemplateDefinition {
                    Name = "Exam Schedule Published",
                    Subject = "Exam Date Sheet Released â€“ {{SchoolName}}",
                    Title = "Examination Schedule Published",
                    ThemeColor = "#3B82F6",
                    MessageText = "The exam schedule for <strong>{{SchoolName}}</strong> has been published. Please review the date sheet below.",
                    InfoCardHtml = "<strong>Exam Name:</strong> {{ExamName}}<br/><strong>Type:</strong> {{ExamType}}<br/><strong>Start Date:</strong> {{StartDate}}<br/><strong>End Date:</strong> {{EndDate}}<br/><strong>Status:</strong> {{Status}}",
                    ButtonText = "View Date Sheet",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{ExamName}}, {{ExamType}}, {{StartDate}}, {{EndDate}}, {{Status}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Hall Ticket Available",
                    Subject = "Hall Ticket Ready â€“ {{SchoolName}}",
                    Title = "Admit Card / Hall Ticket",
                    ThemeColor = "#10B981",
                    MessageText = "Your hall ticket for the upcoming exam at <strong>{{SchoolName}}</strong> is ready. Please download and carry it to the examination hall.",
                    InfoCardHtml = "<strong>Exam:</strong> {{ExamName}}<br/><strong>Exam Type:</strong> {{ExamType}}<br/><strong>Start Date:</strong> {{StartDate}}<br/><strong>Student ID:</strong> {{StudentId}}",
                    ButtonText = "Download Hall Ticket",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{ExamName}}, {{ExamType}}, {{StartDate}}, {{StudentId}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Result Published",
                    Subject = "Exam Results Declared â€“ {{SchoolName}}",
                    Title = "Examination Results Declared",
                    ThemeColor = "#10B981",
                    MessageText = "The results for the examination at <strong>{{SchoolName}}</strong> have been declared. Check your result below.",
                    InfoCardHtml = "<strong>Exam:</strong> {{ExamName}}<br/><strong>Subject:</strong> {{Subject}}<br/><strong>Marks Obtained:</strong> {{MarksObtained}} / {{TotalMarks}}<br/><strong>Grade:</strong> {{Grade}}<br/><strong>Result:</strong> {{Status}}",
                    ButtonText = "View Report Card",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{StudentId}}, {{ExamName}}, {{Subject}}, {{MarksObtained}}, {{TotalMarks}}, {{Grade}}, {{Status}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Marksheet Generated",
                    Subject = "Official Marksheet Available â€“ {{SchoolName}}",
                    Title = "Marksheet Ready",
                    ThemeColor = "#10B981",
                    MessageText = "Your official marksheet from <strong>{{SchoolName}}</strong> has been generated and is available for download.",
                    InfoCardHtml = "<strong>Student ID:</strong> {{StudentId}}<br/><strong>Exam:</strong> {{ExamName}}<br/><strong>Final Grade:</strong> {{Grade}}<br/><strong>Result:</strong> {{Status}}",
                    ButtonText = "Download Marksheet",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{StudentId}}, {{ExamName}}, {{Grade}}, {{Status}}, {{LoginUrl}}"
                },

                // â”€â”€â”€ PAYROLL (PayrollRun entity) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                new TemplateDefinition {
                    Name = "Salary Generated",
                    Subject = "Payslip Ready â€“ {{SchoolName}}",
                    Title = "Salary Payslip Generated",
                    ThemeColor = "#3B82F6",
                    MessageText = "Your monthly payslip for <strong>{{SchoolName}}</strong> has been generated. Please review the details below.",
                    InfoCardHtml = "<strong>Employee:</strong> {{FirstName}} {{LastName}} ({{EmployeeCode}})<br/><strong>Month:</strong> {{Month}}<br/><strong>Gross Salary:</strong> â‚¹{{GrossSalary}}<br/><strong>Deductions:</strong> â‚¹{{TotalDeductions}}<br/><strong>Net Salary:</strong> â‚¹{{NetSalary}}<br/><strong>Status:</strong> {{Status}}",
                    ButtonText = "View Payslip",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{FirstName}}, {{LastName}}, {{EmployeeCode}}, {{Month}}, {{GrossSalary}}, {{TotalDeductions}}, {{NetSalary}}, {{Status}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Salary Paid",
                    Subject = "Salary Credited â€“ {{SchoolName}}",
                    Title = "Salary Disbursed",
                    ThemeColor = "#10B981",
                    MessageText = "Your salary for <strong>{{Month}}</strong> from <strong>{{SchoolName}}</strong> has been processed and credited.",
                    InfoCardHtml = "<strong>Employee:</strong> {{FirstName}} {{LastName}}<br/><strong>Employee Code:</strong> {{EmployeeCode}}<br/><strong>Month:</strong> {{Month}}<br/><strong>Gross Salary:</strong> â‚¹{{GrossSalary}}<br/><strong>Deductions:</strong> â‚¹{{TotalDeductions}}<br/><strong>Net Salary:</strong> â‚¹{{NetSalary}}",
                    ButtonText = "Employee Portal",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{FirstName}}, {{LastName}}, {{EmployeeCode}}, {{Month}}, {{GrossSalary}}, {{TotalDeductions}}, {{NetSalary}}, {{LoginUrl}}"
                },

                // â”€â”€â”€ TRANSPORT (TransportRoute / Vehicle entities) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                new TemplateDefinition {
                    Name = "Bus Assigned",
                    Subject = "Transport Route Assigned â€“ {{SchoolName}}",
                    Title = "Transport Route Allocated",
                    ThemeColor = "#3B82F6",
                    MessageText = "A transport route has been assigned for your daily commute at <strong>{{SchoolName}}</strong>.",
                    InfoCardHtml = "<strong>Route Name:</strong> {{RouteName}}<br/><strong>Vehicle No:</strong> {{VehicleNumber}}<br/><strong>Driver:</strong> {{DriverName}}<br/><strong>Driver Phone:</strong> {{DriverPhone}}<br/><strong>Status:</strong> {{Status}}",
                    ButtonText = "View Route Details",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{RouteName}}, {{VehicleNumber}}, {{DriverName}}, {{DriverPhone}}, {{Status}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Route Changed",
                    Subject = "Transport Route Updated â€“ {{SchoolName}}",
                    Title = "Route Change Notification",
                    ThemeColor = "#F59E0B",
                    MessageText = "Your transport route at <strong>{{SchoolName}}</strong> has been updated. Please review the new route details.",
                    InfoCardHtml = "<strong>New Route:</strong> {{RouteName}}<br/><strong>Vehicle No:</strong> {{VehicleNumber}}<br/><strong>Effective From:</strong> {{CurrentDate}}",
                    ButtonText = "View Route",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{RouteName}}, {{VehicleNumber}}, {{CurrentDate}}, {{LoginUrl}}"
                },

                // â”€â”€â”€ EVENTS (Event entity) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                new TemplateDefinition {
                    Name = "Event Invitation",
                    Subject = "You're Invited â€“ {{Title}} | {{SchoolName}}",
                    Title = "Event Invitation",
                    ThemeColor = "#10B981",
                    MessageText = "You are cordially invited to an upcoming event organized by <strong>{{SchoolName}}</strong>. We look forward to your participation.",
                    InfoCardHtml = "<strong>Event:</strong> {{Title}}<br/><strong>Date:</strong> {{EventDate}}<br/><strong>Venue:</strong> {{Location}}<br/><strong>Description:</strong> {{Description}}",
                    ButtonText = "View Event Details",
                    ButtonLink = "{{EventLink}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{Title}}, {{EventDate}}, {{Location}}, {{Description}}, {{EventLink}}"
                },
                new TemplateDefinition {
                    Name = "Event Reminder",
                    Subject = "Reminder: {{Title}} is Tomorrow â€“ {{SchoolName}}",
                    Title = "Event Reminder",
                    ThemeColor = "#F59E0B",
                    MessageText = "This is a reminder that the event <strong>{{Title}}</strong> at <strong>{{SchoolName}}</strong> is scheduled.",
                    InfoCardHtml = "<strong>Event:</strong> {{Title}}<br/><strong>Date:</strong> {{EventDate}}<br/><strong>Venue:</strong> {{Location}}",
                    ButtonText = "Event Details",
                    ButtonLink = "{{EventLink}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{Title}}, {{EventDate}}, {{Location}}, {{EventLink}}"
                },

                // â”€â”€â”€ FEE / FINANCE â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                new TemplateDefinition {
                    Name = "Fee Generated",
                    Subject = "Fee Invoice Generated â€“ {{SchoolName}}",
                    Title = "Fee Payment Invoice",
                    ThemeColor = "#EF4444",
                    MessageText = "A new fee invoice has been generated for <strong>{{SchoolName}}</strong>. Please pay before the due date to avoid penalties.",
                    InfoCardHtml = "<strong>Student:</strong> {{StudentName}}<br/><strong>Invoice No:</strong> {{InvoiceNumber}}<br/><strong>Amount:</strong> â‚¹{{FeeAmount}}<br/><strong>Due Date:</strong> {{DueDate}}<br/><strong>Fee Type:</strong> {{FeeType}}",
                    ButtonText = "Pay Now",
                    ButtonLink = "{{PaymentLink}}",
                    Placeholders = "{{SchoolName}}, {{ParentName}}, {{StudentName}}, {{InvoiceNumber}}, {{FeeAmount}}, {{DueDate}}, {{FeeType}}, {{PaymentLink}}"
                },
                new TemplateDefinition {
                    Name = "Fee Paid Successfully",
                    Subject = "Fee Payment Confirmed â€“ {{SchoolName}}",
                    Title = "Payment Successful âœ…",
                    ThemeColor = "#10B981",
                    MessageText = "Thank you! The fee payment for <strong>{{SchoolName}}</strong> has been received and confirmed.",
                    InfoCardHtml = "<strong>Student:</strong> {{StudentName}}<br/><strong>Receipt No:</strong> {{ReceiptNumber}}<br/><strong>Amount Paid:</strong> â‚¹{{FeeAmount}}<br/><strong>Transaction ID:</strong> {{TransactionId}}<br/><strong>Date:</strong> {{CurrentDate}}",
                    ButtonText = "Download Receipt",
                    ButtonLink = "{{ReceiptLink}}",
                    Placeholders = "{{SchoolName}}, {{ParentName}}, {{StudentName}}, {{ReceiptNumber}}, {{FeeAmount}}, {{TransactionId}}, {{CurrentDate}}, {{ReceiptLink}}"
                },
                new TemplateDefinition {
                    Name = "Fee Due Reminder",
                    Subject = "Fee Overdue Reminder â€“ {{SchoolName}}",
                    Title = "Fee Payment Overdue âš ",
                    ThemeColor = "#EF4444",
                    MessageText = "This is a reminder that the fee payment for <strong>{{SchoolName}}</strong> is overdue. Please pay immediately to avoid late charges.",
                    InfoCardHtml = "<strong>Student:</strong> {{StudentName}}<br/><strong>Invoice No:</strong> {{InvoiceNumber}}<br/><strong>Overdue Amount:</strong> â‚¹{{FeeAmount}}<br/><strong>Due Date:</strong> {{DueDate}}",
                    ButtonText = "Pay Now",
                    ButtonLink = "{{PaymentLink}}",
                    Placeholders = "{{SchoolName}}, {{ParentName}}, {{StudentName}}, {{InvoiceNumber}}, {{FeeAmount}}, {{DueDate}}, {{PaymentLink}}"
                },

                // â”€â”€â”€ CERTIFICATES â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                new TemplateDefinition {
                    Name = "Bonafide Certificate Ready",
                    Subject = "Bonafide Certificate Ready â€“ {{SchoolName}}",
                    Title = "Certificate Ready",
                    ThemeColor = "#10B981",
                    MessageText = "Your Bonafide Certificate from <strong>{{SchoolName}}</strong> has been processed and is ready for download.",
                    InfoCardHtml = "<strong>Student:</strong> {{Name}}<br/><strong>Student ID:</strong> {{StudentId}}<br/><strong>Certificate No:</strong> {{CertificateNumber}}<br/><strong>Issued On:</strong> {{CurrentDate}}",
                    ButtonText = "Download Certificate",
                    ButtonLink = "{{CertificateLink}}",
                    Placeholders = "{{SchoolName}}, {{Name}}, {{StudentId}}, {{CertificateNumber}}, {{CurrentDate}}, {{CertificateLink}}"
                },
                new TemplateDefinition {
                    Name = "Transfer Certificate Ready",
                    Subject = "Transfer Certificate Ready â€“ {{SchoolName}}",
                    Title = "Transfer Certificate (TC)",
                    ThemeColor = "#10B981",
                    MessageText = "Your Transfer Certificate (TC) from <strong>{{SchoolName}}</strong> is ready. Please collect or download it.",
                    InfoCardHtml = "<strong>Student:</strong> {{Name}}<br/><strong>Student ID:</strong> {{StudentId}}<br/><strong>TC Number:</strong> {{CertificateNumber}}<br/><strong>Issued On:</strong> {{CurrentDate}}",
                    ButtonText = "Download TC",
                    ButtonLink = "{{CertificateLink}}",
                    Placeholders = "{{SchoolName}}, {{Name}}, {{StudentId}}, {{CertificateNumber}}, {{CurrentDate}}, {{CertificateLink}}"
                },
                new TemplateDefinition {
                    Name = "Character Certificate Ready",
                    Subject = "Character Certificate Ready â€“ {{SchoolName}}",
                    Title = "Character Certificate",
                    ThemeColor = "#10B981",
                    MessageText = "Your Character Certificate from <strong>{{SchoolName}}</strong> has been issued.",
                    InfoCardHtml = "<strong>Student:</strong> {{Name}}<br/><strong>Student ID:</strong> {{StudentId}}<br/><strong>Certificate No:</strong> {{CertificateNumber}}<br/><strong>Issued On:</strong> {{CurrentDate}}",
                    ButtonText = "Download Certificate",
                    ButtonLink = "{{CertificateLink}}",
                    Placeholders = "{{SchoolName}}, {{Name}}, {{StudentId}}, {{CertificateNumber}}, {{CurrentDate}}, {{CertificateLink}}"
                },

                // â”€â”€â”€ NOTICE BOARD / BROADCAST â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                new TemplateDefinition {
                    Name = "New Notice",
                    Subject = "New Notice Published â€“ {{SchoolName}}",
                    Title = "Notice Board Update",
                    ThemeColor = "#3B82F6",
                    MessageText = "A new notice has been published on the <strong>{{SchoolName}}</strong> notice board. Please review it at your earliest.",
                    InfoCardHtml = "<strong>Notice Title:</strong> {{NoticeTitle}}<br/><strong>Published On:</strong> {{CurrentDate}}<br/><strong>Published By:</strong> {{PublishedBy}}",
                    ButtonText = "View Notice",
                    ButtonLink = "{{NoticeLink}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{NoticeTitle}}, {{CurrentDate}}, {{PublishedBy}}, {{NoticeLink}}"
                },
                new TemplateDefinition {
                    Name = "Holiday Announcement",
                    Subject = "Holiday Notice â€“ {{SchoolName}}",
                    Title = "Holiday Announcement",
                    ThemeColor = "#10B981",
                    MessageText = "<strong>{{SchoolName}}</strong> will remain closed on the following date(s) due to a holiday.",
                    InfoCardHtml = "<strong>Occasion:</strong> {{HolidayName}}<br/><strong>Date(s):</strong> {{HolidayDates}}",
                    ButtonText = "Academic Calendar",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{HolidayName}}, {{HolidayDates}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Broadcast Email",
                    Subject = "Important Announcement â€“ {{SchoolName}}",
                    Title = "School Announcement",
                    ThemeColor = "#3B82F6",
                    MessageText = "The following important announcement has been issued by <strong>{{SchoolName}}</strong> administration.",
                    InfoCardHtml = "<strong>Subject:</strong> {{AnnouncementTitle}}<br/><strong>Message:</strong> {{MessageDetails}}",
                    ButtonText = "View Portal",
                    ButtonLink = "{{LoginUrl}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{AnnouncementTitle}}, {{MessageDetails}}, {{LoginUrl}}"
                },
                new TemplateDefinition {
                    Name = "Maintenance Notice",
                    Subject = "Scheduled Maintenance â€“ {{SchoolName}}",
                    Title = "System Maintenance",
                    ThemeColor = "#EF4444",
                    MessageText = "The <strong>{{SchoolName}}</strong> portal will be temporarily unavailable due to scheduled maintenance.",
                    InfoCardHtml = "<strong>Start Time:</strong> {{StartTime}}<br/><strong>Expected Duration:</strong> {{Duration}} hours<br/><strong>Reason:</strong> {{Reason}}",
                    ButtonText = "Support",
                    ButtonLink = "mailto:{{SupportEmail}}",
                    Placeholders = "{{SchoolName}}, {{UserName}}, {{StartTime}}, {{Duration}}, {{Reason}}, {{SupportEmail}}"
                },
                new TemplateDefinition {
                    Name = "Fee Payment Confirmation",
                    Subject = "Fee Payment Confirmation - {{ReceiptNo}}",
                    Title = "Payment Received",
                    ThemeColor = "#10B981",
                    MessageText = "We have successfully received your fee payment for <strong>{{StudentName}}</strong>. Thank you for your payment.",
                    InfoCardHtml = "<strong>Receipt No:</strong> {{ReceiptNo}}<br/><strong>Amount Paid:</strong> {{AmountPaid}}<br/><strong>Payment Date:</strong> {{PaymentDate}}<br/><strong>Payment Mode:</strong> {{PaymentMode}}<br/><strong>Installment:</strong> {{InstallmentName}}",
                    ButtonText = "Download Receipt",
                    ButtonLink = "{{ReceiptLink}}",
                    Placeholders = "{{SchoolName}}, {{StudentName}}, {{ReceiptNo}}, {{AmountPaid}}, {{PaymentDate}}, {{PaymentMode}}, {{InstallmentName}}, {{ReceiptLink}}"
                },
            };

            var templates = new List<EmailTemplate>();
            foreach (var def in defs)
            {
                templates.Add(new EmailTemplate
                {
                    TemplateName = def.Name,
                    Subject = def.Subject,
                    BodyHtml = BuildTemplateHtml(def),
                    Placeholder = def.Placeholders,
                    IsActive = true,
                    IsDeleted = false,
                    EmailServerSettingId = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                });
            }
            return templates;
        }
    }
}
