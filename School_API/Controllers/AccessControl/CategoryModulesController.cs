using Microsoft.AspNetCore.Mvc;
using School.Models.Module;
using School.Services.AccessControl.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers.AccessControl
{
    public class CategoryModulesController : BaseController
    {
        private readonly ICategoryModuleService _categoryModuleService;

        public CategoryModulesController(
            ICategoryModuleService categoryModuleService,
            ICurrentUserService currentUser) : base(currentUser)
        {
            _categoryModuleService = categoryModuleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryModule([FromBody] CategoryModuleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.CreatedBy = UserName;
            var res = await _categoryModuleService.AddCategoryModuleAsync(model);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryModuleById(int id)
        {
            var res = await _categoryModuleService.GetCategoryModuleByIdAsync(id);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoryModules()
        {
            var res = await _categoryModuleService.GetAllCategoryModulesAsync();
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategoryModule([FromBody] CategoryModuleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.UpdatedBy = UserName;
            var res = await _categoryModuleService.UpdateCategoryModuleAsync(model);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategoryModule(int id)
        {
            var res = await _categoryModuleService.DeleteCategoryModuleAsync(id);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleCategoryModuleStatus(int id)
        {
            var res = await _categoryModuleService.ToggleCategoryModuleStatusAsync(id);
            return Ok(res);
        }
    }
}
