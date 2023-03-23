using Agora.Stores;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
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
    [HttpPost]
    [Route("requests")] // Or open-requests
    public async Task<ActionResult> OpenStoreAsync(OpenStoreRequest req)
    {
        var resp = await storeService.SubmitOpenStoreRequestAsync(req);

        return resp.Match<ActionResult>(
            appId => CreatedAtAction(nameof(GetStoreApplication), new { Id = appId }, appId),
            _ => BadRequest()
        );
    }

    [HttpGet]
    [Route("requests/{id}")]
    public async Task<ActionResult> GetStoreApplication(Guid id)
    {
        return Ok(Task.CompletedTask);
    }
}