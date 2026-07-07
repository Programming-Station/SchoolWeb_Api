using School.Domain.AccessControl;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.AccessControl
{
    public interface IMenuRepository : IRepository<Menu>
    {
        Task<Menu> AddMenuAsync(Menu entity);
        Task<Menu> GetMenuByIdAsync(int menuId);
        Task<IEnumerable<Menu>> GetAllMenusAsync();
        Task<int> UpdateMenuAsync(Menu entity);
        Task<int> DeleteMenuAsync(int menuId);
        Task<int> ToggleMenuStatusAsync(int menuId);
    }
}
