using School_DTOs;
using School_DTOs.School;
using School.Models.School;
using System.Threading.Tasks;

namespace School.Services.School.ISchoolServices
{
    public interface IOrganizationProfileService
    {
        Task<APIResponse<OrganizationProfileDto>> GetMyProfileAsync();
        Task<APIResponse<OrganizationProfileDto>> GetByTenantIdAsync(int tenantId);
        Task<APIResponse<OrganizationProfileDto>> UpdateAsync(OrganizationProfileModel model);
        Task<APIResponse<string>> UploadBrandingFileAsync(string fileType, string base64Data, string fileName);
        Task<APIResponse<System.Collections.Generic.List<global::School.Domain.School.OrganizationProfileAudit>>> GetAuditHistoryAsync();
        Task<APIResponse<OrganizationProfileDto>> ResetBrandingAsync();
        Task<APIResponse<string>> ExportBrandingAsync();
        Task<APIResponse<OrganizationProfileDto>> ImportBrandingAsync(string jsonConfig);
    }

    public interface IBrandingService
    {
        Task<OrganizationProfileDto> GetProfileAsync();
    }

    public interface IThemeService
    {
        Task<ThemeSettingsDto> GetThemeSettingsAsync();
    }

    public interface ILogoService
    {
        Task<LogoSettingsDto> GetLogosAsync();
    }

    public interface ISignatureService
    {
        Task<SignatureSettingsDto> GetSignaturesAsync();
    }

    public interface IWatermarkService
    {
        Task<WatermarkSettingsDto> GetWatermarkAsync();
    }

    public interface IOrganizationCacheService
    {
        OrganizationProfileDto? Get(int tenantId);
        void Set(int tenantId, OrganizationProfileDto profile);
        void Remove(int tenantId);
    }

    public interface IReportBrandingService
    {
        Task<OrganizationProfileDto> GetBrandingAsync();
    }

    public interface ITenantBrandingProvider
    {
        Task<OrganizationProfileDto> GetBrandingAsync();
    }
}
