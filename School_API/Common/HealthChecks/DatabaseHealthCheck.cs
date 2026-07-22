using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace School_API.Common.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public DatabaseHealthCheck(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT 1;";
                        await command.ExecuteScalarAsync(cancellationToken);
                    }
                }
                return HealthCheckResult.Healthy("Database is responding normally.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Database connection failed.", ex);
            }
        }
    }
}
