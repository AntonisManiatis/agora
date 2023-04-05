using Agora.Stores.Core.Products;
using Agora.Stores.Core.Stores;

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