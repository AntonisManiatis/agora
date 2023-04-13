using Castle.DynamicProxy;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Shared.Infrastructure.DependencyInjection;

public static class TransactionServiceCollectionExtensions
{
    public static IServiceCollection AddTransactions(this IServiceCollection services)
    {
        services.AddSingleton<ProxyGenerator>();
        services.AddScoped<IInterceptor, TransactionInterceptor>();
        services.AddScoped<AsyncTransactionInterceptor>();

        return services;
    }

    public static IServiceCollection AddProxiedScoped<TInterface, TImplementation>(this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.AddScoped<TImplementation>();
        services.AddScoped(typeof(TInterface), serviceProvider =>
        {
            var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
            var actual = serviceProvider.GetRequiredService<TImplementation>();
            var interceptors = serviceProvider.GetServices<IInterceptor>().ToArray();
            return proxyGenerator.CreateInterfaceProxyWithTarget(typeof(TInterface), actual, interceptors);
        });

        return services;
    }

    public static IServiceCollection AddProxiedScoped<TService>(this IServiceCollection services)
        where TService : class
    {
        services.AddScoped(typeof(TService), serviceProvider =>
        {
            var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
            var actual = ActivatorUtilities.CreateInstance<TService>(serviceProvider);
            var interceptors = serviceProvider.GetServices<IInterceptor>().ToArray();
            return proxyGenerator.CreateClassProxyWithTarget(typeof(TService), actual, interceptors);
        });

        return services;
    }
}