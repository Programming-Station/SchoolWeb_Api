using Microsoft.EntityFrameworkCore;
using School.Domain.AccessControl;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories.AccessControl
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        private readonly SchoolDbContext _context;

        public MenuRepository(DbFactory dbFactory, SchoolDbContext context) : base(dbFactory)
        {
            _context = context;
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

            entity.IsDeleted = false;
            await AddAsync(entity);
            await _context.SaveChangesAsync();
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
            existingMenu.UpdatedDate = DateTime.UtcNow;

            Update(existingMenu);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteMenuAsync(int menuId)
        {
            var result = await FindAsync(x => x.Id == menuId && !x.IsDeleted);

            if (result != null)
            {
                result.UpdatedDate = DateTime.UtcNow;
                result.IsDeleted = true;

                // Cascade soft delete to submenus
                var submenus = await _context.SubMenus.Where(sm => sm.MenuId == menuId && !sm.IsDeleted).ToListAsync();
                foreach (var subMenu in submenus)
                {
                    subMenu.IsDeleted = true;
                    subMenu.UpdatedDate = DateTime.UtcNow;
                    subMenu.UpdatedBy = result.UpdatedBy;
                }

                Delete(result);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> ToggleMenuStatusAsync(int menuId)
        {
            var entity = await _context.Menus
                .FirstOrDefaultAsync(m => m.Id == menuId && !m.IsDeleted);

            if (entity != null)
            {
                entity.IsVisible = !entity.IsVisible;
                entity.UpdatedDate = DateTime.UtcNow;
                Update(entity);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}
