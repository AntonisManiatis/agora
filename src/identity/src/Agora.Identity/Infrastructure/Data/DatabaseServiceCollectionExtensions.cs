using Agora.Identity.Core;
using Agora.Identity.Infrastructure.Data.Migrations;

using FluentMigrator.Runner;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Identity.Infrastructure.Data;

public static class DatabaseServiceCollectionExtensions
{
    // TODO: Rename.
    public static IServiceCollection Add(this IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, PostgreSqlUserRepository>();
        return services;
    }

    public static IServiceCollection AddIdentityMigrations(this IServiceCollection services, string connectionString)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(c =>
            {
                c.AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(InitUsers).Assembly).For.Migrations();
            })
            .AddLogging();

        return services;
    }
}