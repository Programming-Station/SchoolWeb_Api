using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Models.School;
using School.Services.School.ISchoolServices;
using School_API.Common.Interface;
using School_DTOs;
using School_DTOs.School;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace School_API.Controllers.School
{
    [Route("api/[controller]")]
    public class OrganizationProfileController : BaseController
    {
        private readonly IOrganizationProfileService _profileService;

        public OrganizationProfileController(
            IOrganizationProfileService profileService,
            ICurrentUserService currentUserService) : base(currentUserService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse<OrganizationProfileDto>>> GetMyProfile()
        {
            var response = await _profileService.GetMyProfileAsync();
            return Ok(response);
        }

        [HttpGet("tenant/{tenantId}")]
        public async Task<ActionResult<APIResponse<OrganizationProfileDto>>> GetByTenantId(int tenantId)
        {
            var response = await _profileService.GetByTenantIdAsync(tenantId);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse<OrganizationProfileDto>>> Update([FromBody] OrganizationProfileModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _profileService.UpdateAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("upload-file")]
        public async Task<ActionResult<APIResponse<string>>> UploadFile([FromQuery] string fileType, [FromQuery] string fileName, [FromBody] string base64Data)
        {
            if (string.IsNullOrWhiteSpace(base64Data))
            {
                return BadRequest("File data is required");
            }

            var response = await _profileService.UploadBrandingFileAsync(fileType, base64Data, fileName);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("audit-history")]
        public async Task<ActionResult<APIResponse<List<global::School.Domain.School.OrganizationProfileAudit>>>> GetAuditHistory()
        {
            var response = await _profileService.GetAuditHistoryAsync();
            return Ok(response);
        }

        [HttpPost("reset")]
        public async Task<ActionResult<APIResponse<OrganizationProfileDto>>> ResetBranding()
        {
            var response = await _profileService.ResetBrandingAsync();
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("export")]
        public async Task<ActionResult<APIResponse<string>>> ExportBranding()
        {
            var response = await _profileService.ExportBrandingAsync();
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("import")]
        public async Task<ActionResult<APIResponse<OrganizationProfileDto>>> ImportBranding([FromBody] string jsonConfig)
        {
            if (string.IsNullOrWhiteSpace(jsonConfig))
            {
                return BadRequest("Configuration data is required");
            }

            var response = await _profileService.ImportBrandingAsync(jsonConfig);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("preview")]
        public async Task<ActionResult<APIResponse<OrganizationProfileDto>>> PreviewBranding([FromBody] OrganizationProfileModel model)
        {
            // Just maps model to DTO and returns for client-side preview
            var dto = new OrganizationProfileDto
            {
                OrganizationName = model.OrganizationName,
                ShortName = model.ShortName,
                DisplayName = model.DisplayName,
                SchoolName = model.SchoolName,
                CollegeName = model.CollegeName,
                UniversityName = model.UniversityName,
                CampusName = model.CampusName,
                PrimaryColor = model.PrimaryColor,
                SecondaryColor = model.SecondaryColor,
                AccentColor = model.AccentColor,
                Theme = model.Theme,
                FontFamily = model.FontFamily,
                FontSize = model.FontSize,
                HeaderLogo = model.HeaderLogo,
                FooterLogo = model.FooterLogo,
                LogoLight = model.LogoLight,
                LogoDark = model.LogoDark,
                LoginLogo = model.LoginLogo,
                Favicon = model.Favicon,
                ReportWatermark = model.ReportWatermark,
                PrincipalSignature = model.PrincipalSignature,
                DirectorSignature = model.DirectorSignature,
                OfficialSeal = model.OfficialSeal,
                ReportFooterText = model.ReportFooterText,
                CopyrightText = model.CopyrightText,
                Disclaimer = model.Disclaimer,
                TermsAndConditions = model.TermsAndConditions,
                Status = true
            };

            return Ok(new APIResponse<OrganizationProfileDto>
            {
                Success = true,
                Data = dto,
                Message = "Preview generated successfully",
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }
    }
}
