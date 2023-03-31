using Agora.Identity.Infrastructure.Data;
using Agora.Identity.Infrastructure.Tokens;
using Agora.Identity.Services;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Identity;

public static class IdentityServiceCollectionExtensions
{
    public static IServiceCollection AddIdentity(this IServiceCollection services,
        Func<IServiceProvider, Func<JwtOptions>> jwtOptionsFunc)
    {
        // Infrastructure services.
        services.AddRepositories();

        // Tokens.
        // ? this might be confusing to someone but I basically don't want to reference IOptions here.
        services.AddSingleton(jwtOptionsFunc);
        services.AddSingleton<IJwtGenerator, JwtGenerator>();

        // ? Can this be a singleton?
        // See: https://docs.fluentvalidation.net/en/latest/aspnet.html#getting-started
        services.AddTransient<IValidator<RegisterCommand>, RegisterCommandValidator>();
        services.AddScoped<IUserService, UserService>();

        services.AddTransient<IValidator<AuthenticationCommand>, AuthenticationCommandValidator>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        return services;
    }
}