namespace School_API.Common.Interface
{
    public interface ICurrentUserService
    {
        string SessionId { get; }
        string UserId { get; }
        string UserName { get; }
        string Name { get; }
        string RoleId { get; }
        string RoleName { get; }
        bool IsAuthenticated { get; }
        string Latitude { get; }
        string Longitude { get; }
        int? TenantId { get; }
    }
}

