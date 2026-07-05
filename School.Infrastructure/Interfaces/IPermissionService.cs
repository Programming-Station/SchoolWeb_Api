using System.Threading.Tasks;

namespace School.Infrastructure.Interfaces
{
    public interface IPermissionService
    {
        Task<bool> HasModulePermissionAsync(string moduleName);
    }
}
