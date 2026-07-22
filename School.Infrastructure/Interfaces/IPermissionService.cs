namespace School.Infrastructure.Interfaces
{
    public interface IPermissionService
    {
        Task<bool> HasModulePermissionAsync(string moduleName);
    }
}
