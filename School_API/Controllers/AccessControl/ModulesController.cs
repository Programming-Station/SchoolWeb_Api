using Microsoft.AspNetCore.Mvc;
using School.Models.Module;
using School.Services.AccessControl.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers.AccessControl
{
    public class ModulesController : BaseController
    {
        private readonly IModuleService _moduleService;

        public ModulesController(
            IModuleService moduleService,
            ICurrentUserService currentUser) : base(currentUser)
        {
            _moduleService = moduleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateModule([FromBody] ModuleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.CreatedBy = UserName;
            var res = await _moduleService.AddModuleAsync(model);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetModuleById(int id)
        {
            var res = await _moduleService.GetModuleByIdAsync(id);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllModules()
        {
            var res = await _moduleService.GetAllModulesAsync();
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateModule([FromBody] ModuleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.UpdatedBy = UserName;
            var res = await _moduleService.UpdateModuleAsync(model);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var res = await _moduleService.DeleteModuleAsync(id);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleModuleStatus(int id)
        {
            var res = await _moduleService.ToggleModuleStatusAsync(id);
            return Ok(res);
        }
    }
}
