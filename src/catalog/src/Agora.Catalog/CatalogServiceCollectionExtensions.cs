using Agora.Catalog.Infrastructure.Data;
using Agora.Catalog.Services.Categories;
using Agora.Catalog.Services.Listings;
using Agora.Catalog.Services.Stores;
using Agora.Shared.Infrastructure.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Catalog;

public static class CatalogServiceCollectionExtensions
{
    public static IServiceCollection AddCatalog(this IServiceCollection services)
    {
        // Infrastructure
        services.AddRepositories();

        services.AddProxiedScoped<ICategoryService, CategoryService>();
        services.AddScoped<IListingService, ListingService>();
        services.AddScoped<IStoreService, StoreService>();
        return services;
    }
}