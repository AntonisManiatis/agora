using Agora.Shared;
using Agora.Shared.Infrastructure;
using Agora.Shared.IntegrationTests;
using Agora.Stores.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Stores.IntegrationTests;

public class ServiceFixture : IDisposable
{
    private readonly ServiceProvider? provider;

    public ServiceFixture(PostgreSqlFixture fixture)
    {
        var services = new ServiceCollection();
        services.AddShared(fixture.ConnectionString!);
        services.AddMigrations(fixture.ConnectionString!, typeof(IStoreService).Assembly);
        services.AddStores();

        provider = services.BuildServiceProvider(validateScopes: true);

        provider.MigrateUp();
    }

    internal Scoped<IStoreService> StoreService => new(provider!.CreateScope());

    internal Scoped<IInventoryService> InventoryService => new(provider!.CreateScope());

    internal Scoped<ICategoryService> CategoryService => new(provider!.CreateScope());

    public void Dispose() => provider?.Dispose();
}