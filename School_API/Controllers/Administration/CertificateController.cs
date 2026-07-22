using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School_DTOs.Administration;

namespace School_API.Controllers.Administration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CertificateController : BaseController
    {
        private readonly ICertificateService _svc;
        private readonly ITenantService _tenant;

        public CertificateController(ICertificateService svc, ITenantService tenant, ICurrentUserService cur) : base(cur)
        {
            _svc = svc;
            _tenant = tenant;
        }

        private int SchoolId => _tenant.GetTenantId() ?? 1;

        [HttpGet]
        public async Task<IActionResult> GetCertificates([FromQuery] CertificateFilterDto filter)
        {
            var r = await _svc.GetCertificatesAsync(filter, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCertificateById(int id)
        {
            var r = await _svc.GetCertificateByIdAsync(id, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> IssueCertificate([FromBody] CreateCertificateIssuanceDto dto)
        {
            var r = await _svc.IssueCertificateAsync(dto, UserId, UserName, SchoolId);
            return StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> RevokeCertificate(int id, [FromQuery] string reason)
        {
            var r = await _svc.RevokeCertificateAsync(id, reason, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> IncrementPrintCount(int id)
        {
            var r = await _svc.IncrementPrintCountAsync(id, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> GetCertificatePdf(int id)
        {
            var r = await _svc.GeneratePdfAsync(id, SchoolId);
            if (!r.Success || r.Data == null)
            {
                return StatusCode((int)r.StatusCode, r);
            }
            return File(r.Data, "application/pdf", $"certificate_{id}.pdf");
        }
    }
}
