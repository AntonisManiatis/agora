using Agora.API.Stores.Models;
using Agora.API.Stores.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Stores;

[Route("[controller]")]
[Produces("application/json")]
public class StoresController : ApiController
{
    private readonly StoreService storeService;

    public StoresController(StoreService storeService) =>
        this.storeService = storeService;

    /// <summary>
    /// Registers a store.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RegisterStoreAsync(RegisterStoreRequest req)
    {
        var userId = User.GetUserId();

        var result = await storeService.RegisterAsync((userId, req));

        return result.Match<IActionResult>(
            // ! see if I can avoid the allocation.
            // ? should I return the entire store again?
            storeId => CreatedAtAction("GetStore", new { storeId = storeId }, storeId),
            errors => Problem(errors)
        );
    }

    /*
    [HttpGet]
    public async Task<IEnumerable<Store>> GetStoresAsync() =>
        await storeService.GetStoresAsync();
    */

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