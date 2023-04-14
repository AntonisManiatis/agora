using Agora.Catalogs.Infrastructure.Data;
using Agora.Catalogs.Services;
using Agora.Catalogs.Services.Stores;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Catalogs;

public static class CatalogsServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogs(this IServiceCollection services)
    {
        // Infrastructure
        services.AddRepositories();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IStoreService, StoreService>();
        return services;
    }
}