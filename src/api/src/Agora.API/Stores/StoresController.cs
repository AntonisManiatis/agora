using Agora.API.Stores.Models;
using Agora.Stores.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Stores;

[Route("[controller]")]
[Produces("application/json")]
public class StoresController : ApiController
{
    private readonly IStoreService storeService;
    private readonly Agora.Catalogs.Services.Stores.IStoreService listStoreService;

    public StoresController(
        IStoreService storeService,
        Agora.Catalogs.Services.Stores.IStoreService listStoreService
    )
    {
        this.storeService = storeService;
        this.listStoreService = listStoreService;
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
        var userId = Guid.NewGuid();  // TODO: GET user by token

        var command = new RegisterStoreCommand(
            storeId,
            userId
        );

        // ! should be in the same transaction.
        var result = await listStoreService.ListStoreAsync(
            new Agora.Catalogs.Services.Stores.ListStoreCommand(
                storeId,
                req.Name,
                req.Preferences.Language
            )
        );

        if (result.IsError)
        {
            return Problem(result.Errors);
        }

        result = await storeService.RegisterStoreAsync(command);

        if (result.IsError)
        {
            return Problem(result.Errors);
        }

        // ! see if I can avoid the allocation.
        // ? should I return the entire store again?
        return CreatedAtAction("GetStore", new { storeId = storeId }, storeId);
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