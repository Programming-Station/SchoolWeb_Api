using School.Models.AffiliationCollege;
using School.Services.Interfaces;
using School_API.Common.Interface;
using Microsoft.AspNetCore.Mvc;

namespace School_API.Controllers
{
    public class AffiliationCollegeController : BaseController
    {
        private readonly IAffiliatedService _collegeService;

        public AffiliationCollegeController(
            IAffiliatedService collegeService,
            ICurrentUserService currentUser) : base(currentUser)
        {
            _collegeService = collegeService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCollege([FromBody] AffiliatedModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.CreatedBy = UserName;
            var result = await _collegeService.AddAffiliationCollegeAsync(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCollegeById(int id)
        {
            var result = await _collegeService.GetAffiliationCollegeByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllColleges(
            [FromQuery] int? stateId = null,
            [FromQuery] int? cityId = null,
            [FromQuery] bool? isActive = null)
        {
            var result = await _collegeService.GetAllAffiliationCollegesAsync(stateId, cityId, isActive);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCollege([FromBody] AffiliatedModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.UpdatedBy = UserName;
            var result = await _collegeService.UpdateAffiliationCollegeAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCollege(int id)
        {
            var result = await _collegeService.DeleteAffiliationCollegeAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch("toggle-status/{id}")]
        public async Task<IActionResult> ToggleCollegeStatus(int id)
        {
            var result = await _collegeService.ToggleAffiliationCollegeStatusAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}

