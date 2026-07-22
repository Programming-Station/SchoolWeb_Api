using Microsoft.EntityFrameworkCore;
using School.Domain.Reporting;

namespace School.Infrastructure.Repositories
{
    /// <summary>
    /// Generic reporting repository — handles all CRUD and complex queries
    /// for the reporting engine entities.
    /// </summary>
    public class ReportingRepository
    {
        private readonly SchoolDbContext _db;

        public ReportingRepository(SchoolDbContext db)
        {
            _db = db;
        }

        // ─── Report Categories ────────────────────────────────────────────────

        public async Task<List<ReportCategory>> GetCategoriesAsync(int? tenantId = null)
        {
            return await _db.ReportCategories
                .Where(c => !c.IsDeleted && c.IsActive &&
                            (c.TenantId == null || c.TenantId == tenantId))
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<ReportCategory?> GetCategoryByIdAsync(int id)
            => await _db.ReportCategories.FindAsync(id);

        public async Task<ReportCategory> CreateCategoryAsync(ReportCategory category)
        {
            _db.ReportCategories.Add(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task UpdateCategoryAsync(ReportCategory category)
        {
            _db.ReportCategories.Update(category);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var cat = await _db.ReportCategories.FindAsync(id);
            if (cat != null)
            {
                cat.IsDeleted = true;
                cat.DeletedDate = System.DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
        }

        // ─── Report Templates ─────────────────────────────────────────────────

        public async Task<List<ReportTemplate>> GetTemplatesAsync(
            int? tenantId = null, int? categoryId = null, string? searchTerm = null,
            bool onlyVisible = true)
        {
            var query = _db.ReportTemplates
                .Include(t => t.ReportCategory)
                .Include(t => t.ReportParameters)
                .Where(t => !t.IsDeleted &&
                            (t.TenantId == null || t.TenantId == tenantId) &&
                            (!onlyVisible || t.IsVisible));

            if (categoryId.HasValue)
                query = query.Where(t => t.ReportCategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lower = searchTerm.ToLower();
                query = query.Where(t =>
                    t.ReportName.ToLower().Contains(lower) ||
                    (t.Description != null && t.Description.ToLower().Contains(lower)) ||
                    (t.SearchTags != null && t.SearchTags.ToLower().Contains(lower)) ||
                    (t.ModuleTag != null && t.ModuleTag.ToLower().Contains(lower)));
            }

            return await query
                .OrderBy(t => t.ReportCategoryId)
                .ThenBy(t => t.SortOrder)
                .ThenBy(t => t.ReportName)
                .ToListAsync();
        }

        public async Task<ReportTemplate?> GetTemplateByIdAsync(int id)
            => await _db.ReportTemplates
                .Include(t => t.ReportCategory)
                .Include(t => t.ReportParameters.OrderBy(p => p.SortOrder))
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        public async Task<ReportTemplate?> GetTemplateByCodeAsync(string code, int? tenantId = null)
            => await _db.ReportTemplates
                .Include(t => t.ReportParameters.OrderBy(p => p.SortOrder))
                .FirstOrDefaultAsync(t =>
                    t.ReportCode == code && !t.IsDeleted &&
                    (t.TenantId == null || t.TenantId == tenantId));

        public async Task<ReportTemplate> CreateTemplateAsync(ReportTemplate template)
        {
            _db.ReportTemplates.Add(template);
            await _db.SaveChangesAsync();
            return template;
        }

        public async Task UpdateTemplateAsync(ReportTemplate template)
        {
            _db.ReportTemplates.Update(template);
            await _db.SaveChangesAsync();
        }

        public async Task IncrementExecutionCountAsync(int templateId)
        {
            await _db.ReportTemplates
                .Where(t => t.Id == templateId)
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.ExecutionCount, t => t.ExecutionCount + 1));
        }

        public async Task DeleteTemplateAsync(int id, bool hardDelete = false)
        {
            var tpl = await _db.ReportTemplates.FindAsync(id);
            if (tpl == null) return;

            if (hardDelete && !tpl.IsSystem)
            {
                _db.ReportTemplates.Remove(tpl);
            }
            else
            {
                tpl.IsDeleted = true;
                tpl.IsVisible = false;
                tpl.DeletedDate = System.DateTime.UtcNow;
            }
            await _db.SaveChangesAsync();
        }

        // ─── Report Parameters ────────────────────────────────────────────────

        public async Task<List<ReportParameter>> GetParametersForTemplateAsync(int templateId)
            => await _db.ReportParameters
                .Where(p => p.ReportTemplateId == templateId && !p.IsDeleted)
                .OrderBy(p => p.SortOrder)
                .ToListAsync();

        public async Task SaveParametersAsync(int templateId, IEnumerable<ReportParameter> parameters)
        {
            // Remove existing
            var existing = await _db.ReportParameters
                .Where(p => p.ReportTemplateId == templateId)
                .ToListAsync();
            _db.ReportParameters.RemoveRange(existing);

            // Add new
            foreach (var p in parameters)
            {
                p.ReportTemplateId = templateId;
                _db.ReportParameters.Add(p);
            }
            await _db.SaveChangesAsync();
        }

        // ─── Report History ───────────────────────────────────────────────────

        public async Task<(List<ReportHistory> Items, int Total)> GetHistoryAsync(
            int? tenantId, int? templateId, string? generatedBy,
            System.DateTime? fromDate, System.DateTime? toDate,
            int page = 1, int pageSize = 20)
        {
            var query = _db.ReportHistories
                .Include(h => h.ReportTemplate)
                .Where(h => !h.IsDeleted &&
                            (tenantId == null || h.TenantId == tenantId));

            if (templateId.HasValue)
                query = query.Where(h => h.ReportTemplateId == templateId.Value);

            if (!string.IsNullOrWhiteSpace(generatedBy))
                query = query.Where(h => h.GeneratedBy == generatedBy);

            if (fromDate.HasValue)
                query = query.Where(h => h.GeneratedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(h => h.GeneratedAt <= toDate.Value.AddDays(1));

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(h => h.GeneratedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<ReportHistory> LogHistoryAsync(ReportHistory history)
        {
            _db.ReportHistories.Add(history);
            await _db.SaveChangesAsync();
            return history;
        }

        public async Task UpdateHistoryAsync(ReportHistory history)
        {
            _db.ReportHistories.Update(history);
            await _db.SaveChangesAsync();
        }

        public async Task CleanOldHistoryAsync(int daysToKeep = 90)
        {
            var cutoff = System.DateTime.UtcNow.AddDays(-daysToKeep);
            await _db.ReportHistories
                .Where(h => h.GeneratedAt < cutoff)
                .ExecuteDeleteAsync();
        }

        // ─── Report Schedules ─────────────────────────────────────────────────

        public async Task<List<ReportSchedule>> GetSchedulesAsync(int? tenantId = null)
            => await _db.ReportSchedules
                .Include(s => s.ReportTemplate)
                .Where(s => !s.IsDeleted && s.IsActive &&
                            (tenantId == null || s.TenantId == tenantId))
                .ToListAsync();

        public async Task<List<ReportSchedule>> GetDueSchedulesAsync(System.DateTime asOf)
            => await _db.ReportSchedules
                .Include(s => s.ReportTemplate)
                    .ThenInclude(t => t!.ReportParameters)
                .Where(s => !s.IsDeleted && s.IsActive &&
                            s.NextRunAt.HasValue &&
                            s.NextRunAt.Value <= asOf)
                .ToListAsync();

        public async Task<ReportSchedule?> GetScheduleByIdAsync(int id)
            => await _db.ReportSchedules
                .Include(s => s.ReportTemplate)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

        public async Task<ReportSchedule> CreateScheduleAsync(ReportSchedule schedule)
        {
            _db.ReportSchedules.Add(schedule);
            await _db.SaveChangesAsync();
            return schedule;
        }

        public async Task UpdateScheduleAsync(ReportSchedule schedule)
        {
            _db.ReportSchedules.Update(schedule);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteScheduleAsync(int id)
        {
            var s = await _db.ReportSchedules.FindAsync(id);
            if (s != null)
            {
                s.IsDeleted = true;
                await _db.SaveChangesAsync();
            }
        }

        // ─── Report Permissions ───────────────────────────────────────────────

        public async Task<List<ReportPermission>> GetPermissionsAsync(
            int? tenantId = null, int? templateId = null)
        {
            var query = _db.ReportPermissions
                .Include(p => p.ReportTemplate)
                .Where(p => !p.IsDeleted);

            if (tenantId.HasValue) query = query.Where(p => p.TenantId == tenantId.Value);
            if (templateId.HasValue) query = query.Where(p => p.ReportTemplateId == templateId.Value);

            return await query.ToListAsync();
        }

        public async Task<ReportPermission?> GetPermissionAsync(int templateId, string roleName, int? tenantId = null)
            => await _db.ReportPermissions.FirstOrDefaultAsync(p =>
                p.ReportTemplateId == templateId &&
                p.RoleName == roleName &&
                !p.IsDeleted &&
                (tenantId == null || p.TenantId == tenantId));

        public async Task SavePermissionAsync(ReportPermission permission)
        {
            var existing = await _db.ReportPermissions.FirstOrDefaultAsync(p =>
                p.ReportTemplateId == permission.ReportTemplateId &&
                p.RoleName == permission.RoleName &&
                p.TenantId == permission.TenantId);

            if (existing == null)
                _db.ReportPermissions.Add(permission);
            else
            {
                existing.CanView = permission.CanView;
                existing.CanExportPdf = permission.CanExportPdf;
                existing.CanExportExcel = permission.CanExportExcel;
                existing.CanExportWord = permission.CanExportWord;
                existing.CanExportCsv = permission.CanExportCsv;
                existing.CanPrint = permission.CanPrint;
                existing.CanEmail = permission.CanEmail;
                existing.CanSchedule = permission.CanSchedule;
                existing.CanManageTemplate = permission.CanManageTemplate;
            }
            await _db.SaveChangesAsync();
        }

        // ─── Branding ─────────────────────────────────────────────────────────

        public async Task<ReportBranding?> GetBrandingAsync(int tenantId)
            => await _db.ReportBrandings.FirstOrDefaultAsync(b => b.TenantId == tenantId && !b.IsDeleted);

        public async Task<ReportBranding?> GetDefaultBrandingAsync()
            => await _db.ReportBrandings.FirstOrDefaultAsync(b => !b.IsDeleted);

        public async Task<ReportBranding> SaveBrandingAsync(ReportBranding branding)
        {
            var existing = await _db.ReportBrandings
                .FirstOrDefaultAsync(b => b.TenantId == branding.TenantId && !b.IsDeleted);

            if (existing == null)
            {
                _db.ReportBrandings.Add(branding);
            }
            else
            {
                // Update all fields
                _db.Entry(existing).CurrentValues.SetValues(branding);
                existing.Id = existing.Id; // preserve PK
            }
            await _db.SaveChangesAsync();
            return branding;
        }

        // ─── Execution Telemetry ──────────────────────────────────────────────

        public async Task<ReportExecution> LogExecutionAsync(ReportExecution execution)
        {
            _db.ReportExecutions.Add(execution);
            await _db.SaveChangesAsync();
            return execution;
        }

        public async Task UpdateExecutionAsync(ReportExecution execution)
        {
            _db.ReportExecutions.Update(execution);
            await _db.SaveChangesAsync();
        }
    }
}
