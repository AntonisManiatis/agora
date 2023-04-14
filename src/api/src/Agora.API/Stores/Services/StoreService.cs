using Agora.API.Stores.Models;
using Agora.Shared;
using Agora.Stores.Services;

using ErrorOr;

namespace Agora.API.Stores.Services;

public class StoreService
{
    private readonly Agora.Stores.Services.IStoreService storeService;
    private readonly Agora.Catalogs.Services.Stores.IStoreService listStoreService;

    public StoreService(
        Agora.Stores.Services.IStoreService storeService,
        Agora.Catalogs.Services.Stores.IStoreService listStoreService
    )
    {
        this.storeService = storeService;
        this.listStoreService = listStoreService;
    }

    [Transactional]
    public virtual async Task<ErrorOr<Guid>> RegisterAsync((Guid userId, RegisterStoreRequest) request)
    {
        var storeId = Guid.NewGuid(); // TODO: Figure out who makes this

        var (userId, req) = request;
        var command = new RegisterStoreCommand(
            storeId,
            userId
        );

        var result = await listStoreService.ListStoreAsync(
            new Agora.Catalogs.Services.Stores.ListStoreCommand(
                storeId,
                req.Name,
                req.Preferences.Language
            )
        );

        if (result.IsError)
        {
            return result.Errors;
        }

        result = await storeService.RegisterStoreAsync(command);

        if (result.IsError)
        {
            return result.Errors;
        }

        return storeId;
    }
}