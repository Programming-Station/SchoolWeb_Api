using Microsoft.AspNetCore.Mvc;
using School.Models.Menu;
using School.Services.AccessControl.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers.AccessControl
{
    public class SubMenusController : BaseController
    {
        private readonly ISubMenuService _subMenuService;

        public SubMenusController(
            ISubMenuService subMenuService,
            ICurrentUserService currentUser) : base(currentUser)
        {
            _subMenuService = subMenuService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubMenu([FromBody] SubMenuModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.CreatedBy = UserName;
            var res = await _subMenuService.AddSubMenuAsync(model);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetSubMenuById(int id)
        {
            var res = await _subMenuService.GetSubMenuByIdAsync(id);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetSubMenusByMenuId(int menuId)
        {
            var res = await _subMenuService.GetSubMenusByMenuIdAsync(menuId);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubMenus()
        {
            var res = await _subMenuService.GetAllSubMenusAsync();
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSubMenu([FromBody] SubMenuModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.UpdatedBy = UserName;
            var res = await _subMenuService.UpdateSubMenuAsync(model);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSubMenu(int subMenuId)
        {
            var res = await _subMenuService.DeleteSubMenuAsync(subMenuId);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleSubMenuStatus(int subMenuId)
        {
            var res = await _subMenuService.ToggleSubMenuStatusAsync(subMenuId);
            return Ok(res);
        }
    }
}
