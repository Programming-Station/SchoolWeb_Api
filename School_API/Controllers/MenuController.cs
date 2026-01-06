using School.Models.Menu;
using School.Models.Module;
using School.Services.Interfaces;
using School_API.Common.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace School_API.Controllers
{
    public class MenuController : BaseController
    {
        private readonly IMenuService _menuService;
        private readonly IModuleService _moduleService;
        private readonly ICategoryModuleService _categoryModuleService;
        
        public MenuController(
            IMenuService menuService, 
            IModuleService moduleService,
            ICategoryModuleService categoryModuleService,
            ICurrentUserService currentUser) : base(currentUser)
        {
            _menuService = menuService;
            _moduleService = moduleService;
            _categoryModuleService = categoryModuleService;
        }
        // ========== MENU ENDPOINTS ==========

        /// <summary>
        /// Create a new menu
        /// </summary>
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

        /// <summary>
        /// Get menu by ID
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMenuById(int id)
        {
            var res = await _menuService.GetMenuByIdAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Get all menus
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllMenus()
        {
            var res = await _menuService.GetAllMenusAsync();
            return Ok(res);
        }

        /// <summary>
        /// Update menu
        /// </summary>
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

        /// <summary>
        /// Delete menu (soft delete)
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var res = await _menuService.DeleteMenuAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Toggle menu visibility status
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ToggleMenuStatus(int id)
        {
            var res = await _menuService.ToggleMenuStatusAsync(id);
            return Ok(res);
        }

        // ========== SUBMENU ENDPOINTS ==========

        /// <summary>
        /// Create a new submenu
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateSubMenu([FromBody] SubMenuModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.CreatedBy = UserName;
            var res = await _menuService.AddSubMenuAsync(model);
            return Ok(res);
        }

        /// <summary>
        /// Get submenu by ID
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSubMenuById(int id)
        {
            var res = await _menuService.GetSubMenuByIdAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Get all submenus by menu ID
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSubMenusByMenuId(int menuId)
        {
            var res = await _menuService.GetSubMenusByMenuIdAsync(menuId);
            return Ok(res);
        }

        /// <summary>
        /// Update submenu
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateSubMenu([FromBody] SubMenuModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.UpdatedBy = UserName;
            var res = await _menuService.UpdateSubMenuAsync(model);
            return Ok(res);
        }

        /// <summary>
        /// Delete submenu (soft delete)
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteSubMenu(int subMenuId)
        {
            var res = await _menuService.DeleteSubMenuAsync(subMenuId);
            return Ok(res);
        }

        // ========== MENU PERMISSION ENDPOINTS ==========
        [HttpPost]
        public async Task<IActionResult> GiveMenuPermission([FromBody] MenuPermissionModel model)
        {

            model.CreateedBy = UserName;
            model.UpdatedBy = UserName;
            var res = await _menuService.GiveMenuPermissionAsync(model);

            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetMenuPermissionByRoleId(string? roleId)
        {
            var res = await _menuService.GetMenuPermissionAsync(roleId);

            return Ok(res);
        }

        // ========== Module Endpoints ==========

        /// <summary>
        /// Create a new module (Super Admin only)
        /// </summary>
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

        /// <summary>
        /// Get module by ID
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetModuleById(int id)
        {
            var res = await _moduleService.GetModuleByIdAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Get all modules
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllModules()
        {
            var res = await _moduleService.GetAllModulesAsync();
            return Ok(res);
        }

        /// <summary>
        /// Update module
        /// </summary>
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

        /// <summary>
        /// Delete module (soft delete)
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var res = await _moduleService.DeleteModuleAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Toggle module active status
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ToggleModuleStatus(int id)
        {
            var res = await _moduleService.ToggleModuleStatusAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Get modules assigned to current user
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMyModules()
        {
            var res = await _moduleService.GetModulesByUserIdAsync(UserId);
            return Ok(res);
        }

        /// <summary>
        /// Get modules assigned to specific user (Super Admin only)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetModulesByUserId(string userId)
        {
            var res = await _moduleService.GetModulesByUserIdAsync(userId);
            return Ok(res);
        }

        /// <summary>
        /// Assign modules to user (Super Admin only)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AssignModulesToUser([FromBody] AssignModulesToUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.CreatedBy = UserName;
            var res = await _moduleService.AssignModulesToUserAsync(model);
            return Ok(res);
        }

        /// <summary>
        /// Remove module permission from user
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> RemoveModulePermission(int moduleId, string userId)
        {
            var res = await _moduleService.RemoveModulePermissionAsync(moduleId, userId);
            return Ok(res);
        }

        /// <summary>
        /// Get module permissions for user
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetModulePermissionsByUserId(string userId)
        {
            var res = await _moduleService.GetModulePermissionsByUserIdAsync(userId);
            return Ok(res);
        }

        // ========== Category Module Endpoints ==========

        /// <summary>
        /// Create a new category module (Super Admin only)
        /// </summary>
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

        /// <summary>
        /// Get category module by ID
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCategoryModuleById(int id)
        {
            var res = await _categoryModuleService.GetCategoryModuleByIdAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Get all category modules
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllCategoryModules()
        {
            var res = await _categoryModuleService.GetAllCategoryModulesAsync();
            return Ok(res);
        }

        /// <summary>
        /// Update category module
        /// </summary>
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

        /// <summary>
        /// Delete category module (soft delete)
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteCategoryModule(int id)
        {
            var res = await _categoryModuleService.DeleteCategoryModuleAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Toggle category module active status
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ToggleCategoryModuleStatus(int id)
        {
            var res = await _categoryModuleService.ToggleCategoryModuleStatusAsync(id);
            return Ok(res);
        }
    }
}
