using Agora.Stores.Core.Products;
using Agora.Stores.Core.Stores;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Stores.Infrastructure.Data;

static class DatabaseServiceCollectionExtensions
{
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IStoreRepository, PostgreSqlStoreRepository>();
        services.AddScoped<IProductRepository, PostgreSqlProductRepository>();
        return services;
    }
}