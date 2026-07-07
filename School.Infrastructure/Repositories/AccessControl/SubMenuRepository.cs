using Microsoft.EntityFrameworkCore;
using School.Domain.AccessControl;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories.AccessControl
{
    public class SubMenuRepository : Repository<SubMenu>, ISubMenuRepository
    {
        private readonly SchoolDbContext _context;

        public SubMenuRepository(DbFactory dbFactory, SchoolDbContext context) : base(dbFactory)
        {
            _context = context;
        }

        public async Task<SubMenu> AddSubMenuAsync(SubMenu entity)
        {
            var existingSubMenu = await DbSet.FirstOrDefaultAsync(x =>
                                 x.SubMenuName.ToLower() == entity.SubMenuName.ToLower() &&
                                 x.MenuId == entity.MenuId &&
                                 !x.IsDeleted);

            if (existingSubMenu != null)
            {
                existingSubMenu.Id = 0;
                return existingSubMenu;
            }

            entity.IsDeleted = false;
            await AddAsync(entity);
            await _context.SaveChangesAsync();
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
            return await DbSet
                .Where(x => x.MenuId == menuId && !x.IsDeleted)
                .OrderBy(x => x.Priority)
                .ThenBy(x => x.SubMenuName)
                .ToListAsync();
        }

        public async Task<IEnumerable<SubMenu>> GetAllSubMenusAsync()
        {
            return await DbSet
                .Include(x => x.MainMenu)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Priority)
                .ToListAsync();
        }

        public async Task<int> UpdateSubMenuAsync(SubMenu entity)
        {
            var existingSubMenu = await DbSet
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
            existingSubMenu.UpdatedDate = DateTime.UtcNow;

            Update(existingSubMenu);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteSubMenuAsync(int subMenuId)
        {
            var result = await DbSet
                .FirstOrDefaultAsync(x => x.Id == subMenuId && !x.IsDeleted);

            if (result != null)
            {
                result.UpdatedDate = DateTime.UtcNow;
                result.IsDeleted = true;
                Delete(result);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> ToggleSubMenuStatusAsync(int subMenuId)
        {
            var entity = await DbSet
                .FirstOrDefaultAsync(m => m.Id == subMenuId && !m.IsDeleted);

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
