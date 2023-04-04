using Agora.Stores.Core;
using Agora.Stores.Core.Products;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Stores.Infrastructure.Data;

public static class DatabaseServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IStoreRepository, PostgreSqlStoreRepository>();
        services.AddSingleton<IProductRepository, PostgreSqlProductRepository>();
        return services;
    }
}