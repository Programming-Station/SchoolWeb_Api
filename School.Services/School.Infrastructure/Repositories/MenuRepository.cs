using School.Domain;
using School_DTOs.Menu;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Models.Menu;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace School.Infrastructure.Repositories
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public MenuRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }


        public async Task<Menu> AddMenuAsync(Menu entity)
        {
            var existingMenu = await DbSet.FirstOrDefaultAsync(x =>
                               x.MenuName.ToLower() == entity.MenuName.ToLower() &&
                               !x.IsDeleted);

            if (existingMenu != null)
            {
                existingMenu.Id = 0;
                return existingMenu;
            }

            var subMenus = entity.SubMenus?.ToList();
            entity.SubMenus = null;

            await AddAsync(entity);
            await _unitOfWork.CommitAsync();

            if (subMenus != null && subMenus.Any())
            {
                foreach (var subMenu in subMenus)
                {
                    subMenu.MenuId = entity.Id;
                    subMenu.IsDeleted = false;
                    if (subMenu.CreatedDate == default)
                        subMenu.CreatedDate = DateTime.Now;
                    _context.SubMenus.Add(subMenu);
                }
                await _unitOfWork.CommitAsync();
                
                entity = await GetMenuByIdAsync(entity.Id);
            }

            return entity;
        }

        public async Task<Menu> GetMenuByIdAsync(int menuId)
        {
            return await _context.Menus
                .Include(x => x.SubMenus.Where(sm => !sm.IsDeleted))
                .FirstOrDefaultAsync(x => x.Id == menuId && !x.IsDeleted) ?? new Menu();
        }

        public async Task<IEnumerable<Menu>> GetAllMenusAsync()
        {
            return await _context.Menus
                .Where(x => !x.IsDeleted)
                .Include(x => x.SubMenus.Where(sm => !sm.IsDeleted))
                .OrderBy(x => x.Priority)
                .ThenBy(x => x.MenuName)
                .ToListAsync();
        }

        public async Task<int> UpdateMenuAsync(Menu entity)
        {
            var existingMenu = await _context.Menus
                .Include(x => x.SubMenus)
                .FirstOrDefaultAsync(x => x.Id == entity.Id && !x.IsDeleted);

            if (existingMenu == null)
                return 0;

            existingMenu.MenuName = entity.MenuName;
            existingMenu.URL = entity.URL;
            existingMenu.Priority = entity.Priority;
            existingMenu.MenuIcon = entity.MenuIcon;
            existingMenu.Controller = entity.Controller;
            existingMenu.Action = entity.Action;
            existingMenu.IsVisible = entity.IsVisible;
            existingMenu.UpdatedBy = entity.UpdatedBy;
            existingMenu.UpdatedDate = DateTime.Now;

            if (entity.SubMenus != null && entity.SubMenus.Any())
            {
                var subMenuIdsToKeep = entity.SubMenus
                    .Where(sm => sm.Id > 0)
                    .Select(sm => sm.Id)
                    .ToList();

                var subMenusToDelete = existingMenu.SubMenus
                    .Where(sm => !subMenuIdsToKeep.Contains(sm.Id))
                    .ToList();

                foreach (var subMenu in subMenusToDelete)
                {
                    subMenu.IsDeleted = true;
                    subMenu.UpdatedDate = DateTime.Now;
                    subMenu.UpdatedBy = entity.UpdatedBy;
                }

                foreach (var subMenuDto in entity.SubMenus)
                {
                    if (subMenuDto.Id > 0)
                    {
                        var existingSubMenu = existingMenu.SubMenus
                            .FirstOrDefault(sm => sm.Id == subMenuDto.Id);

                        if (existingSubMenu != null)
                        {
                            existingSubMenu.SubMenuName = subMenuDto.SubMenuName;
                            existingSubMenu.URL = subMenuDto.URL;
                            existingSubMenu.Priority = subMenuDto.Priority;
                            existingSubMenu.Icon = subMenuDto.Icon;
                            existingSubMenu.Controller = subMenuDto.Controller;
                            existingSubMenu.Action = subMenuDto.Action;
                            existingSubMenu.IsVisible = subMenuDto.IsVisible;
                            existingSubMenu.AccesibleFor = subMenuDto.AccesibleFor;
                            existingSubMenu.UpdatedBy = entity.UpdatedBy;
                            existingSubMenu.UpdatedDate = DateTime.Now;
                            existingSubMenu.IsDeleted = false;
                        }
                    }
                    else
                    {
                        var newSubMenu = new SubMenu
                        {
                            MenuId = entity.Id,
                            SubMenuName = subMenuDto.SubMenuName,
                            URL = subMenuDto.URL,
                            Priority = subMenuDto.Priority,
                            Icon = subMenuDto.Icon,
                            Controller = subMenuDto.Controller,
                            Action = subMenuDto.Action,
                            IsVisible = subMenuDto.IsVisible,
                            AccesibleFor = subMenuDto.AccesibleFor,
                            CreatedBy = entity.CreatedBy ?? entity.UpdatedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false
                        };
                        existingMenu.SubMenus.Add(newSubMenu);
                    }
                }
            }
            else
            {
                foreach (var subMenu in existingMenu.SubMenus.Where(sm => !sm.IsDeleted))
                {
                    subMenu.IsDeleted = true;
                    subMenu.UpdatedDate = DateTime.Now;
                    subMenu.UpdatedBy = entity.UpdatedBy;
                }
            }

            Update(existingMenu);
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<int> DeleteMenuAsync(int menuId)
        {
            var result = await FindAsync(expression: x => x.Id == menuId && !x.IsDeleted);

            if (result != null)
            {
                result.UpdatedDate = DateTime.Now;
                result.IsDeleted = true;

                if (result.SubMenus != null)
                {
                    foreach (var subMenu in result.SubMenus.Where(sm => !sm.IsDeleted))
                    {
                        subMenu.IsDeleted = true;
                        subMenu.UpdatedDate = DateTime.Now;
                    }
                }

                Delete(result);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }

        public async Task<int> ToggleMenuStatusAsync(int menuId)
        {
            var entity = await _context.Menus
                .FirstOrDefaultAsync(m => m.Id == menuId && !m.IsDeleted);

            if (entity != null)
            {
                entity.IsVisible = !entity.IsVisible;
                entity.UpdatedDate = DateTime.Now;
                Update(entity);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }


        public async Task<SubMenu> AddSubMenuAsync(SubMenu entity)
        {
            var existingSubMenu = await _context.SubMenus.FirstOrDefaultAsync(x =>
                                 x.SubMenuName.ToLower() == entity.SubMenuName.ToLower() &&
                                 x.MenuId == entity.MenuId &&
                                 !x.IsDeleted);

            if (existingSubMenu != null)
            {
                existingSubMenu.Id = 0;
                return existingSubMenu;
            }

            entity.IsDeleted = false;
            if (entity.CreatedDate == default)
                entity.CreatedDate = DateTime.Now;

            _context.SubMenus.Add(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<SubMenu> GetSubMenuByIdAsync(int subMenuId)
        {
            return await _context.SubMenus
                .Include(x => x.MainMenu)
                .FirstOrDefaultAsync(x => x.Id == subMenuId && !x.IsDeleted) ?? new SubMenu();
        }

        public async Task<IEnumerable<SubMenu>> GetSubMenusByMenuIdAsync(int menuId)
        {
            return await _context.SubMenus
                .Where(x => x.MenuId == menuId && !x.IsDeleted)
                .OrderBy(x => x.Priority)
                .ThenBy(x => x.SubMenuName)
                .ToListAsync();
        }

        public async Task<int> UpdateSubMenuAsync(SubMenu entity)
        {
            var existingSubMenu = await _context.SubMenus
                .FirstOrDefaultAsync(x => x.Id == entity.Id && !x.IsDeleted);

            if (existingSubMenu == null)
                return 0;

            existingSubMenu.SubMenuName = entity.SubMenuName;
            existingSubMenu.URL = entity.URL;
            existingSubMenu.Priority = entity.Priority;
            existingSubMenu.Icon = entity.Icon;
            existingSubMenu.Controller = entity.Controller;
            existingSubMenu.Action = entity.Action;
            existingSubMenu.IsVisible = entity.IsVisible;
            existingSubMenu.AccesibleFor = entity.AccesibleFor;
            existingSubMenu.UpdatedBy = entity.UpdatedBy;
            existingSubMenu.UpdatedDate = DateTime.Now;

            _context.SubMenus.Update(existingSubMenu);
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<int> DeleteSubMenuAsync(int subMenuId)
        {
            var result = await _context.SubMenus
                .FirstOrDefaultAsync(x => x.Id == subMenuId && !x.IsDeleted);

            if (result != null)
            {
                result.UpdatedDate = DateTime.Now;
                result.IsDeleted = true;
                _context.SubMenus.Update(result);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }


        public async Task<MenuPermissionsDto> GetMenuPermissionAsync(string? roleId)
        {
            IQueryable<MenuPermession> menuPermessions = _context.MenuPermessions
                .Where(x => x.RoleId == roleId && !x.IsDeleted);

            var result = await _context.Menus
                .Where(x => !x.IsDeleted)
                .Include(x => x.SubMenus.Where(sm => !sm.IsDeleted))
                .Select(menu => new MenuDto
                {
                    Id = menu.Id,
                    MenuName = menu.MenuName,
                    Priority = menu.Priority,
                    Action = menu.Action,
                    Controller = menu.Controller,
                    URL = menu.URL,
                    MenuIcon = menu.MenuIcon,
                    IsVisible = menu.IsVisible,
                    IsSelected = menuPermessions.Any(mp => mp.MenuId == menu.Id && mp.IsVisible && !mp.IsDeleted),
                    SubMenus = menu.SubMenus.Select(x => new SubMenuDto
                    {
                        Id = x.Id,
                        SubMenuName = x.SubMenuName,
                        URL = x.URL,
                        Priority = x.Priority,
                        Icon = x.Icon,
                        Controller = x.Controller,
                        Action = x.Action,
                        IsVisible = x.IsVisible,
                        AccesibleFor = x.AccesibleFor,
                        IsSelected = menuPermessions.Any(mp => mp.SubMenuId == x.Id && mp.IsVisible && !mp.IsDeleted),
                    }).ToList()
                })
                .OrderBy(x => x.Priority)
                .ThenBy(x => x.MenuName)
                .ToListAsync();

            return new MenuPermissionsDto
            {
                menuPermissions = result
            };
        }

        public async Task<int> GiveMenuPermissionAsync(MenuPermissionModel model)
        {
            var existingPermissions = await _context.MenuPermessions
                .Where(x => x.RoleId == model.RoleId && !x.IsDeleted)
                .ToListAsync();

            if (existingPermissions.Any())
            {
                foreach (var perm in existingPermissions)
                {
                    perm.IsDeleted = true;
                    perm.UpdatedDate = DateTime.Now;
                }
                await _context.SaveChangesAsync();
            }

            List<MenuPermession> menuPermessions = new List<MenuPermession>();

            foreach (var item in model.menuPermissions)
            {
                if (item.IsSelected)
                {
                    menuPermessions.Add(new MenuPermession
                    {
                        RoleId = model.RoleId,
                        Id = Guid.NewGuid(),
                        IsDeleted = false,
                        IsVisible = true,
                        MenuId = item.Id,
                        SubMenuId = null,
                        CreatedBy = model.CreateedBy,
                        CreatedDate = DateTime.Now
                    });
                }

                if (item.SubMenus != null && item.SubMenus.Any())
                {
                    menuPermessions.AddRange(item.SubMenus
                        .Where(x => x.IsSelected)
                        .Select(x => new MenuPermession
                        {
                            RoleId = model.RoleId,
                            Id = Guid.NewGuid(),
                            IsDeleted = false,
                            IsVisible = true,
                            MenuId = item.Id,
                            SubMenuId = x.Id,
                            CreatedBy = model.CreateedBy,
                            CreatedDate = DateTime.Now
                        }));
                }
            }

            if (menuPermessions.Any())
            {
                await _context.MenuPermessions.AddRangeAsync(menuPermessions);
            }

            return await _unitOfWork.CommitAsync();
        }
    }
}
