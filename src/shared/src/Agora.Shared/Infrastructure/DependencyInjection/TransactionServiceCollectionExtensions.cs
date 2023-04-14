using Castle.DynamicProxy;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Shared.Infrastructure.DependencyInjection;

public static class TransactionServiceCollectionExtensions
{
    internal static IServiceCollection AddTransactionInterceptors(this IServiceCollection services)
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
        services.AddScoped(typeof(TInterface), provider =>
        {
            var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
            var target = provider.GetRequiredService<TImplementation>();
            var interceptors = provider.GetServices<IInterceptor>().ToArray();
            return proxyGenerator.CreateInterfaceProxyWithTarget(typeof(TInterface), target, interceptors);
        });

        return services;
    }

    // ? Maybe document how this works?
    public static IServiceCollection AddProxiedScoped<TService>(this IServiceCollection services)
        where TService : class
    {
        services.AddScoped(typeof(TService), provider =>
        {
            var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
            var target = ActivatorUtilities.CreateInstance<TService>(provider);
            var interceptors = provider.GetServices<IInterceptor>().ToArray();

            var ctor = typeof(TService).GetConstructors()[0];
            var args = ctor.GetParameters()
                .Select(p => provider.GetService(p.ParameterType))
                .ToArray();

            return proxyGenerator.CreateClassProxyWithTarget(
                typeof(TService),
                target,
                args,
                interceptors
            );
        });

        return services;
    }
}