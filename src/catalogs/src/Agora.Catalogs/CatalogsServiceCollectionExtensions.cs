using Agora.Catalogs.Services;
using Agora.Catalogs.Infrastructure.Data;

using Microsoft.Extensions.DependencyInjection;
using Agora.Catalogs.Services.Stores;

namespace Agora.Catalogs;

public static class CatalogsServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogs(this IServiceCollection services)
    {
        // Infrastructure
        services.AddRepositories();

        services.AddSingleton<IProductService, ProductService>();
        services.AddSingleton<IStoreService, StoreService>();
        return services;
    }
}