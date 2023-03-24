using Agora.Stores;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
// [Authorize] // ! Once we setup auth, uncomment.
public class StoresController : ControllerBase
{
    private readonly StoreService storeService;

    public StoresController(StoreService storeService)
    {
        this.storeService = storeService;
    }

    /// <summary>
    /// Submits a request to open a store.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("requests")] // Or open-requests
    public async Task<ActionResult> OpenStoreAsync(OpenStoreRequest req)
    {
        var result = await storeService.SubmitOpenStoreRequestAsync(req);

        return result.Match<ActionResult>(
            appId => CreatedAtAction(nameof(GetStoreApplication), new { Id = appId }, appId),
            _ => BadRequest()
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("requests/{id}")]
    public async Task<ActionResult> GetStoreApplication(Guid id)
    {
        var result = await storeService.GetApplication(id);

        return result.MatchFirst<ActionResult>(
            app => Ok(app),
            error => NotFound(error)
        );
    }

    [HttpPost]
    [Route("requests/{id}/approve")]
    public async Task<ActionResult> Approve(Guid id)
    {
        // TODO: Use claims. Only authorized people can do this.
        throw new NotImplementedException();
    }

    [HttpPost]
    [Route("requests/{id}/reject")]
    public async Task<ActionResult> Reject(Guid id)
    {
        // TODO: Use claims. Only authorized people can do this.
        throw new NotImplementedException();
    }
}