using Microsoft.Extensions.DependencyInjection;

namespace Agora.Catalogs.Infrastructure.Data;

static class PostgreSqlServiceCollectionExtensions
{
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IProductRepository, PostgreSqlProductRepository>();
        return services;
    }
}