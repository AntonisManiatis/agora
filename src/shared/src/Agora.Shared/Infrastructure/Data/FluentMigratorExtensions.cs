using FluentMigrator.Runner;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Shared.Infrastructure;

public static class FluentMigratorExtensions
{
    public static void MigrateUp(this IServiceProvider provider)
    {
        using (var scope = provider.CreateScope())
        {
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            // Execute the migrations
            runner.MigrateUp();
        }
    }
}