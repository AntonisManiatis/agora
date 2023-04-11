using Agora.Stores.Services;

using Mapster;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Stores;

public record StorePreferences(
    string Language,
    string? Country, // Optional
    string? Currency // Optional
);

public record RegisterStoreRequest(
    StorePreferences Preferences,
    string Name
);

// TaxAddress TaxAddress,
// // ? Or Tax Identification Number 
// string Tin,
// // AKA GEMI in Greece.
// string? Brn,
// IEnumerable<ProductListing> Listings

public record ProductListing(
    string Title,
    string Description
);

public record TaxAddress
{
    public static readonly TaxAddress Undefined = new();

    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
}

[Route("[controller]")]
[Produces("application/json")]
public class StoresController : ApiController
{
    private readonly IStoreService storeService;

    public StoresController(IStoreService storeService)
    {
        this.storeService = storeService;
    }

    /// <summary>
    /// Registers a store.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RegisterStoreAsync(RegisterStoreRequest req)
    {
        var storeId = Guid.NewGuid(); // TODO: Figure out who makes this
        var command = new RegisterStoreCommand(
            storeId,
            Guid.NewGuid() // TODO: GET user by token
        );

        var result = await storeService.RegisterStoreAsync(command);

        return result.Match<IActionResult>(
            // ! see if I can avoid the allocation.
            storeId => CreatedAtAction("GetStore", new { storeId = storeId }, storeId),
            errors => Problem(errors)
        );
    }

    [HttpGet]
    public async Task<IEnumerable<Store>> GetStoresAsync() =>
        await storeService.GetStoresAsync();

    /// <summary>
    /// Retrieves a store by id.
    /// </summary>
    /// <param name="storeId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{storeId}")]
    public async Task<IActionResult> GetStoreAsync(Guid storeId)
    {
        var result = await storeService.GetStoreAsync(storeId);

        return result.Match<IActionResult>(
            store => Ok(store),
            errors => Problem(errors)
        );
    }

    [HttpGet]
    [Route("{storeId}/categories")]
    public Task<IActionResult> GetStoreCategoriesAsync(Guid storeId)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [Route("{storeId}/categories")]
    [Authorize] // TODO: using specific role/policy
    public Task<IActionResult> MakeCategoryAsync(Guid storeId)
    {
        throw new NotImplementedException();
    }

    [HttpPatch]
    [Route("{storeId}/categories")]
    [Authorize] // TODO: using specific role/policy
    public Task<IActionResult> RenameCategoryAsync(Guid storeId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    [Route("{storeId}/categories/{categoryId}")]
    [Authorize] // TODO: using specific role/policy
    public Task<IActionResult> DeleteCategoryAsync(Guid storeId, int categoryId)
    {
        throw new NotImplementedException();
    }
}