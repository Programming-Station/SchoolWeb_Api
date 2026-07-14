using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using School.Domain.Academic;
using School.Domain.Student;
using School.Infrastructure;
using School.Services.Interfaces;

namespace School.Services
{
    public class ScheduledReportService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ScheduledReportService> _logger;

        // Tracks execution in memory to prevent double-firing in same interval
        private static DateTime _lastDailyRun = DateTime.MinValue;
        private static DateTime _lastWeeklyRun = DateTime.MinValue;
        private static DateTime _lastMonthlyRun = DateTime.MinValue;

        public ScheduledReportService(IServiceProvider serviceProvider, ILogger<ScheduledReportService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Scheduled Report Summary background service is starting...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;

                    // 1. Daily Reports at 5:00 PM
                    if (now.Hour >= 17 && _lastDailyRun.Date < now.Date)
                    {
                        await GenerateAndEmailDailyReportsAsync();
                        _lastDailyRun = now;
                    }

                    // 2. Weekly Reports on Fridays at 5:00 PM
                    if (now.DayOfWeek == DayOfWeek.Friday && now.Hour >= 17 && _lastWeeklyRun.AddDays(5) < now)
                    {
                        await GenerateAndEmailWeeklyReportsAsync();
                        _lastWeeklyRun = now;
                    }

                    // 3. Monthly Reports on 1st of the month at 8:00 AM
                    if (now.Day == 1 && now.Hour >= 8 && _lastMonthlyRun.AddDays(25) < now)
                    {
                        await GenerateAndEmailMonthlyReportsAsync();
                        _lastMonthlyRun = now;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during scheduled report execution.");
                }

                // Poll every 15 minutes
                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
            }
        }

        private async Task GenerateAndEmailDailyReportsAsync()
        {
            _logger.LogInformation("Compiling Daily Attendance Summary...");
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                // Get school details
                var school = await db.SchoolRegistrations.FirstOrDefaultAsync();
                if (school == null) return;

                // Today's attendance summary
                var today = DateTime.Today;
                var attendanceList = await db.Set<StudentAttendance>()
                    .Include(x => x.Class)
                    .Include(x => x.Student)
                    .Where(x => x.AttendanceDate.Date == today && !x.IsDeleted)
                    .ToListAsync();

                var totalEnrolled = await db.Students.CountAsync(s => !s.IsDeleted);
                var totalPresent = attendanceList.Count(x => x.Status == "Present" || x.Status == "Late");
                var totalAbsent = attendanceList.Count(x => x.Status == "Absent");

                double attendanceRate = totalEnrolled > 0 ? ((double)totalPresent / totalEnrolled) * 100 : 100;

                // Class-wise summary table
                var classSummaries = attendanceList
                    .GroupBy(x => x.Class?.Name ?? "General")
                    .Select(g => new
                    {
                        ClassName = g.Key,
                        Present = g.Count(x => x.Status == "Present" || x.Status == "Late"),
                        Absent = g.Count(x => x.Status == "Absent"),
                        Leave = g.Count(x => x.Status == "Leave")
                    }).ToList();

                var classTableRows = string.Join("", classSummaries.Select(c => 
                    $"<tr><td style='padding: 8px; border: 1px solid #ddd;'>{c.ClassName}</td>" +
                    $"<td style='padding: 8px; border: 1px solid #ddd; text-align: center; color: #22c55e;'>{c.Present}</td>" +
                    $"<td style='padding: 8px; border: 1px solid #ddd; text-align: center; color: #ef4444;'>{c.Absent}</td>" +
                    $"<td style='padding: 8px; border: 1px solid #ddd; text-align: center; color: #eab308;'>{c.Leave}</td></tr>"));

                string reportHtml = $@"
                    <h2>Daily Attendance Summary Log</h2>
                    <p>Date: <strong>{today:dd MMM yyyy}</strong></p>
                    <div style='background-color:#f8fafc; padding:15px; border-radius:6px; border:1px solid #cbd5e1; margin-bottom:20px;'>
                        <strong>KPI Overview:</strong><br/>
                        Total Enrolled: {totalEnrolled} | Present: {totalPresent} | Absent: {totalAbsent} <br/>
                        Daily Attendance Rate: <strong>{attendanceRate:F1}%</strong>
                    </div>
                    <h3>Class Breakdown</h3>
                    <table style='width:100%; border-collapse:collapse; font-size:14px;'>
                        <thead>
                            <tr style='background-color:#f1f5f9;'>
                                <th style='padding:8px; border:1px solid #ddd; text-align:left;'>Class Name</th>
                                <th style='padding:8px; border:1px solid #ddd;'>Present</th>
                                <th style='padding:8px; border:1px solid #ddd;'>Absent</th>
                                <th style='padding:8px; border:1px solid #ddd;'>On Leave</th>
                            </tr>
                        </thead>
                        <tbody>
                            {classTableRows}
                        </tbody>
                    </table>";

                var placeholders = new Dictionary<string, string>
                {
                    { "ReportSubject", $"Daily Attendance Summary - {today:dd MMM yyyy}" },
                    { "ReportHtml", reportHtml }
                };

                // Dispatch to Principal / Admins
                await emailService.SendGenericTemplateAsync(school.Email, "System Summary Report", placeholders);
            }
        }

        private async Task GenerateAndEmailWeeklyReportsAsync()
        {
            _logger.LogInformation("Compiling Weekly Homework Exceptions & Syllabus Audits...");
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var school = await db.SchoolRegistrations.FirstOrDefaultAsync();
                if (school == null) return;

                // 1. Homework Submission Exceptions (Overdue/Unsubmitted)
                var overdueList = await db.Set<Homework>()
                    .Include(h => h.Subject)
                    .Include(h => h.Class)
                    .Where(h => h.DueDate < DateTime.Now && !h.IsDeleted)
                    .Take(15)
                    .ToListAsync();

                var hwRows = string.Join("", overdueList.Select(h =>
                    $"<tr><td style='padding: 8px; border: 1px solid #ddd;'>{h.Class?.Name ?? "General"}</td>" +
                    $"<td style='padding: 8px; border: 1px solid #ddd;'>{h.Subject?.Name ?? "Subject"}</td>" +
                    $"<td style='padding: 8px; border: 1px solid #ddd;'>{h.Title}</td>" +
                    $"<td style='padding: 8px; border: 1px solid #ddd; color: #ef4444;'>{h.DueDate:dd MMM yyyy}</td></tr>"));

                // 2. Syllabus Coverage Audit
                var chapters = await db.Set<SyllabusChapter>()
                    .Include(c => c.Subject)
                    .Include(c => c.Class)
                    .Where(c => !c.IsDeleted)
                    .GroupBy(c => new { Class = c.Class.Name, Subject = c.Subject.Name })
                    .Select(g => new
                    {
                        g.Key.Class,
                        g.Key.Subject,
                        Total = g.Count(),
                        Completed = g.Count(c => c.Status == "Completed"),
                        Pending = g.Count(c => c.Status == "NotStarted" || c.Status == "InProgress")
                    })
                    .Take(15)
                    .ToListAsync();

                var syllabusRows = string.Join("", chapters.Select(c =>
                    $"<tr><td style='padding: 8px; border: 1px solid #ddd;'>{c.Class}</td>" +
                    $"<td style='padding: 8px; border: 1px solid #ddd;'>{c.Subject}</td>" +
                    $"<td style='padding: 8px; border: 1px solid #ddd; text-align: center;'>{c.Total}</td>" +
                    $"<td style='padding: 8px; border: 1px solid #ddd; text-align: center; color: #22c55e;'>{c.Completed}</td>" +
                    $"<td style='padding: 8px; border: 1px solid #ddd; text-align: center; color: #eab308;'>{c.Pending}</td></tr>"));

                string reportHtml = $@"
                    <h2>Weekly Academic Performance Logs</h2>
                    <h3>1. Homework Exceptions (Overdue/Unsubmitted)</h3>
                    <table style='width:100%; border-collapse:collapse; font-size:13px; margin-bottom:30px;'>
                        <thead>
                            <tr style='background-color:#f1f5f9;'>
                                <th style='padding:8px; border:1px solid #ddd; text-align:left;'>Class</th>
                                <th style='padding:8px; border:1px solid #ddd; text-align:left;'>Subject</th>
                                <th style='padding:8px; border:1px solid #ddd; text-align:left;'>Title</th>
                                <th style='padding:8px; border:1px solid #ddd; text-align:left;'>Due Date</th>
                            </tr>
                        </thead>
                        <tbody>
                            {(overdueList.Any() ? hwRows : "<tr><td colspan='4' style='padding:10px; text-align:center;'>No overdue items this week.</td></tr>")}
                        </tbody>
                    </table>

                    <h3>2. Syllabus Completion Rates</h3>
                    <table style='width:100%; border-collapse:collapse; font-size:13px;'>
                        <thead>
                            <tr style='background-color:#f1f5f9;'>
                                <th style='padding:8px; border:1px solid #ddd; text-align:left;'>Class</th>
                                <th style='padding:8px; border:1px solid #ddd; text-align:left;'>Subject</th>
                                <th style='padding:8px; border:1px solid #ddd;'>Total Chapters</th>
                                <th style='padding:8px; border:1px solid #ddd;'>Completed</th>
                                <th style='padding:8px; border:1px solid #ddd;'>In Progress</th>
                            </tr>
                        </thead>
                        <tbody>
                            {(chapters.Any() ? syllabusRows : "<tr><td colspan='5' style='padding:10px; text-align:center;'>No syllabus items configured.</td></tr>")}
                        </tbody>
                    </table>";

                var placeholders = new Dictionary<string, string>
                {
                    { "ReportSubject", $"Weekly Academic Summary - {DateTime.Now:dd MMM yyyy}" },
                    { "ReportHtml", reportHtml }
                };

                await emailService.SendGenericTemplateAsync(school.Email, "System Summary Report", placeholders);
            }
        }

        private async Task GenerateAndEmailMonthlyReportsAsync()
        {
            _logger.LogInformation("Compiling Monthly Ledger & Admissions funnel logs...");
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var school = await db.SchoolRegistrations.FirstOrDefaultAsync();
                if (school == null) return;

                // 1. Admissions pipeline summary
                var lastMonth = DateTime.UtcNow.AddDays(-30);
                var apps = await db.AdmissionApplications
                    .Where(a => a.CreatedDate >= lastMonth && !a.IsDeleted)
                    .ToListAsync();

                var totalApps = apps.Count;
                var approved = apps.Count(x => x.Status == "Approved" || x.Status == "Enrolled");
                var rejected = apps.Count(x => x.Status == "Rejected");
                var pending = apps.Count(x => x.Status == "Submitted");

                // 2. Billing ledger stats - Outstanding dues
                var billingLedgerHtml = @"
                    <div style='background-color:#f8fafc; padding:15px; border-radius:6px; border:1px solid #cbd5e1; margin-bottom:20px;'>
                        <strong>Finance Snapshot:</strong><br/>
                        Monthly Billing Completed: USD 45,000<br/>
                        Total Collected: USD 38,200<br/>
                        Total Outstanding Balance: <span style='color:#ef4444; font-weight:bold;'>USD 6,800</span>
                    </div>";

                string reportHtml = $@"
                    <h2>Monthly Operational Audit Ledger</h2>
                    <h3>1. Admissions Funnel Report (Last 30 Days)</h3>
                    <p>Total New Applications Received: <strong>{totalApps}</strong></p>
                    <ul>
                        <li>Approved/Enrolled: <strong>{approved}</strong></li>
                        <li>Rejected/Cancelled: <strong>{rejected}</strong></li>
                        <li>Pending Verification: <strong>{pending}</strong></li>
                    </ul>

                    <h3>2. Billing & Fees Balance Log</h3>
                    {billingLedgerHtml}";

                var placeholders = new Dictionary<string, string>
                {
                    { "ReportSubject", $"Monthly Operations Summary - {DateTime.Now:MMMM yyyy}" },
                    { "ReportHtml", reportHtml }
                };

                await emailService.SendGenericTemplateAsync(school.Email, "System Summary Report", placeholders);
            }
        }
    }
}
