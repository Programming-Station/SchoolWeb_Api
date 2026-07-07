using School.Domain.AccessControl;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.AccessControl
{
    public interface ISubMenuRepository : IRepository<SubMenu>
    {
        Task<SubMenu> AddSubMenuAsync(SubMenu entity);
        Task<SubMenu> GetSubMenuByIdAsync(int subMenuId);
        Task<IEnumerable<SubMenu>> GetSubMenusByMenuIdAsync(int menuId);
        Task<IEnumerable<SubMenu>> GetAllSubMenusAsync();
        Task<int> UpdateSubMenuAsync(SubMenu entity);
        Task<int> DeleteSubMenuAsync(int subMenuId);
        Task<int> ToggleSubMenuStatusAsync(int subMenuId);
    }
}
