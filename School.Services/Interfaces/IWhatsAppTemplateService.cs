using School_DTOs.Communication;

namespace School.Services.Interfaces
{
    public interface IWhatsAppTemplateService
    {
        Task<string> ParseTemplateAsync(string templateName, Dictionary<string, string> variables);
        Task<bool> AddTemplateAsync(WhatsAppTemplateDto dto, string userName);
    }
}
