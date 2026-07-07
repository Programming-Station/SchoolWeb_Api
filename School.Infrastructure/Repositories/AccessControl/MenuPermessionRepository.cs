using Microsoft.EntityFrameworkCore;
using School.Domain.AccessControl;
using School.Infrastructure.UnitOfWork;
using School.Models.Menu;
using School_DTOs.Menu;

namespace School.Infrastructure.Repositories.AccessControl
{
    public class MenuPermessionRepository : Repository<MenuPermession>, IMenuPermessionRepository
    {
        private readonly SchoolDbContext _context;

        public MenuPermessionRepository(DbFactory dbFactory, SchoolDbContext context) : base(dbFactory)
        {
            _context = context;
        }

        public async Task<MenuPermession> AddMenuPermissionAsync(MenuPermession entity)
        {
            var alreadyExists = await DbSet.AnyAsync(x => x.RoleId == entity.RoleId 
                                && x.MenuId == entity.MenuId 
                                && x.SubMenuId == entity.SubMenuId 
                                && !x.IsDeleted);
            if (alreadyExists) return new MenuPermession { Id = 0 };
            
            entity.IsDeleted = false;
            await AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<MenuPermession> GetMenuPermissionByIdAsync(int id)
        {
            return await _context.MenuPermessions
                .Include(x => x.MainMenu)
                .Include(x => x.SubMenu)
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new MenuPermession();
        }

        public async Task<IEnumerable<MenuPermession>> GetAllMenuPermissionsAsync()
        {
            return await _context.MenuPermessions
                .Include(x => x.MainMenu)
                .Include(x => x.SubMenu)
                .Include(x => x.Role)
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> UpdateMenuPermissionAsync(MenuPermession entity)
        {
            var existing = await DbSet.FirstOrDefaultAsync(x => x.Id == entity.Id && !x.IsDeleted);
            if (existing == null) return 0;

            existing.IsVisible = entity.IsVisible;
            existing.MenuId = entity.MenuId;
            existing.SubMenuId = entity.SubMenuId;
            existing.RoleId = entity.RoleId;
            existing.UpdatedBy = entity.UpdatedBy;
            existing.UpdatedDate = DateTime.UtcNow;

            Update(existing);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteMenuPermissionAsync(int id)
        {
            var entity = await FindAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null) return 0;
            Delete(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> ToggleMenuPermissionStatusAsync(int id)
        {
            var entity = await DbSet.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
            if (entity != null)
            {
                entity.IsVisible = !entity.IsVisible;
                entity.UpdatedDate = DateTime.UtcNow;
                Update(entity);
                return await _context.SaveChangesAsync();
            }
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
                    perm.UpdatedDate = DateTime.UtcNow;
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
                        IsDeleted = false,
                        IsVisible = true,
                        MenuId = item.Id,
                        SubMenuId = null,
                        CreatedBy = model.CreateedBy,
                        CreatedDate = DateTime.UtcNow
                    });
                }

                if (item.SubMenus != null && item.SubMenus.Any())
                {
                    menuPermessions.AddRange(item.SubMenus
                         .Where(x => x.IsSelected)
                         .Select(x => new MenuPermession
                         {
                             RoleId = model.RoleId,
                             IsDeleted = false,
                             IsVisible = true,
                             MenuId = item.Id,
                             SubMenuId = x.Id,
                             CreatedBy = model.CreateedBy,
                             CreatedDate = DateTime.UtcNow
                         }));
                }
            }

            if (menuPermessions.Any())
            {
                await _context.MenuPermessions.AddRangeAsync(menuPermessions);
            }

            return await _context.SaveChangesAsync();
        }
    }
}
