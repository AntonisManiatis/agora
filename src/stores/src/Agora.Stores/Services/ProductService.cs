using Agora.Stores.Core.Products;
using Agora.Stores.Core.Stores;

using ErrorOr;

namespace Agora.Stores.Services;

public interface IProductService
{
    // ? guid?
    Task<ErrorOr<Guid>> AddListingAsync(ListProductCommand command);
}

internal sealed class ProductService : IProductService
{
    private readonly IStoreRepository storeRepository;
    private readonly IProductRepository productRepository;

    public ProductService(
        IStoreRepository storeRepository,
        IProductRepository productRepository)
    {
        this.storeRepository = storeRepository;
        this.productRepository = productRepository;
    }

    public async Task<ErrorOr<Guid>> AddListingAsync(ListProductCommand command)
    {
        // TODO: "superficially" validate command.

        var store = await storeRepository.GetStoreAsync(command.StoreId);
        if (store is null)
        {
            return Error.NotFound("Store.NotFound", "Store not found"); // ? Include id?
        }



        return Guid.NewGuid();
    }
}