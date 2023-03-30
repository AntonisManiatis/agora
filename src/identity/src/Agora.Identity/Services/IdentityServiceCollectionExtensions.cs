using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Identity.Services;

public static class IdentityServiceCollectionExtensions
{
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        // ? Can this be a singleton?
        // See: https://docs.fluentvalidation.net/en/latest/aspnet.html#getting-started
        services.AddTransient<IValidator<RegisterCommand>, RegisterCommandValidator>();
        services.AddScoped<UserService>();
        return services;
    }
}