using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using School.Infrastructure;
using School.Services.Interfaces;

namespace School.Services
{
    public class FineCalculationBackgroundJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FineCalculationBackgroundJob> _logger;

        public FineCalculationBackgroundJob(
            IServiceProvider serviceProvider,
            ILogger<FineCalculationBackgroundJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Late Fine Calculation Background Job is starting...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
                        var fineService = scope.ServiceProvider.GetRequiredService<IFineCalculationService>();

                        var schoolIds = await dbContext.SchoolRegistrations
                            .Where(s => !s.IsDeleted && s.IsActive)
                            .Select(s => s.Id)
                            .ToListAsync(stoppingToken);

                        foreach (var schoolId in schoolIds)
                        {
                            try
                            {
                                _logger.LogInformation($"Calculating fines for school ID {schoolId}...");
                                var (success, message) = await fineService.RunDailyFineCalculationAsync(schoolId);
                                if (success)
                                {
                                    _logger.LogInformation($"Successfully calculated fines for school ID {schoolId}: {message}");
                                }
                                else
                                {
                                    _logger.LogWarning($"Calculation failed for school ID {schoolId}: {message}");
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Error calculating fines for school ID {schoolId}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in Late Fine Calculation Background Job cycle.");
                }

                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
            }

            _logger.LogInformation("Late Fine Calculation Background Job is stopped.");
        }
    }
}
