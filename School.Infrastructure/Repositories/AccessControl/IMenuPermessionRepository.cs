using School.Domain.AccessControl;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Models.Menu;
using School_DTOs.Menu;

namespace School.Infrastructure.Repositories.AccessControl
{
    public interface IMenuPermessionRepository : IRepository<MenuPermession>
    {
        Task<MenuPermession> AddMenuPermissionAsync(MenuPermession entity);
        Task<MenuPermession> GetMenuPermissionByIdAsync(int id);
        Task<IEnumerable<MenuPermession>> GetAllMenuPermissionsAsync();
        Task<int> UpdateMenuPermissionAsync(MenuPermession entity);
        Task<int> DeleteMenuPermissionAsync(int id);
        Task<int> ToggleMenuPermissionStatusAsync(int id);
        
        Task<MenuPermissionsDto> GetMenuPermissionAsync(string? roleId);
        Task<int> GiveMenuPermissionAsync(MenuPermissionModel model);
    }
}
