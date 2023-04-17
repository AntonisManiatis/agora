using Microsoft.Extensions.DependencyInjection;

namespace Agora.Catalog.Infrastructure.Data;

static class RepositoryServiceCollectionExtensions
{
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, PostgreSqlCategoryRepository>();
        services.AddScoped<IProductRepository, PostgreSqlProductRepository>();
        services.AddScoped<IStoreRepository, PostgreSqlStoreRepository>();
        return services;
    }
}