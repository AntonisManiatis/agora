using Microsoft.Extensions.DependencyInjection;

namespace Agora.Catalog.Infrastructure.Data;

static class PostgreSqlServiceCollectionExtensions
{
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, PostgreSqlProductRepository>();
        services.AddScoped<IStoreRepository, PostgreSqlStoreRepository>();
        return services;
    }
}