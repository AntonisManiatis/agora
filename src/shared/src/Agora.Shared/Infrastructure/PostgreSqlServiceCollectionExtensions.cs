using Agora.Shared.Infrastructure.Data;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Shared.Infrastructure;

public static class PostgreSqlServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreSql(this IServiceCollection services, string connectionString)
    {
        services.AddTransient<IDbConnector>(sp => new PostgreSqlDbConnector(connectionString));
        return services;
    }
}