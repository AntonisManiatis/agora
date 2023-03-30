using Agora.Stores;
using Agora.Stores.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API;

[Route("[controller]")]
[Produces("application/json")]
public class StoresController : ApiController
{
    private readonly StoreService storeService;

    public StoresController(StoreService storeService)
    {
        this.storeService = storeService;
    }

    /// <summary>
    /// Opens a store.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> OpenStoreAsync(OpenStoreRequest req)
    {
        var result = await storeService.OpenStoreAsync(req);

        return result.Match<ActionResult>(
            storeId => CreatedAtAction(nameof(GetStoreAsync), new { Id = storeId }, storeId),
            _ => BadRequest()
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
    public async Task<ActionResult> GetStoreAsync(Guid storeId)
    {
        var result = await storeService.GetStoreAsync(storeId);

        return result.MatchFirst<ActionResult>(
            app => Ok(app),
            error => NotFound(error)
        );
    }
}