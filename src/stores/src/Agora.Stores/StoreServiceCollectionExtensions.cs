using Agora.Stores.Infrastructure.Data;
using Agora.Stores.Services;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Stores;

public static class StoreServiceCollectionExtensions
{
    public static IServiceCollection AddStores(this IServiceCollection services)
    {
        // Infrastructure.
        services.AddRepositories();

        // Mappings
        Mappings.Init();

        // ? Can this be a singleton?
        // See: https://docs.fluentvalidation.net/en/latest/aspnet.html#getting-started
        services.AddTransient<IValidator<OpenStoreCommand>, OpenStoreRequestValidator>();
        services.AddScoped<IStoreService, StoreService>(); // ? Singleton maybe?

        services.AddScoped<ICategoryService, CategoryService>();

        services.AddTransient<IValidator<ListProductCommand>, ListProductCommandValidator>();
        services.AddScoped<IInventoryService, InventoryService>();
        return services;
    }
}