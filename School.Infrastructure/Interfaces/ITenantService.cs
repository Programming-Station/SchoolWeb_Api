namespace School.Infrastructure.Interfaces
{
    public interface ITenantService
    {
        int? GetTenantId();
        void SetTenantId(int tenantId);
    }
}

