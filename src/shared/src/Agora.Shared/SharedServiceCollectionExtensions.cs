using Agora.Shared.Infrastructure.Data;
using Agora.Shared.Infrastructure.DependencyInjection;
using Agora.Shared.Infrastructure.Messaging;

using Castle.DynamicProxy;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Shared;

public static class SharedServiceCollectionExtensions
{
    static SharedServiceCollectionExtensions()
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    // ? Consider a Func<string> for connectionString in case of configuration reloading(?).
    public static IServiceCollection AddShared(this IServiceCollection services, string connectionString)
    {
        // PostgreSQL services.
        services.AddPostgreSql(connectionString);

        // Transactions
        services.AddSingleton<ProxyGenerator>();
        // ? Maybe should be transient
        services.AddScoped<IInterceptor, TransactionInterceptor>();
        services.AddScoped<AsyncTransactionInterceptor>();

        // Messaging services.
        // TODO: Use MassTransit soon-ish.
        services.TryAddMessaging();

        return services;
    }
}