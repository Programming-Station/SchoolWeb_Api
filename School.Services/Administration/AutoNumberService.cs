using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Administration;
using School.Infrastructure;
using School.Services.Interfaces;

namespace School.Services.Administration
{
    public class AutoNumberService : IAutoNumberService
    {
        private readonly SchoolDbContext _context;

        public AutoNumberService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateNextNumberAsync(string entityType, int schoolId)
        {
            // Retrieve sequence configuration or create a default seed if not configured yet
            var setting = await _context.AutoNumberSettings
                .Where(s => s.EntityType == entityType && s.SchoolRegistrationId == schoolId && !s.IsDeleted)
                .FirstOrDefaultAsync();

            if (setting == null)
            {
                var defaultPrefix = entityType.Length >= 3 ? entityType.Substring(0, 3).ToUpper() : entityType.ToUpper();
                setting = new AutoNumberSetting
                {
                    EntityType = entityType,
                    Prefix = defaultPrefix,
                    NextValue = 1,
                    PaddingLength = 4,
                    IncludeYear = true,
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "System"
                };
                _context.AutoNumberSettings.Add(setting);
            }

            var currentVal = setting.NextValue;
            setting.NextValue++;
            await _context.SaveChangesAsync();

            var yearPart = setting.IncludeYear ? $"{DateTime.UtcNow.Year}-" : "";
            var paddedNumber = currentVal.ToString().PadLeft(setting.PaddingLength, '0');
            var prefixPart = string.IsNullOrEmpty(setting.Prefix) ? "" : $"{setting.Prefix}-";
            var suffixPart = string.IsNullOrEmpty(setting.Suffix) ? "" : $"-{setting.Suffix}";

            return $"{prefixPart}{yearPart}{paddedNumber}{suffixPart}";
        }
    }
}
