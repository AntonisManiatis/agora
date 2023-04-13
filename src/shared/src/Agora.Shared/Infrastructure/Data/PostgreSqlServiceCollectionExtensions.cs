using Microsoft.Extensions.DependencyInjection;

namespace Agora.Shared.Infrastructure.Data;

public static class PostgreSqlServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreSql(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IDbConnector>(_ => new PostgreSqlDbConnector(connectionString));
        return services;
    }
}