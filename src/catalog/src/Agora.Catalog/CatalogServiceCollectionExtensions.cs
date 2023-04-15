using Agora.Catalog.Infrastructure.Data;
using Agora.Catalog.Services;
using Agora.Catalog.Services.Stores;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Catalog;

public static class CatalogServiceCollectionExtensions
{
    public static IServiceCollection AddCatalog(this IServiceCollection services)
    {
        // Infrastructure
        services.AddRepositories();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IStoreService, StoreService>();
        return services;
    }
}