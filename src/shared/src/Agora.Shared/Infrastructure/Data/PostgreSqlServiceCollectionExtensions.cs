using Microsoft.Extensions.DependencyInjection;

namespace Agora.Shared.Infrastructure.Data;

public static class PostgreSqlServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreSql(this IServiceCollection services, string connectionString)
    {
        // ? connector scoped and keeps 1 instance
        services.AddTransient<IDbConnector>(_ => new PostgreSqlDbConnector(connectionString));
        // ? A func of the "current" IDbConnector for singletons.
        return services;
    }
}