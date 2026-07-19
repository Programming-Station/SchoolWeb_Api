using Cronos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using School.Domain.Reporting;
using School.Infrastructure;
using School.Infrastructure.Repositories;
using School.Services.Interfaces;
using School_DTOs.Reporting;
using System.Text.Json;

namespace School.Services.Reporting
{
    /// <summary>
    /// DB-driven report scheduling service.
    /// CRUD for ReportSchedule records + BackgroundService that polls every minute
    /// and executes overdue schedules using Cronos cron expression parsing.
    /// </summary>
    public class ReportScheduleService : BackgroundService, IReportScheduleService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReportScheduleService> _logger;

        public ReportScheduleService(
            IServiceProvider serviceProvider,
            ILogger<ReportScheduleService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        // ─── BackgroundService ────────────────────────────────────────────────

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Report Schedule Engine started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessDueSchedulesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Report Schedule Engine.");
                }

                // Poll every 60 seconds
                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }

        private async Task ProcessDueSchedulesAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ReportingRepository>();
            var dueSchedules = await repo.GetDueSchedulesAsync(DateTime.UtcNow);

            foreach (var schedule in dueSchedules)
            {
                try
                {
                    await ExecuteScheduleInScopeAsync(schedule, scope);
                    schedule.LastRunAt = DateTime.UtcNow;
                    schedule.LastRunStatus = "Success";
                    schedule.FailureCount = 0;
                    schedule.NextRunAt = ComputeNextRun(schedule.CronExpression);
                    await repo.UpdateScheduleAsync(schedule);

                    _logger.LogInformation(
                        "Scheduled report [{Name}] executed successfully. Next run: {Next}",
                        schedule.ScheduleName, schedule.NextRunAt);
                }
                catch (Exception ex)
                {
                    schedule.FailureCount++;
                    schedule.LastRunStatus = "Failed";
                    schedule.LastError = ex.Message;
                    schedule.NextRunAt = ComputeNextRun(schedule.CronExpression);

                    if (schedule.FailureCount >= schedule.MaxRetries)
                    {
                        schedule.IsActive = false;
                        _logger.LogWarning(
                            "Schedule [{Name}] disabled after {Count} failures.",
                            schedule.ScheduleName, schedule.FailureCount);
                    }
                    await repo.UpdateScheduleAsync(schedule);
                    _logger.LogError(ex, "Failed executing schedule [{Name}].", schedule.ScheduleName);
                }
            }
        }

        private async Task ExecuteScheduleInScopeAsync(ReportSchedule schedule, IServiceScope scope)
        {
            var engineService = scope.ServiceProvider.GetRequiredService<IReportingEngineService>();
            var emailService = scope.ServiceProvider.GetRequiredService<IReportEmailService>();

            var parameters = string.IsNullOrWhiteSpace(schedule.ParametersJson)
                ? new Dictionary<string, string>()
                : JsonSerializer.Deserialize<Dictionary<string, string>>(schedule.ParametersJson)
                  ?? new Dictionary<string, string>();

            var request = new ReportExecutionRequest
            {
                ReportTemplateId = schedule.ReportTemplateId,
                Format = schedule.Format,
                Parameters = parameters,
                TenantId = schedule.TenantId
            };

            var fileBytes = await engineService.GenerateAsync(
                request, $"Scheduler:{schedule.ScheduleName}", schedule.TenantId);

            // Email the generated report
            var toEmails = schedule.RecipientEmails.Split(';', ',')
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .ToList();

            var ccEmails = string.IsNullOrWhiteSpace(schedule.CcEmails)
                ? new List<string>()
                : schedule.CcEmails.Split(';', ',').Select(e => e.Trim())
                    .Where(e => !string.IsNullOrWhiteSpace(e)).ToList();

            var emailRequest = new ReportEmailRequest
            {
                ReportTemplateId = schedule.ReportTemplateId,
                ReportCode = schedule.ReportTemplate?.ReportCode ?? string.Empty,
                Format = schedule.Format,
                Parameters = parameters,
                ToEmails = toEmails,
                CcEmails = ccEmails,
                Subject = ResolveEmailSubject(schedule),
                Body = ResolveEmailBody(schedule)
            };

            await emailService.EmailReportAsync(emailRequest, $"Scheduler", schedule.TenantId);
        }

        private static string ResolveEmailSubject(ReportSchedule schedule)
        {
            var template = schedule.EmailSubjectTemplate
                ?? "{ReportName} - Scheduled Report for {Date}";
            return template
                .Replace("{ReportName}", schedule.ReportTemplate?.ReportName ?? schedule.ScheduleName)
                .Replace("{Date}", DateTime.Now.ToString("dd-MMM-yyyy"))
                .Replace("{ScheduleName}", schedule.ScheduleName);
        }

        private static string ResolveEmailBody(ReportSchedule schedule)
        {
            var template = schedule.EmailBodyTemplate
                ?? "Please find the attached {ReportName} report for {Date}.\n\nThis is an automated report sent by SchoolSaaS ERP.";
            return template
                .Replace("{ReportName}", schedule.ReportTemplate?.ReportName ?? schedule.ScheduleName)
                .Replace("{Date}", DateTime.Now.ToString("dd-MMM-yyyy"))
                .Replace("{ScheduleName}", schedule.ScheduleName);
        }

        private static DateTime? ComputeNextRun(string cronExpression)
        {
            try
            {
                // Support friendly presets
                var cron = cronExpression switch
                {
                    "DAILY_5PM" => "0 17 * * *",
                    "WEEKLY_FRIDAY" => "0 17 * * 5",
                    "MONTHLY_1ST" => "0 8 1 * *",
                    "HOURLY" => "0 * * * *",
                    _ => cronExpression
                };

                var expression = CronExpression.Parse(cron);
                return expression.GetNextOccurrence(DateTimeOffset.UtcNow, TimeZoneInfo.Utc)?.UtcDateTime;
            }
            catch
            {
                return DateTime.UtcNow.AddHours(24); // Fallback: next day
            }
        }

        // ─── IReportScheduleService CRUD ───────────────────────────────────────

        public async Task<List<ReportScheduleDto>> GetAllAsync(int? tenantId = null)
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ReportingRepository>();
            var schedules = await repo.GetSchedulesAsync(tenantId);
            return schedules.Select(MapToDto).ToList();
        }

        public async Task<ReportScheduleDto?> GetByIdAsync(int id)
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ReportingRepository>();
            var schedule = await repo.GetScheduleByIdAsync(id);
            return schedule == null ? null : MapToDto(schedule);
        }

        public async Task<ReportScheduleDto> CreateAsync(
            CreateReportScheduleRequest request, int? tenantId = null)
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ReportingRepository>();

            var entity = new ReportSchedule
            {
                TenantId = tenantId,
                ReportTemplateId = request.ReportTemplateId,
                ScheduleName = request.ScheduleName,
                CronExpression = request.CronExpression,
                RecipientEmails = request.RecipientEmails,
                CcEmails = request.CcEmails,
                Format = request.Format,
                ParametersJson = request.Parameters.Count > 0
                    ? JsonSerializer.Serialize(request.Parameters) : null,
                EmailSubjectTemplate = request.EmailSubjectTemplate,
                EmailBodyTemplate = request.EmailBodyTemplate,
                NextRunAt = ComputeNextRun(request.CronExpression),
                IsActive = true
            };

            var saved = await repo.CreateScheduleAsync(entity);
            return MapToDto(saved);
        }

        public async Task<ReportScheduleDto> UpdateAsync(
            int id, CreateReportScheduleRequest request)
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ReportingRepository>();
            var existing = await repo.GetScheduleByIdAsync(id)
                ?? throw new InvalidOperationException($"Schedule {id} not found.");

            existing.ScheduleName = request.ScheduleName;
            existing.CronExpression = request.CronExpression;
            existing.RecipientEmails = request.RecipientEmails;
            existing.CcEmails = request.CcEmails;
            existing.Format = request.Format;
            existing.ParametersJson = request.Parameters.Count > 0
                ? JsonSerializer.Serialize(request.Parameters) : null;
            existing.EmailSubjectTemplate = request.EmailSubjectTemplate;
            existing.EmailBodyTemplate = request.EmailBodyTemplate;
            existing.NextRunAt = ComputeNextRun(request.CronExpression);

            await repo.UpdateScheduleAsync(existing);
            return MapToDto(existing);
        }

        public async Task DeleteAsync(int id)
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ReportingRepository>();
            await repo.DeleteScheduleAsync(id);
        }

        public async Task ToggleActiveAsync(int id, bool isActive)
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ReportingRepository>();
            var schedule = await repo.GetScheduleByIdAsync(id);
            if (schedule != null)
            {
                schedule.IsActive = isActive;
                await repo.UpdateScheduleAsync(schedule);
            }
        }

        public async Task<List<ReportScheduleDto>> GetDueSchedulesAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ReportingRepository>();
            var schedules = await repo.GetDueSchedulesAsync(DateTime.UtcNow);
            return schedules.Select(MapToDto).ToList();
        }

        public async Task ExecuteScheduledAsync(int scheduleId)
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ReportingRepository>();
            var schedule = await repo.GetScheduleByIdAsync(scheduleId)
                ?? throw new InvalidOperationException($"Schedule {scheduleId} not found.");
            await ExecuteScheduleInScopeAsync(schedule, scope);
        }

        private static ReportScheduleDto MapToDto(ReportSchedule s) => new()
        {
            Id = s.Id,
            ReportTemplateId = s.ReportTemplateId,
            ReportName = s.ReportTemplate?.ReportName,
            ScheduleName = s.ScheduleName,
            CronExpression = s.CronExpression,
            RecipientEmails = s.RecipientEmails,
            CcEmails = s.CcEmails,
            Format = s.Format,
            Parameters = string.IsNullOrWhiteSpace(s.ParametersJson)
                ? new Dictionary<string, string>()
                : JsonSerializer.Deserialize<Dictionary<string, string>>(s.ParametersJson)
                  ?? new Dictionary<string, string>(),
            LastRunAt = s.LastRunAt,
            NextRunAt = s.NextRunAt,
            LastRunStatus = s.LastRunStatus,
            FailureCount = s.FailureCount,
            IsActive = s.IsActive
        };
    }
}
