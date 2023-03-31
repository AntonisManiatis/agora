using Agora.Identity.Core;
using Agora.Identity.Infrastructure.Data.Migrations;

using FluentMigrator.Runner;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Identity.Infrastructure.Data;

public static class DatabaseServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, PostgreSqlUserRepository>();
        return services;
    }
}