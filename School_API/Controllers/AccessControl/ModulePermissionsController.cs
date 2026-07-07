using Microsoft.AspNetCore.Mvc;
using School.Models.Module;
using School.Services.AccessControl.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers.AccessControl
{
    public class ModulePermissionsController : BaseController
    {
        private readonly IModulePermissionService _modulePermissionService;

        public ModulePermissionsController(
            IModulePermissionService modulePermissionService,
            ICurrentUserService currentUser) : base(currentUser)
        {
            _modulePermissionService = modulePermissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyModules()
        {
            var res = await _modulePermissionService.GetModulesByUserIdAsync(UserId);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetModulesByUserId(string userId)
        {
            var res = await _modulePermissionService.GetModulesByUserIdAsync(userId);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AssignModulesToUser([FromBody] AssignModulesToUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.CreatedBy = UserName;
            var res = await _modulePermissionService.AssignModulesToUserAsync(model);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveModulePermission(int moduleId, string userId)
        {
            var res = await _modulePermissionService.RemoveModulePermissionAsync(moduleId, userId);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetModulePermissionsByUserId(string userId)
        {
            var res = await _modulePermissionService.GetModulePermissionsByUserIdAsync(userId);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddModulePermission([FromBody] ModulePermissionModel model)
        {
            model.CreatedBy = UserName;
            var res = await _modulePermissionService.AddModulePermissionAsync(model);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteModulePermission(int id)
        {
            var res = await _modulePermissionService.DeleteModulePermissionAsync(id);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleModulePermissionStatus(int id)
        {
            var res = await _modulePermissionService.ToggleModulePermissionStatusAsync(id);
            return Ok(res);
        }
    }
}
