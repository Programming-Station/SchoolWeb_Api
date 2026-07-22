using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using School.Domain.School;
using School.Infrastructure;
using School.Infrastructure.Interfaces;
using School.Infrastructure.Repositories.School;
using School.Models.School;
using School.Services.Interfaces;
using School.Services.School.ISchoolServices;
using School_DTOs;
using School_DTOs.School;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace School.Services.School
{
    public class OrganizationProfileService : IOrganizationProfileService
    {
        private readonly IOrganizationProfileRepository _profileRepo;
        private readonly ISchoolRepository _schoolRepo;
        private readonly ITenantService _tenantService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly SchoolDbContext _dbContext;
        private readonly IOrganizationCacheService _cacheService;
        private readonly IDocumentService _documentService;

        public OrganizationProfileService(
            IOrganizationProfileRepository profileRepo,
            ISchoolRepository schoolRepo,
            ITenantService tenantService,
            IMapper mapper,
            IWebHostEnvironment env,
            SchoolDbContext dbContext,
            IOrganizationCacheService cacheService,
            IDocumentService documentService)
        {
            _profileRepo = profileRepo;
            _schoolRepo = schoolRepo;
            _tenantService = tenantService;
            _mapper = mapper;
            _env = env;
            _dbContext = dbContext;
            _cacheService = cacheService;
            _documentService = documentService;
        }

        public async Task<APIResponse<OrganizationProfileDto>> GetMyProfileAsync()
        {
            var tenantId = _tenantService.GetTenantId();
            if (!tenantId.HasValue || tenantId.Value == 0)
            {
                // Fallback for Superadmin or Pre-Login state
                return new APIResponse<OrganizationProfileDto>
                {
                    Success = true,
                    Data = new OrganizationProfileDto
                    {
                        SchoolRegistrationId = 0,
                        OrganizationName = "SchoolSaaS Master Control",
                        SchoolName = "SchoolSaaS",
                        SchoolCode = "SAAS-001",
                        PrimaryColor = "#1e3a8a",
                        SecondaryColor = "#0d9488",
                        Theme = "Light",
                        HeaderLogo = "assets/images/logo.png", // Or default platform logo
                        Status = true
                    },
                    Message = "Global Platform Profile loaded for Superadmin",
                    StatusCode = HttpStatusCode.OK
                };
            }

            return await GetByTenantIdAsync(tenantId.Value);
        }

        public async Task<APIResponse<OrganizationProfileDto>> GetByTenantIdAsync(int tenantId)
        {
            try
            {
                var profile = await _profileRepo.GetByTenantIdAsync(tenantId);
                if (profile == null)
                {
                    // Fallback to SchoolRegistration to seed initial configuration details
                    var school = await _schoolRepo.GetSchoolByIdAsync(tenantId);
                    if (school == null)
                    {
                        return new APIResponse<OrganizationProfileDto>
                        {
                            Success = false,
                            Message = "Tenant registration record not found",
                            StatusCode = HttpStatusCode.NotFound
                        };
                    }

                    var dto = new OrganizationProfileDto
                    {
                        SchoolRegistrationId = tenantId,
                        OrganizationName = school.SchoolName,
                        SchoolName = school.SchoolName,
                        SchoolCode = school.SchoolCode,
                        Email = school.Email,
                        Phone = school.PhoneNumber,
                        AddressLine1 = school.Address,
                        Pincode = school.Pincode,
                        HeaderLogo = school.Logo,
                        GSTNumber = school.GSTNumber,
                        PANNumber = school.PANNumber,
                        AffiliationNumber = school.AffiliationNumber,
                        PrincipalName = school.ContactPersonName,
                        PrimaryColor = "#1e3a8a",
                        SecondaryColor = "#0d9488",
                        Theme = "Light",
                        Status = true
                    };

                    return new APIResponse<OrganizationProfileDto>
                    {
                        Success = true,
                        Data = dto,
                        Message = "Default settings resolved from school registration profile",
                        StatusCode = HttpStatusCode.OK
                    };
                }

                return new APIResponse<OrganizationProfileDto>
                {
                    Success = true,
                    Data = _mapper.Map<OrganizationProfileDto>(profile),
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<OrganizationProfileDto>
                {
                    Success = false,
                    Message = $"Failed to retrieve profile: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<OrganizationProfileDto>> UpdateAsync(OrganizationProfileModel model)
        {
            try
            {
                var tenantId = _tenantService.GetTenantId();
                if (!tenantId.HasValue || tenantId.Value == 0)
                {
                    return new APIResponse<OrganizationProfileDto>
                    {
                        Success = false,
                        Message = "Tenant context is invalid",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var existingProfile = await _profileRepo.GetByTenantIdAsync(tenantId.Value);

                if (existingProfile == null)
                {
                    // Create new profile record
                    var newProfile = _mapper.Map<OrganizationProfile>(model);
                    newProfile.SchoolRegistrationId = tenantId.Value;
                    newProfile.CreatedDate = DateTime.Now;
                    newProfile.CreatedBy = "System";

                    newProfile = await _profileRepo.AddAsync(newProfile);

                    // Log creation audit
                    await LogChangeAsync(tenantId.Value, "Create", "OrganizationProfile", null, newProfile.OrganizationName, "Initial setup");
                    await _dbContext.SaveChangesAsync();

                    var resultDto = _mapper.Map<OrganizationProfileDto>(newProfile);
                    _cacheService.Set(tenantId.Value, resultDto);

                    return new APIResponse<OrganizationProfileDto>
                    {
                        Success = true,
                        Message = "Organization profile created successfully",
                        Data = resultDto,
                        StatusCode = HttpStatusCode.Created
                    };
                }
                else
                {
                    // Track changes for Audits
                    var properties = typeof(OrganizationProfileModel).GetProperties();
                    foreach (var prop in properties)
                    {
                        if (prop.Name == "Id" || prop.Name == "SchoolRegistrationId" || prop.Name == "BranchId") continue;

                        var newValue = prop.GetValue(model)?.ToString();
                        var entityProp = typeof(OrganizationProfile).GetProperty(prop.Name);
                        var oldValue = entityProp?.GetValue(existingProfile)?.ToString();

                        if (newValue != oldValue)
                        {
                            await LogChangeAsync(tenantId.Value, "Update", prop.Name, oldValue, newValue, "Settings modification");
                        }
                    }

                    // Update existing profile details
                    _mapper.Map(model, existingProfile);
                    existingProfile.SchoolRegistrationId = tenantId.Value;
                    existingProfile.UpdatedDate = DateTime.Now;
                    existingProfile.UpdatedBy = "System";

                    await _profileRepo.UpdateAsync(existingProfile);
                    await _dbContext.SaveChangesAsync();

                    var resultDto = _mapper.Map<OrganizationProfileDto>(existingProfile);
                    _cacheService.Set(tenantId.Value, resultDto);

                    return new APIResponse<OrganizationProfileDto>
                    {
                        Success = true,
                        Message = "Organization profile updated successfully",
                        Data = resultDto,
                        StatusCode = HttpStatusCode.OK
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse<OrganizationProfileDto>
                {
                    Success = false,
                    Message = $"Failed to update profile: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<string>> UploadBrandingFileAsync(string fileType, string base64Data, string fileName)
        {
            try
            {
                var tenantId = _tenantService.GetTenantId() ?? 1;

                var base64Parts = base64Data.Split(',');
                string cleanBase64 = base64Parts.Length > 1 ? base64Parts[1] : base64Parts[0];
                byte[] bytes = Convert.FromBase64String(cleanBase64);

                string extension = Path.GetExtension(fileName);
                if (string.IsNullOrEmpty(extension))
                {
                    extension = ".png";
                }

                try
                {
                    ValidateAndOptimizeImage(bytes, extension);
                }
                catch (Exception valEx)
                {
                    return new APIResponse<string>
                    {
                        Success = false,
                        Message = valEx.Message,
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                string folderName = $"branding_{tenantId}";
                string webPath = await _documentService.UploadAsync(bytes, fileName, folderName);
                return new APIResponse<string>
                {
                    Success = true,
                    Data = webPath,
                    Message = "File uploaded and validated successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>
                {
                    Success = false,
                    Message = $"Upload failed: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<System.Collections.Generic.List<global::School.Domain.School.OrganizationProfileAudit>>> GetAuditHistoryAsync()
        {
            var tenantId = _tenantService.GetTenantId() ?? 1;
            var audits = await _dbContext.OrganizationProfileAudits
                .Where(a => a.SchoolRegistrationId == tenantId)
                .OrderByDescending(a => a.PerformedDate)
                .ToListAsync();

            return new APIResponse<System.Collections.Generic.List<global::School.Domain.School.OrganizationProfileAudit>>
            {
                Success = true,
                Data = audits,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<OrganizationProfileDto>> ResetBrandingAsync()
        {
            try
            {
                var tenantId = _tenantService.GetTenantId() ?? 1;
                var school = await _schoolRepo.GetSchoolByIdAsync(tenantId);
                if (school == null)
                {
                    return new APIResponse<OrganizationProfileDto>
                    {
                        Success = false,
                        Message = "School registration details not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var existingProfile = await _profileRepo.GetByTenantIdAsync(tenantId);
                if (existingProfile == null)
                {
                    existingProfile = new OrganizationProfile { SchoolRegistrationId = tenantId };
                    _dbContext.OrganizationProfiles.Add(existingProfile);
                }

                existingProfile.OrganizationName = school.SchoolName;
                existingProfile.SchoolName = school.SchoolName;
                existingProfile.ShortName = school.SchoolCode;
                existingProfile.SchoolCode = school.SchoolCode;
                existingProfile.Email = school.Email;
                existingProfile.Phone = school.PhoneNumber;
                existingProfile.AddressLine1 = school.Address;
                existingProfile.Pincode = school.Pincode;
                existingProfile.HeaderLogo = school.Logo;
                existingProfile.PrimaryColor = "#1e3a8a";
                existingProfile.SecondaryColor = "#0d9488";
                existingProfile.AccentColor = "#3b82f6";
                existingProfile.Theme = "Light";
                existingProfile.FontFamily = "Inter";
                existingProfile.FontSize = "14px";
                existingProfile.PrincipalName = school.ContactPersonName;
                existingProfile.PrincipalSignature = null;
                existingProfile.DirectorSignature = null;
                existingProfile.OfficialSeal = null;
                existingProfile.RoundSeal = null;
                existingProfile.RectangleSeal = null;
                existingProfile.ReportWatermark = null;
                existingProfile.ReportBackground = null;
                existingProfile.Status = true;
                existingProfile.UpdatedDate = DateTime.Now;
                existingProfile.UpdatedBy = "System Reset";

                await _dbContext.SaveChangesAsync();
                _cacheService.Remove(tenantId);

                var resultDto = _mapper.Map<OrganizationProfileDto>(existingProfile);
                _cacheService.Set(tenantId, resultDto);

                await LogChangeAsync(tenantId, "Reset", "All", null, null, "Branding config reset to system defaults");
                await _dbContext.SaveChangesAsync();

                return new APIResponse<OrganizationProfileDto>
                {
                    Success = true,
                    Data = resultDto,
                    Message = "Branding configuration has been reset to defaults",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<OrganizationProfileDto>
                {
                    Success = false,
                    Message = $"Reset failed: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<string>> ExportBrandingAsync()
        {
            var tenantId = _tenantService.GetTenantId() ?? 1;
            var profile = await _profileRepo.GetByTenantIdAsync(tenantId);
            if (profile == null)
            {
                return new APIResponse<string>
                {
                    Success = false,
                    Message = "No branding profile exists to export",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var jsonConfig = System.Text.Json.JsonSerializer.Serialize(profile, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            return new APIResponse<string>
            {
                Success = true,
                Data = jsonConfig,
                Message = "Branding settings serialized successfully",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<OrganizationProfileDto>> ImportBrandingAsync(string jsonConfig)
        {
            try
            {
                var tenantId = _tenantService.GetTenantId() ?? 1;
                var imported = System.Text.Json.JsonSerializer.Deserialize<OrganizationProfile>(jsonConfig);
                if (imported == null)
                {
                    return new APIResponse<OrganizationProfileDto>
                    {
                        Success = false,
                        Message = "Invalid backup configuration data",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var existingProfile = await _profileRepo.GetByTenantIdAsync(tenantId);
                if (existingProfile == null)
                {
                    existingProfile = new OrganizationProfile { SchoolRegistrationId = tenantId };
                    _dbContext.OrganizationProfiles.Add(existingProfile);
                }

                existingProfile.OrganizationName = imported.OrganizationName;
                existingProfile.ShortName = imported.ShortName;
                existingProfile.DisplayName = imported.DisplayName;
                existingProfile.SchoolName = imported.SchoolName;
                existingProfile.CollegeName = imported.CollegeName;
                existingProfile.UniversityName = imported.UniversityName;
                existingProfile.CampusName = imported.CampusName;
                existingProfile.AffiliationNumber = imported.AffiliationNumber;
                existingProfile.RecognitionNumber = imported.RecognitionNumber;
                existingProfile.SchoolCode = imported.SchoolCode;
                existingProfile.CollegeCode = imported.CollegeCode;
                existingProfile.UniversityCode = imported.UniversityCode;
                existingProfile.Board = imported.Board;
                existingProfile.University = imported.University;
                existingProfile.RegistrationNumber = imported.RegistrationNumber;
                existingProfile.GSTNumber = imported.GSTNumber;
                existingProfile.PANNumber = imported.PANNumber;
                existingProfile.TANNumber = imported.TANNumber;
                existingProfile.UDISENumber = imported.UDISENumber;
                existingProfile.AISHECode = imported.AISHECode;
                existingProfile.AddressLine1 = imported.AddressLine1;
                existingProfile.AddressLine2 = imported.AddressLine2;
                existingProfile.Landmark = imported.Landmark;
                existingProfile.City = imported.City;
                existingProfile.District = imported.District;
                existingProfile.State = imported.State;
                existingProfile.Country = imported.Country;
                existingProfile.Pincode = imported.Pincode;
                existingProfile.Phone = imported.Phone;
                existingProfile.Mobile = imported.Mobile;
                existingProfile.WhatsApp = imported.WhatsApp;
                existingProfile.Email = imported.Email;
                existingProfile.Website = imported.Website;
                existingProfile.HelpdeskEmail = imported.HelpdeskEmail;
                existingProfile.SupportPhone = imported.SupportPhone;
                existingProfile.FacebookUrl = imported.FacebookUrl;
                existingProfile.InstagramUrl = imported.InstagramUrl;
                existingProfile.LinkedInUrl = imported.LinkedInUrl;
                existingProfile.TwitterUrl = imported.TwitterUrl;
                existingProfile.YouTubeUrl = imported.YouTubeUrl;
                existingProfile.Telegram = imported.Telegram;
                existingProfile.PrincipalName = imported.PrincipalName;
                existingProfile.DirectorName = imported.DirectorName;
                existingProfile.ChairmanName = imported.ChairmanName;
                existingProfile.SecretaryName = imported.SecretaryName;
                existingProfile.RegistrarName = imported.RegistrarName;
                existingProfile.VicePrincipalName = imported.VicePrincipalName;
                existingProfile.AccountantName = imported.AccountantName;
                existingProfile.PrimaryColor = imported.PrimaryColor;
                existingProfile.SecondaryColor = imported.SecondaryColor;
                existingProfile.AccentColor = imported.AccentColor;
                existingProfile.Theme = imported.Theme;
                existingProfile.FontFamily = imported.FontFamily;
                existingProfile.FontSize = imported.FontSize;
                existingProfile.ReportFooterText = imported.ReportFooterText;
                existingProfile.CopyrightText = imported.CopyrightText;
                existingProfile.Disclaimer = imported.Disclaimer;
                existingProfile.TermsAndConditions = imported.TermsAndConditions;
                existingProfile.Status = true;
                existingProfile.UpdatedDate = DateTime.Now;
                existingProfile.UpdatedBy = "Backup Restoration";

                if (!string.IsNullOrEmpty(imported.HeaderLogo)) existingProfile.HeaderLogo = imported.HeaderLogo;
                if (!string.IsNullOrEmpty(imported.FooterLogo)) existingProfile.FooterLogo = imported.FooterLogo;
                if (!string.IsNullOrEmpty(imported.LogoLight)) existingProfile.LogoLight = imported.LogoLight;
                if (!string.IsNullOrEmpty(imported.LogoDark)) existingProfile.LogoDark = imported.LogoDark;
                if (!string.IsNullOrEmpty(imported.LoginLogo)) existingProfile.LoginLogo = imported.LoginLogo;
                if (!string.IsNullOrEmpty(imported.Favicon)) existingProfile.Favicon = imported.Favicon;
                if (!string.IsNullOrEmpty(imported.ReportWatermark)) existingProfile.ReportWatermark = imported.ReportWatermark;
                if (!string.IsNullOrEmpty(imported.PrincipalSignature)) existingProfile.PrincipalSignature = imported.PrincipalSignature;
                if (!string.IsNullOrEmpty(imported.DirectorSignature)) existingProfile.DirectorSignature = imported.DirectorSignature;
                if (!string.IsNullOrEmpty(imported.OfficialSeal)) existingProfile.OfficialSeal = imported.OfficialSeal;

                await _dbContext.SaveChangesAsync();
                _cacheService.Remove(tenantId);

                var resultDto = _mapper.Map<OrganizationProfileDto>(existingProfile);
                _cacheService.Set(tenantId, resultDto);

                await LogChangeAsync(tenantId, "Import", "All", null, null, "Branding config restored from backup file");
                await _dbContext.SaveChangesAsync();

                return new APIResponse<OrganizationProfileDto>
                {
                    Success = true,
                    Data = resultDto,
                    Message = "Branding settings restored successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<OrganizationProfileDto>
                {
                    Success = false,
                    Message = $"Import failed: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        private void ValidateAndOptimizeImage(byte[] bytes, string extension)
        {
            if (bytes.Length > 5 * 1024 * 1024)
            {
                throw new Exception("File size exceeds the 5MB upload limit.");
            }

            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".webp", ".svg", ".ico", ".pdf" };
            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                throw new Exception($"File format '{extension}' is not supported. Supported extensions: PNG, SVG, JPEG, WEBP, ICO, PDF");
            }

            if (extension.Equals(".svg", StringComparison.OrdinalIgnoreCase) ||
                extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase) ||
                extension.Equals(".ico", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            try
            {
                using var image = Image.Load(bytes);
                if (image.Width < 10 || image.Height < 10)
                {
                    throw new Exception("Image dimensions are too small (minimum size: 10x10 px)");
                }
            }
            catch (Exception ex) when (!ex.Message.Contains("supported") && !ex.Message.Contains("too small"))
            {
                throw new Exception($"Failed to decode and validate image format: {ex.Message}");
            }
        }

        private void GenerateThumbnail(byte[] bytes, string uploadFolder, string safeFileName, string extension)
        {
            if (extension.Equals(".svg", StringComparison.OrdinalIgnoreCase) ||
                extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase) ||
                extension.Equals(".ico", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            try
            {
                using var image = Image.Load(bytes);
                image.Mutate(x => x.Resize(new Size(128, 128)));
                string thumbFileName = Path.GetFileNameWithoutExtension(safeFileName) + "_thumb" + extension;
                string thumbPath = Path.Combine(uploadFolder, thumbFileName);
                image.Save(thumbPath);
            }
            catch
            {
                // Suppress thumbnail failures silently to not block file upload
            }
        }

        private async Task LogChangeAsync(int tenantId, string action, string propertyName, string? oldValue, string? newValue, string? reason)
        {
            var audit = new OrganizationProfileAudit
            {
                SchoolRegistrationId = tenantId,
                Action = action,
                PropertyName = propertyName,
                OldValue = oldValue ?? string.Empty,
                NewValue = newValue ?? string.Empty,
                Reason = reason ?? "Branding changes updated via settings panel",
                PerformedBy = "School Admin",
                PerformedDate = DateTime.UtcNow
            };
            await _dbContext.OrganizationProfileAudits.AddAsync(audit);
        }
    }

    public class BrandingService : IBrandingService
    {
        private readonly ITenantBrandingProvider _brandingProvider;

        public BrandingService(ITenantBrandingProvider brandingProvider)
        {
            _brandingProvider = brandingProvider;
        }

        public Task<OrganizationProfileDto> GetProfileAsync()
        {
            return _brandingProvider.GetBrandingAsync();
        }
    }

    public class ThemeService : IThemeService
    {
        private readonly ITenantBrandingProvider _brandingProvider;

        public ThemeService(ITenantBrandingProvider brandingProvider)
        {
            _brandingProvider = brandingProvider;
        }

        public async Task<ThemeSettingsDto> GetThemeSettingsAsync()
        {
            var branding = await _brandingProvider.GetBrandingAsync();
            return new ThemeSettingsDto
            {
                PrimaryColor = branding.PrimaryColor,
                SecondaryColor = branding.SecondaryColor,
                AccentColor = branding.AccentColor,
                FontFamily = branding.FontFamily,
                FontSize = branding.FontSize,
                Theme = branding.Theme,
                LightTheme = branding.LightTheme,
                DarkTheme = branding.DarkTheme
            };
        }
    }

    public class LogoService : ILogoService
    {
        private readonly ITenantBrandingProvider _brandingProvider;

        public LogoService(ITenantBrandingProvider brandingProvider)
        {
            _brandingProvider = brandingProvider;
        }

        public async Task<LogoSettingsDto> GetLogosAsync()
        {
            var branding = await _brandingProvider.GetBrandingAsync();
            return new LogoSettingsDto
            {
                LogoLight = branding.LogoLight,
                LogoDark = branding.LogoDark,
                HeaderLogo = branding.HeaderLogo,
                FooterLogo = branding.FooterLogo,
                LoginLogo = branding.LoginLogo,
                Favicon = branding.Favicon,
                MobileAppIcon = branding.MobileAppIcon,
                SplashScreenLogo = branding.SplashScreenLogo,
                EmailLogo = branding.EmailLogo,
                PDFLogo = branding.PDFLogo
            };
        }
    }

    public class SignatureService : ISignatureService
    {
        private readonly ITenantBrandingProvider _brandingProvider;

        public SignatureService(ITenantBrandingProvider brandingProvider)
        {
            _brandingProvider = brandingProvider;
        }

        public async Task<SignatureSettingsDto> GetSignaturesAsync()
        {
            var branding = await _brandingProvider.GetBrandingAsync();
            return new SignatureSettingsDto
            {
                ChairmanSignature = branding.ChairmanSignature,
                DirectorSignature = branding.DirectorSignature,
                PrincipalSignature = branding.PrincipalSignature,
                RegistrarSignature = branding.RegistrarSignature,
                DigitalSignature = branding.DigitalSignature,
                OfficialSeal = branding.OfficialSeal,
                RoundSeal = branding.RoundSeal,
                RectangleSeal = branding.RectangleSeal
            };
        }
    }

    public class WatermarkService : IWatermarkService
    {
        private readonly ITenantBrandingProvider _brandingProvider;

        public WatermarkService(ITenantBrandingProvider brandingProvider)
        {
            _brandingProvider = brandingProvider;
        }

        public async Task<WatermarkSettingsDto> GetWatermarkAsync()
        {
            var branding = await _brandingProvider.GetBrandingAsync();
            return new WatermarkSettingsDto
            {
                ReportWatermark = branding.ReportWatermark,
                ReportBackground = branding.ReportBackground,
                WatermarkImage = branding.ReportWatermark
            };
        }
    }

    public class OrganizationCacheService : IOrganizationCacheService
    {
        private readonly IMemoryCache _cache;
        private const string CacheKeyPrefix = "OrgProfile_";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(2);

        public OrganizationCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public OrganizationProfileDto? Get(int tenantId)
        {
            _cache.TryGetValue(GetCacheKey(tenantId), out OrganizationProfileDto? profile);
            return profile;
        }

        public void Set(int tenantId, OrganizationProfileDto profile)
        {
            _cache.Set(GetCacheKey(tenantId), profile, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = CacheDuration
            });
        }

        public void Remove(int tenantId)
        {
            _cache.Remove(GetCacheKey(tenantId));
        }

        private string GetCacheKey(int tenantId) => $"{CacheKeyPrefix}{tenantId}";
    }

    public class TenantBrandingProvider : ITenantBrandingProvider
    {
        private readonly IOrganizationProfileService _profileService;
        private readonly ITenantService _tenantService;
        private readonly IOrganizationCacheService _cacheService;

        public TenantBrandingProvider(
            IOrganizationProfileService profileService,
            ITenantService tenantService,
            IOrganizationCacheService cacheService)
        {
            _profileService = profileService;
            _tenantService = tenantService;
            _cacheService = cacheService;
        }

        public async Task<OrganizationProfileDto> GetBrandingAsync()
        {
            var tenantId = _tenantService.GetTenantId() ?? 1;
            var cached = _cacheService.Get(tenantId);
            if (cached != null)
            {
                return cached;
            }

            var response = await _profileService.GetByTenantIdAsync(tenantId);
            if (response.Success && response.Data != null)
            {
                _cacheService.Set(tenantId, response.Data);
                return response.Data;
            }

            return new OrganizationProfileDto
            {
                SchoolRegistrationId = tenantId,
                OrganizationName = "Default SchoolSaaS",
                PrimaryColor = "#1e3a8a",
                SecondaryColor = "#0d9488",
                Theme = "Light"
            };
        }
    }

    public class ReportBrandingService : IReportBrandingService
    {
        private readonly IOrganizationProfileService _profileService;
        private readonly ITenantService _tenantService;

        public ReportBrandingService(IOrganizationProfileService profileService, ITenantService tenantService)
        {
            _profileService = profileService;
            _tenantService = tenantService;
        }

        public async Task<OrganizationProfileDto> GetBrandingAsync()
        {
            var tenantId = _tenantService.GetTenantId() ?? 1;
            var response = await _profileService.GetByTenantIdAsync(tenantId);

            if (response.Success && response.Data != null)
            {
                return response.Data;
            }

            return new OrganizationProfileDto
            {
                SchoolRegistrationId = tenantId,
                OrganizationName = "Default SchoolSaaS",
                PrimaryColor = "#1e3a8a",
                SecondaryColor = "#0d9488",
                Theme = "Light"
            };
        }
    }
}
