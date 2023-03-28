using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Agora.Shared.Infrastructure.Messaging;

public static class MessagingServiceCollectionExtension
{
    public static IServiceCollection TryAddMessaging(this IServiceCollection services)
    {
        services.TryAddSingleton<IMessagePublisher, NullMessagePublisher>();
        return services;
    }

    public static IServiceCollection AddMassTransit(this IServiceCollection service)
    {
        throw new NotImplementedException();
    }
}