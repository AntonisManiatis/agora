using System.Reflection;

using FluentMigrator.Runner;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Shared.Infrastructure;

public static class FluentMigratorExtensions
{
    public static void MigrateUp(this IServiceProvider provider)
    {
        using (var scope = provider.CreateScope())
        {
            var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
            // Execute the migrations
            runner?.MigrateUp();
        }
    }

    public static IServiceCollection AddMigrations(this IServiceCollection services,
        string connectionString, params Assembly[] assemblies)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(c =>
            {
                c.AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(assemblies).For.Migrations();
            })
            .AddLogging();

        return services;
    }
}