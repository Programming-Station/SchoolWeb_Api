using System.Net;
using Microsoft.EntityFrameworkCore;
using School.Infrastructure;
using School_DTOs;
using School_DTOs.School;

namespace School.Services.Administration
{
    public interface ITenantAdminService
    {
        Task<PagedResponse<SchoolRegistrationDto>> GetAllTenantsAsync(int pageNumber = 1, int pageSize = 20, string? search = null, string? status = null);
        Task<APIResponse<SchoolRegistrationDto>> GetTenantByIdAsync(int id);
        Task<APIResponse> ApproveTenantAsync(int id);
        Task<APIResponse> RejectTenantAsync(int id, string? reason = null);
        Task<APIResponse> SuspendTenantAsync(int id, string? reason = null);
        Task<APIResponse> ActivateTenantAsync(int id);
        Task<APIResponse> UpdateTenantQuotaAsync(int id, int? maxStudents);
        Task<APIResponse<TenantStatsDto>> GetTenantStatsAsync();
    }

    public class TenantStatsDto
    {
        public int TotalTenants { get; set; }
        public int ActiveTenants { get; set; }
        public int PendingTenants { get; set; }
        public int SuspendedTenants { get; set; }
        public int RejectedTenants { get; set; }
    }

    public class TenantAdminService : ITenantAdminService
    {
        private readonly SchoolDbContext _db;

        public TenantAdminService(SchoolDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResponse<SchoolRegistrationDto>> GetAllTenantsAsync(int pageNumber = 1, int pageSize = 20, string? search = null, string? status = null)
        {
            var query = _db.SchoolRegistrations
                .Include(s => s.Country)
                .Include(s => s.State)
                .Include(s => s.City)
                .Include(s => s.SchoolType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(s => s.SchoolName.ToLower().Contains(search)
                    || s.SchoolCode.ToLower().Contains(search)
                    || s.Email.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(s => s.ApprovalStatus == status);
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(s => s.RegistrationDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new SchoolRegistrationDto
                {
                    Id = s.Id,
                    SchoolName = s.SchoolName,
                    SchoolCode = s.SchoolCode,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address ?? "",
                    Pincode = s.Pincode ?? "",
                    Logo = s.Logo ?? "",
                    ContactPersonName = s.ContactPersonName ?? "",
                    ApprovalStatus = s.ApprovalStatus,
                    IsActive = s.IsActive,
                    CountryName = s.Country != null ? s.Country.Name : "",
                    StateName = s.State != null ? s.State.Name : "",
                    CityName = s.City != null ? s.City.Name : "",
                    MaxStudentsAllowed = s.MaxStudentsAllowed,
                    SubDomain = s.SubDomain,
                    RegistrationDate = s.RegistrationDate
                })
                .ToListAsync();

            return new PagedResponse<SchoolRegistrationDto>
            {
                Data = items,
                TotalRecords = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Success = true,
                Message = "Tenants retrieved successfully.",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<SchoolRegistrationDto>> GetTenantByIdAsync(int id)
        {
            var entity = await _db.SchoolRegistrations
                .Include(s => s.Country)
                .Include(s => s.State)
                .Include(s => s.City)
                .Include(s => s.SchoolType)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (entity == null)
            {
                return new APIResponse<SchoolRegistrationDto>
                {
                    Success = false,
                    Message = "Tenant not found.",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var dto = new SchoolRegistrationDto
            {
                Id = entity.Id,
                SchoolName = entity.SchoolName,
                SchoolCode = entity.SchoolCode,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                Address = entity.Address ?? "",
                Pincode = entity.Pincode ?? "",
                Logo = entity.Logo ?? "",
                ContactPersonName = entity.ContactPersonName ?? "",
                ApprovalStatus = entity.ApprovalStatus,
                IsActive = entity.IsActive,
                CountryName = entity.Country?.Name ?? "",
                StateName = entity.State?.Name ?? "",
                CityName = entity.City?.Name ?? "",
                MaxStudentsAllowed = entity.MaxStudentsAllowed,
                SubDomain = entity.SubDomain,
                RegistrationDate = entity.RegistrationDate
            };

            return new APIResponse<SchoolRegistrationDto>
            {
                Data = dto,
                Success = true,
                Message = "Tenant details retrieved.",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse> ApproveTenantAsync(int id)
        {
            var entity = await _db.SchoolRegistrations.FindAsync(id);
            if (entity == null) return new APIResponse { Success = false, Message = "Tenant not found.", StatusCode = HttpStatusCode.NotFound };

            entity.ApprovalStatus = "Approved";
            entity.IsActive = true;
            await _db.SaveChangesAsync();

            return new APIResponse { Success = true, Message = $"Tenant '{entity.SchoolName}' has been approved.", StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse> RejectTenantAsync(int id, string? reason = null)
        {
            var entity = await _db.SchoolRegistrations.FindAsync(id);
            if (entity == null) return new APIResponse { Success = false, Message = "Tenant not found.", StatusCode = HttpStatusCode.NotFound };

            entity.ApprovalStatus = "Rejected";
            entity.IsActive = false;
            await _db.SaveChangesAsync();

            return new APIResponse { Success = true, Message = $"Tenant '{entity.SchoolName}' has been rejected.{(reason != null ? $" Reason: {reason}" : "")}", StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse> SuspendTenantAsync(int id, string? reason = null)
        {
            var entity = await _db.SchoolRegistrations.FindAsync(id);
            if (entity == null) return new APIResponse { Success = false, Message = "Tenant not found.", StatusCode = HttpStatusCode.NotFound };

            entity.ApprovalStatus = "Suspended";
            entity.IsActive = false;
            await _db.SaveChangesAsync();

            return new APIResponse { Success = true, Message = $"Tenant '{entity.SchoolName}' has been suspended.{(reason != null ? $" Reason: {reason}" : "")}", StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse> ActivateTenantAsync(int id)
        {
            var entity = await _db.SchoolRegistrations.FindAsync(id);
            if (entity == null) return new APIResponse { Success = false, Message = "Tenant not found.", StatusCode = HttpStatusCode.NotFound };

            entity.ApprovalStatus = "Approved";
            entity.IsActive = true;
            await _db.SaveChangesAsync();

            return new APIResponse { Success = true, Message = $"Tenant '{entity.SchoolName}' has been activated.", StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse> UpdateTenantQuotaAsync(int id, int? maxStudents)
        {
            var entity = await _db.SchoolRegistrations.FindAsync(id);
            if (entity == null) return new APIResponse { Success = false, Message = "Tenant not found.", StatusCode = HttpStatusCode.NotFound };

            entity.MaxStudentsAllowed = maxStudents;
            await _db.SaveChangesAsync();

            return new APIResponse { Success = true, Message = $"Quota updated for '{entity.SchoolName}'. Max students: {maxStudents?.ToString() ?? "Unlimited"}.", StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<TenantStatsDto>> GetTenantStatsAsync()
        {
            var all = await _db.SchoolRegistrations.ToListAsync();

            var stats = new TenantStatsDto
            {
                TotalTenants = all.Count,
                ActiveTenants = all.Count(s => s.IsActive && s.ApprovalStatus == "Approved"),
                PendingTenants = all.Count(s => s.ApprovalStatus == "Pending"),
                SuspendedTenants = all.Count(s => s.ApprovalStatus == "Suspended"),
                RejectedTenants = all.Count(s => s.ApprovalStatus == "Rejected")
            };

            return new APIResponse<TenantStatsDto>
            {
                Data = stats,
                Success = true,
                Message = "Stats retrieved.",
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
