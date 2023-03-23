using Agora.Stores.Infrastructure.Data;
using Agora.Stores.Infrastructure.Data.Migrations;

using FluentMigrator.Runner;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Stores.Infrastructure;

public static class StoreServiceCollectionExtensions
{
    public static IServiceCollection AddStores(this IServiceCollection services, string connectionString)
    {
        // TODO: this line could be in a separate package.
        services.AddTransient<IDbConnector>(sp => new PostgreSqlDbConnector(connectionString));
        services.AddScoped<StoreService>(); // ? Singleton maybe?
        return services;
    }

    // ? should probably be global.
    public static IServiceCollection AddStoreMigrations(this IServiceCollection services, string connectionString)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(c =>
            {
                c.AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(Init).Assembly).For.Migrations();
            })
            .AddLogging();

        return services;
    }
}