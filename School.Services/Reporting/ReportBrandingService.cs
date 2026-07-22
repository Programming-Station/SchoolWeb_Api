using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using School.Domain.Reporting;
using School.Infrastructure;
using School.Infrastructure.Repositories;
using School.Services.Interfaces;
using School_DTOs.Reporting;

namespace School.Services.Reporting
{
    /// <summary>
    /// Tenant-aware branding service with 15-minute in-memory cache.
    /// Maps between ReportBranding entity and ReportBrandingDto.
    /// Also handles logo upload and file path resolution.
    /// </summary>
    public class ReportingBrandingService : IReportingBrandingService
    {
        private readonly SchoolDbContext _db;
        private readonly ReportingRepository _repo;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ReportingBrandingService> _logger;
        private readonly string _storagePath;
        private const string CacheKeyPrefix = "ReportBranding_";
        private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(15);

        public ReportingBrandingService(
            SchoolDbContext db,
            ReportingRepository repo,
            IMemoryCache cache,
            ILogger<ReportingBrandingService> logger,
            Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _db = db;
            _repo = repo;
            _cache = cache;
            _logger = logger;

            var configuredPath = configuration.GetSection("AppSettings:ImageStoragePath").Value;
            _storagePath = string.IsNullOrWhiteSpace(configuredPath)
                ? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads")
                : Path.GetFullPath(configuredPath.Trim().Replace('/', Path.DirectorySeparatorChar));
        }

        public async Task<ReportBrandingDto> GetBrandingAsync(int? tenantId = null)
        {
            var cacheKey = $"{CacheKeyPrefix}{tenantId ?? 0}";

            if (_cache.TryGetValue(cacheKey, out ReportBrandingDto? cached) && cached != null)
                return cached;

            ReportBranding? entity;
            if (tenantId.HasValue)
                entity = await _repo.GetBrandingAsync(tenantId.Value);
            else
                entity = await _repo.GetDefaultBrandingAsync();

            // Fallback: try to populate from SchoolRegistration if no branding record exists
            if (entity == null)
            {
                var school = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                    .FirstOrDefaultAsync(_db.SchoolRegistrations);

                entity = new ReportBranding
                {
                    TenantId = tenantId ?? 0,
                    SchoolName = school?.SchoolName ?? "SchoolSaaS ERP",
                    OrganizationName = school?.SchoolName ?? "SchoolSaaS ERP",
                    Phone = school?.PhoneNumber ?? string.Empty,
                    Email = school?.Email ?? string.Empty,
                    Website = school?.WebsiteUrl ?? string.Empty,
                    AffiliationNumber = school?.AffiliationNumber ?? string.Empty,
                    PrimaryColor = "#1e3a8a",
                    SecondaryColor = "#2563eb",
                    FontFamily = "Arial"
                };
            }

            var dto = MapToDto(entity);
            _cache.Set(cacheKey, dto, CacheTtl);
            return dto;
        }

        public async Task<ReportBrandingDto> SaveBrandingAsync(ReportBrandingDto dto)
        {
            var entity = MapToEntity(dto);
            var saved = await _repo.SaveBrandingAsync(entity);
            InvalidateBrandingCache(dto.TenantId);
            return MapToDto(saved);
        }

        public async Task<string?> UploadLogoAsync(
            int tenantId, byte[] imageBytes, string fileName, string logoType)
        {
            try
            {
                var dir = Path.Combine(_storagePath, "Branding", tenantId.ToString());
                Directory.CreateDirectory(dir);

                var ext = Path.GetExtension(fileName);
                var safeName = $"{logoType}_{tenantId}{ext}";
                var fullPath = Path.Combine(dir, safeName);
                await File.WriteAllBytesAsync(fullPath, imageBytes);

                // Return a virtual path that maps to /uploads/
                return $"/uploads/Branding/{tenantId}/{safeName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload logo for tenant {TenantId}", tenantId);
                return null;
            }
        }

        public void InvalidateBrandingCache(int tenantId)
        {
            _cache.Remove($"{CacheKeyPrefix}{tenantId}");
            _cache.Remove($"{CacheKeyPrefix}0");
        }

        // ─── Mappers ──────────────────────────────────────────────────────────

        private static ReportBrandingDto MapToDto(ReportBranding e) => new()
        {
            Id = e.Id,
            TenantId = e.SchoolRegistrationId,
            SchoolName = e.SchoolName,
            OrganizationName = e.OrganizationName,
            TagLine = e.TagLine,
            EstablishedYear = e.EstablishedYear,
            HeaderLogo = e.HeaderLogo,
            FooterLogo = e.FooterLogo,
            LogoLight = e.LogoLight,
            LogoDark = e.LogoDark,
            AddressLine1 = e.AddressLine1,
            AddressLine2 = e.AddressLine2,
            City = e.City,
            State = e.State,
            PinCode = e.PinCode,
            Phone = e.Phone,
            Mobile = e.Mobile,
            Email = e.Email,
            Website = e.Website,
            AffiliationNumber = e.AffiliationNumber,
            RegistrationNumber = e.RegistrationNumber,
            PrincipalName = e.PrincipalName,
            PrincipalSignature = e.PrincipalSignature,
            DirectorName = e.DirectorName,
            DirectorSignature = e.DirectorSignature,
            OfficialSeal = e.OfficialSeal,
            DigitalSignature = e.DigitalSignature,
            ReportWatermark = e.ReportWatermark,
            WatermarkText = e.WatermarkText,
            ReportFooterText = e.ReportFooterText,
            CopyrightText = e.CopyrightText,
            Disclaimer = e.Disclaimer,
            PrimaryColor = e.PrimaryColor,
            SecondaryColor = e.SecondaryColor,
            AccentColor = e.AccentColor,
            FontFamily = e.FontFamily,
            ReportMarginTop = e.ReportMarginTop,
            ReportMarginBottom = e.ReportMarginBottom,
            ReportMarginLeft = e.ReportMarginLeft,
            ReportMarginRight = e.ReportMarginRight,
            QrVerificationBaseUrl = e.QrVerificationBaseUrl,
            BarcodePrefix = e.BarcodePrefix,
            CurrentAcademicSession = e.CurrentAcademicSession,
            CampusName = e.CampusName
        };

        private static ReportBranding MapToEntity(ReportBrandingDto d) => new()
        {
            Id = d.Id,
            TenantId = d.TenantId,
            SchoolName = d.SchoolName,
            OrganizationName = d.OrganizationName,
            TagLine = d.TagLine,
            EstablishedYear = d.EstablishedYear,
            HeaderLogo = d.HeaderLogo,
            FooterLogo = d.FooterLogo,
            LogoLight = d.LogoLight,
            LogoDark = d.LogoDark,
            AddressLine1 = d.AddressLine1,
            AddressLine2 = d.AddressLine2,
            City = d.City,
            State = d.State,
            PinCode = d.PinCode,
            Phone = d.Phone,
            Mobile = d.Mobile,
            Email = d.Email,
            Website = d.Website,
            AffiliationNumber = d.AffiliationNumber,
            RegistrationNumber = d.RegistrationNumber,
            PrincipalName = d.PrincipalName,
            PrincipalSignature = d.PrincipalSignature,
            DirectorName = d.DirectorName,
            DirectorSignature = d.DirectorSignature,
            OfficialSeal = d.OfficialSeal,
            DigitalSignature = d.DigitalSignature,
            ReportWatermark = d.ReportWatermark,
            WatermarkText = d.WatermarkText,
            ReportFooterText = d.ReportFooterText,
            CopyrightText = d.CopyrightText,
            Disclaimer = d.Disclaimer,
            PrimaryColor = d.PrimaryColor,
            SecondaryColor = d.SecondaryColor,
            AccentColor = d.AccentColor,
            FontFamily = d.FontFamily,
            ReportMarginTop = d.ReportMarginTop,
            ReportMarginBottom = d.ReportMarginBottom,
            ReportMarginLeft = d.ReportMarginLeft,
            ReportMarginRight = d.ReportMarginRight,
            QrVerificationBaseUrl = d.QrVerificationBaseUrl,
            BarcodePrefix = d.BarcodePrefix,
            CurrentAcademicSession = d.CurrentAcademicSession,
            CampusName = d.CampusName
        };
    }
}
