using Microsoft.AspNetCore.Mvc;
using School.Models.Fee;
namespace School_API.Controllers.Fee
{
    public partial class FeeController
    {

        [HttpGet]
        public async Task<IActionResult> GetFeeTypeByBySchoolId(int schoolId)
        {
            var result = await _feetype.GetFeeTypesBySchoolIdAsync(schoolId);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateFeeType(FeeTypeModel model)
        {
            model.CreatedBy = UserName;

            var result = await _feetype.AddFeeTypeAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
