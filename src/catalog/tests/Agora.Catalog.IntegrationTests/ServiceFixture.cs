using Agora.Catalog.Services.Categories;
using Agora.Catalog.Services.Listings;
using Agora.Catalog.Services.Stores;
using Agora.Shared;
using Agora.Shared.Infrastructure;
using Agora.Shared.IntegrationTests;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Catalog.IntegrationTests;

public class ServiceFixture : IDisposable
{
    private readonly ServiceProvider? provider;

    public ServiceFixture(PostgreSqlFixture fixture)
    {
        var services = new ServiceCollection();
        services.AddShared(fixture.ConnectionString!);
        services.AddMigrations(fixture.ConnectionString!, typeof(IStoreService).Assembly);
        services.AddCatalog();

        provider = services.BuildServiceProvider(validateScopes: true);

        provider.MigrateUp();
    }

    internal Scoped<ICategoryService> CategoryService => new(provider!.CreateScope());

    internal Scoped<IListingService> ProductService => new(provider!.CreateScope());

    internal Scoped<IStoreService> StoreService => new(provider!.CreateScope());

    public void Dispose() => provider?.Dispose();
}