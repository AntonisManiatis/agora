using Agora.Catalogs.Services;
using Agora.Shared;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Catalogs.IntegrationTests;

public class ProductServiceFixture : IDisposable
{
    private ServiceProvider? provider;

    internal IProductService Service { get; private set; }

    public ProductServiceFixture(CatalogPostgreSqlFixture fixture)
    {
        var services = new ServiceCollection();
        services.AddShared(fixture.ConnectionString!);
        services.AddCatalogs();

        provider = services.BuildServiceProvider(validateScopes: true);
        Service = provider.GetRequiredService<IProductService>();
    }

    public void Dispose()
    {
        provider?.Dispose();
    }
}