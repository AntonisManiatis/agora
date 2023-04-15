using Agora.Shared.Infrastructure;

namespace Agora.API;

sealed class TruncateDatabaseHostedService : IHostedService
{
    private readonly ILogger<TruncateDatabaseHostedService> logger;
    private readonly IServiceProvider provider;

    public TruncateDatabaseHostedService(
        ILogger<TruncateDatabaseHostedService> logger,
        IServiceProvider provider
    )
    {
        this.logger = logger;
        this.provider = provider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Migrating...");

        provider.Rollback();
        provider.MigrateUp();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}