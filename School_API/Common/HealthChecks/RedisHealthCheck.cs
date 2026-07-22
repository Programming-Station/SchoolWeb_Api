using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace School_API.Common.HealthChecks
{
    public class RedisHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public RedisHealthCheck(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("RedisConnection") ?? "";
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                return HealthCheckResult.Healthy("Redis connection string is not configured (falling back to Memory Cache).");
            }

            try
            {
                using (var connection = ConnectionMultiplexer.Connect(_connectionString))
                {
                    var db = connection.GetDatabase();
                    db.Ping();
                }
                return HealthCheckResult.Healthy("Redis cache is responding normally.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Redis connection failed.", ex);
            }
        }
    }
}
