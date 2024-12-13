using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace VsaArchitecture.Api;

public static class HealthCheck
{
    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
        .AddSqlServer(
            configuration.GetConnectionString("ConnectionString"), 
            healthQuery: "select 1", 
            name: "SQL Server", 
            failureStatus: HealthStatus.Unhealthy, 
            tags: new[] { "Feedback", "Database" });

        return services;
    }
}
