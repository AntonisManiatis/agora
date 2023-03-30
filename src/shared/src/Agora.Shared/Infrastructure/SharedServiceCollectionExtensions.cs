using Agora.Shared.Infrastructure.Data;
using Agora.Shared.Infrastructure.Messaging;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Shared.Infrastructure;

public static class SharedServiceCollectionExtensions
{
    public static IServiceCollection AddShared(this IServiceCollection services, string connectionString)
    {
        // PostgreSQL services.
        services.AddPostgreSql(connectionString);

        // Messaging services.
        // TODO: Use MassTransit soon-ish.
        services.TryAddMessaging();

        return services;
    }
}