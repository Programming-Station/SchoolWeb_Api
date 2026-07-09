using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers.Fee
{
    public class FeeReportController : BaseController
    {
        private readonly IFeeReportService _reportService;
        private readonly ITenantService _tenant;

        public FeeReportController(ICurrentUserService cur, IFeeReportService reportService, ITenantService tenant) : base(cur)
        {
            _reportService = reportService;
            _tenant = tenant;
        }

        [HttpGet("Summary")]
        public async Task<IActionResult> GetSummary([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var summary = await _reportService.GetCollectionSummaryAsync(fromDate, toDate, schoolId);
            return Ok(summary);
        }

        [HttpGet("HeadBreakup")]
        public async Task<IActionResult> GetHeadBreakup([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var breakup = await _reportService.GetHeadBreakupAsync(fromDate, toDate, schoolId);
            return Ok(breakup);
        }

        [HttpGet("ClassWiseSummary")]
        public async Task<IActionResult> GetClassWiseSummary()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var summary = await _reportService.GetClassWiseSummaryAsync(schoolId);
            return Ok(summary);
        }
    }
}
