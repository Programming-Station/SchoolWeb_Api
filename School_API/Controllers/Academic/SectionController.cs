using Microsoft.AspNetCore.Mvc;
using School.Models.Class;
using School.Services.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers.Academic
{
    public class SectionController : BaseController
    {
        private readonly ISectionService _sectionService;

        public SectionController(ISectionService sectionService, ICurrentUserService currentUser) : base(currentUser)
        {
            _sectionService = sectionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSection([FromBody] SectionModel model)
        {
            model.CreatedBy = UserName;
            var result = await _sectionService.AddSectionAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetSectionById(int id)
        {
            var result = await _sectionService.GetSectionByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSections()
        {
            var result = await _sectionService.GetAllSectionsAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetSectionsByClassId(int classId)
        {
            var result = await _sectionService.GetSectionsByClassIdAsync(classId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSection([FromBody] SectionModel model)
        {
            model.CreatedBy = UserName;
            var result = await _sectionService.UpdateSectionAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSection(int id)
        {
            var result = await _sectionService.DeleteSectionAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> ToggleSectionStatus(int id)
        {
            var result = await _sectionService.ToggleSectionStatusAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
