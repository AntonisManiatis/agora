using Agora.Catalogs.Services.Stores;
using Agora.Shared;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Catalogs.IntegrationTests;

public class StoreServiceFixture : IDisposable
{
    private ServiceProvider? provider;

    internal IStoreService Service { get; private set; }

    public StoreServiceFixture(CatalogPostgreSqlFixture fixture)
    {
        var services = new ServiceCollection();
        services.AddShared(fixture.ConnectionString!);
        services.AddCatalogs();

        provider = services.BuildServiceProvider(validateScopes: true);
        Service = provider.GetRequiredService<IStoreService>();
    }

    public void Dispose()
    {
        provider?.Dispose();
    }

}