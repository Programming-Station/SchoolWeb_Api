using Microsoft.AspNetCore.Mvc;
using School.Models.Menu;
using School.Services.AccessControl.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers.AccessControl
{
    public class MenuPermissionsController : BaseController
    {
        private readonly IMenuPermessionService _menuPermessionService;

        public MenuPermissionsController(
            IMenuPermessionService menuPermessionService,
            ICurrentUserService currentUser) : base(currentUser)
        {
            _menuPermessionService = menuPermessionService;
        }

        [HttpPost]
        public async Task<IActionResult> GiveMenuPermission([FromBody] MenuPermissionModel model)
        {
            model.CreateedBy = UserName;
            model.UpdatedBy = UserName;
            var res = await _menuPermessionService.GiveMenuPermissionAsync(model);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetMenuPermissionByRoleId(string? roleId)
        {
            var res = await _menuPermessionService.GetMenuPermissionAsync(roleId);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddMenuPermission([FromBody] MenuPermissionModel model)
        {
            model.CreateedBy = UserName;
            model.UpdatedBy = UserName;
            var res = await _menuPermessionService.AddMenuPermissionAsync(model);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMenuPermission(int id)
        {
            var res = await _menuPermessionService.DeleteMenuPermissionAsync(id);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleMenuPermissionStatus(int id)
        {
            var res = await _menuPermessionService.ToggleMenuPermissionStatusAsync(id);
            return Ok(res);
        }
    }
}
