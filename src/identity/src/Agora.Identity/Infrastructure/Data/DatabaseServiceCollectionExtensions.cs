using Agora.Identity.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Identity.Infrastructure.Data;

public static class DatabaseServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, PostgreSqlUserRepository>();
        return services;
    }
}