using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Communication;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs.Communication;

namespace School.Services.Communication
{
    public class WhatsAppTemplateService : IWhatsAppTemplateService
    {
        private readonly SchoolDbContext _context;

        public WhatsAppTemplateService(SchoolDbContext context)
        {
            _context = context;
        }

        private int GetCurrentSchoolId()
        {
            if (_context.CurrentTenantId.HasValue)
                return _context.CurrentTenantId.Value;
            var school = _context.SchoolRegistrations.IgnoreQueryFilters().FirstOrDefault();
            return school?.Id ?? 1;
        }

        public async Task<string> ParseTemplateAsync(string templateName, Dictionary<string, string> variables)
        {
            var schoolId = GetCurrentSchoolId();
            var template = await _context.WhatsAppTemplates
                .FirstOrDefaultAsync(t => t.SchoolRegistrationId == schoolId && t.TemplateName == templateName && !t.IsDeleted);

            if (template == null) return $"Template '{templateName}' not found.";

            string messageBody = template.BodyTemplate;
            if (variables != null)
            {
                foreach (var v in variables)
                {
                    messageBody = messageBody.Replace($"{{{{{v.Key}}}}}", v.Value);
                }
            }
            return messageBody;
        }

        public async Task<bool> AddTemplateAsync(WhatsAppTemplateDto dto, string userName)
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
            return true;
        }
    }
}
