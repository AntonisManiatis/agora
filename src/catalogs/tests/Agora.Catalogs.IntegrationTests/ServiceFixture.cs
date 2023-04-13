using Agora.Catalogs.Services;
using Agora.Catalogs.Services.Stores;
using Agora.Shared;
using Agora.Shared.Infrastructure;
using Agora.Shared.IntegrationTests;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Catalogs.IntegrationTests;

public class ServiceFixture : IDisposable
{
    private ServiceProvider? provider;

    public ServiceFixture(PostgreSqlFixture fixture)
    {
        var services = new ServiceCollection();
        services.AddShared(fixture.ConnectionString!);
        services.AddMigrations(fixture.ConnectionString!, typeof(IStoreService).Assembly);
        services.AddCatalogs();

        provider = services.BuildServiceProvider(validateScopes: true);

        provider.MigrateUp();
    }

    internal Scoped<IProductService> ProductService => new(provider!.CreateScope());

    internal Scoped<IStoreService> StoreService => new(provider!.CreateScope());

    public void Dispose() => provider?.Dispose();
}