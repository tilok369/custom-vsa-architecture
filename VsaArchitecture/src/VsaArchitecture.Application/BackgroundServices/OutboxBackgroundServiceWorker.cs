using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VsaArchitecture.Application.Contracts.BackgroundServices;

namespace VsaArchitecture.Application.BackgroundServices;

public class OutboxBackgroundServiceWorker(IServiceScopeFactory serviceScopeFactory) 
    : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var outboxService = scope.ServiceProvider.GetRequiredService<IOutboxBackgroundService>();
            await outboxService.Run();

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}