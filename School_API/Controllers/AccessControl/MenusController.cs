using Microsoft.AspNetCore.Mvc;
using School.Models.Menu;
using School.Services.AccessControl.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers.AccessControl
{
    public class MenusController : BaseController
    {
        private readonly IMenuService _menuService;

        public MenusController(
            IMenuService menuService,
            ICurrentUserService currentUser) : base(currentUser)
        {
            _menuService = menuService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenu([FromBody] MenuModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.CreatedBy = UserName;
            var res = await _menuService.AddMenuAsync(model);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetMenuById(int id)
        {
            var res = await _menuService.GetMenuByIdAsync(id);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMenus()
        {
            var res = await _menuService.GetAllMenusAsync();
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> EditMenu([FromBody] MenuModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.UpdatedBy = UserName;
            var res = await _menuService.UpdateMenuAsync(model);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var res = await _menuService.DeleteMenuAsync(id);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleMenuStatus(int id)
        {
            var res = await _menuService.ToggleMenuStatusAsync(id);
            return Ok(res);
        }
    }
}
