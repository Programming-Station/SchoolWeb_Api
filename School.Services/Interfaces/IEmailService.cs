namespace School.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendTemplateAsync(string recipientEmail, string templateName, Dictionary<string, string>? placeholders, byte[]? attachmentBytes = null, string? attachmentName = null);
        Task<bool> SendWelcomeEmailAsync(string recipientEmail, string userName, string password, string status);
        Task<bool> SendForgotPasswordAsync(string recipientEmail, string userName, string resetLink);
        Task<bool> SendResetPasswordAsync(string recipientEmail, string userName);
        Task<bool> SendOtpAsync(string recipientEmail, string userName, string otp);
        Task<bool> SendVerificationAsync(string recipientEmail, string userName, string verificationLink);
        Task<bool> SendGenericTemplateAsync(string recipientEmail, string templateName, Dictionary<string, string>? placeholders);

        // Background Queueing
        void QueueTemplateEmail(string recipientEmail, string templateName, Dictionary<string, string>? placeholders, byte[]? attachmentBytes = null, string? attachmentName = null);

        // Cache Invalidation
        void InvalidateTemplateCache(string templateName);
        void InvalidateSmtpCache();
    }
}
