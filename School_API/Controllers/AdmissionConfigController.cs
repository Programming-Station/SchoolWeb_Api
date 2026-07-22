using Microsoft.AspNetCore.Mvc;
using School.Models.Academic;
using School.Models.Fee;
using School.Models.School;
using School.Models.Student;
using School.Services.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers
{
    public class AdmissionConfigController : BaseController
    {
        private readonly IAdmissionService _admissionService;

        public AdmissionConfigController(
            IAdmissionService admissionService,
            ICurrentUserService currentUserService)
            : base(currentUserService)
        {
            _admissionService = admissionService;
        }

        #region Campus Endpoints
        [HttpPost]
        public async Task<IActionResult> CreateCampus([FromBody] CampusModel model)
        {
            var result = await _admissionService.CreateCampusAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCampusById(int id)
        {
            var result = await _admissionService.GetCampusByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCampuses([FromQuery] bool? isActive = null)
        {
            var result = await _admissionService.GetAllCampusesAsync(isActive);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCampus([FromBody] CampusModel model)
        {
            var result = await _admissionService.UpdateCampusAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampus(int id)
        {
            var result = await _admissionService.DeleteCampusAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Education Level Endpoints
        [HttpPost]
        public async Task<IActionResult> CreateEducationLevel([FromBody] EducationLevelModel model)
        {
            var result = await _admissionService.CreateEducationLevelAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEducationLevelById(int id)
        {
            var result = await _admissionService.GetEducationLevelByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetEducationLevels([FromQuery] bool? isActive = null)
        {
            var result = await _admissionService.GetAllEducationLevelsAsync(isActive);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEducationLevel([FromBody] EducationLevelModel model)
        {
            var result = await _admissionService.UpdateEducationLevelAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEducationLevel(int id)
        {
            var result = await _admissionService.DeleteEducationLevelAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Program Endpoints
        [HttpPost]
        public async Task<IActionResult> CreateProgram([FromBody] ProgramModel model)
        {
            var result = await _admissionService.CreateProgramAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgramById(int id)
        {
            var result = await _admissionService.GetProgramByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPrograms([FromQuery] int? educationLevelId = null, [FromQuery] bool? isActive = null)
        {
            var result = await _admissionService.GetAllProgramsAsync(educationLevelId, isActive);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProgram([FromBody] ProgramModel model)
        {
            var result = await _admissionService.UpdateProgramAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            var result = await _admissionService.DeleteProgramAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Branch Endpoints
        [HttpPost]
        public async Task<IActionResult> CreateBranch([FromBody] BranchModel model)
        {
            var result = await _admissionService.CreateBranchAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBranchById(int id)
        {
            var result = await _admissionService.GetBranchByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetBranches([FromQuery] int? programId = null, [FromQuery] bool? isActive = null)
        {
            var result = await _admissionService.GetAllBranchesAsync(programId, isActive);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBranch([FromBody] BranchModel model)
        {
            var result = await _admissionService.UpdateBranchAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var result = await _admissionService.DeleteBranchAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Year/Semester Endpoints
        [HttpPost]
        public async Task<IActionResult> CreateYearSemester([FromBody] YearSemesterModel model)
        {
            var result = await _admissionService.CreateYearSemesterAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetYearSemesterById(int id)
        {
            var result = await _admissionService.GetYearSemesterByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetYearSemesters([FromQuery] bool? isActive = null)
        {
            var result = await _admissionService.GetAllYearSemestersAsync(isActive);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateYearSemester([FromBody] YearSemesterModel model)
        {
            var result = await _admissionService.UpdateYearSemesterAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteYearSemester(int id)
        {
            var result = await _admissionService.DeleteYearSemesterAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Batch Endpoints
        [HttpPost]
        public async Task<IActionResult> CreateBatch([FromBody] BatchModel model)
        {
            var result = await _admissionService.CreateBatchAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBatchById(int id)
        {
            var result = await _admissionService.GetBatchByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetBatches([FromQuery] int? programId = null, [FromQuery] bool? isActive = null)
        {
            var result = await _admissionService.GetAllBatchesAsync(programId, isActive);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBatch([FromBody] BatchModel model)
        {
            var result = await _admissionService.UpdateBatchAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBatch(int id)
        {
            var result = await _admissionService.DeleteBatchAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Admission Form Config Endpoints
        [HttpPost]
        public async Task<IActionResult> CreateFormConfig([FromBody] AdmissionFormConfigModel model)
        {
            var result = await _admissionService.CreateFormConfigAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFormConfigById(int id)
        {
            var result = await _admissionService.GetFormConfigByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormConfig([FromQuery] int campusId, [FromQuery] int educationLevelId, [FromQuery] int? programId = null)
        {
            var result = await _admissionService.GetFormConfigAsync(campusId, educationLevelId, programId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormConfigs()
        {
            var result = await _admissionService.GetAllFormConfigsAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFormConfig([FromBody] AdmissionFormConfigModel model)
        {
            var result = await _admissionService.UpdateFormConfigAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFormConfig(int id)
        {
            var result = await _admissionService.DeleteFormConfigAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Admission Rules Endpoints
        [HttpPost]
        public async Task<IActionResult> CreateRule([FromBody] AdmissionRuleModel model)
        {
            var result = await _admissionService.CreateRuleAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRuleById(int id)
        {
            var result = await _admissionService.GetRuleByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetRules()
        {
            var result = await _admissionService.GetAllRulesAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRule([FromBody] AdmissionRuleModel model)
        {
            var result = await _admissionService.UpdateRuleAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRule(int id)
        {
            var result = await _admissionService.DeleteRuleAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Fee Structure Endpoints
        [HttpPost]
        public async Task<IActionResult> CreateFeeStructure([FromBody] FeeStructureModel model)
        {
            var result = await _admissionService.CreateFeeStructureAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeeStructureById(int id)
        {
            var result = await _admissionService.GetFeeStructureByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetFeeStructure([FromQuery] int campusId, [FromQuery] int programId, [FromQuery] int batchId)
        {
            var result = await _admissionService.GetFeeStructureAsync(campusId, programId, batchId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetFeeStructures()
        {
            var result = await _admissionService.GetAllFeeStructuresAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFeeStructure([FromBody] FeeStructureModel model)
        {
            var result = await _admissionService.UpdateFeeStructureAsync(model, UserName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeeStructure(int id)
        {
            var result = await _admissionService.DeleteFeeStructureAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}
