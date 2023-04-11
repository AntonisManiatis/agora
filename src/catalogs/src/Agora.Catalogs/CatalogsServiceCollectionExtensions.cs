using Agora.Catalogs.Services;
using Agora.Catalogs.Infrastructure.Data;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Catalogs;

public static class CatalogsServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogs(this IServiceCollection services)
    {
        services.AddRepositories();

        services.AddSingleton<IProductService, ProductService>();
        return services;
    }
}