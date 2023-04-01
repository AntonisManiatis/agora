using Agora.Stores;
using Agora.Stores.Services;

using Mapster;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Stores;

public record OpenStoreRequest(
    string Name,
    TaxAddress TaxAddress,
    // ? Or Tax Identification Number 
    string Tin,
    // AKA GEMI in Greece.
    string? Brn
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
    /// Opens a store.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> OpenStoreAsync(OpenStoreRequest req)
    {
        var command = req.Adapt<OpenStoreCommand>();
        command.UserId = Guid.NewGuid(); // TODO: Use current user.

        var result = await storeService.OpenStoreAsync(command);

        return result.Match<IActionResult>(
            // ! see if I can avoid the allocation.
            storeId => CreatedAtAction("GetStore", new { storeId = storeId }, storeId),
            errors => Problem(errors)
        );
    }

    [HttpGet]
    public async Task<IEnumerable<StoreDTO>> GetStoresAsync() =>
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
}