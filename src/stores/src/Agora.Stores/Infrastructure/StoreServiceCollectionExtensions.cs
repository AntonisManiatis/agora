using Agora.Stores.Infrastructure.Data.Migrations;
using Agora.Stores.Services;

using FluentMigrator.Runner;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Stores.Infrastructure;

public static class StoreServiceCollectionExtensions
{
    public static IServiceCollection AddStores(this IServiceCollection services)
    {
        // ? Can this be a singleton?
        // See: https://docs.fluentvalidation.net/en/latest/aspnet.html#getting-started
        services.AddTransient<IValidator<OpenStoreRequest>, OpenStoreRequestValidator>();
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