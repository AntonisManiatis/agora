using Agora.Stores;
using Agora.Stores.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API;

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
    public async Task<ActionResult> OpenStoreAsync(OpenStoreRequest req)
    {
        var result = await storeService.OpenStoreAsync(req);

        return result.Match<ActionResult>(
            // ! see if I can avoid the allocation.
            storeId => CreatedAtAction("GetStore", new { storeId = storeId }, storeId),
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