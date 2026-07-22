using School.Infrastructure.Interfaces;
using School_DTOs.School;

namespace School.Services.School.ISchoolServices
{
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
